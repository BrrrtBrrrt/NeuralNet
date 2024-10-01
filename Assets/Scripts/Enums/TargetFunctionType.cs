using UnityEngine;

namespace Assets.Scripts.Enums
{
    /// <summary>
    /// Enum representing different types of target functions that can be used in neural network models or simulations.
    /// </summary>
    public enum TargetFunctionType
    {
        /// <summary>
        /// Represents an exponential function.
        /// </summary>
        [InspectorName("Exponential")]
        EXPONENTIAL,

        /// <summary>
        /// Represents a linear function.
        /// </summary>
        [InspectorName("Linear")]
        LINEAR,

        /// <summary>
        /// Represents a sine wave function.
        /// </summary>
        [InspectorName("Sine wave")]
        SIN,

        [InspectorName("Custom step")]
        CUSTOM_STEP,
    }
}