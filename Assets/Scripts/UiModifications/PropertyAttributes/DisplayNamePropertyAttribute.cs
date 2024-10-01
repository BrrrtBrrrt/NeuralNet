using UnityEngine;

namespace Assets.Scripts.UiModifications.PropertyAttributes
{
#if UNITY_EDITOR
    /// <summary>
    /// Attribute to specify a custom display name for a property in the Unity Inspector.
    /// </summary>
    public class DisplayNamePropertyAttribute : PropertyAttribute
    {
        /// <summary>
        /// Gets the custom display name for the property.
        /// </summary>
        public string NewName { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="DisplayNamePropertyAttribute"/> class with a specified display name.
        /// </summary>
        /// <param name="name">The custom display name for the property.</param>
        public DisplayNamePropertyAttribute(string name)
        {
            NewName = name;
        }
    }
#endif
}
