using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainGameScript : MonoBehaviour {

	public GameObject pBlockPrefab;

	int delayCount=0;
	public int Delay;
	Board board;

	// Use this for initialization
	void Start () {
		board = new Board(2,2, pBlockPrefab, transform);
		Debug.Log("Starting a Main Game Script");

	}
	
	// Update is called once per frame
	void Update () {
		
		if (delayCount==0) {
			if (Input.GetKey("z")) {
				board.moveActiveBlockRight();
			}

			if (Input.GetKey("x")) {
				board.moveActiveBlockLeft();
			}

			if (Input.GetKey("v")) {
				board.moveActiveBlockDown();
			}

			if (Input.GetKey("c")) {
				board.moveActiveBlockUp();
			}

			if (Input.GetKey("s")) {
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
		
	class Board {
		BlockController activeTile;
		int XIndexOfActiveBlock=0;
		int ZIndexOfActiveBlock=0;
		BlockController[,]tiles;

		int numberOfXBlocks;
		int numberOfZBlocks;

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
			if (validMove(newX, ZIndexOfActiveBlock)) {
				updateActiveBlock(newX, ZIndexOfActiveBlock);
			}
		}

		public void moveActiveBlockUp() {
			int newY = inc(ZIndexOfActiveBlock, numberOfZBlocks);
			if (validMove(XIndexOfActiveBlock, newY)) {
				updateActiveBlock(XIndexOfActiveBlock, newY);
			}
		}

		public void moveActiveBlockDown() {
			int newY=dec(ZIndexOfActiveBlock, numberOfZBlocks);
			if (validMove(XIndexOfActiveBlock, newY)) {
				updateActiveBlock(XIndexOfActiveBlock, newY);
			}
		}

		public void moveActiveBlockLeft() {
			int newX = inc(XIndexOfActiveBlock, numberOfXBlocks);
			if (validMove(newX, ZIndexOfActiveBlock)) {
				updateActiveBlock(newX, ZIndexOfActiveBlock);
			}
		}

		private void updateActiveBlock(int newX, int newY) {
			Debug.Log("nX: " + newX );
			Debug.Log("nY: " + newY );
			Debug.Log("X: " + XIndexOfActiveBlock );
			Debug.Log("Y: " + ZIndexOfActiveBlock );
			activeTile.Active=false;
			XIndexOfActiveBlock=newX;
			ZIndexOfActiveBlock=newY;
			activeTile = tiles[XIndexOfActiveBlock, ZIndexOfActiveBlock];
			activeTile.Active=true;
			Debug.Log("X: " + XIndexOfActiveBlock );
			Debug.Log("Y: " + ZIndexOfActiveBlock );
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
		}
	}
}


