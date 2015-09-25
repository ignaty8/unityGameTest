using UnityEngine;
using System.Collections;

public class TextBagroundGenerator : MonoBehaviour {

	// Use this for initialization
	void Start() {
		Bounds textBounds = transform.parent.GetComponent<Renderer>().bounds;
		GetComponent<MeshFilter> ().mesh.bounds = textBounds;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
