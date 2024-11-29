using UnityEngine;

namespace SadUtils.UI
{
    public abstract class Popup : MonoBehaviour
    {
        public abstract void Construct(PopupData data);
    }
}
