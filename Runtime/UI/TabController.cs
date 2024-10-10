using System;
using UnityEngine;

namespace SadUtils.UI
{
    public class TabController : MonoBehaviour
    {
        private const string DEFAULT_TAB_PREF_SUFFIX = "_defaultTab";

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
        [Tooltip("Determines how the controller reacts to being disabled.\n" +
            "Reset: Resets the current tab to DefaultTabIndex.\n" +
            "None: Does not react to being disabled.\n" +
            "Retain: Retains the current active tab between play sessions.")]
        [SerializeField] private DisableBehaviour disableBehaviour;

        private int currentIndex;

        private void Awake()
        {
            if (tabButtons.Length != tabContents.Length)
                throw new Exception("Number of tab buttons does not match tab contents!");
        }

        private void Start()
        {
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
            ShowTab(GetDefaultTabIndex());
        }

        private int GetDefaultTabIndex()
        {
            if (disableBehaviour == DisableBehaviour.Retain)
            {
                string prefKey = gameObject.name + DEFAULT_TAB_PREF_SUFFIX;

                if (PlayerPrefs.HasKey(prefKey))
                    defaultTabIndex = PlayerPrefs.GetInt(prefKey);
            }

            return defaultTabIndex;
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
                    SwitchTab(defaultTabIndex);
                    break;

                case DisableBehaviour.Retain:
                    StoreCurrentIndex();
                    break;
            }
        }

        private void StoreCurrentIndex()
        {
            string prefKey = gameObject.name + DEFAULT_TAB_PREF_SUFFIX;
            PlayerPrefs.SetInt(prefKey, currentIndex);
        }
    }
}
