using UnityEngine;
using System.Collections;
using System.Collections.Generic;
//Might remove the above, but I need the below to use Maths
using System;

public class PathFinder : MonoBehaviour {
	//where this Pathfinder "leads"
	public PathGridGenerator.Coords objective;
	//how close until this pathfinder considers we are there
	public float tolerance;
	//RBN, Random Big Number
	//nothin in this code should be bigger
	static float infinity = Single.MaxValue;
	//Placeholder used sometimes as default value for scripts where it is not obvious we will find a value
	static PathGridGenerator.Coords origin_coords = new PathGridGenerator.Coords(0,0,0);
	static NodePathFinder origin_node = new NodePathFinder(origin_coords);


	//Should be imported, but I will put it here for now
	// Min distance between separate points on all 3 axes.
	public const double COORD_SENSITIVITY = 0.01;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
	
	}


	//Pathfinder function//

	//Pathfinder function//

	//Pathfinder function//

	//PathGridGenerator is another Script/monobehaviour class containing the Node & Coords Class
	public List<NodePathFinder> Pathfinder(PathGridGenerator.Coords OriginCoords, PathGridGenerator.Coords DestinationCoords, PathGridGenerator.Graph graph){
		//This pathfinder uses the standard A* pathfinding program




		//Initialisation//
		//Getting Dictionary I guess ?
		SortedDictionary<PathGridGenerator.Coords,PathGridGenerator.Node> dictionary;
		dictionary = graph.GetDictionary();
		SortedDictionary<PathGridGenerator.Coords,NodePathFinder> InternalMemory
																=
																new SortedDictionary<PathGridGenerator.Coords,NodePathFinder>();



		//Setting up origin & destination
		NodePathFinder origin = GetNewNodePathfinder (dictionary, InternalMemory, OriginCoords);
		NodePathFinder destination = GetNewNodePathfinder (dictionary, InternalMemory, DestinationCoords);

		//Nodes we need to have Checked/Evaluated
		HashSet<NodePathFinder> OpenNodes = new HashSet<NodePathFinder>();

		//Nodes we have already Checked/Evaluated
		HashSet<NodePathFinder> ClosedNodes = new HashSet<NodePathFinder>();

		//We are using Hashsets for optimisation reasons
		//arrays have issues with modifying specific elements.
		//lists are less efficient
		//dictionaries, well not sure what the point of a key would be here
		//etc

		//At first only origin is known
		OpenNodes.Add(origin);

		//Set from which we most efficiently access current node
		//if set contains multiple elements, will eventually boil down to the most efficient one
		HashSet<NodePathFinder> RelativeOrigin;

		//Evidently origin's gcost is 0, as it does not take you any time to get there
		origin.gcost = 0;

		//Now setting origin's hcost
		//okay may be ineffecient
		origin.hcost = origin.EstimateHCost(destination.coords);

		//some variables for the loop
		NodePathFinder NeighbouringNode;
		NodePathFinder CurrentNode;
		float temp_gcost;

		//End of Initialisation//



		while(OpenNodes.Count > 0){
			//Get the Currently Cheapest Node
			CurrentNode = GetCheapest(OpenNodes);

			//Since we are dealing with this node it has to be removed from the "to do list"
			OpenNodes.Remove(CurrentNode);
			//And Added to the "done" list
			ClosedNodes.Add(CurrentNode);

			//Dealing with every adjacent tile now
			foreach (PathGridGenerator.Coords neighbour in CurrentNode.neighbours){
				//Setting them up in the system (ie adding the NeighbouringNode)
				NeighbouringNode = GetNewNodePathfinder (dictionary, InternalMemory, neighbour);
				if(CurrentNode == destination){
					//if we arrived then we use the memory stored in each node of where it came from to backtrack
					return GetPath(origin,CurrentNode,InternalMemory);
				}

				//Skip this whole procedure if we already dealt with that neighbour
				if(ClosedNodes.Contains(NeighbouringNode)){
					continue;
				}

				//Estimate of distance between start & here
				temp_gcost = CurrentNode.gcost + (float)GetDistance(NeighbouringNode.coords,CurrentNode.coords);


				if(OpenNodes.Contains(NeighbouringNode) == false){
					//If Neighbouring Node has not been considered yet then consider it now
					OpenNodes.Add(NeighbouringNode);
				}else{
					if(temp_gcost >= NeighbouringNode.gcost){
						//if the g cost of going to that neighbour by passing through the current node is greater
						//than its current estimate gcost, then clearly this is NOT a good path
						continue;
					}
				}
				//otherwise this path is da best !
				//Saving better gcost
				NeighbouringNode.gcost = temp_gcost;
				//Saving better hcost
				NeighbouringNode.hcost = NeighbouringNode.EstimateHCost(destination.coords);
				//saving better CameFrom, to be used now for backtracking
				NeighbouringNode.CameFrom = CurrentNode.coords;

			}


		}

		//this actually should NEVER happen, but just in case
		//at worst this pathfinder makes you immobile
		return GetPath (origin,origin,InternalMemory);


	}




	//Pathfinder Auxiliary Functions//

	//Pathfinder Auxiliary Functions//

	//Pathfinder Auxiliary Functions//


	//This script takes a coord
	//looks up the appropriat node in the dictionary
	//Sets up a NodePathFinder with identical values and standard pathfinder values
	//adds it to memory
	//returns that NodePathfinder
	public NodePathFinder GetNewNodePathfinder(
												SortedDictionary<PathGridGenerator.Coords,PathGridGenerator.Node> dictionary,
												SortedDictionary<PathGridGenerator.Coords,NodePathFinder> memory,
												PathGridGenerator.Coords coord){


		NodePathFinder output;

		PathGridGenerator.Node temp = dictionary [coord];

		output = new NodePathFinder (temp.coords, temp.neighbours);

		memory.Add (temp.coords, output);

		return output;
	}

	//Gets the node with lowest Fcost in Nodes
	public NodePathFinder GetCheapest(HashSet<NodePathFinder> Nodes){
		//DANGER DANGER, TEST BELOW
		//output stores the node which for now has lowest fcost
		//Initialisation
		NodePathFinder output;
		float smallestfcost;
		float CurrentFCost;
		//this is just to avoid having null values, shouldn't happen though in practice

		output = origin_node;


		//standard value
		smallestfcost = infinity + infinity;
		foreach(NodePathFinder node in Nodes){
			CurrentFCost = node.GetFcost();
			if(CurrentFCost < smallestfcost){
				smallestfcost = CurrentFCost;
				output = node;
			}
		}

		return output;

	}


	//Fairly Straighforward I think
	static double GetDistance(PathGridGenerator.Coords coords_01, PathGridGenerator.Coords coords_02){
		double output;

		output = 
			square(coords_01.x - coords_02.x)
			+ square(coords_01.y - coords_02.y)
			+ square(coords_01.z - coords_02.z);

		return root2(output);
	}

	//For cleanliness
	static double square(double double_){
		return Math.Pow(double_,2);
	}

	static double root2(double double_){
		return Math.Sqrt (double_);
		}

	public List<NodePathFinder> GetPath(
										NodePathFinder origin,
										NodePathFinder destination,
										SortedDictionary<PathGridGenerator.Coords,NodePathFinder> dictionary){


		//Gets the path leading to destination from origin
		//Actually I think its from destination to origin, depends on your perspective

		List<NodePathFinder> output = new List<NodePathFinder>();

		NodePathFinder temp_node;
		temp_node = destination;

		while(temp_node != origin){
			output.Add(temp_node);

			temp_node = dictionary[temp_node.coords];
		}

		output.Add(origin);

		return output;
	}



	//Used Classes//

	//Used Classes//

	//Used Classes//




	//So I need to change the node class a tiny bit
	public class NodePathFinder{

		public PathGridGenerator.Coords coords;

		public HashSet<PathGridGenerator.Coords> neighbours;

		//Added Part
		//this value estimates the "cost" of going from the origin to this Node in the a* algo
		public float gcost;
		//this value estimates the "cost" of going from the destination to this Node in the a* algo
		public float hcost;
		//unnecessary, just the sum of gcost & hcost. we COULD use this instead of hcost
		//unless it proves more efficient in someway I won't though
		//public float fcost;
		//Used in the algo again, its the best coord to go to this Node
		public PathGridGenerator.Coords CameFrom;



		//public Hash128 hash;

		public NodePathFinder(PathGridGenerator.Coords coords) {
			this.coords = coords.copy();
			//hash = new Hash128(coords.x);
			neighbours = new HashSet<PathGridGenerator.Coords>(new PathGridGenerator.CoordsComparer());
			//Setting Standard Value
			gcost = infinity;
			hcost = infinity;
			//doesn't matter really, will be changed
			CameFrom = coords.copy();
		}

		public NodePathFinder(PathGridGenerator.Coords coords, HashSet<PathGridGenerator.Coords> neighbours) {
			this.coords = coords.copy();
			this.neighbours = neighbours;
			//Setting Standard Value
			gcost = infinity;
			hcost = infinity;
			//doesn't matter really, will be changed
			CameFrom = coords.copy();
		}

		//This returns the total "cost" of going through this node to go to the destination
		public float GetFcost(){
			return gcost+hcost;
		}

		//Generates an approximation of the cost of getting to the destination (higly inacurate)
		public float EstimateHCost(PathGridGenerator.Coords destination){
			//Our "Estimation" is just going to be calculating the distance from here to there
			return (float)GetDistance(destination,this.coords);
		}
	}



}


