using System.Collections.Generic;

namespace SadUtils.UI
{
    public class PopupFactory
    {
        private bool hasTitle;
        private string title;

        private readonly List<PopupContentData> contents;

        private readonly List<PopupButtonData> buttons;

        private bool hasLifeTime;
        private float lifeTime;

        #region Constructor
        public PopupFactory()
        {
            contents = new();
            buttons = new();
        }
        #endregion

        #region Add Elements
        public PopupFactory AddTitle(string title)
        {
            hasTitle = true;
            this.title = title;

            return this;
        }

        public PopupFactory AddContents(params PopupContentData[] contents)
        {
            foreach (PopupContentData content in contents)
                this.contents.Add(content);

            return this;
        }

        public PopupFactory AddContents(IEnumerable<PopupContentData> contents)
        {
            return AddContents(contents.ToArray());
        }

        public PopupFactory AddButtons(params PopupButtonData[] buttons)
        {
            foreach (PopupButtonData button in buttons)
                this.buttons.Add(button);

            return this;
        }

        public PopupFactory AddButtons(IEnumerable<PopupButtonData> buttons)
        {
            return AddButtons(buttons.ToArray());
        }

        public PopupFactory AddLifeTime(float lifeTime)
        {
            hasLifeTime = true;
            this.lifeTime = lifeTime;

            return this;
        }
        #endregion

        #region Build
        public PopupData Build()
        {
            return new()
            {
                hasTitle = hasTitle,
                title = title,

                contents = contents.ToArray(),

                buttons = buttons.ToArray(),

                hasLifeTime = hasLifeTime,
                lifeTime = lifeTime
            };
        }
        #endregion
    }
}
