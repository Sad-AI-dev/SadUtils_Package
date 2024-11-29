using System;
using UnityEngine;

namespace SadUtils.UI
{
    public class PopupManager : Singleton<PopupManager>
    {
        public event Action<Popup> OnShowPopup;
        public event Action OnHidePopup;

        [SerializeField] private Transform popupHolder;
        [SerializeField] private Popup defaultPopupPrefab;

        public Popup ActivePopup { get; private set; }

        protected override void Awake()
        {
            SetInstance(this);
        }

        #region Manage Displayed Popup
        public void DisplayPopup(PopupData data)
        {
            DisplayPopup(defaultPopupPrefab, data);
        }

        public virtual void DisplayPopup(Popup popupPrefab, PopupData data)
        {
            if (ActivePopup != null)
                DestroyCurrentPopup();

            ActivePopup = Instantiate(popupPrefab, popupHolder);
            ActivePopup.Construct(data);
        }

        public void DestroyCurrentPopup()
        {
            Destroy(ActivePopup.gameObject);
        }

        public void DestroyCurrentPopup(float delay)
        {
            Destroy(ActivePopup.gameObject, delay);
        }
        #endregion
    }
}
