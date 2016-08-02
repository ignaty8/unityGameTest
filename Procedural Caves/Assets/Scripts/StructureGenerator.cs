using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class StructureGenerator : MonoBehaviour {

    // Size of the square in which structure will be generated.
    public int structureSize = 1;

    // Make sure each array below has same length!
    public List<GameObject> objectsToSpawn = new List<GameObject>();
    // Stores object spawn chances in int out of 10k
    public List<int> objectSpawnChance = new List<int>();

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    // This takes the full level map for now, and calculates its own submap for now.
    // posX/Y are positions on the procedural grid, not the transform coordinates.
    public void GenerateSubstructures(int[,] map, GameObject structureInstance, string seed, int posX, int posY, float offsetScale)
    {
        int[,] localMap = new int[structureSize * 2, structureSize * 2];
        

        // Needs testing!
        // Make sure structureSize is respected.
        for(int x = posX - structureSize / 2; x < map.Length && ((structureSize % 2 == 0)? (x < posX + structureSize / 2) : (x < posX + structureSize / 2 + 1)); x++) {
            for(int y = posY - structureSize / 2; y < map.Length && ((structureSize % 2 == 0) ? (y < posY + structureSize / 2) : (y < posY + structureSize / 2 + 1)); y++)
            {
                // This part isn't generalised
                localMap [x % structureSize * 2, y % structureSize * 2] = map [x,y];
                localMap [x % structureSize * 2 + 1, y % structureSize * 2 + 1] = map [x,y];
            }
        }

        PropPlacer propPlacer = GetComponent<PropPlacer>();
        for (int k = 0; k < objectsToSpawn.Count; k++)
        {
            GameObject objectToSpawn = objectsToSpawn[k];
            if (objectToSpawn != null)
            {
                // To ensure random distribution, the seed is modified depending on object order in the list.
                // Without that having the same @spawn chance@ spawns things in the same place.
                propPlacer.ObjectMapGenerator(localMap, 0.5f, seed + k, objectSpawnChance[k], objectToSpawn, posX, posY, offsetScale / 2f);
            }
        }
    }
}
