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

        private const string INTERNAL_DICT_LIST_PROPERTY_NAME = "dictionary";
        private const string DICT_KEY_PROPERTY_NAME = "key";
        private const string DICT_VALUE_PROPERTY_NAME = "value";

        private const string VISUAL_DATA_COLOR_PROPERTY_NAME = "color";
        private const string VISUAL_DATA_COLOR_TRANSITION_PROPERTY_NAME = "colorTransitionDuration";
        private const string VISUAL_DATA_SPRITE_PROPERTY_NAME = "sprite";
        private const string VISUAL_DATA_TRANSITION_TRIGGER_PROPERTY_NAME = "transitionTrigger";
        private const string VISUAL_DATA_TEXT_PROPERTY_NAME = "text";
        private const string VISUAL_DATA_TEXT_COLOR_PROPERTY_NAME = "textColor";
        private const string VISUAL_DATA_TEXT_COLOR_TRANSITION_PROPERTY_NAME = "textColorTransitionDuration";

        private const string ON_CLICK_PROPERTY_NAME = "OnClick";

        // --- Vars ---
        private SerializedProperty interactableProperty;
        private SerializedProperty frozenProperty;

        private SerializedProperty transitionsProperty;

        private SerializedProperty targetImageProperty;
        private SerializedProperty targetAnimatorProperty;
        private SerializedProperty targetTextProperty;

        private SerializedProperty visualDataDictListProperty;

        private SerializedProperty onClickProperty;

        private SadButton buttonReference;

        private static bool[] foldOutStates;

        #region Cache Data
        private void OnEnable()
        {
            CacheButtonReference();

            CachePropertyFields();

            InitFoldOutStates();
        }

        private void CacheButtonReference()
        {
            buttonReference = target as SadButton;
        }

        private void CachePropertyFields()
        {
            interactableProperty = serializedObject.FindProperty(INTERACTABLE_PROPERTY_NAME);
            frozenProperty = serializedObject.FindProperty(FROZEN_PROPERTY_NAME);

            transitionsProperty = serializedObject.FindProperty(TRANSITIONS_PROPERTY_NAME);

            targetImageProperty = serializedObject.FindProperty(TARGET_IMAGE_PROPERTY_NAME);
            targetAnimatorProperty = serializedObject.FindProperty(TARGET_ANIMATOR_PROPERTY_NAME);
            targetTextProperty = serializedObject.FindProperty(TARGET_TEXT_PROPERTY_NAME);

            SerializedProperty visualDataDictProperty = serializedObject.FindProperty(VISUAL_DATA_DICT_PROPERTY_NAME);
            visualDataDictListProperty = visualDataDictProperty.FindPropertyRelative(INTERNAL_DICT_LIST_PROPERTY_NAME);

            onClickProperty = serializedObject.FindProperty(ON_CLICK_PROPERTY_NAME);
        }

        private void InitFoldOutStates()
        {
            if (foldOutStates != null)
                return;

            int foldOutCount = visualDataDictListProperty.arraySize;
            foldOutStates = new bool[foldOutCount];
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

        #region Draw Visual Data
        private void DrawVisualData()
        {
            if (!buttonReference.IsAnyTransitionEnabled())
                return;

            DrawHeader("Transition Visual Settings");

            int visualDictEntries = visualDataDictListProperty.arraySize;
            for (int i = 0; i < visualDictEntries; i++)
                DrawVisualDataEntryProperty(visualDataDictListProperty.GetArrayElementAtIndex(i), i);
        }

        private void DrawVisualDataEntryProperty(SerializedProperty visualDataEntryProperty, int entryIndex)
        {
            string foldOutName = GetFoldOutName(visualDataEntryProperty);

            foldOutStates[entryIndex] = EditorGUILayout.Foldout(foldOutStates[entryIndex], foldOutName, true);

            if (foldOutStates[entryIndex])
                DrawVisualDataStruct(visualDataEntryProperty);
        }

        private string GetFoldOutName(SerializedProperty visualDataEntryProperty)
        {
            SerializedProperty keyProperty = visualDataEntryProperty.FindPropertyRelative(DICT_KEY_PROPERTY_NAME);
            SadButton.ButtonState state = (SadButton.ButtonState)keyProperty.enumValueIndex;

            return state.ToString();
        }

        private void DrawVisualDataStruct(SerializedProperty visualDataEntryProperty)
        {
            SerializedProperty structProperty = visualDataEntryProperty.FindPropertyRelative(DICT_VALUE_PROPERTY_NAME);

            if (HasTransition(ButtonTransition.ColorTint))
                DrawVisualDataColorTintFields(structProperty);

            if (HasTransition(ButtonTransition.SpriteSwap))
                DrawVisualDataSpriteField(structProperty);

            if (HasTransition(ButtonTransition.Animation))
                DrawVisualDataAnimationField(structProperty);

            if (HasTransition(ButtonTransition.TextSwap))
                DrawVisualDataTextField(structProperty);

            if (HasTransition(ButtonTransition.TextColorTint))
                DrawVisualDataTextColorTintFields(structProperty);
        }

        private void DrawVisualDataColorTintFields(SerializedProperty structProperty)
        {
            SerializedProperty colorProperty = structProperty.FindPropertyRelative(VISUAL_DATA_COLOR_PROPERTY_NAME);
            SerializedProperty colorTransitionProperty = structProperty.FindPropertyRelative(VISUAL_DATA_COLOR_TRANSITION_PROPERTY_NAME);

            EditorGUILayout.PropertyField(colorProperty);
            EditorGUILayout.PropertyField(colorTransitionProperty);
        }

        private void DrawVisualDataSpriteField(SerializedProperty structProperty)
        {
            SerializedProperty spriteProperty = structProperty.FindPropertyRelative(VISUAL_DATA_SPRITE_PROPERTY_NAME);

            EditorGUILayout.PropertyField(spriteProperty);
        }

        private void DrawVisualDataAnimationField(SerializedProperty structProperty)
        {
            SerializedProperty triggerProperty = structProperty.FindPropertyRelative(VISUAL_DATA_TRANSITION_TRIGGER_PROPERTY_NAME);

            EditorGUILayout.PropertyField(triggerProperty);
        }

        private void DrawVisualDataTextField(SerializedProperty structProperty)
        {
            SerializedProperty textProperty = structProperty.FindPropertyRelative(VISUAL_DATA_TEXT_PROPERTY_NAME);

            EditorGUILayout.PropertyField(textProperty);
        }

        private void DrawVisualDataTextColorTintFields(SerializedProperty structProperty)
        {
            SerializedProperty textColorProperty = structProperty.FindPropertyRelative(VISUAL_DATA_TEXT_COLOR_PROPERTY_NAME);
            SerializedProperty textColorTransitionProperty = structProperty.FindPropertyRelative(VISUAL_DATA_TEXT_COLOR_TRANSITION_PROPERTY_NAME);

            EditorGUILayout.PropertyField(textColorProperty);
            EditorGUILayout.PropertyField(textColorTransitionProperty);
        }
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
