using System.Linq;
using System.Text;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

#if ! GRUMPYBEAR_LEVELMANAGEMENT
using UnityEngine.SceneManagement;
#endif

namespace Games.GrumpyBear.FMOD.Utilities.Editor
{
    [CustomEditor(typeof(WebGLInitializer))]
    public class WebGLInitializerEditor : UnityEditor.Editor
    {
        private WebGLInitializer _webGLInitializer;
        #if GRUMPYBEAR_LEVELMANAGEMENT
        private SerializedProperty _firstSceneGroup;
        #else
        private SerializedProperty _firstScene;
        #endif
        private SerializedProperty _startButton;

        private void OnEnable()
        {
            _webGLInitializer = target as WebGLInitializer;
            #if GRUMPYBEAR_LEVELMANAGEMENT
            _firstSceneGroup = serializedObject.FindProperty("_firstSceneGroup");
            #else
            _firstScene = serializedObject.FindProperty("_firstScene");
            #endif
            _startButton = serializedObject.FindProperty("_startButton");
        }

        public override void OnInspectorGUI()
        {
            var scene = _webGLInitializer.gameObject.scene;
            if (string.IsNullOrEmpty(scene.path))
            {
                EditorGUILayout.HelpBox("Save this scene to enable configuration checks", MessageType.Warning);
            }

            base.OnInspectorGUI();

            FixAllProblemsButton();
            FindAllVolumePreferencesButton();
        }

        private void FixAllProblemsButton()
        {
            var scene = _webGLInitializer.gameObject.scene;

            var buildIndexInCorrect = scene.buildIndex == 0;
            var buttonIsSet = _startButton.objectReferenceValue != null;
            #if GRUMPYBEAR_LEVELMANAGEMENT
            if (buildIndexInCorrect && buttonIsSet) return;
            #else
            var firstSceneIsSet = !string.IsNullOrEmpty(_firstScene.stringValue);
            if (buildIndexInCorrect && firstSceneIsSet && buttonIsSet) return;
            #endif
            
            var problems = new StringBuilder("Problems:\n");
            if (!buildIndexInCorrect) problems.AppendLine("- This scene should be the first scene in the build index");
            #if ! GRUMPYBEAR_LEVELMANAGEMENT
            if (!firstSceneIsSet) problems.AppendLine("- First scene must be set");
            #endif
            if (!buttonIsSet) problems.AppendLine("- Start button must be set");
            
            EditorGUILayout.HelpBox(problems.ToString(), MessageType.Error);
            
            if (!GUILayout.Button("Fix all problems")) return;

            if (string.IsNullOrEmpty(scene.path))
            {
                if (!EditorSceneManager.SaveModifiedScenesIfUserWantsTo(new[] { scene })) return;
            }

            if (string.IsNullOrEmpty(scene.path)) return;

            if (!buildIndexInCorrect)
            {
                var editorBuildSettingsScenes = EditorBuildSettings.scenes.ToList();
                var editorBuildSettingsScene = editorBuildSettingsScenes.Find(editorBuildSettingsScene => editorBuildSettingsScene.path == scene.path);
                    
                if (editorBuildSettingsScene == null)
                {
                    editorBuildSettingsScene = new EditorBuildSettingsScene(scene.path, true);
                }
                else
                {
                    editorBuildSettingsScenes.Remove(editorBuildSettingsScene);
                }
                editorBuildSettingsScenes.Insert(0, editorBuildSettingsScene);
                EditorBuildSettings.scenes = editorBuildSettingsScenes.ToArray();
            }

            #if ! GRUMPYBEAR_LEVELMANAGEMENT
            if (!firstSceneIsSet)
            {
                    
                if (SceneManager.sceneCountInBuildSettings > 1)
                {
                    var firstScene = AssetDatabase.LoadAssetAtPath<SceneAsset>(EditorBuildSettings.scenes[1].path);
                    _firstScene.stringValue = firstScene.name;
                }
            }
            #endif

            var button = FindObjectOfType<Button>();
            if (button != null) _startButton.objectReferenceValue = button;
                
            serializedObject.ApplyModifiedProperties();
        }

        private void FindAllVolumePreferencesButton()
        {
            if (!GUILayout.Button("Find all VolumePreferences")) return;
            Undo.RecordObject(_webGLInitializer, "Find all VolumePreferences");
            _webGLInitializer.ResetVolumePreferences();
        }
    }
}
