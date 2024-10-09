using System;
using UnityEngine;

namespace SadUtils.UI
{
    public class TabController : MonoBehaviour
    {
        private enum DisableBehaviour
        {
            Reset,
            None,
            Retain
        }

        [SerializeField] private SadButton[] tabButtons;
        [SerializeField] private GameObject[] tabContents;

        [Space]
        [SerializeField] private int defaultTabIndex;

        [Header("Disable Behaviour")]
        [SerializeField] private DisableBehaviour disableBehaviour;

        private int currentIndex;

        private void Awake()
        {
            if (tabButtons.Length != tabContents.Length)
                throw new Exception("Number of tab buttons does not match tab contents!");

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

        #region Show / Hide Tab
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
        #endregion

        private void OnDisable()
        {
            switch (disableBehaviour)
            {
                case DisableBehaviour.Reset:
                    ShowTab(defaultTabIndex);
                    break;

                case DisableBehaviour.Retain:
                    StoreCurrentIndex();
                    break;
            }
        }

        private void StoreCurrentIndex()
        {

        }
    }
}
