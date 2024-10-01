# Neural Network Generator and Visualizer Documentation

## Table of Contents
1. **Introduction**
2. **System Requirements**
3. **Installation**
4. **Architecture Overview**
   - 4.1 Neural Network Generator
   - 4.2 3D Visualization
   - 4.3 User Interface
   - 4.4 Training Process
   - 4.5 Synthetic Data Generator
5. **Component Details**
   - 5.1 Network Configuration
   - 5.2 Layer Configuration
   - 5.3 Neuron Configuration
   - 5.4 Weight and Bias Initialization
   - 5.5 Activation Functions
6. **User Interaction**
   - 6.1 Inspecting Components
   - 6.2 Modifying Components
   - 6.3 Visualizing Training Progress
   - 6.4 Setting Training Parameters
7. **Training Algorithms**
   - 7.1 Minibatch Stochastic Gradient Descent (MSGD)
   - 7.2 Adam Optimizer
8. **Synthetic Data Generation**
   - 8.1 Data Generation Parameters
   - 8.2 Supported Target Functions
9. **Conclusion**
10. **Glossary**
11. **References**

## 1. Introduction
This document serves as the comprehensive guide for the Neural Network Generator and Visualizer. It details the software’s architecture, design, features, and user interactions, providing all necessary information to effectively utilize and understand the system.

## 2. System Requirements
- **Operating System:** Windows 10 or higher
- **Development Environment:** Unity 2019.4 or higher
- **Dependencies:** .NET Framework 4.7.2 or higher, Unity ML-Agents Toolkit

## 3. Installation
1. Clone the repository from [GitHub](link-to-repository).
2. Open the project in Unity.
3. Resolve any missing dependencies via the Unity Package Manager.
4. Build and run the project.

## 4. Architecture Overview

### 4.1 Neural Network Generator
The Neural Network Generator creates a feedforward neural network (MLP) based on a list of layer configurations. Each layer is defined by:
- Number of neurons
- Activation function type and its arguments
- Weights and biases initialization strategy

### 4.2 3D Visualization
The network is visualized in a 3D space where each component (network, layer, neuron, weight, activation function, bias, sum function) can be clicked and inspected. The visualization includes:
- Real-time updates during training
- Interactive exploration of each part of the network

### 4.3 User Interface
The UI provides controls for:
- Inspecting and modifying network components
- Setting training parameters
- Visualizing training progress with error graphs and target/prediction graphs

### 4.4 Training Process
Training is handled using a combination of Minibatch Stochastic Gradient Descent (MSGD) and Adam Optimizer. The training parameters can be adjusted through the UI.

### 4.5 Synthetic Data Generator
A synthetic data generator is included, allowing the user to create training and test datasets with configurable parameters such as input range, noise level, data count, and target function type.

## 5. Component Details

### 5.1 Network Configuration
The network configuration is defined by a sequence of layer configurations. Each layer specifies:
- Number of neurons
- Activation function
- Weight and bias initialization strategy

### 5.2 Layer Configuration
Layer configuration includes:
- Type of activation function (e.g., ReLU, Sigmoid, Tanh)
- Number of neurons
- Arguments for the activation function

### 5.3 Neuron Configuration
Each neuron in the layer is configurable with:
- Initial weights
- Initial biases

### 5.4 Weight and Bias Initialization
Supported initialization strategies include:
- Random initialization
- Xavier/Glorot initialization
- He initialization

### 5.5 Activation Functions
Supported activation functions include:
- ReLU
- Sigmoid
- Tanh
- Custom functions with specific arguments

## 6. User Interaction

### 6.1 Inspecting Components
Users can click on any part of the network to inspect its current state, including:
- Weights
- Biases
- Activation function parameters

### 6.2 Modifying Components
Components can be modified through the UI, allowing changes to:
- Neuron count
- Activation function
- Initialization strategy

### 6.3 Visualizing Training Progress
The UI includes graphs for:
- Error over epochs
- Target vs. prediction

### 6.4 Setting Training Parameters
Training parameters that can be set include:
- Learning rate
- Batch size
- Number of epochs

## 7. Training Algorithms

### 7.1 Minibatch Stochastic Gradient Descent (MSGD)
MSGD updates model parameters using small batches of data, which helps in faster convergence and generalization.

### 7.2 Adam Optimizer
The Adam Optimizer combines the advantages of AdaGrad and RMSProp, providing an adaptive learning rate for each parameter.

## 8. Synthetic Data Generation

### 8.1 Data Generation Parameters
Users can configure the following parameters for data generation:
- Input range
- Noise level
- Number of data points

### 8.2 Supported Target Functions
The data generator supports various target functions, including:
- Linear functions
- Polynomial functions
- Sinusoidal functions

## 9. Conclusion
This documentation provides an in-depth overview of the Neural Network Generator and Visualizer. Users can generate, visualize, and interact with neural networks, understand their workings, and experiment with various training parameters and synthetic data.

## 10. Glossary
- **MLP:** Multilayer Perceptron
- **MSGD:** Minibatch Stochastic Gradient Descent
- **UI:** User Interface

## 11. References
- .NET Framework Documentation: [link]
- Adam Optimizer Paper: [https://arxiv.org/pdf/1412.6980]

---