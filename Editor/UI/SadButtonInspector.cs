using SadUtils.UI;
using SadUtils.UI.Types;
using System.Collections.Generic;
using UnityEditor;

namespace SadUtils.Editor
{
    [CustomEditor(typeof(SadButton))]
    public class SadButtonInspector : UnityEditor.Editor
    {
        // --- Property name constants ---
        private const string INTERACTABLE_PROPERTY_NAME = "interactable";
        private const string FROZEN_PROPERTY_NAME = "frozen";

        private const string TRANSITIONS_PROPERTY_NAME = "transitions";

        private const string TARGET_IMAGE_PROPERTY_NAME = "targetImage";
        private const string TARGET_ANIMATOR_PROPERTY_NAME = "targetAnimator";
        private const string TARGET_TEXT_PROPERTY_NAME = "targetText";

        private const string VISUAL_DATA_DICT_PROPERTY_NAME = "visualDataDict";

        private const string ON_CLICK_PROPERTY_NAME = "OnClick";

        // --- Vars ---
        private SerializedProperty interactableProperty;
        private SerializedProperty frozenProperty;

        private SerializedProperty transitionsProperty;

        private SerializedProperty targetImageProperty;
        private SerializedProperty targetAnimatorProperty;
        private SerializedProperty targetTextProperty;

        private SerializedProperty visualDataDictProperty;

        private SerializedProperty onClickProperty;

        private SadButton buttonReference;

        #region Cache Data
        private void OnEnable()
        {
            buttonReference = target as SadButton;

            interactableProperty = serializedObject.FindProperty(INTERACTABLE_PROPERTY_NAME);
            frozenProperty = serializedObject.FindProperty(FROZEN_PROPERTY_NAME);

            transitionsProperty = serializedObject.FindProperty(TRANSITIONS_PROPERTY_NAME);

            targetImageProperty = serializedObject.FindProperty(TARGET_IMAGE_PROPERTY_NAME);
            targetAnimatorProperty = serializedObject.FindProperty(TARGET_ANIMATOR_PROPERTY_NAME);
            targetTextProperty = serializedObject.FindProperty(TARGET_TEXT_PROPERTY_NAME);

            visualDataDictProperty = serializedObject.FindProperty(VISUAL_DATA_DICT_PROPERTY_NAME);

            onClickProperty = serializedObject.FindProperty(ON_CLICK_PROPERTY_NAME);
        }
        #endregion

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            DrawFields();

            serializedObject.ApplyModifiedProperties();
        }

        #region Draw Fields
        private void DrawFields()
        {
            DrawInteractableFields();
            DrawTransitionsField();
            DrawTargetFields();
            DrawVisualData();
            DrawEvents();
        }

        private void DrawInteractableFields()
        {
            EditorGUILayout.PropertyField(interactableProperty);
            EditorGUILayout.PropertyField(frozenProperty);
        }

        private void DrawTransitionsField()
        {
            EditorGUILayout.PropertyField(transitionsProperty);
        }

        private void DrawTargetFields()
        {
            Queue<SerializedProperty> visibleProperties = new();

            if (HasTransition(ButtonTransition.ColorTint) ||
                HasTransition(ButtonTransition.SpriteSwap))
                visibleProperties.Enqueue(targetImageProperty);

            if (HasTransition(ButtonTransition.Animation))
                visibleProperties.Enqueue(targetAnimatorProperty);

            if (HasTransition(ButtonTransition.TextColorTint) ||
                HasTransition(ButtonTransition.TextSwap))
                visibleProperties.Enqueue(targetTextProperty);

            if (visibleProperties.Count > 0)
                DrawHeader("Target References");

            while (visibleProperties.Count > 0)
                EditorGUILayout.PropertyField(visibleProperties.Dequeue());
        }

        private void DrawVisualData()
        {
            if (!buttonReference.IsAnyTransitionEnabled())
                return;

            DrawHeader("Transition Visuals");

            // somehow try to draw visual data in a neat way
            // (preferably through the serialized property)

            // see discussion about it here: https://discussions.unity.com/t/using-serializedproperty-on-custom-classes/45154/2
        }

        #region Draw Visual Data

        #endregion

        private void DrawEvents()
        {
            EditorGUILayout.PropertyField(onClickProperty);
        }
        #endregion

        #region Util
        private bool HasTransition(ButtonTransition transition)
        {
            return buttonReference.IsTransitionEnabled(transition);
        }

        private void DrawHeader(string label)
        {
            EditorGUILayout.Space(EditorGUIUtility.singleLineHeight);
            EditorGUILayout.LabelField(label, EditorStyles.boldLabel);
        }
        #endregion
    }
}
