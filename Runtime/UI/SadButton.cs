using SadUtils.Types;
using SadUtils.UI.Types;
using System;
using System.Collections;
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
        [SerializeField] private bool interactable;

        [Tooltip("Freezing a button prevents state updates")]
        [SerializeField] private bool frozen;

        [Header("Transition Settings")]
        [SerializeField] private TransitionType transitions;

        // Only used when ColorTint or SpriteSwap is enabled.
        [SerializeField] private Image targetImage;
        // Only used when Animation is enabled.
        [SerializeField] private Animator targetAnimator;
        // Only used when TextSwap or TextColorTint is enabled.
        [SerializeField] private TMP_Text targetText;

        [Space]
        [SerializeField] private UnityDictionary<ButtonState, ButtonVisualData> visualDataDict;

        [Header("Events")]
        public UnityEvent OnClick;

        // Keep track of transitions.
        private HashSet<TransitionType> enabledTransitions;

        // Keep track of running coroutines
        private Coroutine colorTransitionRoutine;
        private Coroutine textColorTransitionRoutine;

        // Internal state.
        private ButtonState state;

        private bool isCursorOverButton;
        private bool isButtonPressed;

        #region Awake
        private void Awake()
        {
            CompileEnabledTransitionTypes();
            InitializeVisualData();

            TryFetchImage();
            TryFetchAnimator();
            TryFetchText();

            SetState(DetermineCurrentState(), true);
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

        private void InitializeVisualData()
        {
            if (!IsTransitionEnabled(TransitionType.Animation))
                return;

            foreach (ButtonVisualData stateVisualData in visualDataDict.Values)
                stateVisualData.CalculateTriggerHash();
        }

        private void TryFetchImage()
        {
            if (!IsTransitionEnabled(TransitionType.ColorTint) &&
                !IsTransitionEnabled(TransitionType.SpriteSwap))
                return;

            if (!ReferenceEquals(targetImage, null))
                return;

            // No image assigned, attempt to find one.
            if (!TryGetComponent(out Image image))
                throw new Exception("Target Image required but none found!");

            targetImage = image;
        }

        private void TryFetchAnimator()
        {
            if (!IsTransitionEnabled(TransitionType.Animation))
                return;

            if (targetAnimator != null)
                return;

            // No animator assigned, attempt to find one.
            if (!TryGetComponent(out Animator animator))
                throw new Exception("Target Animator required but none found!");

            targetAnimator = animator;
        }

        private void TryFetchText()
        {
            if (!IsTransitionEnabled(TransitionType.TextSwap) &&
                !IsTransitionEnabled(TransitionType.TextColorTint))
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
        public void SetInteractable(bool isInteractable)
        {
            interactable = isInteractable;

            UpdateState();
        }

        public void Freeze()
        {
            frozen = true;
        }

        public void Unfreeze()
        {
            frozen = false;

            UpdateVisuals();
        }

        public bool IsInteractable() => interactable;
        public bool IsFrozen() => frozen;
        #endregion

        #region Handle State
        protected virtual void UpdateState()
        {
            SetState(DetermineCurrentState());
        }

        private ButtonState DetermineCurrentState()
        {
            if (!interactable)
                return ButtonState.Disabled;

            if (isCursorOverButton)
            {
                if (isButtonPressed)
                    return ButtonState.Pressed;
                else
                    return ButtonState.Highlighted;
            }

            return ButtonState.Normal;
        }

        public void SetState(ButtonState stateToSet, bool forceUpdateVisuals = false)
        {
            if (state == stateToSet && !forceUpdateVisuals)
                return;

            state = stateToSet;

            UpdateVisuals(forceUpdateVisuals);
        }

        #endregion

        #region Configure Visuals
        protected void UpdateVisuals(bool forceUpdate = false)
        {
            if (frozen && !forceUpdate)
                return;

            ApplyVisuals(visualDataDict[state]);
        }

        private void ApplyVisuals(ButtonVisualData visualData)
        {
            if (IsTransitionEnabled(TransitionType.ColorTint))
                StartColorTintTransition(visualData);
            if (IsTransitionEnabled(TransitionType.SpriteSwap))
                SetSprite(visualData);
            if (IsTransitionEnabled(TransitionType.Animation))
                TriggerAnimation(visualData);
            if (IsTransitionEnabled(TransitionType.TextSwap))
                SetText(visualData);
            if (IsTransitionEnabled(TransitionType.TextColorTint))
                StartTextColorTintTransition(visualData);
        }

        private void StartColorTintTransition(ButtonVisualData visualData)
        {
            if (colorTransitionRoutine != null)
                StopCoroutine(colorTransitionRoutine);

            colorTransitionRoutine = StartCoroutine(
                TransitionColorTintCo(
                    visualData.color,
                    visualData.colorTransitionDuration));
        }

        private void SetSprite(ButtonVisualData visualData)
        {
            Sprite sprite = visualData.sprite;

            // if none is set, use normal sprite
            if (ReferenceEquals(sprite, null))
                sprite = visualDataDict[ButtonState.Normal].sprite;

            targetImage.sprite = sprite;
        }

        private void TriggerAnimation(ButtonVisualData visualData)
        {
            if (visualData.transitionTrigger == "")
                return;

            targetAnimator.SetTrigger(visualData.TriggerHash);
        }

        private void SetText(ButtonVisualData visualData)
        {
            string text = visualData.text;

            targetText.text = text;
        }

        private void StartTextColorTintTransition(ButtonVisualData visualData)
        {
            if (textColorTransitionRoutine != null)
                StopCoroutine(textColorTransitionRoutine);

            textColorTransitionRoutine = StartCoroutine(
                TransitionTextColorTintCo(
                    visualData.textColor,
                    visualData.textColorTransitionDuration));
        }

        #region Color Transition Loops
        private IEnumerator TransitionColorTintCo(Color targetColor, float duration)
        {
            Color startColor = targetImage.color;
            float timer = 0;

            while (timer < duration)
            {
                timer += Time.deltaTime;
                targetImage.color = Color.Lerp(startColor, targetColor, timer / duration);

                yield return null;
            }

            targetImage.color = targetColor;
        }

        private IEnumerator TransitionTextColorTintCo(Color targetColor, float duration)
        {
            Color startColor = targetText.color;
            float timer = 0;

            while (timer < duration)
            {
                timer += Time.deltaTime;
                targetText.color = Color.Lerp(startColor, targetColor, timer / duration);

                yield return null;
            }

            targetText.color = targetColor;
        }
        #endregion
        #endregion

        #region Pointer Event Handler
        public void OnPointerEnter(PointerEventData eventData)
        {
            isCursorOverButton = true;

            UpdateState();
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            isCursorOverButton = false;

            UpdateState();
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            isButtonPressed = true;

            UpdateState();
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            isButtonPressed = false;

            if (!interactable)
                return;

            if (isCursorOverButton)
                OnClick?.Invoke();

            UpdateState();
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

        #region Reset
        protected virtual void Reset()
        {
            interactable = true;

            Reset_TryGetExternalComponents();
            Reset_FillVisualDataDict();
        }

        private void Reset_TryGetExternalComponents()
        {
            targetImage = GetComponent<Image>();
            targetText = GetComponentInChildren<TMP_Text>();
            targetAnimator = GetComponent<Animator>();
        }

        private void Reset_FillVisualDataDict()
        {
            visualDataDict = new();

            ButtonState[] visualStates = new ButtonState[4]
            {
                ButtonState.Normal,
                ButtonState.Highlighted,
                ButtonState.Pressed,
                ButtonState.Disabled
            };

            foreach (ButtonState state in visualStates)
                visualDataDict.Add(state, Reset_GetNewVisualData(state));
        }

        private ButtonVisualData Reset_GetNewVisualData(ButtonState state)
        {
            string animTrigger = state.ToString();

            return new ButtonVisualData(animTrigger);
        }
        #endregion
    }
}
