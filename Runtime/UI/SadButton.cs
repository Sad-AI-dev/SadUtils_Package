using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace SadUtils.UI
{
    [RequireComponent(typeof(Image))]
    public class SadButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
    {
        public enum ButtonState { Normal, Highlighted, Pressed, Selected, Disabled }

        [Flags]
        public enum TransitionType
        {
            ColorTint = 1,       // 00001
            SpriteSwap = 2,      // 00010
            Animation = 4,       // 00100
            TextSwap = 8,        // 01000
            TextColorTint = 16,  // 10000
        }

        [Header("Interactable Settings")]
        [SerializeField] private bool interactable = true;

        [Tooltip("Freezing a button prevents state updates")]
        [SerializeField] private bool frozen;

        [Header("Transition Settings")]
        [SerializeField] private TransitionType transitions;

        // Only Shown when ColorTint or SpriteSwap is enabled.
        [SerializeField] private Image targetImage;
        [SerializeField] private TMP_Text targetText;

        [Header("Events")]
        public UnityEvent OnClick;

        // Keep track of transitions.
        private HashSet<TransitionType> enabledTransitions;

        // Internal State.
        private ButtonState state;

        private bool isCursorOverButton;
        private bool isButtonPressed;

        #region Awake
        private void Awake()
        {
            CompileEnabledTransitionTypes();
            TryFetchImage();
            TryFetchText();

            state = ButtonState.Normal;

            UpdateVisuals();
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

        private void TryFetchImage()
        {
            if (!IsTransitionEnabled(TransitionType.ColorTint) &&
                !IsTransitionEnabled(TransitionType.SpriteSwap))
                return;

            if (!ReferenceEquals(targetImage, null))
                return;

            // No image assigned, attempt to find one.
            targetImage = GetComponent<Image>();

            // Due to the RequireComponent tag, this should be impossible.
            // Keeping this here just in case.
            if (ReferenceEquals(targetImage, null))
                throw new Exception("Target Image required but none found!");
        }

        private void TryFetchText()
        {
            if (!IsTransitionEnabled(TransitionType.TextSwap) &&
                !IsTransitionEnabled(TransitionType.ColorTint))
                return;

            if (!ReferenceEquals(targetText, null))
                return;

            // No text assigned, attempt to find one.
            targetText = GetComponentInChildren<TMP_Text>();

            if (ReferenceEquals(targetText, null))
                throw new Exception("Target Text required but none found!");
        }
        #endregion

        #region External Toggles
        public void Freeze()
        {

        }

        public void Unfreeze()
        {

        }

        public void SetInteractable(bool isInteractable)
        {

        }
        #endregion

        #region Handle Visuals
        protected void UpdateVisuals()
        {
            if (frozen)
                return;
        }
        #endregion

        #region Pointer Event Handler
        public void OnPointerEnter(PointerEventData eventData)
        {
            isCursorOverButton = true;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            isCursorOverButton = false;
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            isButtonPressed = true;
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            isButtonPressed = false;

            if (!interactable)
                return;

            if (isCursorOverButton)
                OnClick?.Invoke();

            UpdateVisuals();
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
