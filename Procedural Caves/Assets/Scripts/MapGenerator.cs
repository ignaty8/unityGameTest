using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class MapGenerator : MonoBehaviour {

	public int width;
	public int height;

	public string seed;
	public bool useRandomSeed;

	[Range(0,100)]
	public int randomFillPercent;

	int[,] map;

	//public int typesOfObjectsToSpawn = 4;

    public static int generatedStructureTypeCount = 0;
    // Make sure each array below has same length!
	public List<GameObject> objectsToSpawn = new List<GameObject>(generatedStructureTypeCount);
    // Stores object spawn chances in int out of 10k
    public List<int> objectSpawnChance = new List<int>();

    public List<GameObject> spawnedObjects = new List<GameObject>();


    void Start(){

		GenerateMap ();
	}

	void Update(){
		if (Input.GetMouseButtonDown (0)) {
			GenerateMap();
		}
	}

	public void GenerateMap(){

		map = new int[width, height];
		RandomFillMap ();

		for(int i = 0;i<5;i++){
			SmoothMap();
		}

		ProcessMap ();

		int borderSize = 5;
		int[,] borderedMap = new int[width + borderSize * 2, height + borderSize * 2];

		for (int x = 0; x < borderedMap.GetLength(0); x ++) {
			for (int y = 0; y < borderedMap.GetLength(1); y ++) {
				if (x >= borderSize && x < width + borderSize && y >= borderSize && y < height + borderSize){
					borderedMap[x,y] = map[x-borderSize,y-borderSize];
				}
				else{
					borderedMap[x,y] = 1;
				}
			}
		}

		MeshGenerator meshGen = GetComponent<MeshGenerator> ();
		meshGen.GenerateMesh (borderedMap, 1);


		GameObject[] allCurrentInstances;

		allCurrentInstances = GameObject.FindGameObjectsWithTag ("Destructible");
		
        // Make sure previously generated objects are removed.
		foreach (GameObject objectInstance in spawnedObjects) {
			Destroy(objectInstance);
		}

		// 
		PropPlacer propPlacer = GetComponent<PropPlacer> ();
		for(int k = 0; k < objectsToSpawn.Count; k++){
            GameObject objectToSpawn = objectsToSpawn[k];
			if (objectToSpawn != null){
                // To ensure random distribution, the seed is modified depending on object order in the list.
                // Without that having the same @spawn chance@ spawns things in the same place.
				propPlacer.ObjectMapGenerator(map, 1, seed + k, objectSpawnChance[k], objectToSpawn);
			}
		}
	}

	void ProcessMap() {
		List<List<Coord>> wallRegions = GetRegions (1);

		int wallThresholdSize = 14;

		foreach (List<Coord> wallRegion in wallRegions) {
			if (wallRegion.Count < wallThresholdSize) {
				foreach (Coord tile in wallRegion){
					map[tile.tileX, tile.tileY] = 0;
				}
			}
		}

		List<List<Coord>> roomRegions = GetRegions (0);
		
		int roomThresholdSize = 14;

		List<Room> survivingRooms = new List<Room> ();
		
		foreach (List<Coord> roomRegion in roomRegions) {
			if (roomRegion.Count < roomThresholdSize) {
				foreach (Coord tile in roomRegion){
					map[tile.tileX, tile.tileY] = 1;
				}
			} else {
				survivingRooms.Add(new Room(roomRegion, map));
			}
		}
		survivingRooms.Sort ();	// Sort rooms by size, biggest first!
		survivingRooms [0].isMainRoom = true;
		survivingRooms [0].isAccessibleFromMainRoom = true;

		ConnectClosestRooms (survivingRooms);
	}

	void ConnectClosestRooms(List<Room> allRooms, bool forceAccessibilityFromMainRoom = false){	// Setting it to false makes that the default unless specified.

		List<Room> roomListA = new List<Room> ();
		List<Room> roomListB = new List<Room> ();

		if (forceAccessibilityFromMainRoom) {
			foreach (Room room in allRooms) {
				if (room.isAccessibleFromMainRoom) {
					roomListB.Add (room);
				} else {
					roomListA.Add (room);
				}
			}
		} else {
			roomListA = allRooms;
			roomListB = allRooms;
		}

		int bestDistance = 0;
		Coord bestTileA = new Coord ();
		Coord bestTileB = new Coord ();
		Room bestRoomA = new Room ();
		Room bestRoomB = new Room ();
		bool possibleConnectionFound = false;

		foreach (Room roomA in roomListA) {
			if(!forceAccessibilityFromMainRoom){	// This is needed so that given a group of isolated, 
				// interconnected rooms, all rooms are considered to make the shotest connection to the 
				// group including the main room! (Hope this was intelligible.)
				possibleConnectionFound = false;
				if (roomA.connectedRooms.Count > 0){
					continue;
				}
			}

			foreach(Room roomB in roomListB){
				if (roomA == roomB || roomA.IsConnected(roomB)){
					continue;
				}
				for (int tileIndexA = 0; tileIndexA < roomA.edgeTiles.Count; tileIndexA++){
					for (int tileIndexB = 0; tileIndexB < roomB.edgeTiles.Count; tileIndexB++){
						Coord tileA = roomA.edgeTiles[tileIndexA];
						Coord tileB = roomB.edgeTiles[tileIndexB];
						int distanceBetweenRooms = (int) (Mathf.Pow(tileA.tileX - tileB.tileX,2)
							+ Mathf.Pow(tileA.tileY - tileB.tileY,2));
							// No need to square root to get the accurate distance, since we only need to compare them!

						if (distanceBetweenRooms < bestDistance || !possibleConnectionFound){
							bestDistance = distanceBetweenRooms;
							possibleConnectionFound = true;
							bestTileA = tileA;
							bestTileB = tileB;
							bestRoomA = roomA;
							bestRoomB = roomB;
						}
					}
				}
			}
			if (possibleConnectionFound && !forceAccessibilityFromMainRoom){
				CreatePassage(bestRoomA, bestRoomB, bestTileA, bestTileB);
			}
		}

		if (possibleConnectionFound && forceAccessibilityFromMainRoom) {
			CreatePassage (bestRoomA, bestRoomB, bestTileA, bestTileB);
			ConnectClosestRooms(allRooms, true);
		}
		if (!forceAccessibilityFromMainRoom) {
			ConnectClosestRooms(allRooms, true);
		}
	}

	void CreatePassage(Room roomA, Room roomB, Coord tileA, Coord tileB){
		Room.ConnectRooms(roomA,roomB);
		Debug.DrawLine(CoordToWorldPoint(tileA), CoordToWorldPoint(tileB), Color.green, 100);

		List<Coord> line = GetLine (tileA, tileB);
		foreach (Coord c in line){
			DrawCircle (c,1);
		}
	}

	void DrawCircle(Coord c, int r){
		for (int x = -r; x <= r; x++) {
			for (int y = -r; y <= r; y++) {
				if (x*x + y*y <= r*r){
					int drawX = c.tileX + x;
					int drawY = c.tileY + y;
					if (IsInMapRange(drawX, drawY)){
						map[drawX,drawY] = 0;
					}
				}
			}
		}
	}

	List<Coord> GetLine(Coord from, Coord to){
		List<Coord> line = new List<Coord> ();

		int x = from.tileX;
		int y = from.tileY;

		int dx = to.tileX - from.tileX;
		int dy = to.tileY - from.tileY;

		bool inverted = false;
		int step = Math.Sign (dx);	// Math returns an integer, Mathf a float!
		int gradientStep = Math.Sign (dy);

		int longest = Mathf.Abs (dx);
		int shortest = Mathf.Abs (dy);

		if (longest < shortest) {
			inverted = true;
			longest = Mathf.Abs (dy);
			shortest = Mathf.Abs (dx);

			step = Math.Sign (dy);
			gradientStep = Math.Sign (dx);
		}

		int gradientAccumulation = longest / 2;
		for (int i = 0; i < longest; i++) {
			line.Add(new Coord(x,y));

			if(inverted){
				y+=step;
			} else {
				x+=step;
			}

			gradientAccumulation += shortest;
			if(gradientAccumulation >= longest){
				if (inverted){
					x += gradientStep;
				} else {
					y += gradientStep;
				}
				gradientAccumulation -= longest;
			}
		}
		return line;
	}

	Vector3 CoordToWorldPoint(Coord tile){
		return new Vector3(-width/2+.5f + tile.tileX, 2, -height/2 + .5f + tile.tileY);
	}

	List<List<Coord>> GetRegions(int tileType){
		List<List<Coord>> regions = new List<List<Coord>> ();
		int[,] mapFlags = new int[width, height];
		for (int x = 0; x < width; x ++) {
			for (int y = 0; y < height; y ++) {
				if (mapFlags[x,y] == 0 && map[x,y] == tileType){
					List<Coord> newRegion = GetRegionTiles(x,y);
					regions.Add(newRegion);

					foreach(Coord tile in newRegion){
						mapFlags[tile.tileX, tile.tileY] = 1;
					}
				}
			}
		}

		return regions;
	}

	List<Coord> GetRegionTiles(int startX, int startY){
		List<Coord> tiles = new List<Coord> ();
		int[,] mapFlags = new int[width, height];
		int tileType = map [startX, startY];

		Queue<Coord> queue = new Queue<Coord> ();
		queue.Enqueue (new Coord (startX, startY));
		mapFlags [startX, startY] = 1;

		while (queue.Count > 0) {
			Coord tile = queue.Dequeue();	// Returns first item in queue and removes it from queue!
			tiles.Add(tile);

			for (int x = tile.tileX - 1; x <= tile.tileX + 1; x++){
				for (int y = tile.tileY - 1; y <= tile.tileY + 1; y++){
					if (IsInMapRange(x,y) && (y == tile.tileY || x == tile.tileX)){
						if (mapFlags[x,y] == 0 && map[x,y] == tileType){
							mapFlags[x,y] = 1;
							queue.Enqueue(new Coord(x,y));
						}
					}
				}
			}
		}
		return tiles;
	}

	bool IsInMapRange(int x, int y){
		return x >= 0 && x < width && y >= 0 && y < height;
	}

	void RandomFillMap(){

		if (useRandomSeed) {

			seed = Time.time.ToString ();
		}

		System.Random pseudoRandom = new System.Random (seed.GetHashCode ());

		for (int x = 0; x < width; x ++){
			for (int y = 0; y < height; y ++){
				if(x==0||x==width-1||y==0||y==height-1){
					map[x,y] = 1;
				}
				map[x,y] = (pseudoRandom.Next (0,100) < randomFillPercent)? 1:0;
				//	That's a weird if statement...
			}
			
		}
	}

	void SmoothMap(){

		for (int x = 0; x < width; x ++) {
			for (int y = 0; y < height; y ++) {
				int neigbourWallTiles = GetSurroundingCount(x,y);

				if (neigbourWallTiles > 4){
					map[x,y] = 1;
				} else if (neigbourWallTiles < 4){
					map[x,y] = 0;
				}

			}
		}
	}

	int GetSurroundingCount(int gridX, int gridY){
		int wallCount = 0;
		for (int neighboursX = gridX -1; neighboursX <= gridX +1; neighboursX++) {
			for (int neighboursY = gridY -1; neighboursY <= gridY +1; neighboursY++) {
				if(IsInMapRange(neighboursX, neighboursY)){
					if (neighboursX!=gridX || neighboursY!=gridY){
						wallCount += map[neighboursX,neighboursY];
					}
				} else {
					wallCount++;
				}
			}
		}
		return wallCount;
	}

	struct Coord {
		public int tileX;
		public int tileY;
		
		public Coord(int x, int y){
			tileX = x;
			tileY = y;
		}
	}

	void OnDrawGizmos(){
//		if (map != null) {
//			for (int x = 0; x < width; x ++){
//				for (int y = 0; y < height; y ++){
//					
//					Gizmos.color = (map[x,y] == 1)?Color.black:Color.white;
//					//	Moar ifs ><
//
//					Vector3 pos = new Vector3(-width/2 + x +.5f,0,-height/2 + y +.5f);
//					Gizmos.DrawCube(pos,Vector3.one);
//				}
//				
//			}
//		}
	}

	class Room : IComparable<Room>{	//IC... used to sort rooms by size and stuff
		public List<Coord> tiles;
		public List<Coord> edgeTiles;
		public List<Room> connectedRooms;
		public int roomSize;
		public bool isAccessibleFromMainRoom;
		public bool isMainRoom;

		public Room(){	// Constructer to set Room to an empty room
		}

		public Room(List<Coord> roomTiles, int[,] map){
			tiles = roomTiles;
			roomSize = tiles.Count;
			connectedRooms = new List<Room>();

			edgeTiles = new List<Coord>();
			foreach(Coord tile in tiles){
				for (int x = tile.tileX -1; x <= tile.tileX+1; x++){
					for (int y = tile.tileY -1; y <= tile.tileY+1; y++){
						if((x == tile.tileX || y == tile.tileY) && x >= 0 && x < map.GetLength(0) && y>= 0 && y < map.GetLength(1)){
							
							//Debug.Log(x);
							//Debug.Log(y);
							if(map[x,y] == 1){
								edgeTiles.Add(tile);
							}
						}
					}
				}
			}
		}
		public void SetAccessibleFromMainRoom(){
			if (!isAccessibleFromMainRoom) {
				isAccessibleFromMainRoom = true;
				foreach (Room connectedRoom in connectedRooms){
					connectedRoom.SetAccessibleFromMainRoom();
				}
			}
		}
		public static void ConnectRooms (Room roomA, Room roomB){
			if (roomA.isAccessibleFromMainRoom) {
				roomB.SetAccessibleFromMainRoom();
			}
			else if(roomB.isAccessibleFromMainRoom){
				roomA.SetAccessibleFromMainRoom();
			}
			roomA.connectedRooms.Add (roomB);
			roomB.connectedRooms.Add (roomA);
		}
		public bool IsConnected(Room otherRoom){
			return connectedRooms.Contains (otherRoom);
		}

		public int CompareTo(Room otherRoom){
			return otherRoom.roomSize.CompareTo(roomSize);
		}
	}
}
