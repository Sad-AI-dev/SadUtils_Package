using System;
using UnityEngine;

namespace SadUtils.UI
{
    public class TabController : MonoBehaviour
    {
        [SerializeField] private SadButton[] tabButtons;
        [SerializeField] private GameObject[] tabContents;

        [Space]
        [SerializeField] private int defaultTabIndex;

        private int currentIndex;

        private void Awake()
        {
            if (tabButtons.Length != tabContents.Length)
                throw new System.Exception("Number of tab buttons does not match tab contents!");

            InitButtons();
            ShowDefaultTab();
        }

        private void InitButtons()
        {
            for (int i = 0; i < tabButtons.Length; i++)
                tabButtons[i].OnClick += SwitchTab;
        }

        private void ShowDefaultTab()
        {
            // Hide all tabs.
            for (int i = 0; i < tabButtons.Length; i++)
                HideTab(i);

            // Show default tab.
            ShowTab(defaultTabIndex);
        }

        public void SwitchTab(int targetIndex)
        {
            HideTab(currentIndex);
            ShowTab(targetIndex);
        }

        private void SwitchTab(SadButton tabButton)
        {
            int targetIndex = Array.IndexOf(tabButtons, tabButton);
            SwitchTab(targetIndex);
        }

        private void HideTab(int index)
        {
            tabContents[index].SetActive(false);

            SadButton tabButton = tabButtons[index];

            tabButton.SetState(SadButton.ButtonState.Normal);

            tabButton.SetInteractable(true);
            tabButton.SetFrozen(false);
        }

        private void ShowTab(int index)
        {
            tabContents[index].SetActive(true);

            SadButton tabButton = tabButtons[index];

            tabButton.SetState(SadButton.ButtonState.Selected);

            tabButton.SetFrozen(true);
            tabButton.SetInteractable(false);

            // Store shown tab index.
            currentIndex = index;
        }
    }
}
