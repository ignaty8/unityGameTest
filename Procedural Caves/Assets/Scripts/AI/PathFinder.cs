using UnityEngine;
using System.Collections;
using System.Collections.Generic;
//Might remove the above, but I need the below to use Maths
using System;

public class PathFinder : MonoBehaviour {
	//where this Pathfinder "leads"
	public Coords objective;
	//how close until this pathfinder considers we are there
	public float tolerance;
	//RBN, Random Big Number
	//nothin in this code should be bigger
	static float infinity =100000000;
	//Placeholder used sometimes as default value for scripts where it is not obvious we will find a value
	static Coords origin_coords = new Coords(0,0,0);
	static Node origin_node = new Node(origin_coords);

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



	public List<Node> Pathfinder(Node origin, Node destination, Graph graph){
		//This pathfinder uses the standard A* pathfinding program


		//Initialisation//
		//Getting Dictionary I guess ?
		SortedDictionary<Coords,Node> dictionary;
		dictionary = graph.GetDictionary();
		SortedDictionary<Coords,Node> InternalMemory;

		//Nodes we need to have Checked/Evaluated
		HashSet<Node> OpenNodes = new HashSet<Node>();

		//Nodes we have already Checked/Evaluated
		HashSet<Node> ClosedNodes = new HashSet<Node>();

		//We are using Hashsets for optimisation reasons
		//arrays have issues with modifying specific elements.
		//lists are less efficient
		//dictionaries, well not sure what the point of a key would be here
		//etc

		//At first only origin is known
		OpenNodes.Add(origin);

		//Set from which we most efficiently access current node
		//if set contains multiple elements, will eventually boil down to the most efficient one
		HashSet<Node> RelativeOrigin;

		//Evidently origin's gcost is 0, as it does not take you any time to get there
		origin.gcost = 0;

		//Now setting origin's hcost
		//okay may be ineffecient
		origin.hcost = origin.EstimateHCost(destination.coords);

		//some variables for the loop
		Node NeighbouringNode;
		Node CurrentNode;
		float temp_gcost;

		//End of Initialisation//



		while(OpenNodes.Count > 0){
			//Get the Currently Cheapest Node
			CurrentNode = GetCheapest(OpenNodes);

			//Since we are dealing with this node it has to be removed from the "to do list"
			OpenNodes.Remove(CurrentNode);
			//And Added to the "done" list
			ClosedNodes.Add(CurrentNode);


			foreach (Coords neighbour in CurrentNode.neighbours){
				NeighbouringNode = dictionary[neighbour];
				if(CurrentNode == destination){
					return GetPath(origin,CurrentNode,dictionary);
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
				//saving better CameFrom
				NeighbouringNode.CameFrom = CurrentNode.coords;

			}


		}

		return GetPath (origin,origin,dictionary);


	}




	//Pathfinder Auxiliary Functions//

	//Pathfinder Auxiliary Functions//

	//Pathfinder Auxiliary Functions//





	//Gets the node with lowest Fcost in Nodes
	public Node GetCheapest(HashSet<Node> Nodes){
		//DANGER DANGER, TEST BELOW
		//output stores the node which for now has lowest fcost
		//Initialisation
		Node output;
		float smallestfcost;
		float CurrentFCost;
		//this is just to avoid having null values, shouldn't happen though in practice

		output = origin_node;


		//standard value
		smallestfcost = infinity + infinity;
		foreach(Node node in Nodes){
			CurrentFCost = node.GetFcost();
			if(CurrentFCost < smallestfcost){
				smallestfcost = CurrentFCost;
				output = node;
			}
		}

		return output;

	}


	//Fairly Straighforward I think
	static double GetDistance(Coords coords_01, Coords coords_02){
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

	public List<Node> GetPath(Node origin,Node destination,SortedDictionary<Coords,Node> dictionary){
		//Gets the path leading to destination from origin
		//Actually I think its from destination to origin, depends on your perspective

		List<Node> output = new List<Node>();

		Node temp_node;
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
	public class Node{

		public Coords coords;

		public HashSet<Coords> neighbours;

		//Added Part
		//this value estimates the "cost" of going from the origin to this Node in the a* algo
		public float gcost;
		//this value estimates the "cost" of going from the destination to this Node in the a* algo
		public float hcost;
		//unnecessary, just the sum of gcost & hcost. we COULD use this instead of hcost
		//unless it proves more efficient in someway I won't though
		//public float fcost;
		//Used in the algo again, its the best coord to go to this Node
		public Coords CameFrom;



		//public Hash128 hash;

		public Node(Coords coords) {
			this.coords = coords.copy();
			//hash = new Hash128(coords.x);
			neighbours = new HashSet<Coords>(new CoordsComparer());
			//Setting Standard Value
			gcost = infinity;
			hcost = infinity;
			CameFrom = new Coords(0,0,0);
		}

		public Node(Coords coords, HashSet<Coords> neighbours) {
			this.coords = coords.copy();
			this.neighbours = neighbours;
			//Setting Standard Value
			gcost = infinity;
			hcost = infinity;
			CameFrom = new Coords(0,0,0);
		}

		//This returns the total "cost" of going through this node to go to the destination
		public float GetFcost(){
			return gcost+hcost;
		}

		//Generates an approximation of the cost of getting to the destination (higly inacurate)
		public float EstimateHCost(Coords destination){
			//Our "Estimation" is just going to be calculating the distance from here to there
			return (float)GetDistance(destination,this.coords);
		}
	}

	public class CoordsComparer : IEqualityComparer<Coords> {
		public bool Equals(Coords x, Coords y) {
			return PathGridGenerator.adjacentDoubles(x.x,y.x) && PathGridGenerator.adjacentDoubles(x.y, y.y) && PathGridGenerator.adjacentDoubles(x.z, y.z);
		}

		// Potential for problems from use of doubles....
		// But I don't even know what this will be used for.
		public int GetHashCode(Coords obj) {
			return (((int) (obj.x * 1/COORD_SENSITIVITY)).GetHashCode() * 179424691 + ((int)(obj.y * 1 / COORD_SENSITIVITY)).GetHashCode()) * 179425033 + ((int)(obj.z * 1 / COORD_SENSITIVITY)).GetHashCode();
		}
	}

	public class CoordsComparerFull : IComparer<Coords> {
		public int Compare(Coords x, Coords y) {
			if (PathGridGenerator.adjacentDoubles(x.x, y.x) && PathGridGenerator.adjacentDoubles(x.y, y.y) && PathGridGenerator.adjacentDoubles(x.z, y.z)) {
				return 0;
			} else if((!PathGridGenerator.adjacentDoubles(x.x, y.x) && x.x > y.x) || (PathGridGenerator.adjacentDoubles(x.x, y.x) && !PathGridGenerator.adjacentDoubles(x.y, y.y) && x.y > y.y) 
				|| (PathGridGenerator.adjacentDoubles(x.x, y.x) && PathGridGenerator.adjacentDoubles(x.y, y.y) && !PathGridGenerator.adjacentDoubles(x.z, y.z) && x.z > y.z)) {
				return 1;
			} else {
				return -1;
			}
		}
	}


	public class Coords {

		public double x, y, z;

		public Coords(double x,double y,double z) {
			this.x = x;
			this.y = y;
			this.z = z;
		}

		public Coords copy() {
			return new Coords(x, y, z);
		}
	}

	public class Graph {

		private SortedDictionary<Coords,Node> dictionary;

		// This Set type will make sure there are no duplicate nodes. (I think)
		// Not quite, since there is no compare method in Node right now... 2016.09.12
		private HashSet<Node> nodes;

		public Graph() {
			dictionary = new SortedDictionary<Coords, Node>(new CoordsComparerFull());
			nodes = new HashSet<Node>();
		}

		public Graph(Graph graph) {
			dictionary = new SortedDictionary<Coords, Node>(graph.GetDictionary(), new CoordsComparerFull());
		}

		public SortedDictionary<Coords,Node> GetDictionary() {
			return dictionary;
		}
		// Installs 2-way links between nodes.
		public void Add(Node node) {

			dictionary.Add(node.coords, node);

			//nodes.Add(node);
			foreach (Coords n in node.neighbours) {
				dictionary[n].neighbours.Add(n);
			}
		}
		// Cleans up 2-way links between nodes.
		public void Remove(Coords c) {
			dictionary.Remove(c);
			//nodes.Remove(node);
			foreach(Coords n in dictionary[c].neighbours) {
				dictionary[n].neighbours.Remove(c);
			}
		}


	}

	// Might not be used
	public class Edge {

		public double weight;

		// Stores a pair of Node.
		public NodePair nodes;

		public Edge(Node node0, Node node1) {

			nodes = new NodePair(node0, node1);
		}
	}

	public class NodePair {

		public Node node0, node1;

		public NodePair(Node node0, Node node1) {

			this.node0 = node0;
			this.node1 = node1;
		}
	}
}


