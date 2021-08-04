using UnityEngine;

namespace Games.GrumpyBear.FMOD.Utilities
{
    [CreateAssetMenu(menuName = "Grumpy Bear Games/FMOD Utilities/Bus Volume Preference", fileName = "Bus Volume Preference")]
    public sealed class BusVolumePreference: VolumePreference
    {
        [SerializeField] private string _busPath = "bus:/";
        [SerializeField] private string _playerPrefsKey = "Settings/Audio/MasterVolume";
        [SerializeField][Range(0f, 1f)] private float _defaultVolume = 0.8f;
        [SerializeField] private bool _initializeOnEnable = true;

        private global::FMOD.Studio.Bus _bus;

        public override string PlayerPrefsKey => _playerPrefsKey;
        public override float Volume
        {
            get
            {
                EnsureValid();
                return PlayerPrefs.GetFloat(_playerPrefsKey, _defaultVolume);
            }
            set
            {
                EnsureValid();
                _bus.setVolume(value);
                PlayerPrefs.SetFloat(_playerPrefsKey, value);
                PlayerPrefs.Save();
            }
        }
        
        private void EnsureValid()
        {
            if (_bus.isValid()) return;
            _bus = FMODUnity.RuntimeManager.GetBus(_busPath);
            Debug.Log($"{name} Initializing {Volume}");
            _bus.setVolume(Volume);
        }

        public override void Initialize() => EnsureValid();

#if UNITY_EDITOR
        private void OnEnable()
        {
            UnityEditor.EditorApplication.playModeStateChanged += change =>
            {
                if (change != UnityEditor.PlayModeStateChange.EnteredPlayMode) return;
                if (_initializeOnEnable) EnsureValid();
            };
        }
        
        [ContextMenu("Clear PlayerPrefs")]
        private void ClearPlayerPrefs() => PlayerPrefs.DeleteKey(_playerPrefsKey);
#else
        private void OnEnable() {
            if (_initializeOnEnable) EnsureValid();
        }
#endif
    }}
