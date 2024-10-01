using System;
using UnityEngine;

namespace Assets.Scripts.UiModifications.PropertyAttributes
{
#if UNITY_EDITOR
    /// <summary>
    /// Attribute to conditionally hide or disable a property in the Unity Inspector based on the value of another property.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property |
    AttributeTargets.Class | AttributeTargets.Struct, Inherited = true)]
    public class ConditionalHidePropertyAttribute : PropertyAttribute
    {
        /// <summary>
        /// The name of the boolean field that controls the visibility of the property.
        /// </summary>
        public string ConditionalSourceField = string.Empty;
        /// <summary>
        /// Determines whether the property should be hidden or disabled in the Inspector.
        /// <list type="bullet">
        /// <item><description>True: Hide the property in the Inspector.</description></item>
        /// <item><description>False: Disable the property in the Inspector.</description></item>
        /// </list>
        /// </summary>
        public bool HideInInspector = false;

        /// <summary>
        /// Initializes a new instance of the <see cref="ConditionalHidePropertyAttribute"/> class with a specified
        /// conditional source field and default visibility behavior (not hidden).
        /// </summary>
        /// <param name="conditionalSourceField">The name of the boolean field that controls the visibility.</param>
        public ConditionalHidePropertyAttribute(string conditionalSourceField)
        {
            ConditionalSourceField = conditionalSourceField;
            HideInInspector = false;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ConditionalHidePropertyAttribute"/> class with a specified
        /// conditional source field and visibility behavior.
        /// </summary>
        /// <param name="conditionalSourceField">The name of the boolean field that controls the visibility.</param>
        /// <param name="hideInInspector">True to hide the property, false to disable it.</param>
        public ConditionalHidePropertyAttribute(string conditionalSourceField, bool hideInInspector)
        {
            ConditionalSourceField = conditionalSourceField;
            HideInInspector = hideInInspector;
        }
    }
#endif
}
