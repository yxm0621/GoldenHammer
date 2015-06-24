using UnityEngine;
using System.Collections;

public class GlobalObjects : MonoBehaviour {
	//sky
	public GameObject					lightController;
    public Vector3                      lightPos = new Vector3(0, 5, 0);
    public Vector3                      lightAngle = new Vector3(50f, 120f, 0f);
	public Vector3                      sunPos;
	public GameObject					sun;
	public GameObject					moon;
    public GameObject[]                 cloud;

    public GameObject                   rainCloudObj;
    public GameObject                   rainCloud;
    public Vector3                      rainCloudAppearPos = new Vector3(20f, 7f, 15f);
    public Vector3                      rainCloudPos = new Vector3(11f, 5f, 17f);
    public float                        rainCloudMinX = 5f;
    public float                        rainCloudMaxX = 11f;

    public GameObject[]                 rainCloudsObj;
    public GameObject                   rainObj;
    public Vector3                      rainPos = new Vector3(0f, 2.2f, 0f);
    public float                        rainMinX = -1f;
    public float                        rainMaxX = 2f;
    //Color                               bgColor;

    public Color                        dayLightColor = Color.white;
    public Color                        nightLightColor = new Color(.85f, .96f, 1, 1);

    public Color                        dayBgColor;
    public Color                        nightBgColor = new Color(.19f, .3f, .47f, .02f);

	public SceneManager					sceneMain;
    public bool                         isRaining;
    public GameManager                  gameMain;

    //Global Bosses
    public GameObject                   raijinObj;
    public GameObject                   lightningObj;
    public Vector3[]                    lightningPos;

	// Use this for initialization
	void Start () {
        gameMain = GameManager.manager;

		lightController = new GameObject("Light");
		lightController.AddComponent<Light>();

        iTween.Init(lightController);

		lightController.transform.position = lightPos;
        lightController.transform.eulerAngles = lightAngle;
		
		lightController.GetComponent<Light>().type = LightType.Directional;
        lightController.GetComponent<Light>().color = dayLightColor;
		lightController.GetComponent<Light>().intensity = 0.1f;

        dayBgColor = Camera.main.backgroundColor;
		sunPos = new Vector3 (8.78f, 3.33f, 10.72f);

        AddToParent(sun, sunPos, Quaternion.identity, sun.name, gameObject);

        sceneMain = gameMain.sceneManager;
	}
	
	// Update is called once per frame
	void Update () {
        rainCloud = GameObject.Find("RainClouds");
        if (rainCloud == null) {
            //rainCloud = (GameObject)Instantiate(rainCloudObj, rainCloudAppearPos, rainCloudObj.transform.rotation);
            //rainCloud.name = "RainClouds";
            //rainCloud.transform.parent = gameObject.transform;

            rainCloudPos.x = Random.Range(rainCloudMinX, rainCloudMaxX);
            rainCloud = AddToParentAndReturn(rainCloudObj, rainCloudPos, rainCloudObj.transform.rotation,
                        "RainClouds", gameObject);
            iTween.Init(rainCloud);
            //iTween.MoveTo(rainCloud, rainCloudPos, 2f);
            //iTween.MoveTo(rainCloud, iTween.Hash("position", rainCloudAppearPos, "islocal", true, "time", 2f));
            //iTween.MoveTo(rainCloud, iTween.Hash("position", rainCloudPos, "islocal", true, "time", 2f));
        }
	}

    //Fixed timestep == 0.02
    public void FixedUpdate() {
        //transform.Translate(Vector3.back * Time.deltaTime * .1f);
        if (isRaining) {
            int i = Random.Range(0, 100);
            if (i < 10) {
                Raining();
            }
            if ((i == 10)||(i == 20)) {
                Lightning();
            }
        }
    }

	public void sunDown(){
		lightController.GetComponent<Light>().intensity = 0.3f;
        lightController.GetComponent<Light>().color = nightLightColor;
        AddToParent(moon, sunPos, Quaternion.identity, moon.name, gameObject);
        Camera.main.backgroundColor = nightBgColor;
		Camera.main.GetComponent<Skybox> ().material = sceneMain.nightSky;
	}

	public void moonDown(){
		lightController.GetComponent<Light>().intensity = 0.5f;
        lightController.GetComponent<Light>().color = dayLightColor;
        //sceneMain.changeScene (SceneManager.scene.space);
        sunUp();
	}

	public void sunUp(){
        AddToParent(sun, sunPos, Quaternion.identity, sun.name, gameObject);
        Camera.main.backgroundColor = dayBgColor;
        //sceneMain.changeScene (SceneManager.scene.city);
	}

    public void Rain(Vector3 pos) {
        isRaining = true;
        //Create more rainClouds
        for (int i = 0; i < rainCloudsObj.Length; i++) {
            //GameObject rainClouds = (GameObject)Instantiate(rainCloudsObj[i], pos, rainCloudsObj[i].transform.rotation);
            //rainClouds.name = "RainClouds";
            //rainClouds.transform.parent = gameObject.transform;
            GameObject rainClouds = AddToParentAndReturn(rainCloudsObj[i], pos, rainCloudsObj[i].transform.rotation,
                                    "RainClouds", gameObject);

            iTween.Init(rainClouds);
            //iTween.MoveTo(rainClouds, rainCloudsObj[i].transform.position, Random.Range(0f, 1f));
            iTween.MoveTo(rainClouds, iTween.Hash("position", rainCloudsObj[i].transform.position,
                                                  "islocal", true, 
                                                  "time", Random.Range(0f, 1f)));
        }

        //Create Raijin
        AddToParent(raijinObj, raijinObj.transform.position, raijinObj.transform.rotation, "Raijin", gameObject);

        //Change light color
        lightController.GetComponent<Light>().color = nightLightColor;
    }

    public void Raining() {
        //Create rain
        rainPos.x = Random.Range(rainMinX, rainMaxX);
        AddToParent(rainObj, rainPos, Quaternion.identity,
                    rainObj.name, gameMain.obstacleGroup);
    }

    public void Lightning() {
        //Create lightning
        AddToParent(lightningObj, lightningPos[Random.Range(0, lightningPos.Length)], Quaternion.identity,
                    lightningObj.name, gameMain.obstacleGroup);
    }

    //Rain stops
    public void Sunny() {
        isRaining = false;
        //Change light color
        lightController.GetComponent<Light>().color = dayLightColor;
    }

    //Create GameObject and add to a parent object
    public void AddToParent(GameObject obj, Vector3 pos, Quaternion rot, string name, GameObject parent) {
        GameObject newObj = (GameObject)Instantiate(obj, pos, rot);
        newObj.name = name;
        newObj.transform.parent = parent.transform;
        newObj.transform.position = newObj.transform.position + new Vector3(0f, 0f, gameObject.transform.position.z);
    }

    //Create GameObject and add to a parent object
    public GameObject AddToParentAndReturn(GameObject obj, Vector3 pos, Quaternion rot, string name, GameObject parent) {
        GameObject newObj = (GameObject)Instantiate(obj, pos, rot);
        newObj.name = name;
        newObj.transform.parent = parent.transform;
        newObj.transform.position = newObj.transform.position + new Vector3(0f, 0f, gameObject.transform.position.z);
        return newObj;
    }
}
