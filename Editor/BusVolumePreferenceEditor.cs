using UnityEditor;
using UnityEngine;

namespace Games.GrumpyBear.FMOD.Utilities.Editor
{
    [CustomEditor(typeof(BusVolumePreference))]
    public class BusVolumePreferenceEditor : UnityEditor.Editor
    {
        private SerializedProperty _busPathProp;
        private bool _validBus;
        private BusVolumePreference _busVolumePreference;

        private void OnEnable()
        {
            _busPathProp = serializedObject.FindProperty("_busPath");
            _busVolumePreference = target as BusVolumePreference;
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

            GUILayout.Space(20);

            var oldValue = _busVolumePreference.Volume;
            var newValue = EditorGUILayout.Slider("Volume", oldValue, 0f, 1f);
            if (!Mathf.Approximately(oldValue, newValue)) PlayerPrefs.SetFloat(_busVolumePreference.PlayerPrefsKey, newValue);
            
            if (!PlayerPrefs.HasKey(_busVolumePreference.PlayerPrefsKey)) return;
            EditorGUILayout.HelpBox("Remember: This value is saved in PlayerPrefs; not in this object", MessageType.Info);
            if (GUILayout.Button("Delete")) _busVolumePreference.ClearPlayerPrefs();
        }
    }
}
