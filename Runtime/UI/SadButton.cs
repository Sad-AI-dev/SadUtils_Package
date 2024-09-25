using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace SadUtils.UI
{
    [RequireComponent(typeof(Image))]
    public class SadButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
    {
        public enum ButtonState { normal, highlighted, pressed, selected, disabled }

        [Flags]
        public enum TransitionType
        {
            ColorTint = 1,       // 00001
            SpriteSwap = 2,      // 00010
            Animation = 4,       // 00100
            TextSwap = 8,        // 01000
            TextColorTint = 16,  // 10000
        }

        [Header("Transition Settings")]
        [SerializeField] private TransitionType transitions;

        // Only Shown when ColorTint or SpriteSwap is enabled
        [SerializeField] private Image targetImage;

        private HashSet<TransitionType> enabledTransitions;

        private void Awake()
        {
            CompileEnabledTransitionTypes();
        }

        private void CompileEnabledTransitionTypes()
        {
            enabledTransitions = new();

            Array allTypes = Enum.GetValues(typeof(TransitionType));

            foreach (TransitionType type in allTypes)
            {
                if (IsTransitionBitEnabled(type))
                    enabledTransitions.Add(type);
            }
        }

        #region Pointer Event Handler
        public void OnPointerDown(PointerEventData eventData)
        {

        }

        public void OnPointerEnter(PointerEventData eventData)
        {

        }

        public void OnPointerExit(PointerEventData eventData)
        {

        }

        public void OnPointerUp(PointerEventData eventData)
        {

        }
        #endregion

        #region Util
        /// <summary>
        /// Determines if the bit for the given <paramref name="transitionType"/> is enabled through the inspector.
        /// </summary>
        /// <param name="transitionType">transition to check for.</param>
        private bool IsTransitionBitEnabled(TransitionType transitionType)
        {
            return (transitions & transitionType) != 0;
        }

        public bool IsTransitionEnabled(TransitionType transitionType)
        {
            return enabledTransitions.Contains(transitionType);
        }
        #endregion
    }
}
