using UnityEngine;

namespace SadUtils.UI.Types
{
    [System.Serializable]
    public class ButtonVisualData
    {
        // Color Tint Settings
        public Color color = Color.white;
        public float colorTransitionDuration = 1f;

        [Space]
        // Sprite Swap
        public Sprite sprite;

        [Space]
        // Animation
        public string transitionTrigger;
        public int triggerHash { get; private set; }

        [Space]
        // Text Swap
        public string text;

        [Space]
        // Text Color Tint
        public Color textColor = Color.white;
        public float textColorTransitionDuration = 1f;

        public void CalculateTriggerHash()
        {
            triggerHash = Animator.StringToHash(transitionTrigger);
        }
    }
}
