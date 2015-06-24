using UnityEngine;
using System.Collections;

public class Grid : MonoBehaviour {
	public int totalLength = 100;
	int length; //total length of the grid(x axis)
	int width; // width of each grid(z axis)

	public GameObject[] building;
	public GameObject[] sidewalk;
	public GameObject[] road;
    public GameObject holeObj;
	
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

	//read new object list when scene changes
	public void setGrid(GameObject[] setBuilding, GameObject[] setSide, GameObject[] setRoad, int[,] setBuildingData, int[,] setSidewalkData){
		building = setBuilding;
		sidewalk = setSide;
		road = setRoad;
		buildingData = setBuildingData;
		sidewalkData = setSidewalkData;
	}

	//instanciate the new grid
	public GameObject generateGrid(GameObject spawnPoint, Vector3 pos, int gridLength, int gridWidth) {
		this.length = gridLength;
		this.width = gridWidth;
		GameObject newSeg = (GameObject) Instantiate (spawnPoint, pos, Quaternion.identity);
        newSeg.name = "spawnPoint";
		newSeg.tag = "Segment";
        newSeg.AddComponent<DestroyObjects>();
        newSeg.GetComponent<DestroyObjects>().maxDistance = 4f;

		//road
        int hole = Random.Range((width * 2)+1, (width * 3)); //cancel the hole for now
        generateRoad(road[0], hole, pos, newSeg);

		//sidewalk
        //generateSidewalk(road[1], pos, newSeg);
        GameObject newRoad;
        //Generate right sidewalk
        newRoad = (GameObject)Instantiate(road[1], new Vector3(pos.x + 1.5f, pos.y - .5f, pos.z), Quaternion.identity);
        newRoad.transform.parent = newSeg.transform;
        //Generate left sidewalk
        newRoad = (GameObject)Instantiate(road[1], new Vector3(pos.x - 1.5f, pos.y - .5f, pos.z), Quaternion.identity);
        newRoad.transform.parent = newSeg.transform;

		//ground
        //generateGround(road[2], pos, newSeg);
        //Generate right side ground
        newRoad = (GameObject)Instantiate(road[2], new Vector3(pos.x + length / 4 + 1, pos.y - .5f, pos.z), Quaternion.identity);
        newRoad.transform.parent = newSeg.transform;
        //Generate left side ground
        newRoad = (GameObject)Instantiate(road[2], new Vector3(pos.x - length / 4 - 1, pos.y - .5f, pos.z), Quaternion.identity);
        newRoad.transform.parent = newSeg.transform;

		//water
        //generateWater(road[3], pos, newSeg);
        //Generate right side water
        newRoad = (GameObject)Instantiate(road[3], new Vector3(pos.x + totalLength / 4 + length / 4, pos.y - .5f, pos.z), Quaternion.identity);
        newRoad.transform.parent = newSeg.transform;
        //Generate left side water
        newRoad = (GameObject)Instantiate(road[3], new Vector3(pos.x - totalLength / 4 - length / 4, pos.y - .5f, pos.z), Quaternion.identity);
        newRoad.transform.parent = newSeg.transform;

        //underground
        newRoad = (GameObject)Instantiate(road[4], new Vector3(pos.x, -2f, pos.z), Quaternion.identity);
        newRoad.name = road[4].name;
        newRoad.transform.parent = newSeg.transform;

        //Create the grid
		grid = new GridController(length, width, building, sidewalk, buildingData, sidewalkData);
		grid.createGrid ();

        //Render the grid
        float posX;
        float posY;
        float posZ;
        GameObject newObj;
		for (int x = 0; x < length; x++) {
			for (int z = 0; z < width; z++) {
				if(grid.items[x ,z] != null){
                    posX = pos.x - length / 2 + x + .5f;
                    posY = pos.y;
                    posZ = pos.z - width / 2 + z + .5f;
//					Debug.Log("Instantiate " + grid.items[x ,z].obj.name + " at pos: " + new Vector3(posX, posY, posZ));
					if (grid.items[x ,z].obj.name.Contains("PowerUp")){
                        newObj = (GameObject)Instantiate(grid.items[x, z].obj, new Vector3(posX, posY + .3f, posZ), Quaternion.identity);
					} else if (x == 4){
                        newObj = (GameObject)Instantiate(grid.items[x, z].obj, new Vector3(posX - .3f, posY, posZ), Quaternion.identity);
					} else if (x == 7){
                        newObj = (GameObject)Instantiate(grid.items[x, z].obj, new Vector3(posX + .3f, posY, posZ), Quaternion.identity);
					} else {
                        newObj = (GameObject)Instantiate(grid.items[x, z].obj, new Vector3(posX, posY, posZ), Quaternion.identity);
                        if (grid.items[x, z].obj.name.Contains("Building_1")) {
                            if (posX > 0) {
                                newObj.transform.localEulerAngles = new Vector3(0,90,0);
                            } else {
                                newObj.transform.localEulerAngles = new Vector3(0, -90, 0);
                            }
                        }
					}
//					GameObject newObj = (GameObject) Instantiate (grid.items[x ,z].obj, new Vector3(posX, posY, posZ), Quaternion.identity);
					newObj.transform.parent = newSeg.transform;
					newObj.name = grid.items[x, z].obj.name;
				}
			}
		}
        return newSeg;
	}

