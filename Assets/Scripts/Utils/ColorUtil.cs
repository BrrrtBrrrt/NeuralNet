using UnityEngine;

namespace Assets.Scripts.Utils
{
    /// <summary>
    /// Utility class for color operations.
    /// </summary>
    public static class ColorUtil
    {
        /// <summary>
        /// Interpolates between two colors based on a value within a specified range.
        /// </summary>
        /// <param name="color1">The starting color.</param>
        /// <param name="color2">The ending color.</param>
        /// <param name="minValue">The minimum value of the range.</param>
        /// <param name="maxValue">The maximum value of the range.</param>
        /// <param name="value">The value within the range to interpolate at.</param>
        /// <returns>The interpolated color.</returns>
        public static Color Interpolate(Color color1, Color color2, float minValue, float maxValue, float value)
        {
            value = (value - minValue) / (maxValue - minValue);

            // Interpolate each color component (ARGB)
            float a = color1.a + (color2.a - color1.a) * value;
            float r = color1.r + (color2.r - color1.r) * value;
            float g = color1.g + (color2.g - color1.g) * value;
            float b = color1.b + (color2.b - color1.b) * value;
            return new Color(r, g, b, a);
        }
    }
}
