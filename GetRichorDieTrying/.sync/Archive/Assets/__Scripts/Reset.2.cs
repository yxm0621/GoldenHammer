using UnityEngine;
using System.Collections;

public class Reset : MonoBehaviour {

	public GameObject			button;

	public GameManager			gameMain;

	// Use this for initialization
	void Start () {
		button = this.gameObject;

		gameMain = GameManager.manager;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnMouseDown(){
		Debug.Log ("Restarting Level");
		Application.LoadLevel (gameMain.currentLevel);
	}
}
