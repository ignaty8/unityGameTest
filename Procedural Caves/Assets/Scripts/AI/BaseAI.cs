using UnityEngine;
using System.Collections;

//This Script is supposed to be used by EVERY AI in the Game with no distinction or reference to distinctions
//that way we avoid repeating ourselves
//

//currently it just sets what state the AI will be in

public class BaseAI : MonoBehaviour {


	//Stealth-Variables//
	//Fairely Basic Stealth System I guess ?
	//notice determins if the creature "noticed you"
	//if notice = 100 or more it did, it will stop following you once notice went down under 100
	//if the creatur sees you notice += 1 * attention * attention /distance
	//Once you are off its line of sight it can still follow you
	//the idea being that it still realises you are there (simulates sound/basic local knowledge of creature) and thus tries to follow you
	public float attention;
	public float notice;
	public float test;
	bool noticed;
	float notice_threshold = 100f;

	//GettingAIType variables
	public AIType type;
	//Getting HealthController Variables
	public HealthController HealthCntrl;

	//Timer-Variables//
	//Amount of time between vision checks
	public float checkTimer = 0.5f;
	float timer;

	//Sight-Variables//
	//Affected layermasks
	public LayerMask myLayerMask;

	//Self-explanetory
	Transform player;



	//Height of Player & creature//
	public Vector3 height;
	//we should probably get this from the player but for now...
	public Vector3 player_height;





	//Start Happens AFTER Awake
	// Use this for initialization
	void Start () {
		noticed = false;
		player = GameObject.FindGameObjectWithTag ("Player").transform;
		timer = 0;
		height = type.height;

	}


	void Awake ()
	{
		HealthCntrl = GetComponent<HealthController>;
		type = GetComponent<AIType>;
		HealthCntrl.objectHealthMax = 100 * type.toughness;
	}

	// Update is called once per frame
	void Update () {
		//Timer
		timer += Time.deltaTime;

		//LifeCheck
		//AmIDead(HealthCntrl.objecthealth);

		//Checking vision
		vision();


	}


	//Can I see the player ?
	void vision(){
		//We do NOT want to check at every frame otherwise the notice will go up way to fast
		//WARNING : Will have to test this but I suspect modulus or smthing is more appropriate
		if (timer >= checkTimer) {

			//basically raycasting to check if we can see player
			Ray LoS = new Ray (transform.position + height, player.transform.position + player_height - transform.position);
			//FairlySelf-Explanetory
			RaycastHit Hit;

			//Just in case : physics.raycast(Ray, output RaycastHit, float Raycast_Range, LayersMask stuff_we_can_hit)
			if (Physics.Raycast (LoS, out Hit, 40, myLayerMask)) {
				//Gets what we hit and checks it
				if (Hit.collider.CompareTag ("Player")) {
					notice += 1 * attention * attention 
						/ Vector3.Distance(transform.position + height,player.transform.position + player_height);
				}
			}

			timer = 0;
		}
	}


	//Detemnines current status
	void StatusCheck(){
		//Updates Notice Status
		CheckNotice ();

		//We could easily add other states here, such as LowHP or smthing
		//Checking if we noticed player if yes pursue & attack, otherwise just chill
		if (noticed) {
			//Act & Idle are PlaceHolders, They will be in separate classes in the end
			type.act ();
		} else {
			//Act & Idle are PlaceHolders, They will be in separate classes in the end
			type.idle ();
		}
	}

	//Check Notice//
	void CheckNotice (){

		/*
		if notice >= 100 & noticed = false
		We just NOTICED the player hence :
		Increase notice & set noticed to true 
		*/
		if (notice >= notice_threshold && noticed == false) {
			noticed = true;
			notice = notice * 2;

			/*
		if notice << 100 & noticed = true
		We just Lost the player hence :
		 set noticed to false 
		*/
		} else if (notice < notice_threshold && noticed == true) {
			noticed = false;
		}

		//About Remaining Cases :
		/*
		if notice >= 100 & noticed = true
		Well then we noticed the player for a while and are in pursuite (presumably) 
		*/
		/*
		if notice << 100 & noticed = false
				We just have YET to notice the player
				so nothing happens
				*/
		}

	//I am not sure, will we use the same system as for the other objects for health ?
	/*
	//AI checks if it is alive
	public void AmIDead(float health){
		if (health <= 0) {
			//if health is below 0 then you are dead
			death()
		}
	}

	//AI takes the "easy" way out
	public void death(){
		//Suicide & stuff
		//Disposing of corpses
		Destroy(GameObject);
	}
	*/
}
