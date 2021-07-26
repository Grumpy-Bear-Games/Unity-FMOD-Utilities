using UnityEditor;
using UnityEngine;

namespace Games.GrumpyBear.FMOD.Utilities.Editor
{
    [CustomEditor(typeof(VCAVolume))]
    public class VCAVolumeEditor : UnityEditor.Editor
    {
        private SerializedProperty _vcaPathProp;
        private SerializedProperty _playerPrefsKeyProp;
        private bool _validVCA;

        private void OnEnable()
        {
            _vcaPathProp = serializedObject.FindProperty("_vcaPath");
            _playerPrefsKeyProp = serializedObject.FindProperty("_playerPrefsKey");
            UpdateVCA();
        }

        private void UpdateVCA()
        {
            try
            {
                var vca = FMODUnity.RuntimeManager.GetVCA(_vcaPathProp.stringValue);
                _validVCA = vca.isValid();
            }
            catch (FMODUnity.VCANotFoundException)
            {
                _validVCA = false;
            }
        }

        public override void OnInspectorGUI()
        {
            
            if (!_validVCA) EditorGUILayout.HelpBox("Invalid VCA", MessageType.Error);
            
            EditorGUI.BeginChangeCheck();
            base.OnInspectorGUI();
            if (EditorGUI.EndChangeCheck()) UpdateVCA();

            var key = _playerPrefsKeyProp.stringValue;

            if (!PlayerPrefs.HasKey(key)) return;

            GUILayout.Space(20);

            EditorGUILayout.HelpBox("Remember: This value is saved in PlayerPrefs; not in this object", MessageType.Info);
            var guiStyle = GUI.skin.label;
            guiStyle.alignment = TextAnchor.MiddleRight;
            
                
            EditorGUILayout.LabelField("Current Value", $"{PlayerPrefs.GetFloat(key)}", guiStyle);
            if (GUILayout.Button("Delete")) PlayerPrefs.DeleteKey(key);
        }
    }
}
