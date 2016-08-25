using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

// Note: Implement Dictionary from Coord to Node.

public class PathGridGenerator : MonoBehaviour {

    // Min distance between separate points on all 3 axes.
    public const double COORD_SENSITIVITY = 0.01;

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

    public class Graph {

        // This Set type will make sure there are no duplicate nodes. (I think)
        private HashSet<Node> nodes;

        public Graph() {

            nodes = new HashSet<Node>();
        }
        // Installs 2-way links between nodes.
        public void Add(Node node) {

            nodes.Add(node);
            foreach (Node n in node.neighbours) {
                n.neighbours.Add(node);
            }
        }
        // Cleans up 2-way links between nodes.
        public void Remove(Node node) {
            nodes.Remove(node);
            foreach(Node n in node.neighbours) {
                n.neighbours.Remove(node);
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

    public class Node{

        public Coords coords;

        public HashSet<Node> neighbours;

        public Hash128 hash;

        public Node(Coords coords) {
            this.coords = coords.copy();
            //hash = new Hash128(coords.x);
            neighbours = new HashSet<Node>();
        }

        public Node(Coords coords, HashSet<Node> neighbours) {
            this.coords = coords.copy();
            this.neighbours = neighbours;
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
}
