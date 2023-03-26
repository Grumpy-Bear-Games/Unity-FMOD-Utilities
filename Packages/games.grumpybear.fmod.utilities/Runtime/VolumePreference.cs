using UnityEngine;

namespace Games.GrumpyBear.FMOD.Utilities
{
    /// <summary>
    /// Base class for persistent volume control
    /// </summary>
    public abstract class VolumePreference: ScriptableObject
    {
        [Tooltip("Automatically initialize FMOD on enable.")]
        [SerializeField] private bool _initializeOnEnable = true;
        
        [Tooltip("The default volume set when PlayerPrefs is empty")]
        [SerializeField][Range(0f, 1f)] private float _defaultVolume = 0.8f;

        /// <summary>
        /// Controls whether underlying FMOD object should be automatically initialized on load.
        /// Set this to <c>false</c> when using WebGL to avoid interacting with FMOD before it has
        /// properly loaded. Set it to <c>true</c> on all other platforms.
        /// When set to <c>false</c> initialization can be triggered in 2 ways:
        /// - When something calls <see cref="Initialize"/> explicitly, e.g. <see cref="WebGLInitializer"/>
        ///   (this is the easiest and preferred way).
        /// - On getting or setting <see cref="Volume"/>
        /// </summary>
        /// <value>
        /// Initialize underlying FMOD object and restore volume automatically on load. 
        /// </value>
        public bool InitializeOnEnable
        {
            get => _initializeOnEnable;
            set => _initializeOnEnable = value;
        }

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
        /// - Automatically, when this instance is loaded, if <see cref="InitializeOnEnable"/> is <c>true</c>
        /// - On getting or setting <see cref="Volume"/>
        /// - Explicitly When <see cref="Initialize"/> is called.
        /// </summary>
        private protected abstract void EnsureValid();
        
        /// <summary>
        /// Explicitly initialize the volume from PlayerPrefs.
        /// This will usually happen automatically when <see cref="InitializeOnEnable"/> is <c>true</c>,
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
