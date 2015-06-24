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

	public bool reloadScene = false;
	
	public Material						citySky;
	public Material						nightSky;
	public Material						spaceSky;

	// Use this for initialization
	void Start () {
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
            GameObject.Find("Grid").GetComponent<Grid>().setGrid(building, sidewalk, road, buildingData, sidewalkData);
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
		buildingData = new int[15, 4] {{1,1,0,0},{1,1,0,0},{1,1,0,0},{1,1,0,0},{1,1,0,0},
			{1,1,0,0},{1,1,0,0},{1,1,0,0},{1,1,0,0},{1,1,0,0},
            {1,1,0,0},{1,1,0,0},{1,1,0,0},{1,1,0,0},{1,2,0,0}};
        sidewalkData = new int[15, 4] {{1,1,0,0},{1,1,0,0},{1,1,0,0},{1,1,0,0},{1,1,0,0},
			{1,1,0,0},{1,1,0,0},{1,1,0,0},{1,1,0,0},{1,1,0,0},
            {1,1,0,0},{1,1,0,0},{1,1,0,0},{1,1,0,0},{1,1,0,0}};
	}

	void loadCountryside(){
		building = countrysideBuilding;
		sidewalk = countrysideSidewalk;
		road = countrysideRoad;
	}
	void loadWilderness(){
		building = wildernessBuilding;
		sidewalk = wildernessSidewalk;
		road = wildernessRoad;
		
	}
	void loadSpace(){
		iTweenEvent.GetEvent(Camera.main.gameObject, "loadScene").Play();
		iTweenEvent.GetEvent(Camera.main.gameObject, "loadScene1").Play();
		iTweenEvent.GetEvent(Camera.main.gameObject, "loadScene2").Play();
		building = spaceBuilding;
		sidewalk = spaceSidewalk;
		road = spaceRoad;
		buildingData = new int[15, 4] {{1,1,0,0},{1,1,0,0},{1,1,0,0},{1,1,0,0},{1,1,0,0},
			{1,1,0,0},{1,1,0,0},{1,1,0,0},{1,1,0,0},{1,1,0,0},
            {1,1,0,0},{1,1,0,0},{1,1,0,0},{1,1,0,0},{1,1,0,0}};
		sidewalkData = new int[7, 4] {{1,1,0,0},{1,1,0,0},{1,1,0,0},{1,1,0,0},{1,1,0,0},
            {1,1,0,0},{1,1,0,0}};
		Camera.main.GetComponent<Skybox> ().material = spaceSky;
		reloadScene = true;
        deleteObstacles();
        //GameManager.manager.underground.SetActive(false);
	}
	void loadOcean(){
		building = oceanBuilding;
		sidewalk = oceanSidewalk;
		road = oceanRoad;
        deleteObstacles();
	}
	void loadBattlefield(){
		building = battlefieldBuilding;
		sidewalk = battlefieldSidewalk;
		road = battlefieldRoad;
        deleteObstacles();
	}

    void deleteObstacles() {
        GameObject[] obss = GameObject.FindGameObjectsWithTag("Obstacle");
        foreach (GameObject obs in obss)
        {
            Destroy(obs);
        }
    }
}
