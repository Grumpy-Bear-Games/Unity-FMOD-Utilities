using System.Collections;
using System.Collections.Generic;
using FMOD;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Games.GrumpyBear.FMOD.Utilities
{
    /// <summary>
    /// Initialize FMOD properly for use with WebGL.
    /// See <see href="xref:manual.webgl.md">Using FMOD with WebGL</see> for usage.
    /// </summary>
    /// <remarks>
    /// This class was inspired by
    /// <see href="https://alessandrofama.com/tutorials/fmod-unity/fix-blocked-audio-browsers/">Fix blocked FMOD audio in Browsers</see>
    /// and <see href="https://fmod.com/resources/documentation-unity?version=2.01&amp;page=examples-async-loading.html">Scripting Examples | Asynchronous Loading</see>
    /// </remarks>
    [AddComponentMenu("Grumpy Bear Games/FMOD Utilities/WebGL FMOD Initializer")]
    [HelpURL("https://grumpy-bear-games.github.io/Unity-FMOD-Utilities/api/Games.GrumpyBear.FMOD.Utilities.WebGLInitializer.html")]
    public class WebGLInitializer : MonoBehaviour
    {
        [Tooltip("Name of the first scene to load once FMOD has been initialized")]
        [SerializeField] private string _firstScene;
        
        [Tooltip("The Button which will provide the required explicit user interaction.")]
        [SerializeField] private Button _startButton;
        
        [Tooltip("VolumePreference instances to initialize once FMOD is ready.")]
        [SerializeField] private List<VolumePreference> _volumePreferencesToInitialize = new List<VolumePreference>();
        
        private AsyncOperation _asyncSceneLoading;

        private void Awake()
        {
            _startButton.onClick.AddListener(StartGame);
            _startButton.gameObject.SetActive(false);
        }

        private IEnumerator Start()
        {
            _asyncSceneLoading = SceneManager.LoadSceneAsync(_firstScene);
            _asyncSceneLoading.allowSceneActivation = false;
            
            while (FMODUnity.RuntimeManager.AnyBankLoading())
            {
                yield return null;
            }

            _startButton.gameObject.SetActive(true);
            _startButton.Select();
        }

        private void StartGame()
        {
            var result = FMODUnity.RuntimeManager.CoreSystem.mixerSuspend();
            Assert.AreEqual(result, RESULT.OK);
            result = FMODUnity.RuntimeManager.CoreSystem.mixerResume();
            Assert.AreEqual(result, RESULT.OK);
            _volumePreferencesToInitialize.ForEach(v => v.Initialize());
            _asyncSceneLoading.allowSceneActivation = true;
        }

        #if UNITY_EDITOR
        [UnityEditor.MenuItem("GameObject/Grumpy Bear Games/FMOD Utilities/WebGL Initializer", false, 10)]
        private static void CreateGameObject()
        {
            var go = new GameObject("[WebGL Initializer]", typeof(WebGLInitializer));
            go.GetComponent<WebGLInitializer>()?.ResetVolumePreferences();
            UnityEditor.Undo.RegisterCreatedObjectUndo(go, $"Create {go.name}");
            UnityEditor.Selection.activeObject = go;
        }

        public void ResetVolumePreferences()
        {
            _volumePreferencesToInitialize.Clear();
            var guids = UnityEditor.AssetDatabase.FindAssets("t:"+ nameof(VolumePreference));
            foreach (var t in guids)
            {
                var path = UnityEditor.AssetDatabase.GUIDToAssetPath(t);
                _volumePreferencesToInitialize.Add(UnityEditor.AssetDatabase.LoadAssetAtPath<VolumePreference>(path));
            }
        }
        #endif
    }
}
