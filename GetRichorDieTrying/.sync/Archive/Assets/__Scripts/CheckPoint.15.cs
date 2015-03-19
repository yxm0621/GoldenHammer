using UnityEngine;
using System.Collections;

public class CheckPoint : MonoBehaviour {
	public int goal;
	public GameManager gameMain;
	
<<<<<<< HEAD
=======
	public GameObject 			barColor;		

>>>>>>> origin/master

	// Use this for initialization
	void Start () {
		gameMain = GameManager.manager;
<<<<<<< HEAD
=======

		GameObject barColor = GameObject.Find("CheckPointLine");
		Debug.Log ("Found CheckPointLine");

>>>>>>> origin/master
	}
	
	// Update is called once per frame
	void Update () {
		if (gameObject.transform.position.z < -2) {
			Destroy(this.gameObject);
		}
		if (goal > 0) {

			//Checks if goal has been met. If not Display Goal message
			if(gameMain.score > gameMain.levelGoal){
<<<<<<< HEAD
				gameObject.FindChild("check").GetComponent<TextMesh>().text = "Good Job!";
				GameObject barColor = gameObject.transform.FindChild ("CheckPointLine");//.GetComponent<Renderer>();

				//barColor.renderer.material.color = new Color();
=======
				gameObject.transform.FindChild("check").GetComponent<TextMesh>().text = "Good Job!";
				//.GetComponent<Renderer>();

				barColor.renderer.material.color = new Color(0,1,0);
>>>>>>> origin/master

			}else{
				gameObject.transform.FindChild("check").GetComponent<TextMesh>().text = goal.ToString("Goal $ " + "0");
			}
		}
	}

	void OnTriggerEnter(Collider other) {
		if(other.tag == "Character"){
			if(gameMain.score < goal){
				Debug.Log("Goals not met. Time Up");
				gameMain.GameOver ();
				Destroy(this.gameObject);
			}
			
			//Goal for the level has been met
			if (gameMain.score >= goal){
				Debug.Log ("Goals Met. Adding time. Raise goal.");
				//gameMain.levelTimer = 30; //Adds 30 seconds to the level timer
				gameMain.levelCount ++;
				//gameMain.levelGoal *= 3; //Increase goal x3

				if(gameMain.levelCount == 1){
					gameMain.levelTimer = 30;
					gameMain.levelGoal = 1000;
				}
				if(gameMain.levelCount == 2){
					gameMain.levelTimer = 30;
					gameMain.levelGoal = 2000;
				}
				if(gameMain.levelCount == 3){
					gameMain.levelTimer = 30;
					gameMain.levelGoal = 3500;
				}
				if(gameMain.levelCount == 4){
					gameMain.levelTimer = 30;
					gameMain.levelGoal = 5000;
				}
			}
		}
	}
	
}
