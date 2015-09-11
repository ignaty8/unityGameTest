using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HeightmapGenerator : MonoBehaviour {

	int[,] heightMap;
	public int sizeMagnitude;
	int size;
	private int minMagnitude = 0;
	public int minHeight;
	public int maxHeight;
	public string worldSeed;
	public int standardDeviation = 1;

	void Start(){
		GenerateHeightmap ();
	}

	// Update is called once per frame
	void Update () {
	
	}

	// Here the full heightmap is generated
	void GenerateHeightmap(){
		size = (int) Mathf.Pow(2, sizeMagnitude);
		heightMap = new int[size, size];
		squareGrids = new Square[size,size,sizeMagnitude];
		System.Random randomWorldGenerator = new System.Random (worldSeed.GetHashCode ());
		GenerateStartingSquare (randomWorldGenerator);

		// Tmp code
		
		Square squareTmp = new Square (0, 0, 0);
		GenerateSquare (squareTmp, randomWorldGenerator);
		Square squareTmp2 = new Square (0, 2, 0);
		GenerateSquare (squareTmp, randomWorldGenerator);
	}

	// The biggest square that contains the whole level is generated
	void GenerateStartingSquare(System.Random randomWorldSeed){
		Square startingSquare = new Square (0, 0, sizeMagnitude);
		heightMap [startingSquare.x1, startingSquare.y1] = randomWorldSeed.Next (minHeight, maxHeight);
		heightMap [startingSquare.x2, startingSquare.y1] = randomWorldSeed.Next (minHeight, maxHeight);
		heightMap [startingSquare.x1, startingSquare.y2] = randomWorldSeed.Next (minHeight, maxHeight);
		heightMap [startingSquare.x2, startingSquare.y2] = randomWorldSeed.Next (minHeight, maxHeight);
		squareGrids [0, 0, sizeMagnitude] = startingSquare;
	}

	// Every square that is not a main square gets generated here
	void GenerateSquare(Square square, System.Random randomWorldSeed){

		// Check if containing square exists:
		int containingSquareX = (int) Mathf.Ceil (square.squareX);
		int containingSquareY = (int) Mathf.Ceil (square.squareY);
		int containingSquareMagnitude = square.squareMagnitude + 1;
		Square containingSquare;
		if (squareGrids [containingSquareX, containingSquareY, containingSquareMagnitude] == null) {

			containingSquare = new Square(containingSquareX, containingSquareY, containingSquareMagnitude);
			GenerateSquare(containingSquare, randomWorldSeed);
		} else {
			containingSquare = squareGrids[containingSquareX, containingSquareY, containingSquareMagnitude];
		}
		// This one is already generated
		//heightMap [square.x1, square.y1] = randomWorldSeed.Next
		int meanHeightUp = (int) Mathf.RoundToInt((heightMap [containingSquare.x1, containingSquare.y1] + heightMap [containingSquare.x2, containingSquare.y1])/2);
		heightMap [square.x2, square.y1] = Mathf.RoundToInt(NextGaussianDouble(randomWorldSeed) * standardDeviation) + meanHeightUp;

		int meanHeightLeft = (int) Mathf.RoundToInt((heightMap [containingSquare.x1, containingSquare.y1] + heightMap [containingSquare.x1, containingSquare.y2])/2);
		heightMap [square.x1, square.y2] = Mathf.RoundToInt(NextGaussianDouble(randomWorldSeed) * standardDeviation) + meanHeightLeft;

		int meanHeightCentre = (int) Mathf.RoundToInt((heightMap [containingSquare.x1, containingSquare.y1] + heightMap [containingSquare.x2, containingSquare.y1] + heightMap [containingSquare.x1, containingSquare.y2] + heightMap [containingSquare.x2, containingSquare.y2])/4);
		heightMap [square.x2, square.y2] = Mathf.RoundToInt(NextGaussianDouble(randomWorldSeed) * standardDeviation) + meanHeightCentre;

		squareGrids[square.squareX, square.squareY, square.squareMagnitude] = square;

	}

	//public class SquareGrids{

		//public Square[,] squares;
		public Square[,,] squareGrids;
	//}

	public class Square{

		public int squareX, squareY, squareMagnitude;
		public int x1, x2, y1, y2, meanX, meanY, squareSize;

		public Square(){
		}

		public Square(int _squareX, int _squareY, int _squareMagnitude){
			squareX = _squareX;
			squareY = _squareY;
			squareMagnitude = _squareMagnitude;
			squareSize = (int)Mathf.Pow (2, squareMagnitude);
			x1 = squareX * squareSize;
			x2 = x1 + squareSize;
			y1 = squareY * squareSize;
			y2 = y1 + squareSize;
		}
	}
	public static float NextGaussianDouble(System.Random r)
	{
		double U, u, v;
		float S;
		do
		{
			u = 2.0 * r.NextDouble() - 1.0;
			v = 2.0 * r.NextDouble() - 1.0;
			S = (float) (u * u + v * v);
		}
		while (S >= 1.0);
		
		float fac = Mathf.Sqrt(-2.0f * Mathf.Log(S) / S);
		return (float) (u) * fac;
	}

	void OnDrawGizmos(){
		if (heightMap != null) {
			for (int x = 0; x < size; x ++){
				for (int y = 0; y < size; y ++){
					if (heightMap[x,y] != null){
						Gizmos.color = (heightMap[x,y] > 0)?Color.black:Color.white;
					//	Moar ifs ><

					Vector3 pos = new Vector3(-size/2 + 2 * x +.5f,heightMap[x,y],-size/2 + 2 * y +.5f);
					Gizmos.DrawCube(pos,Vector3.one);
					}
				}
				
			}
		}
		if (squareGrids != null) {
			for (int x = 0; x < size; x ++) {
				for (int y = 0; y < size; y ++) {
					for (int m = 0; m < sizeMagnitude; m ++) {
						if (squareGrids [x, y, m] != null) {
							//Gizmos.color = (heightMap[x,y] > 0)?Color.black:Color.white;
							//	Moar ifs ><
							Gizmos.color = Color.black;
							
							Vector3 pos = new Vector3 (-size / 2 + 2 * x + .5f, -size / 2 + 2 * m + .5f, -size / 2 + 2 * y + .5f);
							Gizmos.DrawCube (pos, Vector3.one);
						}
					}
				}
			}
		}
	}
}
