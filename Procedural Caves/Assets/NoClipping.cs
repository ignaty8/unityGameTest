using UnityEngine;
using System.Collections;

public class NoClipping : MonoBehaviour {

	//bool isColliding = false;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		//if (isColliding) {
			//transform.localPosition += Time.deltaTime * Vector3.forward;
		//}
	}

	void OnTriggerEnter(Collider other){
		if (other.tag == "Immortal") {
			transform.localPosition += .4f * Vector3.forward;
			//isColliding = true;
		}
	}
}
