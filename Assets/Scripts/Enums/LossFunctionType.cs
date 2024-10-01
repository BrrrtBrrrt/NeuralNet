using UnityEngine;

namespace Assets.Scripts.Enums
{
    /// <summary>
    /// Enum representing different types of loss functions used in neural networks for training and evaluation.
    /// </summary>
    public enum LossFunctionType
    {
        /// <summary>
        /// Represents the Huber loss function, which combines the mean squared error (MSE) and mean absolute error (MAE) to be less sensitive to outliers.
        /// </summary>
        [InspectorName("Huber Loss")]
        HUBER,

        /// <summary>
        /// Represents the Root Mean Squared Error (RMSE) loss function, which is the square root of the average of the squares of the differences between predicted and actual values.
        /// </summary>
        [InspectorName("Root Mean Squared Error (RMSE)")]
        RMSE,

        /// <summary>
        /// Represents the Mean Absolute Error (MAE) loss function, which is the average of the absolute differences between predicted and actual values.
        /// </summary>
        [InspectorName("Mean Absolute Error (MAE)")]
        MAE,

        /// <summary>
        /// Represents the Root Mean Log Squared Error (RMLSE) loss function, which is the square root of the average of the log-squared differences between predicted and actual values.
        /// </summary>
        [InspectorName("Root Mean Log Squared Error (RMLSE)")]
        RMLSE,

        /// <summary>
        /// Represents the Mean Squared Error (MSE) loss function, which is the average of the squares of the differences between predicted and actual values.
        /// </summary>
        [InspectorName("Mean Squared Error (MSE)")]
        MSE,

        /// <summary>
        /// Represents the Squared Error loss function, which is the square of the difference between predicted and actual values.
        /// </summary>
        [InspectorName("Squared Error (SE)")]
        SE,

        /// <summary>
        /// Represents a generic Error loss function, which could be a placeholder or used for custom error calculations.
        /// </summary>
        [InspectorName("Error (E)")]
        E,

        /// <summary>
        /// Represents the Mean Binary Cross Entropy loss function, often used for binary classification problems.
        /// </summary>
        [InspectorName("Mean Binary Cross Entropy (MBCE)")]
        MBCE,

        /// <summary>
        /// Represents the Log Cosh loss function, which is a smooth approximation of the absolute error.
        /// </summary>
        [InspectorName("Log Cosh")]
        LOG_COSH,
    }
}