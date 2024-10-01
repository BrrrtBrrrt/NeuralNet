using Assets.Scripts.UiModifications.PropertyAttributes;
using UnityEditor;
using UnityEngine;

namespace Assets.Scripts.UiModifications.PropertyDrawers
{
#if UNITY_EDITOR
    /// <summary>
    /// Custom property drawer for the <see cref="ReadOnlyPropertyAttribute"/>.
    /// </summary>
    [CustomPropertyDrawer(typeof(ReadOnlyPropertyAttribute))]
    public class ReadOnlyPropertyDrawer : PropertyDrawer
    {
        /// <summary>
        /// Calculates the height of the property field in the Inspector.
        /// </summary>
        /// <param name="property">The serialized property to draw.</param>
        /// <param name="label">The label of the property.</param>
        /// <returns>The height of the property field.</returns>
        public override float GetPropertyHeight(SerializedProperty property,
                                                GUIContent label)
        {
            // Return the height of the property field including any additional space needed for its content
            return EditorGUI.GetPropertyHeight(property, label, true);
        }

        /// <summary>
        /// Draws the property field in the Inspector.
        /// </summary>
        /// <param name="position">The position and size of the property field in the Inspector.</param>
        /// <param name="property">The serialized property to draw.</param>
        /// <param name="label">The label of the property.</param>
        public override void OnGUI(Rect position,
                                   SerializedProperty property,
                                   GUIContent label)
        {
            // Disable GUI interaction to make the field read-only
            GUI.enabled = false;
            // Draw the property field with its label
            EditorGUI.PropertyField(position, property, label, true);
            // Re-enable GUI interaction for other elements
            GUI.enabled = true;
        }
    }
#endif
}
