using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace _Scripts.UI {
    [RequireComponent(typeof(Image))]
    public class TabButton : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler {
        
        public TabGroup tabGroup;

        public Image image;
        
        public Color worldBackgroundColor;
        
        public UnityEvent onTabSelected;
        public UnityEvent onTabDeselected;

        private void Awake() {
            tabGroup.Subscribe(this);
        }

        private void Start() {
            TryGetComponent(out image);
        }

        public void OnPointerClick(PointerEventData eventData) {
            tabGroup.OnTabSelected(this);
        }
        
        public void OnPointerEnter(PointerEventData eventData) {
            tabGroup.OnTabEnter(this);
        }
        
        public void OnPointerExit(PointerEventData eventData) {
            tabGroup.OnTabExit(this);
        }

        public void Select() {
            onTabSelected?.Invoke();
        }

        public void Deselect() {
            onTabDeselected?.Invoke();
        }
        
    }
}