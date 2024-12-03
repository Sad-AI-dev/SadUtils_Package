using System;

namespace SadUtils.UI
{
    public abstract class PopupButtonData
    {
        public abstract string Type { get; }
        public abstract Action Callback { get; }
    }
}
