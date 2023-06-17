using UnityEditor;
using UnityEngine;

namespace Games.GrumpyBear.FMOD.Utilities.Editor
{
    [CustomEditor(typeof(VCAVolumePreference))]
    public class VCAVolumePreferenceEditor : UnityEditor.Editor
    {
        private SerializedProperty _vcaPathProp;
        private bool _validVCA;
        private VCAVolumePreference _vcaVolumePreference;

        private void OnEnable()
        {
            _vcaPathProp = serializedObject.FindProperty("_vcaPath");
            _vcaVolumePreference = target as VCAVolumePreference;
            UpdateVCA();
        }

        private void UpdateVCA()
        {
            if (EditorApplication.isPlayingOrWillChangePlaymode) return;
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
            if (!EditorApplication.isPlayingOrWillChangePlaymode)
            {
                if (!_validVCA) EditorGUILayout.HelpBox("Invalid VCA", MessageType.Error);
            }

            EditorGUI.BeginChangeCheck();
            base.OnInspectorGUI();
            if (EditorGUI.EndChangeCheck()) UpdateVCA();

            GUILayout.Space(20);

            var oldValue = _vcaVolumePreference.Volume;
            var newValue = EditorGUILayout.Slider("Volume", oldValue, 0f, 1f);
            if (!Mathf.Approximately(oldValue, newValue)) PlayerPrefs.SetFloat(_vcaVolumePreference.PlayerPrefsKey, newValue);
            
            if (!PlayerPrefs.HasKey(_vcaVolumePreference.PlayerPrefsKey)) return;
            EditorGUILayout.HelpBox("Remember: This value is saved in PlayerPrefs; not in this object", MessageType.Info);
            if (GUILayout.Button("Delete")) _vcaVolumePreference.ClearPlayerPrefs();
        }
    }
}
