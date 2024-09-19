using SadUtils.Types;
using UnityEditor;
using UnityEngine;

namespace SadUtils.Editor
{
    [CustomPropertyDrawer(typeof(UnityDictionary<,>))]
    public class UnityDictionaryDrawer : PropertyDrawer
    {
        private const string DICTIONARY_PROPERTY = "dictionary";

        private SerializedProperty dictionaryProperty;

        private bool cached;

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            if (!cached)
                dictionaryProperty = property.FindPropertyRelative(DICTIONARY_PROPERTY);

            return EditorGUI.GetPropertyHeight(dictionaryProperty, label, true);
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (!cached)
                dictionaryProperty = property.FindPropertyRelative(DICTIONARY_PROPERTY);

            EditorGUI.BeginProperty(position, label, property);

            EditorGUI.PropertyField(position, dictionaryProperty, label, true);

            EditorGUI.EndProperty();
        }
    }
}
