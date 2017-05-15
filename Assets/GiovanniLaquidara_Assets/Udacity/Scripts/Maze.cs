using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Maze : MonoBehaviour {

	[System.Serializable]
	public class Cell {
		public bool visited;
		public GameObject north;
		public GameObject east;
		public GameObject west;
		public GameObject south;
		public GameObject waypoint;
		public bool hasCoin;

	}


	public GameObject wall;
	public GameObject key;
	public GameObject waypoint;
	public GameObject coinToCatch;

	public float wallLenght = 15.0f;
	public int xsize = 5;
	public int ysize = 5;

	private Vector3 initialPos;

	private int numberOfCoins = 5;

	private GameObject wallHolder;
	private GameObject keyHolder;
	private Cell[] cells;
	public int currentCell = 0;
	private int totalCells;
	private int visitedCells = 0;
	private bool startedBuilding = false;
	private int currentNeighbour = 0;
	private List<int> lastCells;
	private int backingUp = 0;
	private int wallToBreak = 0;

	// Use this for initialization
	void Start () {
		 CreateWalls();
	
	}

	void CreateWalls() {

		wallHolder = new GameObject();
		wallHolder.name = "Maze";


		initialPos = new Vector3 ((-xsize / 2) + wallLenght/2,0.0f,(-ysize/2) + wallLenght/2);
		Vector3 myPosition = initialPos;
		GameObject tempWall;


		for (int i = 0; i <= ysize; i++) {
			for (int j = 0; j < xsize; j++) {
				myPosition = new Vector3 (initialPos.x + (j * wallLenght), -4.5f, initialPos.z + (i * wallLenght) -wallLenght);
				tempWall = Instantiate (wall, myPosition, Quaternion.identity) as GameObject;
				tempWall.transform.parent = wallHolder.transform;
			}
		}

		for (int i = 0; i < ysize; i++) {
			for (int j = 0; j <= xsize; j++) {
				myPosition = new Vector3 (initialPos.x + (j * wallLenght) - wallLenght / 2, -4.5f,initialPos.z + (i * wallLenght)-wallLenght/2);
				tempWall = Instantiate (wall, myPosition, Quaternion.Euler(0.0f, 90.0f, 0.0f)) as GameObject;
				tempWall.transform.parent = wallHolder.transform;

			}
		}

		CreateCells ();

		CreateCoins ();

		PlaceKey ();
	}


	void PlaceKey() {
		int choosedCellForKey = Random.Range (0, totalCells);

		Vector3 keyPosition = new Vector3 (cells[choosedCellForKey].south.transform.position.x+wallLenght/3, -4.5f, cells[choosedCellForKey].east.transform.position.z+wallLenght/3);
		Instantiate (key, keyPosition, Quaternion.identity);

	}

	void CreateCells() {
		lastCells = new List<int>();
		lastCells.Clear ();
		totalCells = xsize * ysize;

		GameObject[] allWalls;
		int children = wallHolder.transform.childCount;
		allWalls = new GameObject[children];
		cells = new Cell[xsize * ysize];
		int eastWestProcess = 0;
	

		//Get all children
		for (int i = 0; i < children; i++) {
			allWalls [i] = wallHolder.transform.GetChild (i).gameObject;
		}
		//Assign walls to the cells
		for (int cellprocess = 0; cellprocess < cells.Length; cellprocess++) {

			cells [cellprocess] = new Cell ();
			cells [cellprocess].east = allWalls [eastWestProcess];
			cells [cellprocess].west = allWalls [eastWestProcess+xsize];

			cells [cellprocess].south = allWalls [eastWestProcess+(xsize)*(ysize+1)+(cellprocess / xsize)];
			eastWestProcess++;
			cells [cellprocess].north = allWalls [eastWestProcess+(xsize)*(ysize+1)+(cellprocess / xsize)];

			Vector3 waypointPosition = new Vector3 (cells[cellprocess].south.transform.position.x+wallLenght/2, -4.5f, cells[cellprocess].east.transform.position.z+wallLenght/2);

			cells [cellprocess].waypoint = Instantiate (waypoint, waypointPosition, Quaternion.identity) as GameObject; 

			cells [cellprocess].hasCoin = false;

		}


		CreateMaze ();
	}

	void CreateCoins() {
		for (int cellprocess = 0; cellprocess < cells.Length; cellprocess++) {
			if (numberOfCoins > 0) {
				int cellsWithCoin = Random.Range (0, totalCells);
				if (cells [cellsWithCoin].hasCoin == false) {

					Vector3 coinPosition = new Vector3 (cells [cellsWithCoin].south.transform.position.x + wallLenght / 4, -4.5f, cells [cellsWithCoin].east.transform.position.z + wallLenght / 4);

					Instantiate (coinToCatch, coinPosition, Quaternion.identity);

					cells [cellsWithCoin].hasCoin = true;

					numberOfCoins--;
				}
			}
		}
	}

	void CreateMaze() {
		while (visitedCells < totalCells) {
			if (startedBuilding) {
				GetNeighbour ();
				if (cells [currentNeighbour].visited == false && cells [currentCell].visited == true) {
					BreakWall ();
					cells [currentNeighbour].visited = true;
					visitedCells++;
					lastCells.Add (currentCell);
					currentCell = currentNeighbour;
					if (lastCells.Count > 0) {
						backingUp = lastCells.Count - 1;
					}
				}
			} else {
				currentCell = Random.Range (0, totalCells);
				cells [currentCell].visited = true;
				visitedCells++;
				startedBuilding = true;
			}
		}

		BreakEntranceAndExit ();
		Debug.Log ("Completed");
	}

	void BreakWall() {
		switch (wallToBreak) {
		case 1: Destroy(cells[currentCell].north); break;
		case 2: Destroy(cells[currentCell].east); break;
		case 3: Destroy(cells[currentCell].west); break;
		case 4: Destroy(cells[currentCell].south); break;
		}
	}

	void BreakEntranceAndExit() {
		Destroy(cells[0].east);
		Destroy(cells[(xsize*ysize)-xsize/2].west);
	}


	void GetNeighbour() {
		int length = 0;
		int[] neighbours = new int[4];
		int[] connectingWall = new int[4];
		int check = 0;
	    check = ((currentCell + 1 )/xsize);
		check -= 1;
		check *= xsize;
		check += xsize;


		//west 
		if (currentCell + 1 < totalCells && (currentCell + 1) != check) {
			if (cells[currentCell + 1].visited == false) {
				neighbours [length] = currentCell + 1;
				connectingWall [length] = 1;
				length++;
			}
		}
		//east 
		if (currentCell - 1 >= 0 && currentCell != check) {
			if (cells[currentCell - 1].visited == false) {
				neighbours [length] = currentCell - 1;
				connectingWall [length] = 4;
				length++;
			}
		}
		//north
		if (currentCell + ysize < totalCells) {
			if (cells[currentCell + ysize].visited == false) {
				neighbours [length] = currentCell + ysize;
				connectingWall [length] = 3;
				length++;
			}
		}
		//south
		if (currentCell - ysize >= 0) {
			if (cells[currentCell - ysize].visited == false) {
				neighbours [length] = currentCell - ysize;
				connectingWall [length] = 2;
				length++;
			}
		}

		if (length != 0) {
			int theChoosenOne = Random.Range (0, length);
			currentNeighbour = neighbours [theChoosenOne];
			wallToBreak = connectingWall [theChoosenOne];
		} else {
			if (backingUp > 0) {
				currentCell = lastCells [backingUp];
				backingUp--;
			}
		}

	
	}

	// Update is called once per frame
	void Update () {
	
	}
}
