using UnityEngine;
using System.Collections;

public class PuritySentinel : MonoBehaviour, IEnemy {
	public AIType type;

	// Use this for initialization
	void Start () {
	
	}

	void Awake(){
		initialise ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void ActionLoop(){
	}

	public void initialise(){
		type = GetComponent<AIType>();
		type.range = 100;
		type.damage = 1;
		type.toughness = 1;
		type.speed = 4;
		type.SpecBehavRange = 200;
		type.height = new Vector3(0,1,0);
		type.TypeName = "PuritySentinel";

	}
}
