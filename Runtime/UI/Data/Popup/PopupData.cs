namespace SadUtils.UI
{
    public struct PopupData
    {
        // Title
        public bool hasTitle;
        public string title;

        // Contents
        public PopupContentData[] contents;

        // Response Buttons
        public PopupButtonData[] buttonData;

        // Lifetime
        public bool shouldDestroySelf;
        public float destroySelfDelay;
    }
}
