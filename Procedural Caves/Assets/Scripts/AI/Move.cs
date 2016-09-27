using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class Move : MonoBehaviour,IAction {
	public PathFinder PathfinderScript;
	[HideInInspector] public bool moving;
	BaseAI BaseAIScript;
	AIType AITypeScript;
	Transform player;
	Vector3 PlayerPositionMemory;
	PathGridGenerator PathGridGeneratorScript;
	GameObject GridGen;
	PathGridGenerator.Graph graph;
	List<PathFinder.NodePathFinder> waypoints = new List<PathFinder.NodePathFinder>();
	PathFinder.NodePathFinder CurrentObjective;
	Vector3 difference;
	float sensitivity = 0.02f;


	// Use this for initialization
	void Start () {
		PathfinderScript = GetComponent<PathFinder>();
		AITypeScript = GetComponent<AIType> ();
		moving = false;
		BaseAIScript = GetComponent<BaseAI> ();
		player = GameObject.FindGameObjectWithTag ("Player").transform;
		GridGen = GameObject.FindGameObjectWithTag ("GridGen");
		PathGridGeneratorScript = GridGen.GetComponent<PathGridGenerator>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void ActionLoop(){
		//if notice is above 200 the AI will know WHERE the Player is
		if (BaseAIScript.notice > 200) {
			
			PlayerPositionMemory = player.transform.position;

			//Finding out updated path
			HowToGoTo (PlayerPositionMemory);
		}

		//Moving using Waypoints
		MoveTroughWaypoints ();
		/*
		Note:
		This is ONLY called if the AI Noticed the player, which implies that notice was at one point above 200
		(See Base AI)
		*/
	}

	//Represents the method that asks a path to the objective
	//in practice consider this an "adapter" that adapts our info to the one required by the Pathfinder method
	public void HowToGoTo(Vector3 objective){
		//Getting objective coords
		PathGridGenerator.Coords ObjectiveCoords = new PathGridGenerator.Coords (
																				(double)objective.x,
																				(double)objective.y,
																				(double)objective.z);
		PathGridGenerator.Coords CurrentCoords = new PathGridGenerator.Coords(
																			(double)transform.position.x,
																			(double)transform.position.y,
																			(double)transform.position.z);

		//needs some work, cannot acces graph for instance
		waypoints  =  PathFinder.Pathfinder (CurrentCoords,ObjectiveCoords,graph);
	}

	public void MoveTroughWaypoints(){
		//Remember we were BACKTRACKING in Pathfinder, so we have to do this
		CurrentObjective = waypoints[waypoints.Count - 1];
		//Setting put difference Vector
			difference = transform.position 
			- PathGridGeneratorScript.CoordsToVector(CurrentObjective.coords);

		if (difference.magnitude > sensitivity) {
			//I believe y is up/down axis, and presumably we are not really flying
			difference = new Vector3 (difference.x, 0, difference.z);
			//Getting the direction vector of the movement
			difference.Normalize ();
			//setting up the movment vector
			difference = difference * AITypeScript.speed * Time.deltaTime;
			//Moving the AI
			//PS:feel free to change how we move the AI
			transform.Translate (difference);
		} else {
			//We reached the Waypoint so we need to elimate it
			waypoints.RemoveAt(waypoints.Count-1);
		}
	}
}
