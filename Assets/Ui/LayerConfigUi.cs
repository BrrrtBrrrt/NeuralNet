using Assets.Scripts.Enums;
using Assets.Scripts.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

namespace Assets.Ui
{
    public class LayerConfigUi : VisualElement
    {
        public new class UxmlFactory : UxmlFactory<LayerConfigUi, UxmlTraits> { }

        public new class UxmlTraits : VisualElement.UxmlTraits
        {
            UxmlIntAttributeDescription m_NeuronCount = new() { name = "neuron-count", defaultValue = 1 };
            UxmlEnumAttributeDescription<ActivationFunctionType> m_ActivationFunction = new() { name = "activation-function", defaultValue = ActivationFunctionType.TAN_H };
            UxmlStringAttributeDescription m_ActivationFunctionArgs = new() { name = "activation-function-args", defaultValue = "" };
            UxmlEnumAttributeDescription<WeightsInitializationStrategyType> m_WeightsInitializationStrategy = new() { name = "weights-initialization-strategy", defaultValue = WeightsInitializationStrategyType.XAVIER_UNIFORM };
            UxmlEnumAttributeDescription<BiasesInitializationStrategyType> m_BiasesInitializationStrategy = new() { name = "biases-initialization-strategy", defaultValue = BiasesInitializationStrategyType.VALUE0D01 };

            public override void Init(VisualElement ve, IUxmlAttributes bag, CreationContext cc)
            {
                base.Init(ve, bag, cc);

                LayerConfigUi layerConfigUi = (LayerConfigUi)ve;
                LayerConfig layerConfig = layerConfigUi.layerConfig;

                layerConfig.NeuronCount = m_NeuronCount.GetValueFromBag(bag, cc);
                layerConfig.ActivationFunction = m_ActivationFunction.GetValueFromBag(bag, cc);
                layerConfig.ActivationFunctionArgs = m_ActivationFunctionArgs.GetValueFromBag(bag, cc).Split(Environment.NewLine).ToList();
                layerConfig.WeightsInitializationStrategy = m_WeightsInitializationStrategy.GetValueFromBag(bag, cc);
                layerConfig.BiasesInitializationStrategy = m_BiasesInitializationStrategy.GetValueFromBag(bag, cc);

                layerConfigUi.layerConfig = layerConfig;
            }
        }

        private TextField textFieldNeuronCount;
        private DropdownField dropdownFieldActivationFunction;
        private TextField textFieldActivationFunctionArgs;
        private DropdownField dropdownFieldWeightsInitializationStrategy;
        private DropdownField dropdownFieldBiasesInitializationStrategy;

        private LayerConfig _layerConfig = new();

        public LayerConfig layerConfig
        {
            get
            {
                return new()
                {
                    NeuronCount = int.Parse(textFieldNeuronCount.value),
                    ActivationFunction = Enum.Parse<ActivationFunctionType>(dropdownFieldActivationFunction.value),
                    ActivationFunctionArgs = textFieldActivationFunctionArgs.value.Split(Environment.NewLine).ToList(),
                    WeightsInitializationStrategy = Enum.Parse<WeightsInitializationStrategyType>(dropdownFieldWeightsInitializationStrategy.value),
                    BiasesInitializationStrategy = Enum.Parse<BiasesInitializationStrategyType>(dropdownFieldBiasesInitializationStrategy.value),
                };
            }
            set
            {
                textFieldNeuronCount.value = value.NeuronCount.ToString();
                dropdownFieldActivationFunction.value = value.ActivationFunction.ToString();
                textFieldActivationFunctionArgs.value = string.Join(Environment.NewLine, value.ActivationFunctionArgs);
                dropdownFieldWeightsInitializationStrategy.value = value.WeightsInitializationStrategy.ToString();
                dropdownFieldBiasesInitializationStrategy.value = value.BiasesInitializationStrategy.ToString();
            }
        }

        public LayerConfigUi()
        {
            // Load the UXML file
            VisualTreeAsset visualTree = Resources.Load<VisualTreeAsset>("Ui/LayerConfigUi");
            visualTree.CloneTree(this);

            // Find elements in the UXML
            textFieldNeuronCount = this.Q<TextField>("TextFieldNeuronCount");
            dropdownFieldActivationFunction = this.Q<DropdownField>("DropdownFieldActivationFunction");
            textFieldActivationFunctionArgs = this.Q<TextField>("TextFieldActivationFunctionArgs");
            dropdownFieldWeightsInitializationStrategy = this.Q<DropdownField>("DropdownFieldWeightsInitializationStrategy");
            dropdownFieldBiasesInitializationStrategy = this.Q<DropdownField>("DropdownFieldBiasesInitializationStrategy");

            dropdownFieldActivationFunction.choices = Enum.GetNames(typeof(ActivationFunctionType)).ToList();
            dropdownFieldWeightsInitializationStrategy.choices = Enum.GetNames(typeof(WeightsInitializationStrategyType)).ToList();
            dropdownFieldBiasesInitializationStrategy.choices = Enum.GetNames(typeof(BiasesInitializationStrategyType)).ToList();

            dropdownFieldActivationFunction.index = 0;
            dropdownFieldWeightsInitializationStrategy.index = 0;
            dropdownFieldBiasesInitializationStrategy.index = 0;

        }
    }
}
