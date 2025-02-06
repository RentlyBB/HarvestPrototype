using System;
using System.Collections.Generic;
using _Scripts.GameplayCore;
using _Scripts.UI;
using _Scripts.UnitySingleton;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using VInspector;

namespace _Scripts.Managers {
    public class MenuManager : MonoSingleton<MenuManager> {
    
        public GameData gameData;
        
        // [Header("Tab & Content Setup")]
        // public Transform tabButtonsContainer; // Holds tab buttons
        // public Transform tabContentContainer; // Holds tab panels
        //
        // private List<Button> _tabButtons = new List<Button>();
        // private List<GameObject> _tabPanels = new List<GameObject>();
        // private int _activeTabIndex = 0;  //AKA selected world
        //
        protected override void Awake() {
            base.Awake();
           // SetupTabs();
            gameData = GameDataHandler.Instance.LoadGameData();
        }
        //
        // private void SetupTabs() {
        //     // Get all buttons and panels dynamically
        //     foreach (Transform tab in tabButtonsContainer) {
        //         Button button = tab.GetComponent<Button>();
        //         int index = _tabButtons.Count;
        //         button.onClick.AddListener(() => SwitchTab(index));
        //         _tabButtons.Add(button);
        //     }
        //
        //     foreach (Transform panel in tabContentContainer) {
        //         _tabPanels.Add(panel.gameObject);
        //     }
        // }
        //
        // [Button]
        // public void SwitchTab(int tabIndex) {
        //     if (tabIndex == _activeTabIndex) return;
        //
        //     // Hide all panels & deactivate all tabs
        //     for (int i = 0; i < _tabPanels.Count; i++) {
        //         _tabPanels[i].SetActive(i == tabIndex);
        //         _tabButtons[i].interactable = (i != tabIndex);
        //     }
        //
        //     _activeTabIndex = tabIndex;
        // }
        
        
        
    }
}