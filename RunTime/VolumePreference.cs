using UnityEngine;

namespace Games.GrumpyBear.FMOD.Utilities
{
    public abstract class VolumePreference: ScriptableObject
    {
        public abstract string PlayerPrefsKey { get;  }
        public abstract float Volume { get; set; }
        public abstract void Initialize();
        public void ClearPref() => PlayerPrefs.DeleteKey(PlayerPrefsKey);
    }
}
