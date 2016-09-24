using UnityEngine;
using System.Collections;

//This scrip contains the specifics of the AI (although they are set by other scripts)
//in addition it PERFORMS the action determined by the states set in the BaseAI

public class AIType : MonoBehaviour {
	//Range of attacks
	public float range;
	//Attack power
	public float damage;
	//affects damage taken by player
	public float toughness;
	//Special Behaviour describes behaviour specific to this AI
	//in practice this is ANY behaviour that is not just walking towards the player
	public float SpecBehavRange;
	//Height of creature
	public Vector3 height;
	//type
	public string TypeName;
	//Reference to BaseAI
	public BaseAI Base;

	//Self-explanetory
	Transform player;


	// Use this for initialization
	void Start () {
		player = GameObject.FindGameObjectWithTag ("Player").transform;
		if(TypeName == "PuritySentinel"){
			PuritySentinel script = GetComponent<PuritySentinel>;
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void act(){
		//Acting
	}

	public void idle(){
		//Idling
	}

	public void RangeCheck(){
		Vector3 RangeVector = player.transform.position - transform.position;
		if(RangeVector.Magnitude <= SpecBehavRange){
			if(TypeName == "PuritySentinel"){
				script
			}
		}
	}
}
