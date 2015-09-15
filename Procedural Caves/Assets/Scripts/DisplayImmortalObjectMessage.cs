using UnityEngine;
using System.Collections;

public class DisplayImmortalObjectMessage : MonoBehaviour {

	private Collision collision;
	private Collider collider;
	public GameObject immortalObject;
	private GameObject mainCamera;

	private float timer;

	private bool hasHit = false;
	private bool hasTriggered = false;

	// Use this for initialization
	void Start () {
		mainCamera = GameObject.FindGameObjectWithTag ("MainCamera");
	
	}
	
	// Update is called once per frame
	void Update () {
		if (hasHit && hasTriggered) {
			//if (collision != null){
				foreach (ContactPoint contact in collision.contacts) {
				print(contact.thisCollider.name + " hit " + contact.otherCollider.name);
				//Debug.DrawRay(contact.point, contact.normal, Color.white);
				}
				
				Vector3 collisionPoint = collision.contacts[0].point;
				Quaternion cameraRotation = Quaternion.LookRotation(mainCamera.transform.position - collision.collider.transform.position, Vector3.up);
				//Instantiate(immortalObject, other.transform.position, cameraRotation);
				Instantiate(immortalObject, collisionPoint, cameraRotation);

			hasHit = false;
			hasTriggered = false;
			//}
		}
	}

//	void OnCollisionEnter(Collision _collision){
//		if (_collision.collider.tag == "Immortal") {
//			//collision = _collision;
//			//hasHit = true;
//			//if (collision != null){
////			foreach (ContactPoint contact in collision.contacts) {
////				print(contact.thisCollider.name + " hit " + contact.otherCollider.name);
////				//Debug.DrawRay(contact.point, contact.normal, Color.white);
////			}
//			
//			Vector3 collisionPoint = collision.contacts[0].point;
//			Quaternion cameraRotation = Quaternion.LookRotation(mainCamera.transform.position - transform.position, Vector3.up);
//			//Instantiate(immortalObject, other.transform.position, cameraRotation);
//			Instantiate(immortalObject, collisionPoint, cameraRotation);
//			
//			//hasHit = false;
//			//hasTriggered = false;
//			//}
//		}
//	}

	void OnTriggerEnter(Collider other){
		if (other.tag == "Immortal" && Time.time >= timer) {

			//collider = other;
			//Vector3 collisionPosition = other.;

			//if (collision != null){
			//			foreach (ContactPoint contact in collision.contacts) {
			//				print(contact.thisCollider.name + " hit " + contact.otherCollider.name);
			//				//Debug.DrawRay(contact.point, contact.normal, Color.white);
			//			}
			
			//Vector3 collisionPoint = collision.contacts[0].point;
			Quaternion cameraRotation = Quaternion.LookRotation (mainCamera.transform.position - transform.position, Vector3.up);
			//Instantiate(immortalObject, other.transform.position, cameraRotation);
			Instantiate (immortalObject, transform.position, cameraRotation);

			timer = Time.time + 1.4f;
			//hasHit = false;
			//hasTriggered = false;
			//}

			//hasTriggered = true;
		} else if (other.tag == "Destructible") {
			ControlledSelfDestruct destructScript = other.GetComponent<ControlledSelfDestruct>();
			destructScript.InitiateRemove();
		}
	}

//	void OnTriggerExit(Collider other){
//		if (other.tag == "Sword") {
//			hasHit = false;
//		}
//	}
}
