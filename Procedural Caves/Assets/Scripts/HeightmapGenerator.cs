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
		//squareGrids = new Square[size,size,sizeMagnitude];
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
		heightMap [size, startingSquare.y1] = randomWorldSeed.Next (minHeight, maxHeight);
		heightMap [startingSquare.x1, size] = randomWorldSeed.Next (minHeight, maxHeight);
		heightMap [size, size] = randomWorldSeed.Next (minHeight, maxHeight);
		heightMap [size/2, size/2] = randomWorldSeed.Next (minHeight, maxHeight);
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

		if (square.squareMagnitude != 0) {
			int meanHeightCentre = (int)Mathf.RoundToInt ((heightMap [square.x1, square.y1] + heightMap [square.x2, square.y1] + heightMap [square.x1, square.y2] + heightMap [square.x2, square.y2]) / 4);
			heightMap [square.meanX, square.meanY] = Mathf.RoundToInt (NextGaussianDouble (randomWorldSeed) * standardDeviation) + meanHeightCentre;

			if (!isAtWhichEdge (square.squareX, square.squareY, square.squareMagnitude, 0)) {
				if (heightMap [square.meanX - square.squareSize, square.meanY] != 0) {
					int meanHeightLeftCentre = (int)Mathf.RoundToInt ((heightMap [square.x1, square.y1] + heightMap [square.x1 - square.squareSize, square.y1] + heightMap [square.x1, square.y2] + heightMap [square.x1 - square.squareSize, square.y2]) / 4);
					heightMap [square.meanX - square.squareSize, square.meanY] = Mathf.RoundToInt (NextGaussianDouble (randomWorldSeed) * standardDeviation) + meanHeightLeftCentre;
				}

				int meanHeightLeft = (int)Mathf.RoundToInt ((heightMap [square.x1, square.y1] + heightMap [square.meanX - square.squareSize, square.meanY] + heightMap [square.x1, square.y2] + heightMap [square.meanX, square.meanY]) / 4);
				heightMap [square.x1, square.meanY] = Mathf.RoundToInt (NextGaussianDouble (randomWorldSeed) * standardDeviation) + meanHeightLeft;
			} else {
				int meanHeightLeft = (int)Mathf.RoundToInt ((heightMap [square.x1, square.y1] + heightMap [square.x1, square.y2] + heightMap [square.meanX, square.meanY]) / 3);
				heightMap [square.x1, square.meanY] = Mathf.RoundToInt (NextGaussianDouble (randomWorldSeed) * standardDeviation) + meanHeightLeft;
			}

			if (!isAtWhichEdge (square.squareX, square.squareY, square.squareMagnitude, 1)) {
				if (heightMap [square.meanX, square.meanY + square.squareSize] != 0) {
					int meanHeightTopCentre = (int)Mathf.RoundToInt ((heightMap [square.x1, square.y1] + heightMap [square.x1, square.y1 + square.squareSize] + heightMap [square.x2, square.y1] + heightMap [square.x2, square.y1 + square.squareSize]) / 4);
					heightMap [square.meanX, square.meanY + square.squareSize] = Mathf.RoundToInt (NextGaussianDouble (randomWorldSeed) * standardDeviation) + meanHeightTopCentre;
				}

				int meanHeightTop = (int)Mathf.RoundToInt ((heightMap [square.x1, square.y1] + heightMap [square.meanX, square.meanY + square.squareSize] + heightMap [square.x2, square.y1] + heightMap [square.meanX, square.meanY]) / 4);
				heightMap [square.meanX, square.y1] = Mathf.RoundToInt (NextGaussianDouble (randomWorldSeed) * standardDeviation) + meanHeightTop;
			} else {
				int meanHeightTop = (int)Mathf.RoundToInt ((heightMap [square.x1, square.y1] + heightMap [square.x2, square.y1] + heightMap [square.meanX, square.meanY]) / 3);
				heightMap [square.meanX, square.y1] = Mathf.RoundToInt (NextGaussianDouble (randomWorldSeed) * standardDeviation) + meanHeightTop;
			}

			if (!isAtWhichEdge (square.squareX, square.squareY, square.squareMagnitude, 2)) {
				if (heightMap [square.meanX + square.squareSize, square.meanY] != 0) {
					int meanHeightRightCentre = (int)Mathf.RoundToInt ((heightMap [square.x2, square.y1] + heightMap [square.x2 + square.squareSize, square.y1] + heightMap [square.x2, square.y2] + heightMap [square.x2 + square.squareSize, square.y2]) / 4);
					heightMap [square.meanX + square.squareSize, square.meanY] = Mathf.RoundToInt (NextGaussianDouble (randomWorldSeed) * standardDeviation) + meanHeightRightCentre;
				}

				int meanHeightRight = (int)Mathf.RoundToInt ((heightMap [square.x2, square.y1] + heightMap [square.meanX + square.squareSize, square.meanY] + heightMap [square.x2, square.y2] + heightMap [square.meanX, square.meanY]) / 4);
				heightMap [square.x2, square.meanY] = Mathf.RoundToInt (NextGaussianDouble (randomWorldSeed) * standardDeviation) + meanHeightRight;
			} else {
				int meanHeightRight = (int)Mathf.RoundToInt ((heightMap [square.x2, square.y1] + heightMap [square.x2, square.y2] + heightMap [square.meanX, square.meanY]) / 3);
				heightMap [square.x1, square.meanY] = Mathf.RoundToInt (NextGaussianDouble (randomWorldSeed) * standardDeviation) + meanHeightRight;
			}

			if (!isAtWhichEdge (square.squareX, square.squareY, square.squareMagnitude, 3)) {
				if (heightMap [square.meanX, square.meanY - square.squareSize] != 0) {
					int meanHeightBottomCentre = (int)Mathf.RoundToInt ((heightMap [square.x1, square.y2] + heightMap [square.x1, square.y2 + square.squareSize] + heightMap [square.x2, square.y2] + heightMap [square.x2, square.y2 + square.squareSize]) / 4);
					heightMap [square.meanX, square.meanY - square.squareSize] = Mathf.RoundToInt (NextGaussianDouble (randomWorldSeed) * standardDeviation) + meanHeightBottomCentre;
				}

				int meanHeightBottom = (int)Mathf.RoundToInt ((heightMap [square.x1, square.y2] + heightMap [square.meanX, square.meanY - square.squareSize] + heightMap [square.x2, square.y2] + heightMap [square.meanX, square.meanY]) / 4);
				heightMap [square.meanX, square.y2] = Mathf.RoundToInt (NextGaussianDouble (randomWorldSeed) * standardDeviation) + meanHeightBottom;
			} else {
				int meanHeightBottom = (int)Mathf.RoundToInt ((heightMap [square.x1, square.y2] + heightMap [square.x2, square.y2] + heightMap [square.meanX, square.meanY]) / 3);
				heightMap [square.meanX, square.y2] = Mathf.RoundToInt (NextGaussianDouble (randomWorldSeed) * standardDeviation) + meanHeightBottom;
			}
		}

