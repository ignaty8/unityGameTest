using UnityEngine;
using System.Collections;

public class ControlledSelfDestruct : MonoBehaviour {

	public float delay = 4;

	// Use this for initialization
	public void InitiateRemove () {
		Invoke ("SelfRemove", delay);
	}
	
	void SelfRemove(){
		Destroy (gameObject);
	}
}
