using UnityEngine;

namespace Assets.Scripts.UiModifications.PropertyAttributes
{
#if UNITY_EDITOR
    /// <summary>
    /// Attribute to mark a property as read-only in the Unity Inspector.
    /// </summary>
    public class ReadOnlyPropertyAttribute : PropertyAttribute
    {
        // No additional implementation is required.
        // This class is used solely for marking properties in the Inspector as read-only.
    }
#endif
}
