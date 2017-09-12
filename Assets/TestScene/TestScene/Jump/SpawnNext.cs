using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnNext : MonoBehaviour {
	bool wasSpawned = false;

	public void OnTriggerEnter(Collider other){
		if (!wasSpawned) {
			GameObject newGround = Instantiate (Resources.Load ("Terrain") as GameObject);
			newGround.transform.localScale = this.transform.localScale - Vector3.right * this.transform.localScale.x + Random.Range (8, 12F) * Vector3.right; 
			newGround.transform.position = this.transform.position + (Vector3.right * this.transform.localScale.x) + Random.Range (1, 10) / 5 * Vector3.right + newGround.transform.localScale.x / 4 * Vector3.right; 
			wasSpawned = true; 
		} 
	}
}
