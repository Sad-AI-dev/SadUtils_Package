using System;

namespace SadUtils.UI
{
    public struct PopupData
    {
        // Title
        public bool hasTitle;
        public string title;

        // Content
        public PopupContentData[] contents;

        // Buttons
        public PopupButtonData[] buttons;

        // Lifetime
        public bool hasLifeTime;
        public float lifeTime;

        public bool hasLifeTimeResponse;
        public Action onLifeTimeExpire;
    }
}
