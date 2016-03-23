using UnityEngine;
using System.Collections;
using NeuralNetwork;
using UnityEngine.UI;
using System.Collections.Generic;

public class ColorPicking : MonoBehaviour {

	//Neural Network Variables
	private const double MinimumError = 0.01;
	private const TrainingType TrType = TrainingType.MinimumError;
	private static NeuralNet net;
	private static List<DataSet> dataSets; 

	public Image I1;
	public Image I2;

	public GameObject pointer1;
	public GameObject pointer2;

	bool trained;

	int i = 0;

	// Use this for initialization
	void Start () 
	{
		//Input - 3 (r,g,b) -- Output - 1 (Black/White)
		net = new NeuralNet(3, 4, 1);
		dataSets = new List<DataSet>();
		Next();
	}

	void Next()
	{
		Color c = new Color(Random.Range(0,1f), Random.Range(0,1f), Random.Range(0,1f));
		I1.color = c;
		I2.color = c;
		double[] C = {(double)I1.color.r, (double)I1.color.g, (double)I1.color.b};
		if(trained)
		{
			double d = tryValues(C);
			if(d > 0.5)
			{
				pointer1.SetActive(false);
				pointer2.SetActive(true);
			}
			else
			{
				pointer1.SetActive(true);
				pointer2.SetActive(false);
			}
		}
	}
	
	public void Train(float val)
	{ 
		double[] C = {(double)I1.color.r, (double)I1.color.g, (double)I1.color.b};
		double[] v = {(double)val};
		dataSets.Add(new DataSet(C, v));

		i++;
		if(!trained && i%10 == 9)
			Train();

		Next();

	}

	private void Train()
	{
		net.Train(dataSets, MinimumError);
		trained = true;
	}

	double tryValues(double[] vals)
	{
	 	double[] result = net.Compute(vals);
	 	return result[0];
	}


}
