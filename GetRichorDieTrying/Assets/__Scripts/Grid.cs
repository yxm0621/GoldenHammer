using UnityEngine;
using System.Collections;

public class Grid : MonoBehaviour {
	public int totalLength = 100;
	int length;
	int width;

	public GameObject[] building;
	public GameObject[] sidewalk;
	public GameObject[] road;
	
	public int[,] buildingData;
	public int[,] sidewalkData;

	public GridController grid;
	public bool[,] playerMove;

	public GameObject			gridObject;
	public Grid					thisGridScript;

	// Use this for initialization
	void Start () {

		/*
		if(gridObject == null){
			DontDestroyOnLoad(this.gameObject);
			DontDestroyOnLoad(this);
			gridObject = this.gameObject;
			thisGridScript = this;
		} else if(gridObject != this.gameObject){ //If there is another Game Manager destroy's this one
			Destroy(gameObject);
		}
		*/
	}
	
	// Update is called once per frame
	void Update () {
	}

	//read new object list when scene changes
	public void setGrid(GameObject[] setBuilding, GameObject[] setSide, GameObject[] setRoad, int[,] setBuildingData, int[,] setSidewalkData){
		building = setBuilding;
		sidewalk = setSide;
		road = setRoad;
		buildingData = setBuildingData;
		sidewalkData = setSidewalkData;
	}

	//instanciate the new grid
	public void generateGrid(GameObject spawnPoint, Vector3 pos, int gridLength, int gridWidth){
		this.length = gridLength;
		this.width = gridWidth;
		GameObject newSeg = (GameObject) Instantiate (spawnPoint, pos, Quaternion.identity);
		newSeg.AddComponent<LevelSegments>();
        newSeg.name = "spawnPoint";
		newSeg.tag = "Segment";

		//road
		GameObject newRoad = (GameObject) Instantiate (road[0], new Vector3(pos.x, pos.y, pos.z), Quaternion.identity);
		newRoad.transform.parent = newSeg.transform;
		//sidewalk
		newRoad = (GameObject) Instantiate (road[1], new Vector3(pos.x+1.5f, pos.y, pos.z), Quaternion.identity);
		newRoad.transform.parent = newSeg.transform;
		newRoad = (GameObject) Instantiate (road[1], new Vector3(pos.x-1.5f, pos.y, pos.z), Quaternion.identity);
		newRoad.transform.parent = newSeg.transform;
		//ground
		newRoad = (GameObject) Instantiate (road[2], new Vector3(pos.x+length/4+1, pos.y, pos.z), Quaternion.identity);
		newRoad.transform.parent = newSeg.transform;
		newRoad = (GameObject) Instantiate (road[2], new Vector3(pos.x-length/4-1, pos.y, pos.z), Quaternion.identity);
		newRoad.transform.parent = newSeg.transform;
		//water
		newRoad = (GameObject) Instantiate (road[3], new Vector3(pos.x+totalLength/4+length/4, pos.y, pos.z), Quaternion.identity);
		newRoad.transform.parent = newSeg.transform;
		newRoad = (GameObject) Instantiate (road[3], new Vector3(pos.x-totalLength/4-length/4, pos.y, pos.z), Quaternion.identity);
		newRoad.transform.parent = newSeg.transform;

		grid = new GridController(length, width, building, sidewalk, buildingData, sidewalkData);
		grid.createGrid ();

		for (int x = 0; x < length; x++) {
			for (int z = 0; z < width; z++) {
				if(grid.items[x ,z] != null){
//					Debug.Log("Instantiate " + grid.items[x ,z].obj.name + " at pos: " + new Vector3(pos.x-length/2+x, pos.y, pos.z-width/2+z));
					GameObject newObj;
					if(grid.items[x ,z].obj.name.Contains("env")
//					   || grid.items[x ,z].obj.name.Contains("Tree")
//					   || grid.items[x ,z].obj.name.Contains("Cube")
					   ) {
						newObj = (GameObject) Instantiate (grid.items[x ,z].obj, new Vector3(pos.x-length/2+x +.5f, pos.y + .5f, pos.z-width/2+z +.5f), Quaternion.identity);
					} else if (grid.items[x ,z].obj.name.Contains("Bomb")
					           || grid.items[x ,z].obj.name.Contains("Special")){
						newObj = (GameObject) Instantiate (grid.items[x ,z].obj, new Vector3(pos.x-length/2+x +.5f, pos.y + 1, pos.z-width/2+z +.5f), Quaternion.identity);
					} else if (x == 4){
						newObj = (GameObject) Instantiate (grid.items[x ,z].obj, new Vector3(pos.x-length/2+x +.3f, pos.y + 1, pos.z-width/2+z +.5f), Quaternion.identity);
					} else if (x == 7){
						newObj = (GameObject) Instantiate (grid.items[x ,z].obj, new Vector3(pos.x-length/2+x +.7f, pos.y + 1, pos.z-width/2+z +.5f), Quaternion.identity);
					} else {
						newObj = (GameObject) Instantiate (grid.items[x ,z].obj, new Vector3(pos.x-length/2+x +.5f, pos.y, pos.z-width/2+z +.5f), Quaternion.identity);
					}
//					GameObject newObj = (GameObject) Instantiate (grid.items[x ,z].obj, new Vector3(pos.x-length/2+x +.5f, pos.y, pos.z-width/2+z +.5f), Quaternion.identity);
					newObj.transform.parent = newSeg.transform;
					newObj.name = grid.items[x, z].obj.name;
//					if (newObj.name == "Mshr1Prefab" || 
//					    newObj.name == "Mshr2Prefab" ||
//					    newObj.name == "Mshr3Prefab" ||
//					    newObj.name == "Mshr4Prefab" ||
//					    newObj.name == "Mshr5Prefab") {
//						newObj.transform.localEulerAngles = new Vector3 (270, 0, 0);
//						newObj.transform.position -= new Vector3(0,.5f,0);
//					}
//					newObj.GetComponent<ObjectManager>().objPosX = x;
//					newObj.GetComponent<ObjectManager>().objPosZ = z;
//					newObj.GetComponent<ObjectManager>().objLength = grid.items[x ,z].length;
//					newObj.GetComponent<ObjectManager>().objWidth = grid.items[x ,z].width;
				}
			}
		}
//		playerMove = grid.canMove;
//		GameManager.manager.addMovement (playerMove);
//		newSeg.GetComponent<LevelSegments> ().segEmpty = playerMove;
	}
}

