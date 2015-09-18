using UnityEngine;
using System.Collections;
using Leap;

public class PlayerController : MonoBehaviour {
	
	private Transform cameraTransform;
	Vector3 offset;

	Rigidbody playerRigidbody;
	Vector3 velocity;

	// Some Leap Motion stuff:
	Controller controller = new Controller ();

	Vector3 forwardMovement;
	public float moveForwardSensitivity = 10;

	// Use this for initialization
	void Start () {
		playerRigidbody = GetComponent<Rigidbody> ();
		cameraTransform = GameObject.FindGameObjectWithTag ("MainCamera").transform;
	}
	
	// Update is called once per frame
	void Update () {
		
		forwardMovement = Vector3.zero;

		CheckHand ();

		offset = -(transform.position - cameraTransform.position);
		offset.y = 0;

		velocity = new Vector3 (Input.GetAxisRaw ("Horizontal"), Input.GetAxisRaw("Jump"), Input.GetAxisRaw ("Vertical")).normalized * 10;

		velocity += forwardMovement;

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

	void CheckHand(){
		Frame frame = controller.Frame ();
		HandList hands = frame.Hands;
		foreach (Hand hand in hands) {
			if(hand.IsValid){
				if(hand.IsLeft){
					LeapMover(hand);
				}
			}
		}
	}

	void LeapMover(Hand hand){
		// Warning, this is in radians!
		float handPitch = Mathf.Clamp (hand.PalmNormal.Pitch + Mathf.PI/2, -1.4f, 1.4f);
		//Debug.Log (handPitch);
		if ((handPitch < -.25f || handPitch > .25f)) {// || (handPitch < -.5f && handPitch > -Mathf.PI - .5f)) {
			forwardMovement = new Vector3 (0, 0, handPitch ) * Time.deltaTime * moveForwardSensitivity;
		} else {
		}
	}
}