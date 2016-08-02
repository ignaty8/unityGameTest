﻿using UnityEngine;
using System.Collections;

public class PropPlacer : MonoBehaviour {

    public const int minFrequency = 10000;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public GameObject healthBar;

	/// <summary>
	/// Instantiates a prop randomly throughout the level.
	/// </summary>
	/// <param name="levelMap">Binary map of the level.</param>
	/// <param name="squareSize">Size each square in the map is scaled to.</param>
	/// <param name="randomSeed">Level seed.</param>
	/// <param name="spawnChance">Spawn chance (1 in n).</param>
	/// <param name="objectToSpawn">Object to spawn.</param>
	public void ObjectMapGenerator(int[,] levelMap, float squareSize, string randomSeed, double spawnChance, GameObject objectToSpawn, int offsetX = 0, int offsetY = 0, float offsetSize = 1){
//		GameObject[] allCurrentInstances;
//		while (objectToSpawn.ToString() != null){
//			allCurrentInstances[allCurrentInstances.GetLength(0)] = GameObject.Find (objectToSpawn.ToString());
//		}
//
//		foreach (GameObject objectInstance in allCurrentInstances) {
//			Destroy(objectInstance);
//		}
        if (spawnChance == 0)
        {

            float spawnFrequency = objectToSpawn.GetComponent<SpawnConfig>().spawnChance;
            if (spawnFrequency == 0) {
                spawnChance = 0;
            } else
            {
                spawnChance = minFrequency / spawnFrequency;
            }
        } else
        {
            //spawnChance = minFrequency / spawnChance;
        }

        if (minFrequency < spawnChance) spawnChance = minFrequency;

        System.Random pseudoRandom = new System.Random (randomSeed.GetHashCode ());
        
		int levelMapWidth = levelMap.GetLength(0);
		int levelMapHeight = levelMap.GetLength(1);

		for (int x = 0; x < levelMapWidth; x++){
			for (int y = 0; y < levelMapHeight; y++){
				//Debug.Log(x);
				if (levelMap[x,y] == 0){
					//Debug.Log (x);
					// Inside each square marked as "room" on the map, there is a chance the object will be instantiated.
					if (pseudoRandom.Next(1, minFrequency) < spawnChance){
                        // Indicate cell is now occupied by an object.
                        levelMap[x, y] = 2;

                        // The position of the object is also randomized within the square.
                        float randomOffsetX = (float) pseudoRandom.NextDouble() * squareSize;
						float randomOffsetY = (float) pseudoRandom.NextDouble() * squareSize;
						Vector3 objectPosition = new Vector3(offsetX * offsetSize + x * squareSize + randomOffsetX - levelMapWidth/2, 0, offsetY * offsetSize + y * squareSize + randomOffsetY - levelMapHeight/2);
						GameObject spawnedInstance = (GameObject) Instantiate(objectToSpawn, objectPosition, Quaternion.Euler(0 + objectToSpawn.transform.rotation.eulerAngles.x, pseudoRandom.Next(0,360) + objectToSpawn.transform.rotation.eulerAngles.y, 0 + objectToSpawn.transform.rotation.eulerAngles.z));

                        // Add instance to the spawned object record. This allows it to be removed when a new map is generated.
                        GameObject.FindGameObjectWithTag("MapGenerator").GetComponent<MapGenerator>().spawnedObjects.Add(spawnedInstance);

                        // Generator substructures for "structure" objects.
                        if (objectToSpawn.tag == "Structure")
                        {
                            // Indicate structure centre.
                            levelMap[x, y] = 3;

                            StructureGenerator structureGeneratorScript = objectToSpawn.GetComponent<StructureGenerator>();
                            structureGeneratorScript.GenerateSubstructures(levelMap, spawnedInstance, randomSeed, x, y, offsetSize);
                        }

                        // If spawned GameObject is a Destructible, spawn a child HelathBar
                        if (objectToSpawn.tag == "Destructible"){
							Collider spawnedObjectCollider = spawnedInstance.GetComponent<Collider> ();

							GameObject healthBarInstance = (GameObject) Instantiate(healthBar, spawnedInstance.transform.position, spawnedInstance.transform.rotation * Quaternion.Euler (270,0,0));
							//new Vector3(healthBarInstance.transform.position.x,spawnedObjectCollider.bounds.center.y,healthBar.transform.position.z);
							healthBarInstance.transform.parent = spawnedInstance.transform;

							// After spawning, align HealthBar with top of Object Collider.
							healthBarInstance.transform.position += new Vector3(0, spawnedObjectCollider.bounds.size.y, 0);
							float scaleDimension = Mathf.Max(spawnedObjectCollider.bounds.size.x, spawnedObjectCollider.bounds.size.z);
							float scaleFactor = scaleDimension/healthBarInstance.GetComponent<MeshRenderer>().bounds.size.x;	// Comparison isn't necessary, since size.x = size.y.
							healthBarInstance.transform.localScale = new Vector3(scaleFactor, scaleFactor, scaleFactor);
						}
					}
				}
			}
		}
	}
}
