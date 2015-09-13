using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {
	
	private Transform cameraTransform;
	Vector3 offset;

	Rigidbody playerRigidbody;
	Vector3 velocity;

	// Use this for initialization
	void Start () {
		playerRigidbody = GetComponent<Rigidbody> ();
		cameraTransform = GameObject.FindGameObjectWithTag ("MainCamera").transform;
	}
	
	// Update is called once per frame
	void Update () {
		offset = -(transform.position - cameraTransform.position);
		offset.y = 0;

		velocity = new Vector3 (Input.GetAxisRaw ("Horizontal"), Input.GetAxisRaw("Jump"), Input.GetAxisRaw ("Vertical")).normalized * 10;

		float offsetAngle = Vector3.Angle (Vector3.forward, offset);
		float leftOffsetAngle = Vector3.Angle (Vector3.left, offset);
		float rightOffsetAngle = Vector3.Angle (Vector3.right, offset);

		if (leftOffsetAngle < rightOffsetAngle) {
			offsetAngle = -offsetAngle;
		}
		velocity = Quaternion.Euler (0, offsetAngle, 0) * velocity;
	}

	void FixedUpdate () {
		playerRigidbody.MovePosition (playerRigidbody.position + velocity * Time.fixedDeltaTime);
	}
}