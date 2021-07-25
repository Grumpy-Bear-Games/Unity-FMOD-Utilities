using UnityEngine;

namespace Games.GrumpyBear.FMOD.Utility
{
    [CreateAssetMenu(menuName = "Grumpy Bear Games/FMOD Utility/VCA Volume", fileName = "VCA Volume")]
    public sealed class VCAVolume: ScriptableObject
    {
        [SerializeField] private string _vcaPath = "vca:/";
        [SerializeField] private string _playerPrefsKey = "Settings/Audio/SFXVolume";
        [SerializeField][Range(0f, 1f)] private float _defaultVolume = 0.8f;

        private global::FMOD.Studio.VCA _vca;

        public float Volume
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
            }
        }
        
        private void EnsureValid()
        {
            if (_vca.isValid()) return;
            _vca = FMODUnity.RuntimeManager.GetVCA(_vcaPath);
            _vca.setVolume(Volume);
        }

#if UNITY_EDITOR
        private void OnEnable()
        {
            UnityEditor.EditorApplication.playModeStateChanged += change =>
            {
                if (change != UnityEditor.PlayModeStateChange.EnteredPlayMode) return;
                EnsureValid();
            };
        }
        
        [ContextMenu("Clear PlayerPrefs")]
        private void ClearPlayerPrefs() => PlayerPrefs.DeleteKey(_playerPrefsKey);
#else
        private void OnEnable() => EnsureValid();
#endif
    }}
