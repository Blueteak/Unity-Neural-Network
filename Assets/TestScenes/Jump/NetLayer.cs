using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NeuralNetwork;
using UnityEngine.UI;
using System.Threading;

public class NetLayer : MonoBehaviour {

	//Neural Network Variables
	private const double MinimumError = 0.1;
	private const TrainingType TrType = TrainingType.MinimumError;
	private static NeuralNet net;
	public static List<DataSet> dataSets; 
	public static bool trained;

	private int collectedDatasets = 0;
	private const int maxNumberOfDatasets = 60; 

	public Player player; 

	// Use this for initialization
	void Start () {
		//Initialize the network 
		net = new NeuralNet(2, 3, 1);
		dataSets = new List<DataSet>();
	}
	
	// Update is called once per frame
	void Update () {
		//Let the network decide if the player should jump
		if (trained) {
			double result = compute (new double[]{ player.distanceInPercent, player.canJump });
			if (result > 0.5) {
				player.jump (); 
			}
		}
	}

	public void Train(double canJump, double jumped)
	{ 
		double[] C = {player.distanceInPercent, canJump};
		double[] v = {jumped};
		dataSets.Add(new DataSet(C, v));

		collectedDatasets++;
		if (!trained && collectedDatasets == maxNumberOfDatasets) {
			print ("Start training of the network."); 
			TrainNetwork();
		}
	}

	double compute(double[] vals)
	{
		double[] result = net.Compute(vals);
		return result[0];
	}

	public static void TrainNetwork()
	{
		net.Train(dataSets, MinimumError);
		trained = true;
		print ("Trained!"); 
	}
}