//the grid object which arranges the items on it
public class GridController {
	public int gridLenth;
	public int gridWidth;
	public int sidewalkBegin;

	public GameObject[] building;
	public GameObject[] sidewalk;
	public int[,] buildingData;
	public int[,] sidewalkData;

	public GridItem[,] items;
	public bool[,] canMove;

	//construction of the class
	public GridController(int gridLenth, int gridWidth, GameObject[] building, GameObject[] sidewalk, int[,] buildingData, int[,] sidewalkData) {
		this.gridLenth = gridLenth;
		this.gridWidth = gridWidth;
		this.sidewalkBegin = gridLenth / 2 - 2;
		this.items = new GridItem[gridLenth, gridWidth];
		this.canMove = new bool[gridLenth, gridWidth];
		this.building = building;
		this.sidewalk = sidewalk;
		this.buildingData = buildingData;
		this.sidewalkData = sidewalkData;
	}

	//randomly choose the objects
	public int getObj(GameObject[] objs, int min, int max, int maxWidth){
		int i = Random.Range(min, max);
//		Debug.Log (i);
		GameObject obj = objs[i];
		if (obj.transform.localScale.z > maxWidth) {
			//when the item's width beyond the edge of the grid, change another item
			return getObj(objs, min, max, maxWidth);
		} else {
			return i;
		}
	}

	//create the new grid
	public void createGrid (){
		for(int x = 0; x < gridLenth; ++x) {
			if (x != sidewalkBegin && x != (sidewalkBegin+1) && 
			    x != (sidewalkBegin+2) && x != (sidewalkBegin+3)) {
				//generate buildings on lanes except road and sidewalk
				for (int z = 0; z < gridWidth; ++z) {
					int hasObject = Random.Range(0,2);
					if(hasObject == 0) {
						canMove[x,z] = true;
						continue;
					}
					int i = getObj(building, 0, building.Length, gridWidth-z);
					GameObject obj = building[i];
					items[x, z] = new GridItem(obj, buildingData[i,0], buildingData[i,1], buildingData[i,2], buildingData[i,3]);
					z += (buildingData[i,1] - 1);
					if ((buildingData[i,0] > 1)&&(x != sidewalkBegin-1)&&(x != gridLenth-1)) {
						//avoid overlap
						x++;
					}
				}
			}
		}
		for(int x = 0; x < gridLenth; ++x) {
			if (x == sidewalkBegin || x == (sidewalkBegin+3)) {
				//generate items on sidewalk lanes
				for (int z = 0; z < gridWidth; ++z) {
					int hasObject = Random.Range(0,2);
					if(hasObject == 0) {
						canMove[x,z] = true;
						continue;
					}
					int i = getObj(sidewalk, 0, sidewalk.Length, gridWidth-z);
					GameObject obj = sidewalk[i];
					items[x, z] = new GridItem(obj, sidewalkData[i,0], sidewalkData[i,1], sidewalkData[i,2], sidewalkData[i,3]);
					z += (sidewalkData[i,1] - 1);
				}
			}
		}
		for(int x = sidewalkBegin + 1; x < sidewalkBegin+3; ++x) {
			for (int z = 0; z < gridWidth; ++z) {
				//player can move on the road
				canMove[x,z] = true;
			}
		}
	}
}

//items on the grid
public class GridItem {
	public GameObject obj;
	public int length;
	public int width;
	public float xOffset;
	public float zOffset;

	public GridItem(GameObject obj, int length, int width, float xOffset, float zOffset) {
		this.obj = obj;
//		this.obj.transform.localScale.x = length;
//		this.obj.transform.localScale.z = width;
		this.length = length;
		this.width = width;
		this.xOffset = xOffset;
		this.zOffset = zOffset;
	}
}