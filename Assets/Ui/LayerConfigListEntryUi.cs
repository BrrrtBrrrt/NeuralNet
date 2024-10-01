using Assets.Scripts.Enums;
using Assets.Scripts.Types;
using System;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

namespace Assets.Ui
{
    public class LayerConfigListEntryUi : VisualElement
    {
        public new class UxmlFactory : UxmlFactory<LayerConfigListEntryUi, UxmlTraits> { }

        public new class UxmlTraits : VisualElement.UxmlTraits
        {
            UxmlIntAttributeDescription m_NeuronCount = new() { name = "neuron-count", defaultValue = 1 };
            UxmlEnumAttributeDescription<ActivationFunctionType> m_ActivationFunction = new() { name = "activation-function", defaultValue = ActivationFunctionType.TAN_H };
            UxmlStringAttributeDescription m_ActivationFunctionArgs = new() { name = "activation-function-args", defaultValue = "" };
            UxmlEnumAttributeDescription<WeightsInitializationStrategyType> m_WeightsInitializationStrategy = new() { name = "weights-initialization-strategy", defaultValue = WeightsInitializationStrategyType.XAVIER_UNIFORM };
            UxmlEnumAttributeDescription<BiasesInitializationStrategyType> m_BiasesInitializationStrategy = new() { name = "biases-initialization-strategy", defaultValue = BiasesInitializationStrategyType.VALUE0D01 };
            //TypedUxmlAttributeDescription<LayerConfigUi> m_layer = new() { name = "", defaultValue = new LayerConfig() };

            public override void Init(VisualElement ve, IUxmlAttributes bag, CreationContext cc)
            {
                base.Init(ve, bag, cc);

                LayerConfigListEntryUi layerConfigListEntryUi = (LayerConfigListEntryUi)ve;
                LayerConfig layerConfig = layerConfigListEntryUi.layerConfig;

                layerConfig.NeuronCount = m_NeuronCount.GetValueFromBag(bag, cc);
                layerConfig.ActivationFunction = m_ActivationFunction.GetValueFromBag(bag, cc);
                layerConfig.ActivationFunctionArgs = m_ActivationFunctionArgs.GetValueFromBag(bag, cc).Split(Environment.NewLine).ToList();
                layerConfig.WeightsInitializationStrategy = m_WeightsInitializationStrategy.GetValueFromBag(bag, cc);
                layerConfig.BiasesInitializationStrategy = m_BiasesInitializationStrategy.GetValueFromBag(bag, cc);

                layerConfigListEntryUi.layerConfig = layerConfig;
            }
        }

        private LayerConfigUi layerConfigUi;
        private Button buttonDelete;

        private LayerConfig _layerConfig = new();

        public LayerConfig layerConfig
        {
            get
            {
                return layerConfigUi.layerConfig;
            }
            set
            {
                layerConfigUi.layerConfig = value;
            }
        }

        public LayerConfigListEntryUi()
        {
            // Load the UXML file
            VisualTreeAsset visualTree = Resources.Load<VisualTreeAsset>("Ui/LayerConfigListEntryUi");
            visualTree.CloneTree(this);

            // Find elements in the UXML
            /*textFieldNeuronCount = this.Q<TextField>("TextFieldNeuronCount");
            dropdownFieldActivationFunction = this.Q<DropdownField>("DropdownFieldActivationFunction");
            textFieldActivationFunctionArgs = this.Q<TextField>("TextFieldActivationFunctionArgs");
            dropdownFieldWeightsInitializationStrategy = this.Q<DropdownField>("DropdownFieldWeightsInitializationStrategy");
            dropdownFieldBiasesInitializationStrategy = this.Q<DropdownField>("DropdownFieldBiasesInitializationStrategy");

            dropdownFieldActivationFunction.choices = Enum.GetNames(typeof(ActivationFunctionType)).ToList();
            dropdownFieldWeightsInitializationStrategy.choices = Enum.GetNames(typeof(WeightsInitializationStrategyType)).ToList();
            dropdownFieldBiasesInitializationStrategy.choices = Enum.GetNames(typeof(BiasesInitializationStrategyType)).ToList();

            dropdownFieldActivationFunction.index = 0;
            dropdownFieldWeightsInitializationStrategy.index = 0;
            dropdownFieldBiasesInitializationStrategy.index = 0;*/

        }
    }
}
