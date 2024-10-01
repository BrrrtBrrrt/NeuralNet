using Assets.Scripts.Entities;
using Assets.Scripts.Enums;
using Assets.Scripts.UiModifications.Attributes;
using Assets.Scripts.UiModifications.PropertyAttributes;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Controllers.NeuralNetworkControllers
{
    /// <summary>
    /// Controls the behavior and properties of individual neurons in a neural network.
    /// Manages forward propagation and updates various properties of the neuron.
    /// </summary>
    public class NeuralNeuronController : MonoBehaviour
    {
        /// <summary>
        /// Indicates if the neuron belongs to an output or hidden layer.
        /// </summary>
        [HideInInspector()]
        public bool IsOutputOrHiddenLayerNeuron = false;

        /// <summary>
        /// Flag to trigger forward propagation action.
        /// Set to true to initiate forward propagation in the next frame.
        /// </summary>
        [Header("Actions")]
#if UNITY_EDITOR
        [DisplayNameProperty("Forward propagate")]
        [ConditionalHideProperty(nameof(IsOutputOrHiddenLayerNeuron), true)]
#endif
        public bool IsForwardPropagateActionPressed = false;

        /// <summary>
        /// Gets or sets the bias of the neuron.
        /// </summary>
#if UNITY_EDITOR
        [ExposeProperty]
#endif
        public float Bias
        {
            get
            {
                if (NetworkComponent.Biases[LayerIndex] == null) return float.NaN;
                return NetworkComponent.Biases[LayerIndex][NeuronIndex];
            }
            set
            {
                if (NetworkComponent.Biases[LayerIndex] == null) return;
                NetworkComponent.Biases[LayerIndex][NeuronIndex] = value;
            }
        }

        /// <summary>
        /// Gets or sets the weights of the neuron.
        /// </summary>
#if UNITY_EDITOR
        [ExposeProperty]
#endif
        public float[] Weights
        {
            get
            {
                if (NetworkComponent.Weights[LayerIndex] == null) return Array.Empty<float>();
                return NetworkComponent.Weights[LayerIndex][NeuronIndex];
            }
            set
            {
                if (NetworkComponent.Weights[LayerIndex] == null) return;
                NetworkComponent.Weights[LayerIndex][NeuronIndex] = value;
            }
        }

        /// <summary>
        /// Gets or sets the results of the weights after forward propagation.
        /// </summary>
#if UNITY_EDITOR
        [ExposeProperty]
#endif
        public float[] WeightResults
        {
            get
            {
                if (NetworkComponent.WeightResults[LayerIndex] == null) return Array.Empty<float>();
                return NetworkComponent.WeightResults[LayerIndex][NeuronIndex];
            }
            set
            {
                if (NetworkComponent.WeightResults[LayerIndex] == null) return;
                NetworkComponent.WeightResults[LayerIndex][NeuronIndex] = value;
            }
        }

        /// <summary>
        /// Gets or sets the sum result of the neuron.
        /// </summary>
#if UNITY_EDITOR
        [ExposeProperty]
#endif
        public float SumResult
        {
            get
            {
                if (NetworkComponent.SumResults[LayerIndex] == null) return float.NaN;
                return NetworkComponent.SumResults[LayerIndex][NeuronIndex];
            }
            set
            {
                if (NetworkComponent.SumResults[LayerIndex] == null) return;
                NetworkComponent.SumResults[LayerIndex][NeuronIndex] = value;
            }
        }

        /// <summary>
        /// Gets or sets the activation value of the neuron.
        /// </summary>
#if UNITY_EDITOR
        [ExposeProperty]
#endif
        public float Activation
        {
            get
            {
                return NetworkComponent.Activations[LayerIndex][NeuronIndex];
            }
            set
            {
                NetworkComponent.Activations[LayerIndex][NeuronIndex] = value;
            }
        }

        /// <summary>
        /// Gets or sets the activation function type of the neuron.
        /// </summary>
#if UNITY_EDITOR
        [ExposeProperty]
#endif
        public ActivationFunctionType ActivationFunction
        {
            get
            {
                if (NetworkComponent.ActivationFunctions[LayerIndex] == null) return ActivationFunctionType.RE_LU;
                return NetworkComponent.ActivationFunctions[LayerIndex][NeuronIndex];
            }
            set
            {
                if (NetworkComponent.ActivationFunctions[LayerIndex] == null) return;
                NetworkComponent.ActivationFunctions[LayerIndex][NeuronIndex] = value;
            }
        }

        /// <summary>
        /// Gets or sets the error value of the neuron.
        /// </summary>
#if UNITY_EDITOR
        [ExposeProperty]
#endif
        public float Error
        {
            get
            {
                if (NetworkComponent.Errors[LayerIndex] == null) return float.NaN;
                return NetworkComponent.Errors[LayerIndex][NeuronIndex];
            }
            set
            {
                if (NetworkComponent.Errors[LayerIndex] == null) return;
                NetworkComponent.Errors[LayerIndex][NeuronIndex] = value;
            }
        }

        /// <summary>
        /// The list of GameObjects representing the weights of the neuron.
        /// </summary>
        public List<GameObject> WeightObjects = new();

        /// <summary>
        /// The neural network component associated with this controller.
        /// </summary>
        internal NeuralNetwork NetworkComponent { get; set; }

        /// <summary>
        /// The index of the layer in the neural network.
        /// </summary>
        internal int LayerIndex { get; set; }

        /// <summary>
        /// The index of the neuron in the layer.
        /// </summary>
        internal int NeuronIndex { get; set; }

        /// <summary>
        /// Unity's Update method, called once per frame.
        /// Checks if forward propagation is triggered, and if so, performs the forward propagation.
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
        }

        /// <summary>
        /// Performs forward propagation for the neuron in the neural network.
        /// </summary>
        private void ForwardPropagate()
        {
            NetworkComponent.ForwardPropagateNeuron(LayerIndex, NeuronIndex);
        }
    }
}
