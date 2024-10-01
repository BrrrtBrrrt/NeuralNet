using UnityEngine;

namespace Assets.Scripts.Utils
{
    /// <summary>
    /// Utility class for mathematical operations.
    /// </summary>
    internal static class MathUtil
    {
        /// <summary>
        /// Generates Gaussian noise using the Box-Muller transform.
        /// </summary>
        /// <returns>A float representing Gaussian noise with mean 0 and standard deviation 1.</returns>
        public static float GenerateGaussianNoise()
        {
            // Using Box-Muller transform to generate Gaussian noise
            float u1 = 1.0f - Random.value; // Uniform(0,1] random floats
            float u2 = 1.0f - Random.value;
            float randStdNormal = Mathf.Sqrt(-2.0f * Mathf.Log(u1)) * Mathf.Sin(2.0f * Mathf.PI * u2); // Random normal (0,1)
            return randStdNormal;
        }
    }
}
