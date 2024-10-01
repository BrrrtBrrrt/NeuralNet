using UnityEngine;

namespace Assets.Scripts
{
    /// <summary>
    /// A static class containing constant values used across the project.
    /// </summary>
    internal static class Constants
    {
        /// <summary>
        /// The name of the network object.
        /// </summary>
        public const string NETWORK_OBJECT_NAME = "Network";

        /// <summary>
        /// The name of the network generator object.
        /// </summary>
        public const string NETWORK_GENERATOR_OBJECT_NAME = "NetworkGenerator";

        /// <summary>
        /// The name of the trainer object.
        /// </summary>
        public const string TRAINER_OBJECT_NAME = "Trainer";

        /// <summary>
        /// The name of the data generator object.
        /// </summary>
        public const string DATA_GENERATOR_OBJECT_NAME = "DataGenerator";

        /// <summary>
        /// The width of the axis line.
        /// </summary>
        public const float AXIS_LINE_WIDTH = 1;

        /// <summary>
        /// The radius of the data point.
        /// </summary>
        public const float DATA_POINT_RADIUS = 1.25f;

        /// <summary>
        /// The radius of the error data point.
        /// </summary>
        public const float ERROR_DATA_POINT_RADIUS = 1.5f;

        /// <summary>
        /// xxx.
        /// </summary>
        //public const float NEURON_CONNECTION_LINE_WIDTH = 0.0125f;
        public const float NEURON_CONNECTION_LINE_WIDTH = 0.05f;

        /// <summary>
        /// The color of the axis line.
        /// </summary>
        public static readonly Color AXIS_LINE_COLOR = Color.white;

        /// <summary>
        /// The color of the target data points.
        /// </summary>
        public static readonly Color DATA_POINT_TARGET_COLOR = Color.green;

        /// <summary>
        /// The color of the predicted data points.
        /// </summary>
        public static readonly Color DATA_POINT_PREDICTION_COLOR = Color.red;

        /// <summary>
        /// The color representing the minimum weight.
        /// </summary>
        public static readonly Color WEIGHT_MIN_COLOR = Color.red;

        /// <summary>
        /// The color representing the maximum weight.
        /// </summary>
        public static readonly Color WEIGHT_MAX_COLOR = Color.green;

        /// <summary>
        /// The color of the error line.
        /// </summary>
        public static readonly Color ERROR_LINE_COLOR = Color.red;

        public const int RANDOM_SEED = 23144532;
        //public static Random random = Random.;
    }
}