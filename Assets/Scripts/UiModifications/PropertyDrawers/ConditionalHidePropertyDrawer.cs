using Assets.Scripts.UiModifications.PropertyAttributes;
using UnityEditor;
using UnityEngine;

namespace Assets.Scripts.UiModifications.PropertyDrawers
{
#if UNITY_EDITOR
    /// <summary>
    /// Custom property drawer for the <see cref="ConditionalHidePropertyAttribute"/>.
    /// This drawer conditionally hides or shows a property in the Unity Inspector based on the value of another property.
    /// </summary>
    [CustomPropertyDrawer(typeof(ConditionalHidePropertyAttribute))]
    public class ConditionalHidePropertyDrawer : PropertyDrawer
    {
        /// <summary>
        /// Draws the property field in the Inspector with conditional visibility based on the value of a source property.
        /// </summary>
        /// <param name="position">The position and size of the property field in the Inspector.</param>
        /// <param name="property">The serialized property to draw.</param>
        /// <param name="label">The label of the property.</param>
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            //get the attribute data
            ConditionalHidePropertyAttribute condHAtt = (ConditionalHidePropertyAttribute)attribute;
            //check if the propery we want to draw should be enabled
            bool enabled = GetConditionalHideAttributeResult(condHAtt, property);

            //Enable/disable the property
            bool wasEnabled = GUI.enabled;
            GUI.enabled = enabled;

            //Check if we should draw the property
            if (!condHAtt.HideInInspector || enabled)
            {
                EditorGUI.PropertyField(position, property, label, true);
            }

            //Ensure that the next property that is being drawn uses the correct settings
            GUI.enabled = wasEnabled;
        }

        /// <summary>
        /// Determines whether the property should be enabled based on the value of a source property.
        /// </summary>
        /// <param name="condHAtt">The attribute containing the conditional settings.</param>
        /// <param name="property">The serialized property to check against the condition.</param>
        /// <returns>True if the property should be enabled; otherwise, false.</returns>
        private bool GetConditionalHideAttributeResult(ConditionalHidePropertyAttribute condHAtt, SerializedProperty property)
        {
            bool enabled = true;
            //Look for the sourcefield within the object that the property belongs to
            string propertyPath = property.propertyPath; //returns the property path of the property we want to apply the attribute to
            string conditionPath = propertyPath.Replace(property.name, condHAtt.ConditionalSourceField); //changes the path to the conditionalsource property path
            SerializedProperty sourcePropertyValue = property.serializedObject.FindProperty(conditionPath);

            if (sourcePropertyValue != null)
            {
                enabled = sourcePropertyValue.boolValue;
            }
            else
            {
                Debug.LogWarning("Attempting to use a ConditionalHideAttribute but no matching SourcePropertyValue found in object: " + condHAtt.ConditionalSourceField);
            }

            return enabled;
        }

        /// <summary>
        /// Returns the height of the property field in the Inspector, adjusting based on conditional visibility.
        /// </summary>
        /// <param name="property">The serialized property to measure.</param>
        /// <param name="label">The label of the property.</param>
        /// <returns>The height of the property field.</returns>
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            ConditionalHidePropertyAttribute condHAtt = (ConditionalHidePropertyAttribute)attribute;
            bool enabled = GetConditionalHideAttributeResult(condHAtt, property);

            if (!condHAtt.HideInInspector || enabled)
            {
                return EditorGUI.GetPropertyHeight(property, label);
            }
            else
            {
                //The property is not being drawn
                //We want to undo the spacing added before and after the property
                return -EditorGUIUtility.standardVerticalSpacing;
            }
        }
    }
#endif
}
