using UnityEngine;

namespace Games.GrumpyBear.FMOD.Utilities
{
    [CreateAssetMenu(menuName = "Grumpy Bear Games/FMOD Utilities/Bus Volume Preference", fileName = "Bus Volume Preference")]
    public sealed class BusVolumePreference: VolumePreference
    {
        [SerializeField] private string _busPath = "bus:/";
        [SerializeField] private string _playerPrefsKey = "Settings/Audio/MasterVolume";

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

        protected override void EnsureValid()
        {
            #if UNITY_EDITOR
            if (UnityEditor.EditorApplication.isPlayingOrWillChangePlaymode) return;
            #endif
            
            if (_bus.isValid()) return;
            _bus = FMODUnity.RuntimeManager.GetBus(_busPath);
            Debug.Log($"{name} Initializing {Volume}");
            _bus.setVolume(Volume);
        }
    }}
