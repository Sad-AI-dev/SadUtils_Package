using System;
using UnityEngine;

namespace SadUtils.UI
{
    public class PopupManager : Singleton<PopupManager>
    {
        public event Action<Popup> OnShowPopup;
        public event Action OnHidePopup;

        [SerializeField] protected Transform popupHolder;
        [SerializeField] protected Popup defaultPopupPrefab;

        protected Popup activePopup;

        protected override void Awake()
        {
            SetInstance(this);
        }

        public void ShowPopup(PopupData data) => ShowPopup(defaultPopupPrefab, data);

        public virtual void ShowPopup(Popup prefab, PopupData data)
        {
            if (activePopup != null)
                DestroyActivePopup();

            activePopup = Instantiate(prefab, popupHolder);
            activePopup.Construct(data);

            OnShowPopup?.Invoke(activePopup);
        }

        public void DestroyActivePopup()
        {
            Destroy(activePopup.gameObject);

            OnHidePopup?.Invoke();
        }

        public bool TryGetActivePopup(out Popup popup)
        {
            popup = activePopup;
            return activePopup != null;
        }
    }
}
