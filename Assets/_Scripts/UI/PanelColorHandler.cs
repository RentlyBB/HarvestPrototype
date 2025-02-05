using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace _Scripts.UI {
    [RequireComponent(typeof(Image))]
    public class PanelColorHandler : MonoBehaviour {
       
        private Image _image;
        
        private void Start() {
            TryGetComponent(out _image);
        }

        private void OnEnable() {
            TabGroup.OnWorldSelected += ChangePanelColor;
        }

        private void OnDisable() {
            TabGroup.OnWorldSelected -= ChangePanelColor;
        }

        private void ChangePanelColor(Color color) {
            
            DOVirtual.Color(_image.color, color, 0.2f, (value) => {
                _image.color = value;
            });
        }
    }
}