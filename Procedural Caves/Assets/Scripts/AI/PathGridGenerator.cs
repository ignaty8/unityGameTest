using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;







public class PathGridGenerator : MonoBehaviour {



    // Min distance between separate points on all 3 axes.
    public const double COORD_SENSITIVITY = 0.01;
	//nothin in this code should be bigger
	//static float infinity =100000000;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public Graph PathGridCreator(int[,] map) {

        Graph pathfindingGraph = new Graph();

        for (int x = 0; x < map.GetLength(0); x++) {
            for (int y = 0; y < map.GetLength(1); y++) {
                
            }
        }

        return null;
    }

	// Note: Implement Dictionary from Coord to Node.
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

	//So I need to change the node class a tiny bit
	public class Node{

		public Coords coords;

		public HashSet<Coords> neighbours;

		//Added Part
		//this value estimates the "cost" of going from the origin to this Node in the a* algo
		//public float gcost;
		//this value estimates the "cost" of going from the destination to this Node in the a* algo
		//public float hcost;
		//unnecessary, just the sum of gcost & hcost. we COULD use this instead of hcost
		//unless it proves more efficient in someway I won't though
		//public float fcost;
		//Used in the algo again, its the best coord to go to this Node
		//public Coords CameFrom;



		//public Hash128 hash;

		public Node(Coords coords) {
			this.coords = coords.copy();
			//hash = new Hash128(coords.x);
			neighbours = new HashSet<Coords>(new CoordsComparer());
			//Setting Standard Value
			//gcost = infinity;
			//hcost = infinity;
			//CameFrom = new Coords(0,0,0);
		}

		public Node(Coords coords, HashSet<Coords> neighbours) {
			this.coords = coords.copy();
			this.neighbours = neighbours;
			//Setting Standard Value
			//gcost = infinity;
			//hcost = infinity;
		}

		/*
		//This returns the total "cost" of going through this node to go to the destination
		public float GetFcost(){
			return gcost+hcost;
		}

		//Generates an approximation of the cost of getting to the destination
		public float EstimateHCost(Coords destination){
			//Our "Estimation" is just going to be calculating the distance from here to there
			return (float)GetDistance(destination,coords);
		}
		*/
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

	// Compares 2 doubles according to predefined sensitivity.
	public static bool adjacentDoubles(double x, double y) {
		return x - COORD_SENSITIVITY < y && x + COORD_SENSITIVITY > y;
	}




	//Some Functions From PathFinder
	//Fairly Straighforward I think
	/*
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
	*/
}


