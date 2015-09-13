using UnityEngine;
using System.Collections;

public class MinimapCameraController : MonoBehaviour {

	public Transform target;
	private Vector3 targetPosition;

	// Use this for initialization
	void Start () {
		target = GameObject.FindGameObjectWithTag ("Player").transform;
	}
	
	// Update is called once per frame
	void Update () {
		targetPosition = new Vector3 (target.position.x, 144, target.position.z);
		transform.position = targetPosition;
	}
}
