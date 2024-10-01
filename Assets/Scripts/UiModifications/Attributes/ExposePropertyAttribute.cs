using System;

namespace Assets.Scripts.UiModifications.Attributes
{
#if UNITY_EDITOR
    /// <summary>
    /// Attribute used to mark properties that should be exposed and editable in a custom Unity Inspector.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class ExposePropertyAttribute : Attribute
    {

    }
#endif
}
