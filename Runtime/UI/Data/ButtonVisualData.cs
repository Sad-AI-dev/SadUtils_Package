using UnityEngine;

namespace SadUtils.UI
{
    [System.Serializable]
    public class ButtonVisualData
    {
        private const float DEFAULT_TRANSITION_LENGTH = 0.1f;

        // Color Tint Settings
        public Color color;
        public float colorTransitionDuration;

        [Space]
        // Sprite Swap
        public Sprite sprite;

        [Space]
        // Animation
        public string transitionTrigger;
        public int TriggerHash { get; private set; }

        [Space]
        // Text Swap
        public string text;

        [Space]
        // Text Color Tint
        public Color textColor;
        public float textColorTransitionDuration;

        // Default constructor
        public ButtonVisualData(string transitionTrigger)
        {
            color = Color.white;
            colorTransitionDuration = DEFAULT_TRANSITION_LENGTH;

            sprite = null;

            this.transitionTrigger = transitionTrigger;

            textColor = Color.black;
            textColorTransitionDuration = DEFAULT_TRANSITION_LENGTH;
        }

        public void CalculateTriggerHash()
        {
            TriggerHash = Animator.StringToHash(transitionTrigger);
        }
    }
}
