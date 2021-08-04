using UnityEngine;

namespace Games.GrumpyBear.FMOD.Utilities
{
    [CreateAssetMenu(menuName = "Grumpy Bear Games/FMOD Utilities/VCA Volume Preference", fileName = "VCA Volume Preference")]
    public sealed class VCAVolumePreference: VolumePreference
    {
        [SerializeField] private string _vcaPath = "vca:/";
        [SerializeField] private string _playerPrefsKey = "Settings/Audio/SFXVolume";
        [SerializeField][Range(0f, 1f)] private float _defaultVolume = 0.8f;
        [SerializeField] private bool _initializeOnEnable = true;

        private global::FMOD.Studio.VCA _vca;

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
                _vca.setVolume(value);
                PlayerPrefs.SetFloat(_playerPrefsKey, value);
                PlayerPrefs.Save();
            }
        }
        
        private void EnsureValid()
        {
            if (_vca.isValid()) return;
            _vca = FMODUnity.RuntimeManager.GetVCA(_vcaPath);
            Debug.Log($"{name} Initializing {Volume}");
            _vca.setVolume(Volume);
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
    }
}
