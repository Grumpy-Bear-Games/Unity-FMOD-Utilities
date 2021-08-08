using UnityEngine;

namespace Games.GrumpyBear.FMOD.Utilities
{
    /// <summary>
    /// Persistent volume configuration for an FMOD VCA
    /// </summary>
    [CreateAssetMenu(menuName = "Grumpy Bear Games/FMOD Utilities/VCA Volume Preference", fileName = "VCA Volume Preference")]
    public sealed class VCAVolumePreference: VolumePreference
    {
        [SerializeField] private string _vcaPath = "vca:/";
        [SerializeField] private string _playerPrefsKey = "Settings/Audio/SFXVolume";

        private global::FMOD.Studio.VCA _vca;

        /// <summary>
        /// Path to the VCA.
        /// Notice that changing this during playtime will not reset the previous VCA's volume. 
        /// </summary>
        public string VCAPath
        {
            get => _vcaPath;
            set
            {
                _vcaPath = value;
                _vca.clearHandle();
                EnsureValid();
            }
        }
        
        /// <inheritdoc />
        public override string PlayerPrefsKey => _playerPrefsKey;
        
        /// <inheritdoc />
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

        private protected override void EnsureValid()
        {
            #if UNITY_EDITOR
            if (!UnityEditor.EditorApplication.isPlayingOrWillChangePlaymode) return;
            #endif
            
            if (_vca.isValid()) return;
            _vca = FMODUnity.RuntimeManager.GetVCA(_vcaPath);
            _vca.setVolume(Volume);
        }
    }
}
