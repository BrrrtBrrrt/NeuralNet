using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Utils
{
    /// <summary>
    /// Utility class for various loss functions and their derivatives.
    /// </summary>
    public static class LossFunctionUtil
    {
        /// <summary>
        /// Computes the Log-Cosh loss between actual and predicted values.
        /// </summary>
        /// <param name="actual">The actual values.</param>
        /// <param name="predicted">The predicted values.</param>
        /// <returns>The Log-Cosh loss.</returns>
        public static float LogCosh(IList<float> actual, IList<float> predicted)
        {
            float error = 0;
            for (int i = 0; i < actual.Count; i++)
            {
                error += (float)System.Math.Log(System.Math.Cosh(actual[i] - predicted[i]));
            }
            return error;
        }

        /// <summary>
        /// Computes the derivative of the Log-Cosh loss with respect to the predicted values.
        /// </summary>
        /// <param name="actual">The actual values.</param>
        /// <param name="predicted">The predicted values.</param>
        /// <returns>The derivative of the Log-Cosh loss.</returns>
        public static IList<float> LogCoshDerivative(IList<float> actual, IList<float> predicted)
        {
            List<float> derivatives = new();
            for (int i = 0; i < actual.Count; i++)
            {
                derivatives.Add(-MathF.Tanh(actual[i] - predicted[i]));
            }
            return derivatives;
        }

        /// <summary>
        /// Computes the Mean Squared Error (MSE) between actual and predicted values.
        /// </summary>
        /// <param name="actual">The actual values.</param>
        /// <param name="predicted">The predicted values.</param>
        /// <returns>The Mean Squared Error.</returns>
        public static float MSE(IList<float> actual, IList<float> predicted)
        {
            float error = 0;
            for (int i = 0; i < actual.Count; i++)
            {
                error += Mathf.Pow(actual[i] - predicted[i], 2f);
            }
            return error / actual.Count;
        }

        /// <summary>
        /// Computes the derivative of the Mean Squared Error (MSE) with respect to the predicted values.
        /// </summary>
        /// <param name="actual">The actual values.</param>
        /// <param name="predicted">The predicted values.</param>
        /// <returns>The derivative of the Mean Squared Error.</returns>
        public static IList<float> MSEDerivative(IList<float> actual, IList<float> predicted)
        {
            List<float> derivatives = new();
            for (int i = 0; i < actual.Count; i++)
            {
                derivatives.Add(-2f * (actual[i] - predicted[i]) / actual.Count);
            }
            return derivatives;
        }

        /// <summary>
        /// Computes the Squared Error between actual and predicted values.
        /// </summary>
        /// <param name="actual">The actual values.</param>
        /// <param name="predicted">The predicted values.</param>
        /// <returns>The Squared Error.</returns>
        public static float SE(IList<float> actual, IList<float> predicted)
        {
            float error = 0;
            for (int i = 0; i < actual.Count; i++)
            {
                error += Mathf.Pow(actual[i] - predicted[i], 2f);
            }
            return error;
        }

        /// <summary>
        /// Computes the derivative of the Squared Error with respect to the predicted values.
        /// </summary>
        /// <param name="actual">The actual values.</param>
        /// <param name="predicted">The predicted values.</param>
        /// <returns>The derivative of the Squared Error.</returns>
        public static IList<float> SEDerivative(IList<float> actual, IList<float> predicted)
        {
            List<float> derivatives = new();
            for (int i = 0; i < actual.Count; i++)
            {
                derivatives.Add(-2f * (actual[i] - predicted[i]));
            }
            return derivatives;
        }

        /// <summary>
        /// Computes the simple error (difference) between actual and predicted values.
        /// </summary>
        /// <param name="actual">The actual values.</param>
        /// <param name="predicted">The predicted values.</param>
        /// <returns>The simple error.</returns>
        public static float E(IList<float> actual, IList<float> predicted)
        {
            float error = 0;
            for (int i = 0; i < actual.Count; i++)
            {
                error += actual[i] - predicted[i];
            }
            return error;
        }

        /// <summary>
        /// Computes the derivative of the simple error with respect to the predicted values.
        /// </summary>
        /// <param name="actual">The actual values.</param>
        /// <param name="predicted">The predicted values.</param>
        /// <returns>The derivative of the simple error.</returns>
        public static IList<float> EDerivative(IList<float> actual, IList<float> predicted)
        {
            List<float> derivatives = new();
            for (int i = 0; i < actual.Count; i++)
            {
                derivatives.Add(-1f);
            }
            return derivatives;
        }

        /// <summary>
        /// Computes the Mean Binary Cross Entropy loss between actual and predicted values.
        /// </summary>
        /// <param name="actual">The actual values.</param>
        /// <param name="predicted">The predicted values.</param>
        /// <returns>The Mean Binary Cross Entropy loss.</returns>
        public static float MBCE(IList<float> actual, IList<float> predicted)
        {
            float error = 0;
            for (int i = 0; i < actual.Count; i++)
            {
                error += actual[i] * Mathf.Log(1e-15f + predicted[i]) + (1f - actual[i]) * Mathf.Log(1e-15f + 1f - predicted[i]);
            }
            return -error / actual.Count;
        }

        /// <summary>
        /// Computes the derivative of the Mean Binary Cross Entropy loss with respect to the predicted values.
        /// </summary>
        /// <param name="actual">The actual values.</param>
        /// <param name="predicted">The predicted values.</param>
        /// <returns>The derivative of the Mean Binary Cross Entropy loss.</returns>
        public static IList<float> MBCEDerivative(IList<float> actual, IList<float> predicted)
        {
            List<float> derivatives = new();
            for (int i = 0; i < actual.Count; i++)
            {
                derivatives.Add(-(actual[i] / (predicted[i] + 1e-15f) - (1f - actual[i]) / (1f - predicted[i] + 1e-15f)) / actual.Count);
            }
            return derivatives;
        }

        /// <summary>
        /// Computes the Huber loss between actual and predicted values.
        /// </summary>
        /// <param name="actual">The actual values.</param>
        /// <param name="predicted">The predicted values.</param>
        /// <param name="delta">The delta value for Huber loss calculation.</param>
        /// <returns>The Huber loss.</returns>
        public static float Huber(IList<float> actual, IList<float> predicted, float delta = 1.0f)
        {
            float loss = 0;
            for (int i = 0; i < actual.Count; i++)
            {
                float error = actual[i] - predicted[i];
                if (Math.Abs(error) <= delta)
                {
                    loss += 0.5f * Mathf.Pow(error, 2f);
                }
                else
                {
                    loss += delta * Math.Abs(error) - 0.5f * delta * delta;
                }
            }
            return loss / actual.Count;
        }

        /// <summary>
        /// Computes the derivative of the Huber loss with respect to the predicted values.
        /// </summary>
        /// <param name="actual">The actual values.</param>
        /// <param name="predicted">The predicted values.</param>
        /// <returns>The derivative of the Huber loss.</returns>
        public static IList<float> HuberDerivative(IList<float> actual, IList<float> predicted, float delta = 1.0f)
        {
            List<float> derivatives = new();
            for (int i = 0; i < actual.Count; i++)
            {
                float error = actual[i] - predicted[i];
                if (Math.Abs(error) <= delta)
                {
                    derivatives.Add(-error);
                }
                else
                {
                    derivatives.Add(-delta * Math.Sign(error));
                }
            }
            return derivatives;
        }

        /// <summary>
        /// Computes the Root Mean Squared Error loss between actual and predicted values.
        /// </summary>
        /// <param name="actual">The actual values.</param>
        /// <param name="predicted">The predicted values.</param>
        /// <returns>The Root Mean Squared Error loss.</returns>
        public static float RMSE(IList<float> actual, IList<float> predicted)
        {
            float mse = MSE(actual, predicted);
            return Mathf.Sqrt(mse);
        }

        /// <summary>
        /// Computes the derivative of the Root Mean Squared Error loss with respect to the predicted values.
        /// </summary>
        /// <param name="actual">The actual values.</param>
        /// <param name="predicted">The predicted values.</param>
        /// <returns>The derivative of the Root Mean Squared Error loss.</returns>
        public static IList<float> RMSEDerivative(IList<float> actual, IList<float> predicted)
        {
            float mse = MSE(actual, predicted);
            float rmse = Mathf.Sqrt(mse);
            List<float> derivatives = new();
            for (int i = 0; i < actual.Count; i++)
            {
                float error = actual[i] - predicted[i];
                derivatives.Add(-error / (rmse * actual.Count));
            }
            return derivatives;
        }

        /// <summary>
        /// Computes the Mean Absolute Error loss between actual and predicted values.
        /// </summary>
        /// <param name="actual">The actual values.</param>
        /// <param name="predicted">The predicted values.</param>
        /// <returns>The Mean Absolute Error loss.</returns>
        public static float MAE(IList<float> actual, IList<float> predicted)
        {
            float error = 0;
            for (int i = 0; i < actual.Count; i++)
            {
                error += Mathf.Abs(actual[i] - predicted[i]);
            }
            return error / actual.Count;
        }

        /// <summary>
        /// Computes the derivative of the Mean Absolute Error loss with respect to the predicted values.
        /// </summary>
        /// <param name="actual">The actual values.</param>
        /// <param name="predicted">The predicted values.</param>
        /// <returns>The derivative of the Mean Absolute Error loss.</returns>
        public static IList<float> MAEDerivative(IList<float> actual, IList<float> predicted)
        {
            List<float> derivatives = new();
            for (int i = 0; i < actual.Count; i++)
            {
                float error = actual[i] - predicted[i];
                derivatives.Add(-Mathf.Sign(error));
            }
            return derivatives;
        }

        /// <summary>
        /// Computes the Root Mean Log Squared Error loss between actual and predicted values.
        /// </summary>
        /// <param name="actual">The actual values.</param>
        /// <param name="predicted">The predicted values.</param>
        /// <returns>The Root Mean Log Squared Error loss.</returns>
        public static float RMLSE(IList<float> actual, IList<float> predicted, float epsilon = 1e-15f)
        {
            float error = 0;
            for (int i = 0; i < actual.Count; i++)
            {
                float squaredError = Mathf.Pow(actual[i] - predicted[i], 2f);
                error += Mathf.Log(squaredError + epsilon);
            }
            return Mathf.Sqrt(error / actual.Count);
        }

        /// <summary>
        /// Computes the derivative of the Root Mean Log Squared Error loss with respect to the predicted values.
        /// </summary>
        /// <param name="actual">The actual values.</param>
        /// <param name="predicted">The predicted values.</param>
        /// <returns>The derivative of the Root Mean Log Squared Error loss.</returns>
        public static IList<float> RMLSEDerivative(IList<float> actual, IList<float> predicted, float epsilon = 1e-15f)
        {
            List<float> derivatives = new();
            for (int i = 0; i < actual.Count; i++)
            {
                float squaredError = Mathf.Pow(actual[i] - predicted[i], 2f);
                float derivative = -2f * (actual[i] - predicted[i]) / (squaredError + epsilon);
                derivatives.Add(derivative);
            }
            return derivatives;
        }
    }
}