    //generate road with/without a hole
    void generateRoad(GameObject road, int hole, Vector3 pos, GameObject newSeg) {
        GameObject newRoad;
        for (int i = 0; i < width * 2; i++) {
            float posX = (i < width) ? (pos.x - .5f) : (pos.x + .5f);
            float posY = pos.y - .5f;
            float posZ = pos.z - 2.5f + (i % width);
            
            if (i == hole) {
                newRoad = (GameObject)Instantiate(holeObj, new Vector3(posX, posY, posZ), Quaternion.identity);
                newRoad.name = holeObj.name;
                newRoad.transform.parent = newSeg.transform;
            //} else {
            //    newRoad = (GameObject)Instantiate(road, new Vector3(posX, posY, posZ), Quaternion.identity);
            //    newRoad.name = road.name;
            //    newRoad.transform.parent = newSeg.transform;
            }
            newRoad = (GameObject)Instantiate(road, new Vector3(posX, posY, posZ), Quaternion.identity);
            newRoad.name = road.name;
            newRoad.transform.parent = newSeg.transform;
        }
    }

    void generateSidewalk(GameObject road, Vector3 pos, GameObject newSeg) {

        GameObject newRoad = (GameObject)Instantiate(road, new Vector3(pos.x, pos.y, pos.z), Quaternion.identity);
        newRoad.transform.parent = newSeg.transform;
    }

    void generateGround(GameObject road, Vector3 pos, GameObject newSeg) {

        GameObject newRoad = (GameObject)Instantiate(road, new Vector3(pos.x, pos.y, pos.z), Quaternion.identity);
        newRoad.transform.parent = newSeg.transform;
    }
    void generateWater(GameObject road, Vector3 pos, GameObject newSeg) {

        GameObject newRoad = (GameObject)Instantiate(road, new Vector3(pos.x, pos.y, pos.z), Quaternion.identity);
        newRoad.transform.parent = newSeg.transform;
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

	//construction of the class
	public GridController(int gridLenth, int gridWidth, GameObject[] building, GameObject[] sidewalk, int[,] buildingData, int[,] sidewalkData) {
		this.gridLenth = gridLenth;
		this.gridWidth = gridWidth;
		this.sidewalkBegin = gridLenth / 2 - 2;
		this.items = new GridItem[gridLenth, gridWidth];
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
                        //Don't create object, leave the empty space
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
                        //Don't create object, leave the empty space
						continue;
					}
					int i = getObj(sidewalk, 0, sidewalk.Length, gridWidth-z);
					GameObject obj = sidewalk[i];
					items[x, z] = new GridItem(obj, sidewalkData[i,0], sidewalkData[i,1], sidewalkData[i,2], sidewalkData[i,3]);
					z += (sidewalkData[i,1] - 1);
				}
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