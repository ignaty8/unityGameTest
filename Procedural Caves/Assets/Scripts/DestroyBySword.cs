using UnityEngine;
using System.Collections;

public class DestroyBySword : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	void OnTriggerEnter(Collider other){
		if (other.tag == "Sword") {
			Destroy(gameObject);
		}
	}
}
