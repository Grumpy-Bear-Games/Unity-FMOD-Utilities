using UnityEditor;
using UnityEngine;

namespace Games.GrumpyBear.FMOD.Utilities.Editor
{
    [CustomEditor(typeof(BusVolumePreference))]
    public class BusVolumePreferenceEditor : UnityEditor.Editor
    {
        private SerializedProperty _busPathProp;
        private SerializedProperty _playerPrefsKeyProp;
        private bool _validBus;

        private void OnEnable()
        {
            _busPathProp = serializedObject.FindProperty("_busPath");
            _playerPrefsKeyProp = serializedObject.FindProperty("_playerPrefsKey");
            UpdateBus();
        }

        private void UpdateBus()
        {
            if (EditorApplication.isPlayingOrWillChangePlaymode) return;
            try
            {
                var bus = FMODUnity.RuntimeManager.GetBus(_busPathProp.stringValue);
                _validBus = bus.isValid();
            }
            catch (FMODUnity.BusNotFoundException)
            {
                _validBus = false;
            }
        }

        public override void OnInspectorGUI()
        {
            if (!EditorApplication.isPlayingOrWillChangePlaymode)
            {
                if (!_validBus) EditorGUILayout.HelpBox("Invalid Bus", MessageType.Error);
            }

            EditorGUI.BeginChangeCheck();
            base.OnInspectorGUI();
            if (EditorGUI.EndChangeCheck()) UpdateBus();

            var key = _playerPrefsKeyProp.stringValue;

            if (!PlayerPrefs.HasKey(key)) return;

            GUILayout.Space(20);

            EditorGUILayout.HelpBox("Remember: This value is saved in PlayerPrefs; not in this object", MessageType.Info);
            var guiStyle = GUI.skin.label;
            guiStyle.alignment = TextAnchor.MiddleRight;
            
                
            EditorGUILayout.LabelField("Current Value", $"{PlayerPrefs.GetFloat(key)}", guiStyle);
            if (GUILayout.Button("Delete"))  (target as VolumePreference)?.ClearPref();
        }
    }
}
