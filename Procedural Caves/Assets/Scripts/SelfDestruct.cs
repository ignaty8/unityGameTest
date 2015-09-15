using UnityEngine;
using System.Collections;

public class SelfDestruct : MonoBehaviour {

	public float delay = 4;

	// Use this for initialization
	void Start () {
		// Destroys the object running it after "delay" seconds
		Invoke ("SelfRemove", delay);
	}
	
	void SelfRemove(){
		Destroy (gameObject);
	}
}
