using UnityEngine;

namespace Assets.Scripts.Enums
{
    /// <summary>
    /// Enum representing different strategies for initializing weights in neural networks.
    /// </summary>
    public enum WeightsInitializationStrategyType
    {
        /// <summary>
        /// Initialize weights randomly.
        /// </summary>
        [InspectorName("Random")]
        RANDOM,

        /// <summary>
        /// Initialize weights using a normal distribution.
        /// </summary>
        [InspectorName("Normal")]
        NORMAL,

        /// <summary>
        /// Initialize weights using a uniform distribution according to the Xavier method.
        /// </summary>
        [InspectorName("Uniform Xavier")]
        XAVIER_UNIFORM,

        /// <summary>
        /// Initialize weights using a normal distribution according to the Xavier method.
        /// </summary>
        [InspectorName("Normal Xavier")]
        XAVIER_NORMAL,
    }
}
