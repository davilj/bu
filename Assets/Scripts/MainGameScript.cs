using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainGameScript : MonoBehaviour {

	public GameObject pBlockPrefab;

	int delayCount=0;
	public int Delay;
	Board board;
	RouteFactory routeFactory;

	// Use this for initialization
	void Start () {
		board = new Board(4,4, pBlockPrefab, transform);
		routeFactory = new RouteFactory();
		Debug.Log("Ready to setup route");
		routeFactory.setupRoute(board);
		Debug.Log("Starting a Main Game Script");

	}
	
	// Update is called once per frame
	void Update () {
		
		if (delayCount==0) {
			if (Input.GetKey("z")) {
				Debug.Log("Move active block Right");
				board.moveActiveBlockRight();
			}

			if (Input.GetKey("x")) {
				Debug.Log("Move active block Left");
				board.moveActiveBlockLeft();
			}

			if (Input.GetKey("v")) {
				Debug.Log("Move active block Down");
				board.moveActiveBlockDown();
			}

			if (Input.GetKey("c")) {
				Debug.Log("Move active block Up");
				board.moveActiveBlockUp();
			}

			if (Input.GetKey("b")) {
				Debug.Log("Swap Block");
				board.swapActiveBlockWithSpace();
			}
			delayCount=1;
		} else {
			delayCount=delayCount + 1;
			if (delayCount==Delay) {
				delayCount=0;
			}
		}
	}

	class RouteFactory {
		
		public void setupRoute(Board board)  {
			int[,] tiles = new int[board.numberOfXBlocks, board.numberOfZBlocks];
			Point prev = new Point(0,0);
			Point end = new Point(board.numberOfXBlocks-1, board.numberOfZBlocks-1);
			List<Point> points = new List<Point>();
			points.Add(prev);
			Point newPoint = generateNextPoint(prev, points, board.numberOfXBlocks, board.numberOfZBlocks);
			Debug.Log("newPoint: -" + newPoint);

			while  (!newPoint.Equals(end) ) {
				
				if (!newPoint.Equals(prev)) {
					Debug.Log("Add: -->" + newPoint);
					points.Add(newPoint);
					prev = newPoint;
				}
				newPoint = generateNextPoint(newPoint, points, board.numberOfXBlocks, board.numberOfZBlocks);
			}

			for (int i = 0; i < points.Count-1; i = i + 1) {
				Point thisP = points[i];	
				Point nextP = points[i+1];
				Debug.Log(thisP + " to " + nextP );
				connect(board, thisP, nextP);
			}

			board.shuffle();
		}

		private void connect(Board board, Point one, Point two) {
			BlockController tileOne = board.getBlockControllerAt(one.x, one.y);
			BlockController tileTwo = board.getBlockControllerAt(two.x, two.y);
			if (one.x==two.x) {
				Debug.Log("V");
				if (one.y > two.y) {
					Debug.Log("S");
					tileOne.addSouth();
					tileTwo.addNorth();
				} else {
					Debug.Log("N");
					tileOne.addNorth();
					tileTwo.addSouth();
				}
			} else {
				Debug.Log("H");
				if (one.x > two.x) {
					Debug.Log("W (" + one + "->w, " + two + "->e)");
					tileOne.addWest();
					tileTwo.addEast();
				} else {
					Debug.Log("E (" + one + "->e, " + two + "->w)");
					tileOne.addEast();
					tileTwo.addWest();
				}
			}

		}

		private Point generateNextPoint(Point start, List<Point> existingPoints, int maxX, int maxY) {
			float boolean = Random.Range(0.0f,1.0f);
			Point newPoint = start;
			if (boolean > 0.5) {
				if (newPoint.x + 1 < maxX) {
					newPoint.x = newPoint.x + 1;
				}
			} else {
				if (newPoint.y + 1 < maxY) {
					newPoint.y = newPoint.y + 1;
				}
			}
			return newPoint;
		}
	}

	struct Point {
		public int x,y;
		public Point(int px, int py) {
			x = px;
			y = py;
		}

		public bool Equals(Point other) {
			if (x != other.x) {
				return false;
			}

			if (y != other.y) {
				return false;
			}

			return true;
		}

		public override bool Equals(System.Object obj) {
			// If parameter is null return false.
			if (obj == null)
			{
				return false;
			}

			// If parameter cannot be cast to Point return false.
			if (!(obj is Point)) {
				return false;
			}

			Point p = (Point)obj;	
			// Return true if the fields match:
			return (x == p.x && y == p.y);
		}


		public override string ToString () {
			return string.Format ("[Point(" + x + ", " + y +")]");
		}
	}
		
		
	class Board {
		BlockController activeTile;
		int XIndexOfActiveBlock=0;
		int ZIndexOfActiveBlock=0;
		BlockController[,]tiles;

		public int numberOfXBlocks, numberOfZBlocks;

		public Board(int numberX, int numberZ, GameObject preFab, Transform transform) {
			numberOfXBlocks = numberX;
			numberOfZBlocks = numberZ;
			tiles = new BlockController[numberOfXBlocks, numberOfZBlocks];
			for (int indexX=0; indexX < numberOfXBlocks; indexX++) {
				for (int indexZ=0; indexZ < numberOfZBlocks; indexZ++) {
					if (!(indexZ == numberOfZBlocks-1 && indexX == numberOfXBlocks -1 )) {
						createBlock(indexX, indexZ, preFab, transform);
					}
				}
			}
			Debug.Log("ActiveTime: " + activeTile );
		}

		private void moveBlockInZ(int newZ) {
			float newZCoord = calcCoordFromIndex(newZ);
			float XCoord = calcCoordFromIndex(XIndexOfActiveBlock);
			activeTile.MoveBlock( XCoord, 0, newZCoord);
			tiles[XIndexOfActiveBlock, newZ]=activeTile;
			tiles[XIndexOfActiveBlock, ZIndexOfActiveBlock]=null;
			ZIndexOfActiveBlock=newZ;
		}

		private void moveBlockInX(int newX) {
			float newXCoord = calcCoordFromIndex(newX);
			float ZCoord = calcCoordFromIndex(ZIndexOfActiveBlock);
			activeTile.MoveBlock( newXCoord, 0, ZCoord);
			tiles[newX, ZIndexOfActiveBlock]=activeTile;
			tiles[XIndexOfActiveBlock, ZIndexOfActiveBlock]=null;
			XIndexOfActiveBlock=newX;
		}

		public void swapActiveBlockWithSpace() {
			int newZ = inc(ZIndexOfActiveBlock, numberOfZBlocks);
			if (canSwap(XIndexOfActiveBlock, newZ)) {
				Debug.Log("Swap with Y, up");
				moveBlockInZ(newZ);
				return;
			}

			newZ = dec(ZIndexOfActiveBlock, numberOfZBlocks);
			if (canSwap(XIndexOfActiveBlock, newZ)) {
				Debug.Log("Swap with Y, down");
				moveBlockInZ(newZ);
				return;
			}

			int newX = inc(XIndexOfActiveBlock, numberOfXBlocks);
			if (canSwap(newX, ZIndexOfActiveBlock)) {
				Debug.Log("Swap with X, right");
				moveBlockInX(newX);
				return;
			}

			newX = dec(XIndexOfActiveBlock, numberOfXBlocks);
			if (canSwap(newX, ZIndexOfActiveBlock)) {
				Debug.Log("Swap with X, left");
				moveBlockInX(newX);
				return;
			}
			Debug.Log("NO SWAP!!");	
			
		}

		public void shuffle() {
			for (int x=0; x<this.numberOfXBlocks; x++) {
				for (int z=0; z<this.numberOfZBlocks; x++) {
					//if (x!=0 || z!=0) {
						int swapX = Random.Range(0, this.numberOfXBlocks-1);
						int swapZ = Random.Range(0, this.numberOfZBlocks-1);
						BlockController one = getBlockControllerAt(swapX, swapZ);
						Vector3 locationOne = new Vector3(calcCoordFromIndex(swapX), 0.0F, calcCoordFromIndex(swapZ));
						BlockController two = getBlockControllerAt(x,z);
						Vector3 locationTwo = new Vector3(calcCoordFromIndex(x), 0.0F, calcCoordFromIndex(z));

						one.transform.position = locationTwo;
						two.transform.position = locationOne;

						tiles[swapX, swapZ] = two;
						tiles[x,z]=one;	
					//}
				}
			}
		}

		public BlockController getBlockControllerAt(int x, int z) {
			return tiles[x,z];
		}

		private float calcCoordFromIndex(int index) {
			return index * 12.0F;
		}

		public bool canSwap(int x, int z) {
			Debug.Log(tiles);
			//validMove tells you can't select an empty space, but you can move into an empty space
			return !validMove(x,z);
		}

		public void moveActiveBlockRight() {
			int newX = dec(XIndexOfActiveBlock, numberOfXBlocks);
			Debug.Log("NewX: " + newX);
			if (validMove(newX, ZIndexOfActiveBlock)) {
				Debug.Log("New: [" + newX + ", " + ZIndexOfActiveBlock + "]");
				updateActiveBlock(newX, ZIndexOfActiveBlock);
			}
		}

		public void moveActiveBlockLeft() {
			int newX = inc(XIndexOfActiveBlock, numberOfXBlocks);
			Debug.Log("NewX: " + newX);
			if (validMove(newX, ZIndexOfActiveBlock)) {
				Debug.Log("New: [" + newX + ", " + ZIndexOfActiveBlock + "]");
				updateActiveBlock(newX, ZIndexOfActiveBlock);
			}
		}

		public void moveActiveBlockUp() {
			int newY = inc(ZIndexOfActiveBlock, numberOfZBlocks);
			Debug.Log("NewY: " + newY);
			if (validMove(XIndexOfActiveBlock, newY)) {
				Debug.Log("New: [" + XIndexOfActiveBlock + ", " + newY + "]");
				updateActiveBlock(XIndexOfActiveBlock, newY);
			}
		}

		public void moveActiveBlockDown() {
			int newY=dec(ZIndexOfActiveBlock, numberOfZBlocks);
			Debug.Log("NewY: " + newY);
			if (validMove(XIndexOfActiveBlock, newY)) {
				Debug.Log("New: [" + ZIndexOfActiveBlock + ", " + newY + "]");
				updateActiveBlock(XIndexOfActiveBlock, newY);
			}
		}



		private void updateActiveBlock(int newX, int newY) {
			activeTile.Active=false;
			XIndexOfActiveBlock=newX;
			ZIndexOfActiveBlock=newY;
			activeTile = tiles[XIndexOfActiveBlock, ZIndexOfActiveBlock];
			activeTile.Active=true;
		}

		private bool validMove(int X, int Y) {
			BlockController tile2Check = tiles[X, Y];
			return tile2Check!=null;
		}

		private int inc(int index, int numberOfBlocks) {
			int tmp = index + 1;
			if (tmp > (numberOfBlocks-1)) {
				tmp=index;
			}
			return tmp;
		}

		private int dec(int index, int numberOfBlocks) {
			int tmp = index - 1;
			if (tmp < 0) {
				tmp=index;
			}
			return tmp;
		}

		private void createBlock(int x, int z, GameObject preFab, Transform transform ) {
			Vector3 location = new Vector3(calcCoordFromIndex(x), 0.0F, calcCoordFromIndex(z));

			GameObject block = (GameObject)Instantiate(preFab, location, transform.rotation);
			BlockController blockController = (BlockController) block.GetComponent(typeof(BlockController));
			if (x==0 && z==0) {
				activeTile = blockController;
				activeTile.Active=true;

			}
			tiles[x,z]=blockController;
			blockController.removeAll();
		}
	}
}


