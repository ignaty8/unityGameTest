using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

	Rigidbody playerRigidbody;
	Vector3 velocity;

	// Use this for initialization
	void Start () {
		playerRigidbody = GetComponent<Rigidbody> ();
	}
	
	// Update is called once per frame
	void Update () {
		velocity = new Vector3 (Input.GetAxisRaw ("Horizontal"), Input.GetAxisRaw("Jump"), Input.GetAxisRaw ("Vertical")).normalized * 10;
	}

	void FixedUpdate () {
		playerRigidbody.MovePosition (playerRigidbody.position + velocity * Time.fixedDeltaTime);
	}
}