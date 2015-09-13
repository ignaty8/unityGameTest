using UnityEngine;
using System.Collections;

public class FPCameraController : MonoBehaviour {
	
	//public GameObject player;
	
	//private Vector3 playerPos;
	//private Vector3 oldPlayerPos;
	private Vector3 offset;
	//private float offsetDistance;
	//private float rotOffset;
	
	//public int lookSensitivity;
	public int zoom3rdPersonSensitivity = 1;
	public bool isZoomDirectionInverted = false;
	private int zoomDirection = -1;
	public int minZoomDistance = 1;
	public int maxZoomDistance = 14;
	private float cameraRotation;
	
	public Transform target;
	
	private float distance = -0.4f;
	
	private float xSpeed = 250.0f;
	private float ySpeed = 120.0f;
	
	public int yMinLimit = -5;
	public int yMaxLimit = 5;
	
	private float x = 0.0f;
	private float y = 0.0f;

	public float cameraHeight = 1.78f;
	// Use this for initialization
	void Start () {
		//offsetDistance = Mathf.Sqrt((transform.position - player.transform.position).sqrMagnitude);	// Watch out: The length of Magnitude is a square!
		//oldPlayerPos = player.transform.position;
		target = GameObject.FindGameObjectWithTag ("Player").transform;
		
		Vector3 angles = transform.eulerAngles;
		x = angles.y;
		y = angles.x;
	}
	
	// LateUpdate is run just after Update!
	void LateUpdate () {
		//playerPos = player.transform.position;
		//offset = (transform.position - playerPos).normalized;
		//offset.y = 0;
		//cameraRotation = transform.rotation.eulerAngles.y;
		//rotOffset = Input.GetAxisRaw ("Mouse X") * lookSensitivity;
		//transform.RotateAround(playerPos, Vector3.up, rotOffset);
		//transform.rotation = Quaternion.Euler(Mathf.Acos(offset.x),Mathf.Acos(offset.y),Mathf.Acos(offset.z));
		//if (playerPos != oldPlayerPos) {
		//offset = (transform.position - player.transform.position).normalized;
		//transform.position = playerPos + offset * offsetDistance;
		//}
		//oldPlayerPos = playerPos;
		
		
		if (target) {
			x += Input.GetAxis("Mouse X") * xSpeed * 0.02f;
			y -= Input.GetAxis("Mouse Y") * ySpeed * 0.02f;
			if (isZoomDirectionInverted) {
				zoomDirection = -1;
			} else { zoomDirection = 1; }
//			float zoom3rdPerson = Input.GetAxis("Mouse ScrollWheel") * zoomDirection;
//			distance += zoom3rdPerson * zoom3rdPersonSensitivity;
//			distance = Mathf.Clamp (distance, minZoomDistance, maxZoomDistance);
			
			y = ClampAngle(y, yMinLimit, yMaxLimit);
			
			Quaternion rotation = Quaternion.Euler(y, x, 0);
			Vector3 distanceVector = new Vector3(0, 0, -distance);
			Vector3 position = rotation * distanceVector + target.position;
			position.y += cameraHeight;
			
			transform.rotation = rotation;
			transform.position = position;
		}
	}
	
	static float ClampAngle (float angle, float min, float max) {
		if (angle < -360)
			angle += 360;
		if (angle > 360)
			angle -= 360;
		return Mathf.Clamp (angle, min, max);
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