using UnityEngine;

namespace Assets.Scripts.Enums
{
    /// <summary>
    /// Enum representing various types of activation functions used in neural networks.
    /// </summary>
    public enum ActivationFunctionType
    {
        /// <summary>
        /// Rectified Linear Unit (ReLU) activation function.
        /// </summary>
        [InspectorName("ReLU")]
        RE_LU,

        /// <summary>
        /// Leaky Rectified Linear Unit (Leaky ReLU) activation function.
        /// </summary>
        [InspectorName("LReLU")]
        LRE_LU,

        /// <summary>
        /// Sigmoid activation function.
        /// </summary>
        [InspectorName("Sigmoid")]
        SIGMOID,

        /// <summary>
        /// Hyperbolic Tangent (Tanh) activation function.
        /// </summary>
        [InspectorName("Tanh")]
        TAN_H,

        /// <summary>
        /// Linear activation function.
        /// </summary>
        [InspectorName("Linear")]
        LINEAR,
    }
}