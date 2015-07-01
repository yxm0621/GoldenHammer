using UnityEngine;
using System.Collections;

public class SceneManager : MonoBehaviour {
	public enum scene{city, countryside, wilderness, space, ocean, battlefield};

	public scene previousScene;
	public static scene currentScene;
	public float timePassed;

	//public GameObject	mainCamera; //This script is attached to the main camera

	public GameObject[] building;
	public GameObject[] sidewalk;
	public GameObject[] road;

	public GameObject[] cityBuilding;
	public GameObject[] citySidewalk;
	public GameObject[] cityRoad;

	public GameObject[] countrysideBuilding;
	public GameObject[] countrysideSidewalk;
	public GameObject[] countrysideRoad;

	public GameObject[] wildernessBuilding;
	public GameObject[] wildernessSidewalk;
	public GameObject[] wildernessRoad;

	public GameObject[] spaceBuilding;
	public GameObject[] spaceSidewalk;
	public GameObject[] spaceRoad;

	public GameObject[] oceanBuilding;
	public GameObject[] oceanSidewalk;
	public GameObject[] oceanRoad;
	
	public GameObject[] battlefieldBuilding;
	public GameObject[] battlefieldSidewalk;
	public GameObject[] battlefieldRoad;

	public int[,] buildingData;
	public int[,] sidewalkData;
    public float[] buildingOccur;
    public float[] sidewalkOccur;

	public bool reloadScene = false;
	
	public Material						citySky;
	public Material						nightSky;
	public Material						spaceSky;

    public Google2u.ObjList_G2U         objData;

	// Use this for initialization
	void Start () {
        objData = GameManager.manager.objData;
		changeScene (scene.city);
	}
	
	// Update is called once per frame
	void Update () {
		timePassed = Time.timeSinceLevelLoad;
//		if (timePassed <= 30f) {
//			currentScene = scene.city;
//		}
	}

	public void changeScene(scene newScene) {
		previousScene = currentScene;
		currentScene = newScene;
		switch(currentScene) {
		case scene.city:
			loadCity();
			break;
		case scene.countryside:
			loadCountryside();
			break;
		case scene.wilderness:
			loadWilderness();
			break;
		case scene.space:
			loadSpace();
			break;
		case scene.ocean:loadOcean();
			break;
		case scene.battlefield:
			loadBattlefield();
			break;
		}
        if (GameObject.Find("Grid") != null) {
            GameObject.Find("Grid").GetComponent<Grid>().setGrid(building, sidewalk, road,
                                                                 buildingData, sidewalkData, buildingOccur, sidewalkOccur);
        }
        if (GameObject.Find("Traffic") != null) {
            GameObject.Find("Traffic").GetComponent<TrafficObjects>().changeTraffic();
        }

		if (previousScene == scene.space) {
			iTweenEvent.GetEvent(Camera.main.gameObject, "loadScene").Play();
			iTweenEvent.GetEvent(Camera.main.gameObject, "loadScene1").Play();
			iTweenEvent.GetEvent(Camera.main.gameObject, "loadScene2").Play();
			reloadScene = true;
            deleteObstacles();
            //GameManager.manager.underground.SetActive(true);
		}
	}

	void loadCity(){
		Camera.main.GetComponent<Skybox> ().material = citySky;
		building = cityBuilding;
		sidewalk = citySidewalk;
		road = cityRoad;
        getObjData();
	}
	void loadCountryside(){
		building = countrysideBuilding;
		sidewalk = countrysideSidewalk;
		road = countrysideRoad;
        getObjData();
	}
	void loadWilderness(){
		building = wildernessBuilding;
		sidewalk = wildernessSidewalk;
		road = wildernessRoad;
        getObjData();
	}
	void loadSpace(){
		iTweenEvent.GetEvent(Camera.main.gameObject, "loadScene").Play();
		iTweenEvent.GetEvent(Camera.main.gameObject, "loadScene1").Play();
		iTweenEvent.GetEvent(Camera.main.gameObject, "loadScene2").Play();

		building = spaceBuilding;
		sidewalk = spaceSidewalk;
		road = spaceRoad;
        getObjData();

		Camera.main.GetComponent<Skybox> ().material = spaceSky;
		reloadScene = true;
        deleteObstacles();
        //GameManager.manager.underground.SetActive(false);
	}
	void loadOcean(){
		building = oceanBuilding;
		sidewalk = oceanSidewalk;
		road = oceanRoad;
        getObjData();
        deleteObstacles();
	}
	void loadBattlefield(){
		building = battlefieldBuilding;
		sidewalk = battlefieldSidewalk;
		road = battlefieldRoad;
        getObjData();
        deleteObstacles();
	}

    void getObjData() {
        buildingData = new int[building.Length, 2];
        sidewalkData = new int[sidewalk.Length, 2];
        buildingOccur = new float[building.Length];
        sidewalkOccur = new float[sidewalk.Length];

        if (objData == null) {
            objData = Google2u.ObjList_G2U.Instance;
        }
        string objID = "Static_";
        for (int i = 0; i < building.Length; ++i) {
            objID = "Static_" + building[i].name;
            if ((objData.GetRow(objID) != null) &&
                (objData.GetRow(objID)._Location.Contains("City")) &&
                (objData.GetRow(objID)._Lane.Contains("Ground"))) {
                buildingData[i, 0] = 1;
                buildingData[i, 1] = objData.GetRow(objID)._Size > 0 ? objData.GetRow(objID)._Size : 1;
                buildingOccur[i] = objData.GetRow(objID)._Occurance;
            } else {
                Debug.Log("building data error.");
            }
        }
        for (int i = 0; i < sidewalk.Length; ++i) {
            objID = "Static_" + sidewalk[i].name;
            if ((objData.GetRow(objID) != null) && 
                (objData.GetRow(objID)._Location.Contains("City")) &&
                (objData.GetRow(objID)._Lane.Contains("Sidewalk"))) {
                sidewalkData[i, 0] = 1;
                sidewalkData[i, 1] = objData.GetRow(objID)._Size > 0 ? objData.GetRow(objID)._Size : 1;
                sidewalkOccur[i] = objData.GetRow(objID)._Occurance;
            } else {
                Debug.Log("sidewalk data error.");
            }
        }
    }

    void deleteObstacles() {
        GameObject[] obss = GameObject.FindGameObjectsWithTag("Obstacle");
        foreach (GameObject obs in obss) {
            Destroy(obs);
        }
    }
}
