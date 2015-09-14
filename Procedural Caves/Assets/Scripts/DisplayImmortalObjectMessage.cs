using UnityEngine;
using System.Collections;

public class DisplayImmortalObjectMessage : MonoBehaviour {

	public GameObject immortalObject;
	private GameObject mainCamera;

	// Use this for initialization
	void Start () {
		mainCamera = GameObject.FindGameObjectWithTag ("MainCamera");
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter(Collider other){
		if (other.tag == "Sword") {
			Quaternion cameraRotation = Quaternion.LookRotation(mainCamera.transform.position - other.transform.position, Vector3.up);
			Instantiate(immortalObject, other.transform.position, cameraRotation);
		}
	}
}