//		int meanHeightCentre = (int) Mathf.RoundToInt((heightMap [square.x1, square.y1] + heightMap [square.x2, square.y1] + heightMap [square.x1, square.y2] + heightMap [square.x2, square.y2])/4);
//		heightMap [square.x1, square.y2] = Mathf.RoundToInt(NextGaussianDouble(randomWorldSeed) * standardDeviation) + meanHeightCentre;
//		//if(isAtWhichEdge == 0
//		
//		int meanHeightUp = (int) Mathf.RoundToInt((heightMap [square.x1, square.y1] + heightMap [square.x2, square.y1])/2);
//		heightMap [square.x2, square.y1] = Mathf.RoundToInt(NextGaussianDouble(randomWorldSeed) * standardDeviation) + meanHeightUp;
//
//		int meanHeightLeft = (int) Mathf.RoundToInt((heightMap [square.x1, square.y1] + heightMap [square.x1, square.y2])/2);
//		heightMap [square.x1, square.y2] = Mathf.RoundToInt(NextGaussianDouble(randomWorldSeed) * standardDeviation) + meanHeightLeft;

		squareGrids[square.squareX, square.squareY, square.squareMagnitude] = square;

	}

	void GenerateNeighbourCentres(){

	}
	

	// 0 = left, 1 = top, 2 = right, 3 = bottom
	// 4 = top left, 5 = top right, 6 = bottom right, 7 = bottom left
	bool isAtWhichEdge(int x, int y, int magnitude, int side) {
		float size = Mathf.Pow (2, magnitude);
		switch (side) {
		case 0:
			return (x == 0);

		case 1:
			return (y == 0);

		case 2:
			return (x >= size-1);

		case 3:
			return (y >= size-1);
		}
		return false;

//		if (x == 0 && y == 0) {
//			return 4;
//		} else if (y == 0 && x >= Mathf.Pow (2, magnitude)) {
//			return 5;
//		} else if (x >= Mathf.Pow (2, magnitude) || y >= Mathf.Pow (2, magnitude)) {
//			return 6;
//		} else if (y >= Mathf.Pow (2, magnitude) || x == 0) {
//			return 7;
//		}
//		if (x == 0) {
//			return 0;
//		} else if (x >= Mathf.Pow (2, magnitude)) {
//			return 2;
//		} else if (y == 0) {
//			return 1;
//		} else if (y >= Mathf.Pow (2, magnitude)) {
//			return 3;
//		}
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
			if (squareMagnitude != 0){
				meanX = (x1+x2)/2;
				meanY = (x1+x2)/2;
			}
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
