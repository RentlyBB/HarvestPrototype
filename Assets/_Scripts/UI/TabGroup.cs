using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using VInspector.Libs;

namespace _Scripts.UI {
    public class TabGroup : MonoBehaviour {
        public List<TabButton> tabButtons = new List<TabButton>();
        public List<GameObject> tabs = new List<GameObject>();

        public Sprite tabIdle;
        public Sprite tabHover;
        public Sprite tabActive;

        public TabButton selectedTab;

        public static event UnityAction<Color> OnWorldSelected = delegate { };


        private void Start() {
            // Sort all buttons
            if (tabButtons.Count != 0) {
                tabButtons.SortBy(tab => tab.transform.GetSiblingIndex());
            }
        }

        public void Subscribe(TabButton button) {
            tabButtons.Add(button);
        }

        public void OnTabEnter(TabButton button) {
            ResetTabs();
            if (selectedTab == null || button != selectedTab) {
                button.image.sprite = tabHover;
            }
        }

        public void OnTabExit(TabButton button) {
            ResetTabs();
        }

        public void OnTabSelected(TabButton button) {
            if (selectedTab != null) {
                selectedTab.Deselect();
            }

            selectedTab = button;

            selectedTab.Select();
            OnWorldSelected?.Invoke(selectedTab.worldBackgroundColor);

            ResetTabs();
            button.image.sprite = tabActive;
            int index = button.transform.GetSiblingIndex();

            for (int i = 0; i < tabs.Count; i++) {
                tabs[i].SetActive(i == index);
            }
        }

        public void ArrowSelect(bool toRight) {
            int nextIndex = toRight ? 1 : -1;

            for (int i = 0; i < tabButtons.Count; i++) {
                if (tabButtons[i] == selectedTab) {
                    nextIndex += i;
                    break;
                }
            }

            //loop through menu
            if (nextIndex > tabButtons.Count - 1) {
                nextIndex = 0;
            } else if (nextIndex < 0) {
                nextIndex = tabButtons.Count - 1;
            }

            OnTabSelected(tabButtons[nextIndex]);
        }

        private void ResetTabs() {
            if (tabButtons.Count == 0) return;

            foreach (TabButton button in tabButtons) {
                if (selectedTab != null && button == selectedTab) continue;
                button.image.sprite = tabIdle;
            }
        }
    }
}