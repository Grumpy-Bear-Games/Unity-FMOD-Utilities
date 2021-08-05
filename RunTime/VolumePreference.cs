using UnityEngine;

namespace Games.GrumpyBear.FMOD.Utilities
{
    public abstract class VolumePreference: ScriptableObject
    {
        [SerializeField] protected bool _initializeOnEnable = true;
        [SerializeField][Range(0f, 1f)] protected float _defaultVolume = 0.8f;

        
        public abstract string PlayerPrefsKey { get;  }
        public abstract float Volume { get; set; }

        protected abstract void EnsureValid();
        

        public void Initialize() => EnsureValid();

        protected void OnEnable()
        {
            #if UNITY_EDITOR
            UnityEditor.EditorApplication.playModeStateChanged += change =>
            {
                if (change != UnityEditor.PlayModeStateChange.EnteredPlayMode) return;
                if (_initializeOnEnable) EnsureValid();
            };
            #else
            if (_initializeOnEnable) EnsureValid();
            #endif
        }        

        public void ClearPlayerPrefs() => PlayerPrefs.DeleteKey(PlayerPrefsKey);
    }
}
