# Unity3D Neural Network
This is a C# implementation of a simple feedforward neural network.
<br /><br />
This neural network can be used to solve any problems where an input vector of values from 0-1 results in an output vector of values from 0-1.<br />
To use a neural network you must create a Network and a Dataset
```C#
private static NeuralNetwork.Network net;
private static List<DataSet> dataSets; 

//Initialize Network and Dataset
void Awake()
{
    int numInputs, numHiddenLayers, numOutputs;
    net = new NeuralNetwork.Network(numInputs, numHiddenLayers, numOutputs);
    dataSets = new List<DataSet>();
}

```
You can add data points to the dataset using 
```C#
dataSets.Add(new DataSet(double[] inputs, double[] desiredOutputs));
```
Train the network with by calling
```C#
net.Train(dataSets, MinimumError);
```
MinimumError is the mimimum desired error for the network (The network will run the same dataset multiple times if it does not have enough datapoints to hit the minimum error with one trial).
<br /><br />
Finally, you can test an input
```C#
double[] testInput = {0, 1, 0.2, 0.13};
double[] results = net.Compute(testInput);
```

<br /><br />
##Sample Scene
This project has a demo scene showing the power of the network by using it to determine which color text is easiest to read on randomly colored backgrounds.<br />
![Alt text](http://i.imgur.com/IBu2xU7.png)
<br />

Users can select the color easiest to read, and after a few trials ( > 10 ) the Neural Network will show its pick for 'easiest to read color' for the given background.

