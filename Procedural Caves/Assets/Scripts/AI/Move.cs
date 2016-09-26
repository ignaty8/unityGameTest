using UnityEngine;
using System.Collections;
using System;

public class Move : MonoBehaviour,IAction {
	public PathFinder PathfinderScript;
	[HideInInspector] public bool moving;
	public BaseAI BaseAIScript;
	Transform player;
	Vector3 PlayerPositionMemory;
	PathGridGenerator PathGridGeneratorScript;
	GameObject GridGen;
	PathGridGenerator.Graph graph;
	List<Pathfinder.NodePathFinder> waypoints;

	// Use this for initialization
	void Start () {
		PathfinderScript = GetComponent<PathFinder>();
		moving = false;
		BaseAIScript = GetComponent<BaseAI> ();
		player = GameObject.FindGameObjectWithTag ("Player").transform;
		GridGen = GameObject.FindGameObjectWithTag ("GridGen");
		PathGridGeneratorScript = GridGen.GetComponent<PathGridGenerator>;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void ActionLoop(){
		//if notice is above 200 the AI will know WHERE the Player is
		if (BaseAIScript.notice > 200) {
			
			PlayerPositionMemory = player.transform.position;

			HowToGoTo (PlayerPositionMemory);
		}
		
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
		waypoints  = PathFinder.Pathfinder (CurrentCoords,ObjectiveCoords,graph);
	}

	public void GoTo(){
		
	}
}
