using Assets.Scripts.Entities;
using Assets.Scripts.Enums;
using Assets.Scripts.Types;
using Assets.Scripts.UiModifications.Attributes;
using Assets.Scripts.UiModifications.PropertyAttributes;
using UnityEngine;

namespace Assets.Scripts.Controllers
{
    /// <summary>
    /// Manages the generation of training and test data for neural network training.
    /// This controller interacts with the data generator component to produce data based on specified settings.
    /// </summary>
    public class DataGeneratorController : MonoBehaviour
    {
        /// <summary>
        /// Flag to trigger data generation action. 
        /// Set to true to initiate data generation in the next frame.
        /// </summary>
        [Header("Actions")]
#if UNITY_EDITOR
        [DisplayNameProperty("Generate")]
#endif
        public bool IsGenerateActionPressed = false;

        /// <summary>
        /// Gets or sets the target function type for data generation.
        /// </summary>
#if UNITY_EDITOR
        [ExposeProperty]
#endif
        public TargetFunctionType TargetFunction
        {
            get => dataGeneratorComponent.TargetFunction;
            set => dataGeneratorComponent.TargetFunction = value;
        }

        /// <summary>
        /// Gets or sets the number of data points to generate.
        /// </summary>
#if UNITY_EDITOR
        [ExposeProperty]
#endif
        public int DataCount
        {
            get => dataGeneratorComponent.DataCount;
            set => dataGeneratorComponent.DataCount = value;
        }

        /// <summary>
        /// Gets or sets the minimum value for the x range of the data.
        /// </summary>
#if UNITY_EDITOR
        [ExposeProperty]
#endif
        public float XRangeMin
        {
            get => dataGeneratorComponent.XRangeMin;
            set => dataGeneratorComponent.XRangeMin = value;
        }

        /// <summary>
        /// Gets or sets the maximum value for the x range of the data.
        /// </summary>
#if UNITY_EDITOR
        [ExposeProperty]
#endif
        public float XRangeMax
        {
            get => dataGeneratorComponent.XRangeMax;
            set => dataGeneratorComponent.XRangeMax = value;
        }

        /// <summary>
        /// Gets or sets the minimum scale for the data.
        /// </summary>
#if UNITY_EDITOR
        [ExposeProperty]
#endif
        public float ScaleMin
        {
            get => dataGeneratorComponent.ScaleMin;
            set => dataGeneratorComponent.ScaleMin = value;
        }

        /// <summary>
        /// Gets or sets the maximum scale for the data.
        /// </summary>
#if UNITY_EDITOR
        [ExposeProperty]
#endif
        public float ScaleMax
        {
            get => dataGeneratorComponent.ScaleMax;
            set => dataGeneratorComponent.ScaleMax = value;
        }

        /// <summary>
        /// Gets or sets the factor for splitting data into training and test sets.
        /// </summary>
#if UNITY_EDITOR
        [ExposeProperty]
#endif
        public float TrainTestSplitFactor
        {
            get => dataGeneratorComponent.TrainTestSplitFactor;
            set => dataGeneratorComponent.TrainTestSplitFactor = value;
        }

        /// <summary>
        /// Gets or sets a value indicating whether the test data should be randomly selected from the entire dataset.
        /// </summary>
#if UNITY_EDITOR
        [ExposeProperty]
#endif
        [Tooltip("If true then the test data is randomly selected from the whole data set, if false then thw test data is the remaining data regarding the train test split.")]
        public bool PickTestDataFromWholeSet
        {
            get => dataGeneratorComponent.PickTestDataFromWholeSet;
            set => dataGeneratorComponent.PickTestDataFromWholeSet = value;
        }

        /// <summary>
        /// Gets or sets a value indicating whether the test data should be evenly picked from the whole dataset.
        /// </summary>
#if UNITY_EDITOR
        [ExposeProperty]
        [ConditionalHideProperty(nameof(PickTestDataFromWholeSet), true)]
#endif
        [Tooltip("If true then the test data is evenly picked from the whole data set, if false then it is randomly picked.")]
        public bool TestDataIsEvenlyPicked
        {
            get => dataGeneratorComponent.TestDataIsEvenlyPicked;
            set => dataGeneratorComponent.TestDataIsEvenlyPicked = value;
        }

        /// <summary>
        /// Gets or sets a value indicating whether the data should be shuffled after generation.
        /// </summary>
#if UNITY_EDITOR
        [ExposeProperty]
#endif
        public bool ShuffleData
        {
            get => dataGeneratorComponent.ShuffleData;
            set => dataGeneratorComponent.ShuffleData = value;
        }

        /// <summary>
        /// Gets or sets the noise factor to be applied to the generated data.
        /// </summary>
#if UNITY_EDITOR
        [ExposeProperty]
#endif
        public float NoiseFactor
        {
            get => dataGeneratorComponent.NoiseFactor;
            set => dataGeneratorComponent.NoiseFactor = value;
        }

        /// <summary>
        /// Stores the training data generated by the data generator.
        /// </summary>
        public XYData DataTraining = new();

        /// <summary>
        /// Stores the test data generated by the data generator.
        /// </summary>
        public XYData DataTest = new();

        /// <summary>
        /// Stores the prepared training data for use in training the neural network.
        /// </summary>
        public XYData DataTrainingPrepared = new();

        /// <summary>
        /// Stores the prepared test data for use in testing the neural network.
        /// </summary>
        public XYData DataTestPrepared = new();

        /// <summary>
        /// Stores the prepared training data in its original order before any shuffling.
        /// </summary>
        public XYData DataTrainingPreparedOriginalOrder = new();

        /// <summary>
        /// Stores the prepared test data in its original order before any shuffling.
        /// </summary>
        public XYData DataTestPreparedOriginalOrder = new();

        /// <summary>
        /// Instance of the DataGenerator component responsible for generating the data.
        /// </summary>
        private readonly DataGenerator dataGeneratorComponent = new();

        /// <summary>
        /// Unity's Update method, called once per frame.
        /// Checks if the generation action is triggered, and if so, generates the data.
        /// </summary>
#pragma warning disable IDE0051 // Remove unused private members
        private void Update()
#pragma warning restore IDE0051 // Remove unused private members
        {
            if (IsGenerateActionPressed)
            {
                IsGenerateActionPressed = false;
                // Generate data and store the results in the corresponding fields
                (DataTraining, DataTest, DataTrainingPrepared, DataTestPrepared, DataTrainingPreparedOriginalOrder, DataTestPreparedOriginalOrder) = dataGeneratorComponent.Generate();
                Debug.Log("Generate done");
            }
        }
    }
}
