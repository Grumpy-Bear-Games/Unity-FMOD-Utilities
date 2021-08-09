using UnityEngine;

namespace Games.GrumpyBear.FMOD.Utilities
{
    /// <summary>
    /// Persistent volume configuration for an FMOD bus
    /// </summary>
    [CreateAssetMenu(menuName = "Grumpy Bear Games/FMOD Utilities/Bus Volume Preference", fileName = "Bus Volume Preference")]
    public sealed class BusVolumePreference: VolumePreference
    {
        [SerializeField] private string _busPath = "bus:/";
        [SerializeField] private string _playerPrefsKey = "Settings/Audio/MasterVolume";
        
        private global::FMOD.Studio.Bus _bus;

        /// <summary>
        /// Path to the bus.
        /// Notice that changing this during playtime will not reset the previous bus' volume. 
        /// </summary>
        public string BusPath
        {
            get => _busPath;
            set
            {
                _busPath = value;
                _bus.clearHandle();
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
                return PlayerPrefs.GetFloat(_playerPrefsKey, DefaultVolume);
            }
            set
            {
                EnsureValid();
                _bus.setVolume(value);
                PlayerPrefs.SetFloat(_playerPrefsKey, value);
                PlayerPrefs.Save();
            }
        }

        /// <inheritdoc />
        private protected override void EnsureValid()
        {
            #if UNITY_EDITOR
            if (!UnityEditor.EditorApplication.isPlayingOrWillChangePlaymode) return;
            #endif
            
            if (_bus.isValid()) return;
            _bus = FMODUnity.RuntimeManager.GetBus(_busPath);
            _bus.setVolume(Volume);
        }
    }}
