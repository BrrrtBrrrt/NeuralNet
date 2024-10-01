using UnityEngine;

namespace Assets.Scripts.Utils
{
    /// <summary>
    /// Utility class for various target functions.
    /// </summary>
    public static class TargetFunctionUtil
    {
        /// <summary>
        /// Calculates the exponential function f(x) = x^2.
        /// </summary>
        /// <param name="x">The input value.</param>
        /// <returns>The result of the function f(x) = x^2.</returns>
        public static float Exponential(float x)
        {
            return Mathf.Pow(x, 2);
        }

        /// <summary>
        /// Calculates the linear function f(x) = 1.4 * x + 1.
        /// </summary>
        /// <param name="x">The input value.</param>
        /// <returns>The result of the function f(x) = 1.4 * x + 1.</returns>
        public static float Linear(float x)
        {
            return 1.4f * x + 1;
        }

        /// <summary>
        /// Calculates the sine function f(x) = sin(x).
        /// </summary>
        /// <param name="x">The input value.</param>
        /// <returns>The result of the function f(x) = sin(x).</returns>
        public static float Sin(float x)
        {
            return Mathf.Sin(x);
        }

        public static float CustomStep(float x)
        {
            return Mathf.Abs(((x - 0.5f) / 3f) - Mathf.Floor(x / 3f));
        }
    }
}
