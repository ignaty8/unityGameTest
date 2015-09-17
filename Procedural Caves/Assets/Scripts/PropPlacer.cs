using UnityEngine;
using System.Collections;

public class PropPlacer : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	/// <summary>
	/// Instantiates a prop randomly throughout the level.
	/// </summary>
	/// <param name="levelMap">Binary map of the level.</param>
	/// <param name="squareSize">Size each square in the map is scaled to.</param>
	/// <param name="randomSeed">Level seed.</param>
	/// <param name="spawnChance">Spawn chance out of 1000.</param>
	/// <param name="objectToSpawn">Object to spawn.</param>
	public void ObjectMapGenerator(int[,] levelMap, float squareSize, string randomSeed, int spawnChance, GameObject objectToSpawn){
//		GameObject[] allCurrentInstances;
//		while (objectToSpawn.ToString() != null){
//			allCurrentInstances[allCurrentInstances.GetLength(0)] = GameObject.Find (objectToSpawn.ToString());
//		}
//
//		foreach (GameObject objectInstance in allCurrentInstances) {
//			Destroy(objectInstance);
//		}

		System.Random pseudoRandom = new System.Random (randomSeed.GetHashCode ());

		int levelMapWidth = levelMap.GetLength(0);
		int levelMapHeight = levelMap.GetLength(1);

		for (int x = 0; x < levelMapWidth; x++){
			for (int y = 0; y < levelMapHeight; y++){
				//Debug.Log(x);
				if (levelMap[x,y] == 0){
					//Debug.Log (x);
					if (pseudoRandom.Next(1, 1000) < spawnChance){
						float randomOffsetX = (float) pseudoRandom.NextDouble() * squareSize;
						float randomOffsetY = (float) pseudoRandom.NextDouble() * squareSize;
						Vector3 objectPosition = new Vector3(x * squareSize + randomOffsetX - levelMapWidth/2, 0, y * squareSize + randomOffsetY - levelMapHeight/2);
						Instantiate(objectToSpawn, objectPosition, Quaternion.Euler(0, pseudoRandom.Next(0,360), 0));
					}
				}
			}
		}
	}
}
