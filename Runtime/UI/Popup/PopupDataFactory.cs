using System;
using System.Collections.Generic;
using UnityEngine;

namespace SadUtils.UI
{
    public class PopupDataFactory
    {
        private string title;

        private List<PopupContentData> contents;

        private List<PopupButtonData> buttons;

        private float destroySelfDelay;

        #region Constructor
        public PopupDataFactory()
        {
            SetDefaultValues();
        }

        private void SetDefaultValues()
        {
            contents = new();
            buttons = new();
        }
        #endregion

        #region Add Elements
        public PopupDataFactory AddTitle(string title)
        {
            this.title = title;

            return this;
        }

        #region Add Content
        public PopupDataFactory AddStringContent(params string[] stringContents)
        {
            foreach (string stringContent in stringContents)
                contents.Add(new()
                {
                    type = PopupContentType.String,
                    contentString = stringContent,
                });

            return this;
        }

        public PopupDataFactory AddSpriteContent(params Sprite[] spriteContents)
        {
            foreach (Sprite spriteContent in spriteContents)
                contents.Add(new()
                {
                    type = PopupContentType.Sprite,
                    contentSprite = spriteContent,
                });

            return this;
        }

        public PopupDataFactory AddContentSpacer(params float[] spacerHeights)
        {
            foreach (float spacerHeight in spacerHeights)
                contents.Add(new()
                {
                    type = PopupContentType.Spacer,
                    spacerHeight = spacerHeight,
                });

            return this;
        }

        public PopupDataFactory AddOtherContent(params object[] otherContents)
        {
            foreach (object otherContent in otherContents)
                contents.Add(new()
                {
                    type = PopupContentType.Other,
                    otherData = otherContent,
                });

            return this;
        }
        #endregion

        public PopupDataFactory AddButtons(params PopupButtonData[] buttonData)
        {
            foreach (PopupButtonData data in buttonData)
                AddButton(data.title, data.callback);

            return this;
        }

        public PopupDataFactory AddButton(string title, Action callback)
        {
            buttons.Add(new()
            {
                hasTitle = IsValueNotDefault(title),
                title = title,
                callback = callback,
            });

            return this;
        }

        public PopupDataFactory AddDestroySelfAfterDelay(float delay)
        {
            destroySelfDelay = delay;

            return this;
        }
        #endregion

        #region Build
        public PopupData Build()
        {
            return new()
            {
                // Title
                hasTitle = IsValueNotDefault(title),
                title = title,

                // Contents
                contents = contents.ToArray(),

                // Response Buttons
                buttonData = buttons.ToArray(),

                // Destroy Self
                shouldDestroySelf = IsValueNotDefault(destroySelfDelay),
                destroySelfDelay = destroySelfDelay,
            };
        }

        private bool IsValueNotDefault(string s)
        {
            return s != null && s != "";
        }

        private bool IsValueNotDefault(float f)
        {
            return f > 0;
        }
        #endregion
    }
}
