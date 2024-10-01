using Assets.Scripts.UiModifications.PropertyAttributes;
using UnityEditor;
using UnityEngine;

namespace Assets.Scripts.UiModifications.PropertyDrawers
{
#if UNITY_EDITOR
    /// <summary>
    /// Custom property drawer for the <see cref="DisplayNamePropertyAttribute"/>.
    /// </summary>
    [CustomPropertyDrawer(typeof(DisplayNamePropertyAttribute))]
    public class DisplayNamePropertyDrawer : PropertyDrawer
    {
        /// <summary>
        /// Draws the property field in the Inspector with a custom display name.
        /// </summary>
        /// <param name="position">The position and size of the property field in the Inspector.</param>
        /// <param name="property">The serialized property to draw.</param>
        /// <param name="label">The label of the property.</param>
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            // Retrieve the DisplayNamePropertyAttribute from the PropertyDrawer's attribute field
            DisplayNamePropertyAttribute displayNameAttribute = (DisplayNamePropertyAttribute)attribute;

            // Draw the property field with the new display name
            EditorGUI.PropertyField(position, property, new GUIContent(displayNameAttribute.NewName));
        }
    }
#endif
}
