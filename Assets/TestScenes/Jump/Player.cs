using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using NeuralNetwork;

public class Player : MonoBehaviour {
	public GameObject currentGroundTile;
	public GameObject restartText; 

	//This is the distance between the player and the end of the current staying tile / platform
	public double distanceInPercent;

	public double canJump;

	private const double timeBetweenDatasets = 0.3; 
	float countedTime = 0;

	public NetLayer net; 
	public void Start(){
		canJump = 1; 
	}


	// Update is called once per frame
	void Update () {
		countedTime += Time.deltaTime; 

		//Move the parent (Camera + player)
		this.transform.parent.position += Vector3.right*3F * Time.deltaTime; 
		if (Input.GetKeyDown (KeyCode.Space)) {
			jump (); 
		} else {
			//Adding a new dataset
			if (countedTime >  timeBetweenDatasets && !NetLayer.trained) {
				countedTime = 0; 
				net.Train (canJump, 0);
			}
		}

		//Calculate the distance from player to the end of the current triggered platform in percent
		Vector3 startPointTile = currentGroundTile.transform.position - (Vector3.right * currentGroundTile.transform.localScale.x) / 2; 
		Vector3 endPointTile = currentGroundTile.transform.position + (Vector3.right * currentGroundTile.transform.localScale.x) / 2; 
		Vector3 platformLength = endPointTile - startPointTile; 
		Vector3 distanceToEndOfPlatform = endPointTile - transform.position;
		distanceInPercent = distanceToEndOfPlatform.x / platformLength.x; 

		//Just for visualization draw a line to the end of the platform
		this.gameObject.GetComponent<LineRenderer>().SetPositions (new Vector3[]{ transform.position, endPointTile }); 

		if (distanceToEndOfPlatform.x < 0) {
			distanceToEndOfPlatform = Vector3.zero; 
		}

		checkForGameOver (); 
	}

	//Cube will trigger a platform and can jump again, also we also to transmit the currently triggered platform to the network
	public void OnTriggerEnter(Collider other){
		canJump = 1; 
		currentGroundTile = other.gameObject; 
	}

	private void checkForGameOver(){
		//The player is basically game over
		if (transform.position.y < -24) {
			restartText.SetActive (true); 
			Time.timeScale = 0; 
		}
	}

	//Perform a jump
	public void jump(){
		if (canJump == 1) {
			//Send dataset to the net 
			GameObject.Find("Network").GetComponent<NetLayer>().Train(1, 1); 

			//Jump
			this.gameObject.GetComponent<Rigidbody> ().AddForce (Vector3.up* 450F); 
			canJump = 0; 
		}
	}
}
