using UnityEngine;
using System.Collections;

public class CheckPoint : MonoBehaviour {
	public int goal;
	public GameManager gameMain;

	// Use this for initialization
	void Start () {
		gameMain = GameManager.manager;
	}
	
	// Update is called once per frame
	void Update () {
		if (gameObject.transform.position.z < -2) {
			Destroy(this.gameObject);
		}
		if (goal > 0) {
			gameObject.transform.FindChild("check").GetComponent<TextMesh>().text = goal.ToString("$0");
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

				}
			}
		}
	}
	
}
