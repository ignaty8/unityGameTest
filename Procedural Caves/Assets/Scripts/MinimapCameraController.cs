using UnityEngine;
using System.Collections;

public class MinimapCameraController : MonoBehaviour {

	public Transform target;
	private Vector3 targetPosition;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		targetPosition = new Vector3 (target.position.x, 144, target.position.z);
		transform.position = targetPosition;
	}
}
