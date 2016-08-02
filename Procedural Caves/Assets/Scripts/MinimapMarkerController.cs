using UnityEngine;
using System.Collections;

public class MinimapMarkerController : MonoBehaviour {

    private GameObject mainCamera;

    private Quaternion rotation;
    private Vector3 eulerRotation;

    // This keeps track of the desired offset between marker and camera rotations.
    private Vector3 baseRotation;

    // Finds main camera and calculates offset
    void Start () {
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
        baseRotation = transform.rotation.eulerAngles - mainCamera.transform.rotation.eulerAngles;
	}
	
	// Turns minimap object to follow main camera
	void Update () {
        Quaternion rotation = mainCamera.transform.rotation;
        Vector3 eulerRotation = rotation.eulerAngles;
        eulerRotation.x = 0;
        eulerRotation.z = 0;
        eulerRotation += baseRotation;
        transform.rotation = Quaternion.Euler(eulerRotation);
	}
}
