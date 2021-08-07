using System.Collections;
using FMOD;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Games.GrumpyBear.FMOD.Utilities
{
    /// <summary>
    /// Initialize FMOD properly for use with WebGL.
    /// <list type="number">
    /// <item>
    /// <description>Create an empty scene with a camera, a canvas and a button.</description>
    /// </item>
    /// <item>
    /// <description>Create a new GameObject with <see cref="WebGLInitializer"/>, or simply attach it to an existing GameObject.</description>
    /// </item>
    /// <item>
    /// <description>In the editor, configure <see cref="WebGLInitializer"/> by connecting it to the button and specifying the
    /// "real" first scene to load.
    /// </description>
    /// </item>
    /// <item>
    /// <description>(Optionally) Style and decorate the scene any way you want.</description>
    /// </item>
    /// <item>
    /// <description>Save this scene and make it the first scene in the build configuration.</description>
    /// </item>
    /// </list>
    /// </summary>
    /// <remarks>
    /// This class was inspired by
    /// <see href="https://alessandrofama.com/tutorials/fmod-unity/fix-blocked-audio-browsers/">Fix blocked FMOD audio in Browsers</see>
    /// and <see href="https://fmod.com/resources/documentation-unity?version=2.01&amp;page=examples-async-loading.html">Scripting Examples | Asynchronous Loading</see>
    /// </remarks>
    [AddComponentMenu("Grumpy Bear Games/FMOD Utilities/WebGL Initializer")]
    public class WebGLInitializer : MonoBehaviour
    {
        [SerializeField] private string _firstScene;
        [SerializeField] private Button _startButton;
        
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
            _asyncSceneLoading.allowSceneActivation = true;
        }
        
        #if UNITY_EDITOR
        [UnityEditor.MenuItem("GameObject/Grumpy Bear Games/FMOD Utilities/WebGL Initializer", false, 10)]
        private static void CreateGameObject()
        {
            var go = new GameObject("[WebGL Initializer]", typeof(WebGLInitializer));
            UnityEditor.Undo.RegisterCreatedObjectUndo(go, $"Create {go.name}");
            UnityEditor.Selection.activeObject = go;
        }
        #endif
        
    }
}
