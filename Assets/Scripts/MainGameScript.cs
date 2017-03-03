using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainGameScript : MonoBehaviour {

	public GameObject pBlockPrefab;

	BlockController activeTile;
	int activeTileIndex=0;
	int delayCount=0;
	public int Delay;
	Board board;

	List<BlockController> tiles = new List<BlockController>();

	// Use this for initialization
	void Start () {
		board = new Board(3,3, pBlockPrefab, transform);
		Debug.Log("Starting a Main Game Script");

	}
	
	// Update is called once per frame
	void Update () {
		int index=activeTileIndex;
		if (delayCount==0) {
			if (Input.GetKey("z")) {
				board.moveActiveBlockRight();
			};

			if (Input.GetKey("x")) {
				board.moveActiveBlockLeft();
			};

			if (Input.GetKey("v")) {
				board.moveActiveBlockDown();
			};

			if (Input.GetKey("c")) {
				board.moveActiveBlockUp();
			};
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
		int YIndexOfActiveBlock=0;
		BlockController[,]tiles;

		int numberOfXBlocks;
		int numberOfYBlocks;

		public Board(int numberX, int numberY, GameObject preFab, Transform transform) {
			numberOfXBlocks = numberX;
			numberOfYBlocks = numberY;
			tiles = new BlockController[numberOfXBlocks, numberOfYBlocks];
			for (int indexX=0; indexX < numberOfXBlocks; indexX++) {
				for (int indexY=0; indexY < numberOfYBlocks; indexY++) {
					if (!(indexY == numberOfYBlocks-1 && indexX == numberOfXBlocks -1 )) {
						createBlock(indexX, indexY, preFab, transform);
					}
				}
			}
			Debug.Log("ActiveTime: " + activeTile );
		}

		public void moveActiveBlockRight() {
			int newX = dec(XIndexOfActiveBlock, numberOfXBlocks);
			if (validMove(newX, YIndexOfActiveBlock)) {
				updateActiveBlock(newX, YIndexOfActiveBlock);
			}
		}

		public void moveActiveBlockUp() {
			int newY = inc(YIndexOfActiveBlock, numberOfYBlocks);
			if (validMove(XIndexOfActiveBlock, newY)) {
				updateActiveBlock(XIndexOfActiveBlock, newY);
			}
		}

		public void moveActiveBlockDown() {
			int newY=dec(YIndexOfActiveBlock, numberOfYBlocks);
			if (validMove(XIndexOfActiveBlock, newY)) {
				updateActiveBlock(XIndexOfActiveBlock, newY);
			}
		}

		public void moveActiveBlockLeft() {
			int newX = inc(XIndexOfActiveBlock, numberOfXBlocks);
			if (validMove(newX, YIndexOfActiveBlock)) {
				updateActiveBlock(newX, YIndexOfActiveBlock);
			}
		}

		private void updateActiveBlock(int newX, int newY) {
			Debug.Log("nX: " + newX );
			Debug.Log("nY: " + newY );
			Debug.Log("X: " + XIndexOfActiveBlock );
			Debug.Log("Y: " + YIndexOfActiveBlock );
			activeTile.Active=false;
			XIndexOfActiveBlock=newX;
			YIndexOfActiveBlock=newY;
			activeTile = tiles[XIndexOfActiveBlock, YIndexOfActiveBlock];
			activeTile.Active=true;
			Debug.Log("X: " + XIndexOfActiveBlock );
			Debug.Log("Y: " + YIndexOfActiveBlock );
		}

		private bool validMove(int X, int Y) {
			BlockController tile2Check = tiles[X, Y];
			return tile2Check!=null;
		}

		private int inc(int index, int numberOfBlocks) {
			int tmp = index + 1;
			if (tmp > (numberOfBlocks-1)) {
				tmp=0;
			}
			return tmp;
		}

		private int dec(int index, int numberOfBlocks) {
			int tmp = index - 1;
			if (tmp < 0) {
				tmp=numberOfBlocks-1;
			}
			return tmp;
		}

		private void createBlock(int x, int y, GameObject preFab, Transform transform ) {
			Vector3 location = new Vector3(x * 12.0F, 0.0F, y * 12.0F);

			GameObject block = (GameObject)Instantiate(preFab, location, transform.rotation);
			BlockController blockController = (BlockController) block.GetComponent(typeof(BlockController));
			if (x==0 && y==0) {
				activeTile = blockController;
				activeTile.Active=true;
			}
			tiles[x,y]=blockController;
		}
	}
}


