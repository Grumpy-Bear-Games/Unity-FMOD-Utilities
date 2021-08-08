using UnityEngine;

namespace Games.GrumpyBear.FMOD.Utilities
{
    /// <summary>
    /// Base class for persistent volume control
    /// </summary>
    public abstract class VolumePreference: ScriptableObject
    {
        [Tooltip("Automatically initialize FMOD on enable.")]
        [SerializeField] private protected bool _initializeOnEnable = true;
        
        [Tooltip("The default volume set when PlayerPrefs is empty")]
        [SerializeField][Range(0f, 1f)] private protected float _defaultVolume = 0.8f;

        /// <value>
        /// The default volume set when PlayerPrefs is empty.
        /// </value>
        public float DefaultVolume
        {
            get => _defaultVolume;
            set => _defaultVolume = value;
        }

        /// <value>
        /// The PlayerPrefs key which the volume gets persisted under 
        /// </value>
        public abstract string PlayerPrefsKey { get;  }
        
        /// <value>
        /// The current volume.
        /// The range goes from 0 to 1.
        /// Notice that setting this will both set the volume and persist it to PlayerPrefs.
        /// </value>
        public abstract float Volume { get; set; }

        /// <summary>
        /// Initialize the underlying FMOD object.
        /// This method will be called in a number of situations:
        /// - Automatically, when this ScriptableObject is loaded, if <see cref="_initializeOnEnable"/> is <c>true</c>
        /// - On getting or setting <see cref="Volume"/>
        /// - Explicitly When <see cref="Initialize"/> is called.
        /// </summary>
        private protected abstract void EnsureValid();
        
        /// <summary>
        /// Explicitly initialize the volume from PlayerPrefs.
        /// This will usually happen automatically when _initializeOnEnable is <c>true</c>,
        /// but you can also call this method manually for more direct control.
        /// This is e.g. useful for WebGL, where you must wait until FMOD has been properly
        /// initialized before interacting with  
        /// </summary>
        public void Initialize() => EnsureValid();

        private protected void OnEnable()
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

        /// <summary>
        /// Clear the volume setting stored in PlayerPrefs.
        /// </summary>
        public void ClearPlayerPrefs() => PlayerPrefs.DeleteKey(PlayerPrefsKey);
    }
}
