using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {
	
	public GameObject player;

	private Vector3 playerPos;
	private Vector3 oldPlayerPos;
	private Vector3 offset;
	private float offsetDistance;
	private float rotOffset;

	public int lookSensitivity;
	private float cameraRotation;
	
	// Use this for initialization
	void Start () {
		offsetDistance = Mathf.Sqrt((transform.position - player.transform.position).sqrMagnitude);	// Watch out: The length of Magnitude is a square!
		oldPlayerPos = player.transform.position;
	}
	
	// LateUpdate is run just after Update!
	void LateUpdate () {
		playerPos = player.transform.position;
		//offset = (transform.position - playerPos).normalized;
		//offset.y = 0;
		//cameraRotation = transform.rotation.eulerAngles.y;
		rotOffset = Input.GetAxisRaw ("Mouse X") * lookSensitivity;
		transform.RotateAround(playerPos, Vector3.up, rotOffset);
		//transform.rotation = Quaternion.Euler(Mathf.Acos(offset.x),Mathf.Acos(offset.y),Mathf.Acos(offset.z));
		//if (playerPos != oldPlayerPos) {
			//offset = (transform.position - player.transform.position).normalized;
			//transform.position = playerPos + offset * offsetDistance;
		//}
		//oldPlayerPos = playerPos;
	}

	/*
	void Turning(){
		
		Ray camRay = Camera.main.ScreenPointToRay (Input.mousePosition);
		
		RaycastHit floorHit;
		
		if (Physics.Raycast (camRay, out floorHit, camRayLength, floorMask)) {
			
			Vector3 playerToMouse = floorHit.point - transform.position;
			playerToMouse.y = 0f;
			
			Quaternion newRotation = Quaternion.LookRotation (playerToMouse);
			playerRigidbody.MoveRotation (newRotation);
		}
	}*/
}