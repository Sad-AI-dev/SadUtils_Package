using UnityEngine;

namespace SadUtils.UI
{
    public struct PopupContentData
    {
        public PopupContentType type;

        public string contentString;
        public Sprite contentSprite;
        public float spacerHeight;

        public object otherData;
    }
}
