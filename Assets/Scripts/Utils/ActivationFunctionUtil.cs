using UnityEngine;

namespace Assets.Scripts.Utils
{
    /// <summary>
    /// Utility class for various activation functions and their derivatives.
    /// </summary>
    public static class ActivationFunctionUtil
    {
        /// <summary>
        /// Rectified Linear Unit (ReLU) activation function.
        /// </summary>
        /// <param name="value">The input value.</param>
        /// <returns>The output of the ReLU function.</returns>
        public static float ReLU(float value)
        {
            return value < 0 ? 0 : value;
        }

        /// <summary>
        /// Derivative of the Rectified Linear Unit (ReLU) activation function.
        /// </summary>
        /// <param name="value">The input value.</param>
        /// <returns>The derivative of the ReLU function.</returns>
        public static float ReLUDerivative(float value)
        {
            return value < 0 ? 0 : 1;
        }

        /// <summary>
        /// Leaky Rectified Linear Unit (LReLU) activation function.
        /// </summary>
        /// <param name="value">The input value.</param>
        /// <param name="alpha">The slope of the function for negative input values.</param>
        /// <returns>The output of the LReLU function.</returns>
        public static float LReLU(float value, float alpha)
        {
            return value < 0 ? value * alpha : value;
        }

        /// <summary>
        /// Derivative of the Leaky Rectified Linear Unit (LReLU) activation function.
        /// </summary>
        /// <param name="value">The input value.</param>
        /// <param name="alpha">The slope of the function for negative input values.</param>
        /// <returns>The derivative of the LReLU function.</returns>
        public static float LReLUDerivative(float value, float alpha)
        {
            return value < 0 ? alpha : 1;
        }

        /// <summary>
        /// Sigmoid activation function.
        /// </summary>
        /// <param name="value">The input value.</param>
        /// <returns>The output of the Sigmoid function.</returns>
        public static float Sigmoid(float value)
        {
            return 1f / (1f + Mathf.Exp(-value));
        }

        /// <summary>
        /// Derivative of the Sigmoid activation function.
        /// </summary>
        /// <param name="value">The input value.</param>
        /// <returns>The derivative of the Sigmoid function.</returns>
        public static float SigmoidDerivative(float value)
        {
            float result = Sigmoid(value);
            return result * (1f - result);
        }

        /// <summary>
        /// Hyperbolic Tangent (TanH) activation function.
        /// </summary>
        /// <param name="value">The input value.</param>
        /// <returns>The output of the TanH function.</returns>
        public static float TanH(float value)
        {
            float test = Mathf.Exp(value);
            float test2 = Mathf.Exp(-value);
            float test3 = (test - test2) / (test + test2);


            return test3;
        }

        /// <summary>
        /// Derivative of the Hyperbolic Tangent (TanH) activation function.
        /// </summary>
        /// <param name="value">The input value.</param>
        /// <returns>The derivative of the TanH function.</returns>
        public static float TanHDerivative(float value)
        {
            return 1f - Mathf.Pow(TanH(value), 2);
        }

        /// <summary>
        /// Linear activation function.
        /// </summary>
        /// <param name="value">The input value.</param>
        /// <returns>The output of the Linear function.</returns>
        public static float Linear(float value)
        {
            return value;
        }

        /// <summary>
        /// Derivative of the Linear activation function.
        /// </summary>
        /// <param name="_value">The input value (not used).</param>
        /// <returns>The derivative of the Linear function, which is always 1.</returns>
        public static float LinearDerivative(float _value)
        {
            return 1;
        }
    }
}
