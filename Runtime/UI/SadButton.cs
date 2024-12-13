using System;
using System.Collections;
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

        [Header("Interactable Settings")]
        [SerializeField] private bool interactable;

        [Tooltip("Freezing a button prevents state updates")]
        [SerializeField] private bool frozen;

        [Header("Transition Settings")]
        [SerializeField] private ButtonTransition transitions;

        // Only used when ColorTint or SpriteSwap is enabled.
        [SerializeField] private Image targetImage;
        // Only used when Animation is enabled.
        [SerializeField] private Animator targetAnimator;
        // Only used when TextSwap or TextColorTint is enabled.
        [SerializeField] private TMP_Text targetText;

        [Space]
        [SerializeField] private UnityDictionary<ButtonState, ButtonVisualData> visualDataDict;

        [Header("Events")]
        [SerializeField] private UnityEvent onClick;
        public event Action OnClick;

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
            InitializeVisualData();
            InitializeOnClick();

            TryFetchImage();
            TryFetchAnimator();
            TryFetchText();

            SetState(DetermineCurrentState(), true);
        }

        private void InitializeVisualData()
        {
            if (!IsTransitionEnabled(ButtonTransition.Animation))
                return;

            foreach (ButtonVisualData stateVisualData in visualDataDict.Values)
                stateVisualData.CalculateTriggerHash();
        }

        private void InitializeOnClick()
        {
            onClick.AddListener(() => OnClick?.Invoke());
        }

        private void TryFetchImage()
        {
            if (!IsTransitionEnabled(ButtonTransition.ColorTint) &&
                !IsTransitionEnabled(ButtonTransition.SpriteSwap))
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
            if (!IsTransitionEnabled(ButtonTransition.Animation))
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
            if (!IsTransitionEnabled(ButtonTransition.TextSwap) &&
                !IsTransitionEnabled(ButtonTransition.TextColorTint))
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

        public void SetFrozen(bool isFrozen)
        {
            frozen = isFrozen;

            if (!frozen)
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
            if (IsTransitionEnabled(ButtonTransition.ColorTint))
                StartColorTintTransition(visualData);
            if (IsTransitionEnabled(ButtonTransition.SpriteSwap))
                SetSprite(visualData);
            if (IsTransitionEnabled(ButtonTransition.Animation))
                TriggerAnimation(visualData);
            if (IsTransitionEnabled(ButtonTransition.TextSwap))
                SetText(visualData);
            if (IsTransitionEnabled(ButtonTransition.TextColorTint))
                StartTextColorTintTransition(visualData);
        }

        private void StartColorTintTransition(ButtonVisualData visualData)
        {
            if (colorTransitionRoutine != null)
                StopCoroutine(colorTransitionRoutine);

            if (gameObject.activeInHierarchy)
                colorTransitionRoutine = StartCoroutine(
                    TransitionColorTintCo(
                        visualData.color,
                        visualData.colorTransitionDuration,
                        visualData.ignoreTimeScale));

            else
                targetImage.color = visualData.color;
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

            if (gameObject.activeInHierarchy)
                textColorTransitionRoutine = StartCoroutine(
                    TransitionTextColorTintCo(
                        visualData.textColor,
                        visualData.textColorTransitionDuration,
                        visualData.ignoreTimeScale));

            else
                targetText.color = visualData.textColor;
        }

        #region Color Transition Loops
        private IEnumerator TransitionColorTintCo(Color targetColor, float duration, bool ignoreTimeScale)
        {
            Color startColor = targetImage.color;
            float timer = 0;

            while (timer < duration)
            {
                timer += GetDeltaTime(ignoreTimeScale);
                targetImage.color = Color.Lerp(startColor, targetColor, timer / duration);

                yield return null;
            }

            targetImage.color = targetColor;
        }

        private IEnumerator TransitionTextColorTintCo(Color targetColor, float duration, bool ignoreTimeScale)
        {
            Color startColor = targetText.color;
            float timer = 0;

            while (timer < duration)
            {
                timer += GetDeltaTime(ignoreTimeScale);
                targetText.color = Color.Lerp(startColor, targetColor, timer / duration);

                yield return null;
            }

            targetText.color = targetColor;
        }

        private float GetDeltaTime(bool ignoreTimeScale)
        {
            return ignoreTimeScale ? Time.unscaledDeltaTime : Time.deltaTime;
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
                onClick?.Invoke();

            UpdateState();
        }
        #endregion

        #region Util
        /// <summary>
        /// Determines if the bit for the given <paramref name="transitionType"/> is enabled through the inspector.
        /// </summary>
        /// <param name="transitionType">transition to check for.</param>
        public bool IsTransitionEnabled(ButtonTransition transitionType)
        {
            return (transitions & transitionType) != 0;
        }

        public bool IsAnyTransitionEnabled()
        {
            return transitions != 0;
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

            ButtonState[] visualStates = new ButtonState[5]
            {
                ButtonState.Normal,
                ButtonState.Highlighted,
                ButtonState.Pressed,
                ButtonState.Selected,
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
