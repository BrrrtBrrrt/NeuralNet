using Assets.Scripts.Entities;
using Assets.Scripts.Enums;
using Assets.Scripts.Types;
using Assets.Scripts.UiModifications.Attributes;
using Assets.Scripts.UiModifications.PropertyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Controllers
{
    /// <summary>
    /// Manages the training process of a neural network, providing controls for various training actions
    /// and exposing properties to interact with the training process.
    /// </summary>
    public class NeuralTrainerController : MonoBehaviour
    {
        // Action control flags
        [Header("Actions")]

        /// <summary>
        /// Flag to determine if the calculate error action is pressed.
        /// </summary>
#if UNITY_EDITOR
        [DisplayNameProperty("Calculate error")]
#endif
        public bool IsCalculateErrorActionPressed = false;

        /// <summary>
        /// Flag to determine if the backpropagate error action is pressed.
        /// </summary>
#if UNITY_EDITOR
        [DisplayNameProperty("Backpropagate error")]
#endif
        public bool IsBackpropagateErrorActionPressed = false;

        /// <summary>
        /// Flag to determine if the backpropagate action is pressed.
        /// </summary>
#if UNITY_EDITOR
        [DisplayNameProperty("Backpropagate")]
#endif
        public bool IsBackpropagateActionPressed = false;

        /// <summary>
        /// Flag to determine if the train action is pressed.
        /// </summary>
#if UNITY_EDITOR
        [DisplayNameProperty("Train")]
#endif
        public bool IsTrainActionPressed = false;

        /// <summary>
        /// Flag to determine if the stop training action is pressed.
        /// </summary>
#if UNITY_EDITOR
        [DisplayNameProperty("Stop training")]
#endif
        public bool IsStopTrainingActionPressed = false;

        /// <summary>
        /// Reference to the DataGeneratorController for accessing training and test data.
        /// </summary>
        public DataGeneratorController DataGeneratorController = null;

        /// <summary>
        /// Coroutine used to manage the training process asynchronously.
        /// </summary>
        private Coroutine coroutineTraining = null;

        /// <summary>
        /// The number of epochs to train the neural network.
        /// </summary>
#if UNITY_EDITOR
        [ExposeProperty]
#endif
        public int EpochCount
        {
            get
            {
                if (NeuralTrainerComponent == null) return default;
                return NeuralTrainerComponent.EpochCount;
            }
            set
            {
                NeuralTrainerComponent.EpochCount = value;
            }
        }

        /// <summary>
        /// The current epoch number during training.
        /// </summary>
#if UNITY_EDITOR
        [ExposeProperty]
#endif
        public int CurrentEpoch
        {
            get
            {
                if (NeuralTrainerComponent == null) return default;
                return NeuralTrainerComponent.CurrentEpoch;
            }
        }

        /// <summary>
        /// The total number of iterations to train the neural network.
        /// </summary>
#if UNITY_EDITOR
        [ExposeProperty]
#endif
        public int IterationCount
        {
            get
            {
                if (NeuralTrainerComponent == null) return default;
                return NeuralTrainerComponent.IterationCount;
            }
        }

        /// <summary>
        /// The current iteration number during training.
        /// </summary>
#if UNITY_EDITOR
        [ExposeProperty]
#endif
        public int CurrentIteration
        {
            get
            {
                if (NeuralTrainerComponent == null) return default;
                return NeuralTrainerComponent.CurrentIteration;
            }
        }

        /// <summary>
        /// The learning rate for training the neural network.
        /// </summary>
#if UNITY_EDITOR
        [ExposeProperty]
#endif
        public float LearningRate
        {
            get
            {
                if (NeuralTrainerComponent == null) return default;
                return NeuralTrainerComponent.LearningRate;
            }
            set
            {
                NeuralTrainerComponent.LearningRate = value;
            }
        }

        /// <summary>
        /// The type of loss function used for training.
        /// </summary>
#if UNITY_EDITOR
        [ExposeProperty]
#endif
        public LossFunctionType LossFunction
        {
            get
            {
                if (NeuralTrainerComponent == null) return default;
                return NeuralTrainerComponent.LossFunction;
            }
            set
            {
                NeuralTrainerComponent.LossFunction = value;
            }
        }

        /// <summary>
        /// The expected output values for training.
        /// </summary>
#if UNITY_EDITOR
        [ExposeProperty]
#endif
        public float[] OutputExpected
        {
            get
            {
                if (NeuralTrainerComponent == null) return default;
                return NeuralTrainerComponent.OutputExpected;
            }
            set
            {
                NeuralTrainerComponent.OutputExpected = value;
            }
        }

        /// <summary>
        /// The batch size used during training.
        /// </summary>
#if UNITY_EDITOR
        [ExposeProperty]
#endif
        public int BatchSize
        {
            get
            {
                if (NeuralTrainerComponent == null) return default;
                return NeuralTrainerComponent.BatchSize;
            }
            set
            {
                NeuralTrainerComponent.BatchSize = value;
            }
        }

        /// <summary>
        /// The number of batches processed per render frame.
        /// </summary>
#if UNITY_EDITOR
        [ExposeProperty]
#endif
        public int BatchCountPerRender
        {
            get
            {
                if (NeuralTrainerComponent == null) return default;
                return NeuralTrainerComponent.BatchCountPerRender;
            }
            set
            {
                NeuralTrainerComponent.BatchCountPerRender = value;
            }
        }

#if UNITY_EDITOR
        [ExposeProperty]
#endif
        public bool ReinitializeOptimizer
        {
            get
            {
                if (NeuralTrainerComponent == null) return default;
                return NeuralTrainerComponent.ReinitializeOptimizer;
            }
            set
            {
                NeuralTrainerComponent.ReinitializeOptimizer = value;
            }
        }

        /// <summary>
        /// The training predictions generated during the current epoch.
        /// </summary>
        internal XYData EpochTrainingPredictions
        {
            get
            {
                if (NeuralTrainerComponent == null) return default;
                return NeuralTrainerComponent.EpochTrainingPredictions;
            }
            set
            {
                NeuralTrainerComponent.EpochTrainingPredictions = value;
            }
        }

        /// <summary>
        /// The test predictions generated during the current epoch.
        /// </summary>
        internal XYData EpochTestPredictions
        {
            get
            {
                if (NeuralTrainerComponent == null) return default;
                return NeuralTrainerComponent.EpochTestPredictions;
            }
            set
            {
                NeuralTrainerComponent.EpochTestPredictions = value;
            }
        }

        /// <summary>
        /// The neural network instance being trained.
        /// </summary>
        internal NeuralNetwork Network
        {
            get
            {
                if (NeuralTrainerComponent == null) return default;
                return NeuralTrainerComponent.Network;
            }
            set
            {
                NeuralTrainerComponent.Network = value;
            }
        }

        /// <summary>
        /// The errors recorded for each epoch.
        /// </summary>
        internal List<float> EpochErrors
        {
            get
            {
                if (NeuralTrainerComponent == null) return default;
                return NeuralTrainerComponent.EpochErrors;
            }
            set
            {
                NeuralTrainerComponent.EpochErrors = value;
            }
        }

        /// <summary>
        /// Instance of the neural trainer component managing the training process.
        /// </summary>
        internal readonly NeuralTrainer NeuralTrainerComponent = new();

#pragma warning disable IDE0051 // Remove unused private members
        private void Start()
#pragma warning restore IDE0051 // Remove unused private members
        {
            GameObject dataGeneratorObject = GameObject.Find(Constants.DATA_GENERATOR_OBJECT_NAME);
            DataGeneratorController = dataGeneratorObject.GetComponent<DataGeneratorController>();
        }

#pragma warning disable IDE0051 // Remove unused private members
        private void Update()
#pragma warning restore IDE0051 // Remove unused private members
        {
            if (IsCalculateErrorActionPressed)
            {
                IsCalculateErrorActionPressed = false;
                CalculateError();
            }

            if (IsBackpropagateErrorActionPressed)
            {
                IsBackpropagateErrorActionPressed = false;
                BackpropagateError();
            }

            if (IsTrainActionPressed)
            {
                IsTrainActionPressed = false;
                Train();
            }

            if (IsBackpropagateActionPressed)
            {
                IsBackpropagateActionPressed = false;
                Backpropagate();
            }

            if (IsStopTrainingActionPressed)
            {
                IsStopTrainingActionPressed = false;
                StopTraining();
            }
        }

        /// <summary>
        /// Stops the ongoing training coroutine.
        /// </summary>
        private void StopTraining()
        {
            StopCoroutine(coroutineTraining);
            coroutineTraining = null;
        }

        /// <summary>
        /// Calculates the error for the current epoch using the neural trainer component.
        /// </summary>
        private void CalculateError()
        {
            NeuralTrainerComponent.CalculateError();
        }

        /// <summary>
        /// Backpropagates the error through the network using the neural trainer component.
        /// </summary>
        private void BackpropagateError()
        {
            NeuralTrainerComponent.BackpropagateError();
        }

        /// <summary>
        /// Performs a backpropagation step through the neural network.
        /// </summary>
        private void Backpropagate()
        {
            NeuralTrainerComponent.Backpropagate();
        }

        /// <summary>
        /// Starts the training process in a coroutine to run asynchronously.
        /// </summary>
        private void Train()
        {
            if (coroutineTraining != null) return;
            coroutineTraining = StartCoroutine(TrainCoroutine());
        }

        /// <summary>
        /// Coroutine for training the neural network.
        /// Yields control until the training process is complete.
        /// </summary>
        /// <returns>An IEnumerator for the coroutine.</returns>
        private IEnumerator TrainCoroutine()
        {
            yield return NeuralTrainerComponent.Train(DataGeneratorController.DataTrainingPrepared, DataGeneratorController.DataTestPrepared);
            coroutineTraining = null;
            yield return null;
        }
    }
}
