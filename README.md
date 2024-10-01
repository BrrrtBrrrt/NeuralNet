# NeuralNet
## Description
A framework to generate/train/visualize neural networks in Unity.

## Usage
Clone project, open in Unity Hub, build and run.

## Features
- Mini-Batch gradient descent
- Layer types
  - Input
  - Dense
- Activation functions
  - Rectified linear unit (ReLU)
  - Linear
  - Leaky Rectified linear unit (LReLU)
  - Sigmoid
  - Hyperbolic Tangent (TanH)
- Loss functions
  - Mean Squared Error (MSE)
  - Squared Error (SE)
  - LogCosh
  - Mean Binary Cross Entropy (MBCE)
  - Huber
  - Root Mean Squared Error (RMSE)
  - Mean Absolute Error (MAE)
  - Root Mean Log Squared Error (RMLSE)
- Optimizer types
  - Adaptive Moment Estimation (ADAM)
- Supports changes to the network architecture during runtime, to analyse the impact of changes during training
- Support to view and modify the state of single neuron components

## Status
Working prototype

## Preview
### Visualization of the network (Weight connection colored in gradient between red and green, based on the weight strengh)
![image](https://github.com/user-attachments/assets/94a70566-9fc3-410e-b950-ac5f39f21539)

### Visualization of the training (rate of change of the weights while training)
![image](https://github.com/user-attachments/assets/afdfaf8b-ceb8-44c7-b563-8e0990028baf)

### Visualization of the neurons and neuron connections strength (color gradient between transparent and white to represent the strength of neuron connections)
![image](https://github.com/user-attachments/assets/9788c433-a284-413e-bc17-43f75022e19e)

### Visualization of the neurons activations after training
![image](https://github.com/user-attachments/assets/d05ed8f4-f88e-4e16-8dae-a57bd3e000b5)
