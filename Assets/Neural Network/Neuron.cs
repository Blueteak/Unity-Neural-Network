using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace NeuralNetwork
{
	public class Neuron 
	{
		public List<Synapse> InputSynapses { get; set; }
		public List<Synapse> OutputSynapses { get; set; }
		public double Bias { get; set; }
		public double BiasDelta { get; set; }
		public double Gradient { get; set; }
		public double Value { get; set; }

		public Neuron()
			{
				InputSynapses = new List<Synapse>();
				OutputSynapses = new List<Synapse>();
				Bias = NeuralNet.GetRandom();
			}

			public Neuron(IEnumerable<Neuron> inputNeurons) : this()
			{
				foreach (var inputNeuron in inputNeurons)
				{
					var synapse = new Synapse(inputNeuron, this);
					inputNeuron.OutputSynapses.Add(synapse);
					InputSynapses.Add(synapse);
				}
			}

			public virtual double CalculateValue()
			{
				return Value = Sigmoid.Output(InputSynapses.Sum(a => a.Weight * a.InputNeuron.Value) + Bias);
			}

			public double CalculateError(double target)
			{
				return target - Value;
			}

			public double CalculateGradient(double? target = null)
			{
				if(target == null)
					return Gradient = OutputSynapses.Sum(a => a.OutputNeuron.Gradient * a.Weight) * Sigmoid.Derivative(Value);

				return Gradient = CalculateError(target.Value) * Sigmoid.Derivative(Value);
			}

			public void UpdateWeights(double learnRate, double momentum)
			{
				var prevDelta = BiasDelta;
				BiasDelta = learnRate * Gradient;
				Bias += BiasDelta + momentum * prevDelta;

				foreach (var synapse in InputSynapses)
				{
					prevDelta = synapse.WeightDelta;
					synapse.WeightDelta = learnRate * Gradient * synapse.InputNeuron.Value;
					synapse.Weight += synapse.WeightDelta + momentum * prevDelta;
				}
			}

	}

	public class Synapse
	{
		public Neuron InputNeuron { get; set; }
		public Neuron OutputNeuron { get; set; }
		public double Weight { get; set; }
		public double WeightDelta { get; set; }

		public Synapse(Neuron inputNeuron, Neuron outputNeuron)
		{
			InputNeuron = inputNeuron;
			OutputNeuron = outputNeuron;
			Weight = NeuralNet.GetRandom();
		}
	}

	public static class Sigmoid
	{
		public static double Output(double x)
		{
			return x < -45.0 ? 0.0 : x > 45.0 ? 1.0 : 1.0 / (1.0 + Mathf.Exp((float)-x));
		}

		public static double Derivative(double x)
		{
			return x * (1 - x);
		}
	}

	public class DataSet
	{
		public double[] Values { get; set; }
		public double[] Targets { get; set; }

		public DataSet(double[] values, double[] targets)
		{
			Values = values;
			Targets = targets;
		}
	}

}
