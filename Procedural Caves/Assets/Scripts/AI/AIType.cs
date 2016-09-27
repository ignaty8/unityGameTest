using UnityEngine;
using System.Collections;

//This scrip contains the specifics of the AI (although they are set by other scripts)
//in addition it PERFORMS the action determined by the states set in the BaseAI

public class AIType : MonoBehaviour {
	//Range of attacks
	[HideInInspector] public float range;
	//Attack power
	[HideInInspector] public float damage;
	//affects damage taken by player
	[HideInInspector] public float toughness;
	//Speed
	[HideInInspector] public float speed;
	//Special Behaviour describes behaviour specific to this AI
	//in practice this is ANY behaviour that is not just walking towards the player
	[HideInInspector] public float SpecBehavRange;
	//Height of creature
	[HideInInspector] public Vector3 height;
	//type
	[HideInInspector] public string TypeName;
	//Reference to BaseAI
	[HideInInspector] BaseAI Base;
	[HideInInspector] IEnemy EnemyScript;
	[HideInInspector] PuritySentinel SentinelScript;
	[HideInInspector] Move MoveScript;

	//Self-explanetory
	Transform player;


	// Use this for initialization
	void Start () {
		player = GameObject.FindGameObjectWithTag ("Player").transform;
		if (TypeName == "PuriteSentinel") {
			EnemyScript = SentinelScript;
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void act(){
		//Acting
		RangeCheck();
	}

	public void idle(){
		//Idling
	}

	public void RangeCheck(){
		Vector3 RangeVector = player.transform.position - transform.position;
		if (RangeVector.magnitude <= SpecBehavRange) {
			//Launches the Enemy Unique Script Action Loop
			EnemyScript.ActionLoop ();
		} else {
			//Moves The Enemy Closer To the Player
			MoveScript.ActionLoop();
		}
	}


}
