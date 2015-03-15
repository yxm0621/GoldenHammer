using UnityEngine;
using System.Collections;

public class SceneManager : MonoBehaviour {
	public enum scene{city, countryside, wilderness, space, ocean, battlefield};

	public scene currentScene;
	public float timePassed;

	public GameObject	mainCamera; //This script is attached to the main camera

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

	// Use this for initialization
	void Start () {
		changeScene (scene.city);
	}
	
	// Update is called once per frame
	void Update () {
		timePassed = Time.timeSinceLevelLoad;
		if (timePassed <= 30f) {
			currentScene = scene.city;
		}
	}

	public void changeScene(scene newScene) {
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
		Camera.main.GetComponent<Grid> ().setGrid (building, sidewalk, road, buildingData, sidewalkData);
	}

	void loadCity(){
		building = cityBuilding;
		sidewalk = citySidewalk;
		road = cityRoad;
		buildingData = new int[4, 4] {{1,1,0,0},{1,1,0,0},{1,1,0,0},{1,1,0,0}};
		sidewalkData = new int[6, 4] {{1,1,0,0},{1,1,0,0},{1,1,0,0},{1,1,0,0},{1,1,0,0},{1,1,0,0}};
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
		building = spaceBuilding;
		sidewalk = spaceSidewalk;
		road = spaceRoad;
		
	}
	void loadOcean(){
		building = oceanBuilding;
		sidewalk = oceanSidewalk;
		road = oceanRoad;
		
	}
	void loadBattlefield(){
		building = battlefieldBuilding;
		sidewalk = battlefieldSidewalk;
		road = battlefieldRoad;
		
	}
}
