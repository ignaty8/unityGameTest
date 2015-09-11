using UnityEngine;
using System.Collections;

public class HeightmapGenerator : MonoBehaviour {

	int[,] heightMap;
	int size;
	public int sizeMagnitude;

	void Start(){
		size = 2 ^ sizeMagnitude;
	}

	// Update is called once per frame
	void Update () {
	
	}
}
