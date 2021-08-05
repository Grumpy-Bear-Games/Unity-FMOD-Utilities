using UnityEngine;

namespace Games.GrumpyBear.FMOD.Utilities
{
    [CreateAssetMenu(menuName = "Grumpy Bear Games/FMOD Utilities/VCA Volume Preference", fileName = "VCA Volume Preference")]
    public sealed class VCAVolumePreference: VolumePreference
    {
        [SerializeField] private string _vcaPath = "vca:/";
        [SerializeField] private string _playerPrefsKey = "Settings/Audio/SFXVolume";

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

        protected override void EnsureValid()
        {
            #if UNITY_EDITOR
            if (UnityEditor.EditorApplication.isPlayingOrWillChangePlaymode) return;
            #endif
            
            if (_vca.isValid()) return;
            _vca = FMODUnity.RuntimeManager.GetVCA(_vcaPath);
            Debug.Log($"{name} Initializing {Volume}");
            _vca.setVolume(Volume);
        }
    }
}
