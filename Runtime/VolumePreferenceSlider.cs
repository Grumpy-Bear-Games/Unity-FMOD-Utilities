using UnityEngine;
using UnityEngine.UI;

namespace Games.GrumpyBear.FMOD.Utilities
{
    /// <summary>
    /// Bind a <c>UnityEngine.UI.Slider</c> to a <see cref="VolumePreference"/> to make a volume slider.
    /// Add this class to the GameObject containing the <c>Slider</c>.
    /// See <see href="xref:manual.volumecontrol.md">Persistent Volume Control</see> for usage.
    /// </summary>
    [AddComponentMenu("Grumpy Bear Games/FMOD Utilities/Volume Preference Slider")]
    [HelpURL("https://grumpy-bear-games.github.io/Unity-FMOD-Utilities/api/Games.GrumpyBear.FMOD.Utilities.VolumePreferenceSlider.html")]
    [RequireComponent(typeof(Slider))]
    public class VolumePreferenceSlider : MonoBehaviour
    {
        [SerializeField] private VolumePreference _volumePreference;

        /// <summary>
        /// Get and set the <see cref="VolumePreference"/> to bind to.
        /// </summary>
        /// <remarks>
        /// Setting this property will update the Slider.
        /// </remarks>
        /// <value>
        /// The <see cref="VolumePreference"/> to bind to.
        /// </value>
        public VolumePreference VolumePreference
        {
            get => _volumePreference;
            set
            {
                if (enabled) _slider.onValueChanged.RemoveListener(OnValueChange);
                _volumePreference = value;
                if (!enabled) return;
                _slider.SetValueWithoutNotify(_volumePreference.Volume);
                _slider.onValueChanged.AddListener(OnValueChange);
            }
        }


        private Slider _slider;

        private void Awake() => _slider = GetComponent<Slider>();

        private void OnEnable()
        {
            _slider.SetValueWithoutNotify(_volumePreference.Volume);
            _slider.onValueChanged.AddListener(OnValueChange);
        }

        private void OnDisable() => _slider.onValueChanged.RemoveListener(OnValueChange);

        private void OnValueChange(float volume) => _volumePreference.Volume = volume;
    }
}
