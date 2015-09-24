using UnityEngine;
using System.Collections;

public class GenerateNewLevel : MonoBehaviour {

	GameObject mapGenerator;
	MapGenerator mapGeneratorScript;

	ButtonDemoNoToggle onGraphics;
	bool pressed = false;

	// Use this for initialization
	void Start () {
		onGraphics = transform.parent.FindChild ("Button").GetComponent<ButtonDemoNoToggle>();
	}
	
	// Update is called once per frame
	void Update () {
		CheckButton ();
	}

	void CheckButton(){
		if (!pressed) {
			if (onGraphics.isActive) {
				
				Debug.Log ("pressed");
				pressed = true;
				RegenerateNewLevel ();
			}
		} else {
			if (!onGraphics.isActive) {
				pressed = false;
			}
		}
	}

	void RegenerateNewLevel(){
		mapGenerator = GameObject.FindGameObjectWithTag ("MapGenerator");
		mapGeneratorScript = mapGenerator.GetComponent<MapGenerator> ();
		mapGeneratorScript.GenerateMap ();
	}
}
