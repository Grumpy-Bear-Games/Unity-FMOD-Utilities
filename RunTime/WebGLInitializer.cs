using System.Collections;
using FMOD;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Games.GrumpyBear.FMOD.Utilities
{
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
