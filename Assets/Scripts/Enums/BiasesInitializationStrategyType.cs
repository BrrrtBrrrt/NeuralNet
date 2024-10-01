using UnityEngine;

namespace Assets.Scripts.Enums
{
    /// <summary>
    /// Enum representing different strategies for initializing biases in a neural network.
    /// </summary>
    public enum BiasesInitializationStrategyType
    {
        /// <summary>
        /// Initialize biases with a constant value of 1.
        /// </summary>
        [InspectorName("Constant 1")]
        VALUE1,

        /// <summary>
        /// Initialize biases with a constant value of 0.01.
        /// </summary>
        [InspectorName("Constant 0.01")]
        VALUE0D01,

        /// <summary>
        /// Initialize biases with a constant value of 0.
        /// </summary>
        [InspectorName("Constant 0")]
        VALUE0,
    }
}