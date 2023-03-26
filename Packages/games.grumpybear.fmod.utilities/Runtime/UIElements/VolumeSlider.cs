﻿#if UNITY_2022_1_OR_NEWER
using UnityEngine;
using UnityEngine.UIElements;

namespace Games.GrumpyBear.FMOD.Utilities.UIElements
{
    /// <summary>
    /// A slider VisualElement for building a volume control UI using UI Toolkit.
    /// Simply add this VisualElement to your visual tree and set the <see cref="VolumeSlider.VolumePreference"/>
    /// property on startup 
    /// </summary>
    public class VolumeSlider : VisualElement
    {
        public new class UxmlFactory : UxmlFactory<VolumeSlider, UxmlTraits> { }

        public new class UxmlTraits : VisualElement.UxmlTraits
        {
            private readonly UxmlStringAttributeDescription labelAttr = new()
            {
                name = "label", defaultValue = ""
            };
            
            public override void Init(VisualElement ve, IUxmlAttributes bag, CreationContext cc)
            {
                base.Init(ve, bag, cc);

                var volumeSlider = (ve as VolumeSlider); 
                
                volumeSlider.label = labelAttr.GetValueFromBag(bag, cc);
            }
        };

        private string label
        {
            get => _slider.label;
            set => _slider.label = value;
        }


        private readonly Slider _slider;
        private VolumePreference _volumePreference;
        
        /// <summary>
        /// Get or set the <see cref="Games.GrumpyBear.FMOD.Utilities.VolumePreference"/> bound to this slider.
        /// </summary>
        public VolumePreference VolumePreference
        {
            get => _volumePreference;
            set
            {
                
                _volumePreference = value;
                UpdateUI();
            }
        }

        /// <summary>
        /// Forcefully update the slider to match the current value from the bound
        /// <see cref="Games.GrumpyBear.FMOD.Utilities.VolumePreference"/>
        ///
        /// Normally, you shouldn't have to call this function at all. This is only useful if you have updated
        /// the bound <see cref="Games.GrumpyBear.FMOD.Utilities.VolumePreference"/> manually.
        /// </summary>
        public void UpdateUI()
        {
            _slider.SetValueWithoutNotify(_volumePreference != null ? _volumePreference.Volume : _slider.lowValue);
        }

        public VolumeSlider()
        {
            _slider = new Slider() { lowValue = 0, highValue = 1 };
            _slider.RegisterValueChangedCallback(SetVolume);
            _slider.RegisterCallback<GeometryChangedEvent>(UpdateOnShow);
            hierarchy.Add(_slider);
        }
        
        private void UpdateOnShow(GeometryChangedEvent evt)
        {
            if (evt.oldRect == Rect.zero && evt.newRect != Rect.zero) UpdateUI();
        }


        private void SetVolume(ChangeEvent<float> evt)
        {
            if (_volumePreference == null) return;
            _volumePreference.Volume = evt.newValue;
        }
    }
}
#endif
