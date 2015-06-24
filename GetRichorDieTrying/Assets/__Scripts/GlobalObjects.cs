using UnityEngine;
using System.Collections;

public class GlobalObjects : MonoBehaviour {
	//sky
	public GameObject					lightController;
	public Vector3                      sunPos;
	public GameObject					sun;
	public GameObject					moon;
    public GameObject[]                 cloud;
    public GameObject                   cloudGray;
    public GameObject                   rain;
	Color                               bgColor;
	public SceneManager					sceneMain;

	// Use this for initialization
	void Start () {
		lightController = new GameObject("Light");
		lightController.AddComponent<Light>();
		
		lightController.transform.position = new Vector3(0, 5, 0);
        lightController.transform.eulerAngles = new Vector3(60f, 70f, 0f);
		
		lightController.light.type = LightType.Directional;
		lightController.light.color = Color.white;
		lightController.light.intensity = 0.5f;
		
		bgColor = Camera.main.backgroundColor;
		sunPos = new Vector3 (8.78f, 3.33f, 10.72f);

		GameObject newSun = (GameObject) Instantiate (sun, sunPos, Quaternion.identity);
		newSun.name = sun.gameObject.name;

		sceneMain = GameObject.Find ("SceneManager").GetComponent<SceneManager> ();
	}
	
	// Update is called once per frame
	void Update () {
	}

	public void sunDown(){
		lightController.light.intensity = 0.3f;
		lightController.light.color =  new Color(.85f, .96f, 1, 1);
		GameObject newMoon = (GameObject) Instantiate (moon, sunPos, Quaternion.identity);
		newMoon.name = moon.gameObject.name;
		Camera.main.backgroundColor = new Color(.19f, .3f, .47f, .02f);
		Camera.main.GetComponent<Skybox> ().material = sceneMain.nightSky;
	}

	public void moonDown(){
		lightController.light.intensity = 0.5f;
		lightController.light.color =  Color.white;
		sceneMain.changeScene (SceneManager.scene.space);
	}

	public void sunUp(){
		GameObject newSun = (GameObject) Instantiate (sun, sunPos, Quaternion.identity);
		newSun.name = sun.gameObject.name;
		Camera.main.backgroundColor = bgColor;
		sceneMain.changeScene (SceneManager.scene.city);
	}
}
