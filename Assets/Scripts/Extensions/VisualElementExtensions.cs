using System;
using UnityEngine.UIElements;

namespace Assets.Scripts.Extensions
{
    public static class VisualElementExtensions
    {
        public static VisualElement FirstRek(this VisualElement element, Func<VisualElement, bool> predicate)
        {
            foreach (VisualElement child in element.Children())
            {
                if (predicate(child))
                    return child;
            }

            foreach (VisualElement child in element.Children())
            {
                VisualElement result = child.FirstRek(predicate);
                if (result != null) return result;
            }

            return null;
        }
    }
}
