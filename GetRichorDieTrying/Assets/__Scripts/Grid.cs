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
    public GameObject[] pickup;
	
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
        grid = new GridController(length, width, building, sidewalk, pickup, buildingData, sidewalkData);
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
    public GameObject[] pickup;

	public GridItem[,] items;

	//construction of the class
	public GridController(int gridLenth, int gridWidth, GameObject[] building, GameObject[] sidewalk, GameObject[] pickup, int[,] buildingData, int[,] sidewalkData) {
		this.gridLenth = gridLenth;
		this.gridWidth = gridWidth;
		this.sidewalkBegin = gridLenth / 2 - 2;
		this.items = new GridItem[gridLenth, gridWidth];
		this.building = building;
		this.sidewalk = sidewalk;
        this.pickup = pickup;
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

    //randomly put the pickupItem
    public void PutItem(int x, int z) {
        int i = getObj(pickup, 0, pickup.Length, 1);
        items[x, z] = new GridItem(pickup[i], 1, 1);
    }

	//create the new grid
	public void createGrid (){
        //generate buildings
		for(int x = 0; x < gridLenth; ++x) {
			if (x != sidewalkBegin && x != (sidewalkBegin+1) && 
			    x != (sidewalkBegin+2) && x != (sidewalkBegin+3)) {
				//generate buildings on lanes except road and sidewalk
				for (int z = 0; z < gridWidth; ++z) {
                    //if (Random.Range(0, 2) > 0) {
                        // 50% chance to genarate building
                        int i = getObj(building, 0, building.Length, gridWidth - z);
                        if ((x == 0) || (x == gridLenth - 1)) {
                            //edge of the ground
                            i = getObj(building, 3, 5, 1); // only generate tall buildings
                        }
                        
                        items[x, z] = new GridItem(building[i], buildingData[i, 0], buildingData[i, 1]);
                        z += (buildingData[i, 1] - 1);
                        if ((buildingData[i, 0] > 1) && (x != sidewalkBegin - 1) && (x != gridLenth - 1)) {
                            //avoid overlap
                            x++;
                        }
                    //}
					
				}
			}
		}

        //piukup system
        bool onSidewalk = false;
        //50% chance to create a pickup item on each segment
        if (Random.Range(0, 2) == 0) {
            //75% chance to put the pickup item on the road
            if (Random.Range(0, 4) < 3) {
                //randomly choose a position on the road of the segment
                int x = Random.Range(sidewalkBegin, sidewalkBegin + 2);
                int z = Random.Range(0, gridWidth);
                PutItem(x, z);
            } else {
                //25% chance to put the pickup item on the sidewalk
                onSidewalk = true;
            }
        }

        //generate sidewalk
		for(int x = sidewalkBegin; x <= (sidewalkBegin + 3); ++x) {
			if (x == sidewalkBegin || x == (sidewalkBegin + 3)) {
				//generate items on sidewalk lanes
				for (int z = 0; z < gridWidth; ++z) {
                    //whether need to generate pickup item
                    if (onSidewalk) {
                        if ((x == (sidewalkBegin + 3)) && (z == gridWidth - 1)) {
                            PutItem(x, z);
                        } else {
                            int randomItem = Random.Range(0, 3);
                            if (randomItem > 0) {
                                // 2/3 chance to generate item, 1/3 chance to leave it empty
                                if (randomItem == 1) {
                                    // 1/2 chance to generate pickup item, 1/2 chance to generate static item
                                    PutItem(x, z);
                                    onSidewalk = false;
                                } else {
                                    //generate static item
                                    int i = getObj(sidewalk, 0, sidewalk.Length, gridWidth - z);
                                    items[x, z] = new GridItem(sidewalk[i], sidewalkData[i, 0], sidewalkData[i, 1]);
                                    z += (sidewalkData[i, 1] - 1);
                                }
                            }
                        }
                    } else {
                        //don't need to generate pickup item
                        if (Random.Range(0, 2) > 0) {
                            // 1/2 chance to generate item, 1/2 chance to leave it empty
                            int i = getObj(sidewalk, 0, sidewalk.Length, gridWidth - z);
                            items[x, z] = new GridItem(sidewalk[i], sidewalkData[i, 0], sidewalkData[i, 1]);
                            z += (sidewalkData[i, 1] - 1);
                        }
                    }
				}
			}
		}
	}
}

//items on the grid
public class GridItem {
    public GameObject obj;
    public int width; //width of the obj, normally == 1
	public int length; //ledth of the obj
    //public float xOffset;
    //public float zOffset;

	public GridItem(GameObject obj, int width, int length) {
		this.obj = obj;
//		this.obj.transform.localScale.x = length;
//		this.obj.transform.localScale.z = width;
        this.width = width;
        this.length = length;
	}
}