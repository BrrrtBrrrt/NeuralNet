using UnityEngine;

namespace Assets.Scripts.Utils
{
    /// <summary>
    /// Utility class for weight initialization methods.
    /// </summary>
    public static class WeightInitializerUtil
    {
        /// <summary>
        /// Initializes a weight with a random value between 0 and 1.
        /// </summary>
        /// <returns>A random float value between 0 and 1.</returns>
        public static float RandomInit()
        {
            float value = Random.Range(0f, 1f);
            return value;
        }

        /// <summary>
        /// Initializes a weight with a value following a normal distribution.
        /// </summary>
        /// <returns>A float value following a normal distribution.</returns>
        public static float NormalInit()
        {
            float value = Mathf.Exp(Mathf.Pow(-Random.Range(0f, 1f), 2));
            return value;
        }

        /// <summary>
        /// Initializes a weight using the Xavier/Glorot uniform initialization method.
        /// </summary>
        /// <param name="previousLayerSize">The size of the previous layer.</param>
        /// <param name="nextLayerSize">The size of the next layer.</param>
        /// <returns>A float value initialized using the Xavier/Glorot uniform method.</returns>
        public static float XavierUniformInit(int previousLayerSize, int nextLayerSize)
        {
            float range = Mathf.Sqrt(6f / (previousLayerSize + nextLayerSize));
            float value = Random.Range(-range, range);
            return value;
        }

        /// <summary>
        /// Initializes a weight using the Xavier/Glorot normal initialization method.
        /// </summary>
        /// <param name="previousLayerSize">The size of the previous layer.</param>
        /// <param name="nextLayerSize">The size of the next layer.</param>
        /// <returns>A float value initialized using the Xavier/Glorot normal method.</returns>
        public static float XavierNormalInit(int previousLayerSize, int nextLayerSize)
        {
            float range = Mathf.Sqrt(2f / (previousLayerSize + nextLayerSize));
            float value = Random.Range(-range, range);
            return value;
        }
    }
}
