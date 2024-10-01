using Assets.Scripts.Entities;
using Assets.Scripts.Enums;
using Assets.Scripts.UiModifications.Attributes;
using Assets.Scripts.UiModifications.PropertyAttributes;
using System;
using UnityEngine;

namespace Assets.Scripts.Controllers.NeuralNetworkControllers
{
    /// <summary>
    /// Controls the behavior and properties of a neural network.
    /// Manages forward propagation, resetting the model, and updating neuron hull visibility.
    /// </summary>
    public class NeuralNetworkController : MonoBehaviour
    {
        /// <summary>
        /// Flag to trigger forward propagation action.
        /// Set to true to initiate forward propagation in the next frame.
        /// </summary>
        [Header("Actions")]
#if UNITY_EDITOR
        [DisplayNameProperty("Forward propagate")]
#endif
        public bool IsForwardPropagateActionPressed = false;

        /// <summary>
        /// Flag to trigger reset action.
        /// Set to true to reset the model in the next frame.
        /// </summary>
#if UNITY_EDITOR
        [DisplayNameProperty("Reset")]
#endif
        public bool IsResetActionPressed = false;

        /// <summary>
        /// Gets or sets the input values of the neural network.
        /// </summary>
#if UNITY_EDITOR
        [ExposeProperty]
#endif
        public float[] Inputs
        {
            get
            {
                return NetworkComponent.Activations[0];
            }
            set
            {
                NetworkComponent.Activations[0] = value;
            }
        }

        /// <summary>
        /// Gets or sets the output values of the neural network.
        /// </summary>
#if UNITY_EDITOR
        [ExposeProperty]
#endif
        public float[] Outputs
        {
            get
            {
                return NetworkComponent.Activations[^1];
            }
            set
            {
                NetworkComponent.Activations[^1] = value;
            }
        }

        /// <summary>
        /// Gets or sets the total error of the neural network.
        /// </summary>
#if UNITY_EDITOR
        [ExposeProperty]
#endif
        public float ErrorTotal
        {
            get
            {
                return NetworkComponent.ErrorTotal;
            }
            set
            {
                NetworkComponent.ErrorTotal = value;
            }
        }

        public WeightColorMapType WeightColorMap = WeightColorMapType.WHITE;

        /// <summary>
        /// Indicates if the neuron hull is visible.
        /// </summary>
        public bool IsNeuronHullVisible = true;

        public TimeSpan WeightColorsUpdateDelay = TimeSpan.FromSeconds(1f / 11);

        /// <summary>
        /// Backup of the neural network component used for resetting.
        /// </summary>
        internal NeuralNetwork NetworkComponentBackup { get; set; }

        /// <summary>
        /// The neural network component associated with this controller.
        /// </summary>
        internal NeuralNetwork NetworkComponent { get; set; }

        /// <summary>
        /// Unity's Update method, called once per frame.
        /// Checks if forward propagation or reset actions are triggered and performs the corresponding actions.
        /// Also updates the visibility of the neuron hulls.
        /// </summary>
#pragma warning disable IDE0051 // Remove unused private members
        void Update()
#pragma warning restore IDE0051 // Remove unused private members
        {
            if (IsForwardPropagateActionPressed)
            {
                IsForwardPropagateActionPressed = false;
                ForwardPropagate();
            }

            if (IsResetActionPressed)
            {
                IsResetActionPressed = false;
                ResetModel();
            }

            UpdateNeuronHullVisibility();
        }

        /// <summary>
        /// Resets the neural network model to its initial state.
        /// </summary>
        private void ResetModel()
        {
            NetworkComponent = NetworkComponentBackup.Clone();

            // Update network component references for all child controllers
            NeuralLayerController[] neuralLayerControllers = GetComponentsInChildren<NeuralLayerController>();
            NeuralNeuronController[] neuralNeuronControllers = GetComponentsInChildren<NeuralNeuronController>();
            NeuralWeightController[] neuralWeightControllers = GetComponentsInChildren<NeuralWeightController>();
            NeuralSumController[] neuralSumControllers = GetComponentsInChildren<NeuralSumController>();
            NeuralActivationFunctionController[] neuralActivationFunctionControllers = GetComponentsInChildren<NeuralActivationFunctionController>();

            foreach (NeuralLayerController neuralLayerController in neuralLayerControllers)
            {
                neuralLayerController.NetworkComponent = NetworkComponent;
            }
            foreach (NeuralNeuronController neuralNeuronController in neuralNeuronControllers)
            {
                neuralNeuronController.NetworkComponent = NetworkComponent;
            }
            foreach (NeuralWeightController neuralWeightController in neuralWeightControllers)
            {
                neuralWeightController.NetworkComponent = NetworkComponent;
            }
            foreach (NeuralSumController neuralSumController in neuralSumControllers)
            {
                neuralSumController.NetworkComponent = NetworkComponent;
            }
            foreach (NeuralActivationFunctionController neuralActivationFunctionController in neuralActivationFunctionControllers)
            {
                neuralActivationFunctionController.NetworkComponent = NetworkComponent;
            }

            // Update the network component reference for the trainer controller
            GameObject trainerObject = GameObject.Find(Constants.TRAINER_OBJECT_NAME);
            NeuralTrainerController neuralTrainerController = trainerObject.GetComponent<NeuralTrainerController>();
            neuralTrainerController.Network = NetworkComponent;
        }

        /// <summary>
        /// Updates the visibility of neuron hulls based on the IsNeuronHullVisible flag.
        /// </summary>
        private void UpdateNeuronHullVisibility()
        {
            foreach (Transform layerTransform in transform)
            {
                foreach (Transform neuronTransform in layerTransform.transform)
                {
                    Renderer renderer = neuronTransform.GetComponent<Renderer>();
                    renderer.enabled = IsNeuronHullVisible;
                    Collider collider = neuronTransform.GetComponent<Collider>();
                    collider.enabled = IsNeuronHullVisible;
                }
            }
        }

        /// <summary>
        /// Performs forward propagation for the entire neural network.
        /// </summary>
        private void ForwardPropagate()
        {
            NetworkComponent.ForwardPropagate();
        }
    }
}