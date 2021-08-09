using System.Linq;
using System.Text;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Games.GrumpyBear.FMOD.Utilities.Editor
{
    [CustomEditor(typeof(WebGLInitializer))]
    public class WebGLInitializerEditor : UnityEditor.Editor
    {
        private WebGLInitializer _webGLInitializer;
        private SerializedProperty _firstScene;
        private SerializedProperty _startButton;

        private void OnEnable()
        {
            _webGLInitializer = target as WebGLInitializer;
            _firstScene = serializedObject.FindProperty("_firstScene");
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
            var firstSceneIsSet = !string.IsNullOrEmpty(_firstScene.stringValue);
            var buttonIsSet = _startButton.objectReferenceValue != null;

            if (buildIndexInCorrect && firstSceneIsSet && buttonIsSet) return;

            var problems = new StringBuilder("Problems:\n");
            if (!buildIndexInCorrect) problems.AppendLine("- This scene should be the first scene in the build index");
            if (!firstSceneIsSet) problems.AppendLine("- First scene must be set");
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

            if (!firstSceneIsSet)
            {
                    
                if (SceneManager.sceneCountInBuildSettings > 1)
                {
                    var firstScene = AssetDatabase.LoadAssetAtPath<SceneAsset>(EditorBuildSettings.scenes[1].path);
                    _firstScene.stringValue = firstScene.name;
                }
            }

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
