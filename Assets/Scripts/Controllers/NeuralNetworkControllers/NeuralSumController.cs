using Assets.Scripts.Entities;
using Assets.Scripts.UiModifications.Attributes;
using Assets.Scripts.UiModifications.PropertyAttributes;
using UnityEngine;

namespace Assets.Scripts.Controllers.NeuralNetworkControllers
{
    /// <summary>
    /// Controls the summation of inputs for a neuron in a neural network.
    /// Allows for forward propagation and updating of the sum result.
    /// </summary>
    public class NeuralSumController : MonoBehaviour
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
        /// Gets or sets the sum result of the neuron.
        /// </summary>
#if UNITY_EDITOR
        [ExposeProperty]
#endif
        public float Result
        {
            get
            {
                return NetworkComponent.SumResults[LayerIndex][NeuronIndex];
            }
            set
            {
                NetworkComponent.SumResults[LayerIndex][NeuronIndex] = value;
            }
        }

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
        /// Performs forward propagation for the sum in the neural network.
        /// </summary>
        private void ForwardPropagate()
        {
            NetworkComponent.ForwardPropagateSum(LayerIndex, NeuronIndex);
        }
    }
}
