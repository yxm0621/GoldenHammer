using UnityEngine;
using System.Collections;

public class LevelSegments : MonoBehaviour {

	public GameManager 				gameMain;

	public GameObject				thisSegment;

	//TODO create Array of potential objects to be input and their position to be created. Randomize their generation.

	public Vector3					currentPos;

//	private float					moveToPos = -10.0f;
	public float					timeToPos = .2f;

	public bool[,]					segEmpty;

	public float					roadInitialSpeed = .2f;
	public float					sidewalkInitialSpeed = .1f;
	public float					ComboSpeedBonus = 1f;

	// Use this for initialization
	void Start () {
	
		gameMain = GameManager.manager;

		thisSegment = this.gameObject;

		iTween.Init (thisSegment);

		/*
		if(thisSegment.name == "LevelSegments1"){
			timeToPos /= 5;
			Debug.Log (timeToPos + " Seconds to Position");
		}
		if(thisSegment.name == "LevelSegments2"){
			timeToPos /= 4;
			Debug.Log (timeToPos + " Seconds to Position");
		}
		if(thisSegment.name == "LevelSegments3"){
			timeToPos /= 3;
			Debug.Log (timeToPos + " Seconds to Position");
		}
		if(thisSegment.name == "LevelSegments4"){
			timeToPos /= 2;
			Debug.Log (timeToPos + " Seconds to Position");
		}
		if(thisSegment.name == "LevelSegments5"){
			timeToPos /= 1;
			Debug.Log (timeToPos + " Seconds to Position");
		}
		if(thisSegment.name == "LevelSegments(Clone)"){
			Debug.Log (timeToPos + " Seconds to Position");
		}



		iTween.MoveTo(thisSegment, iTween.Hash("z", moveToPos, "easetype", "linear","time", timeToPos, "oncomplete", "EndOfRoad"));

		*/
		//thisSegment.rigidbody.AddForce(Vector3.forward); //Give forward motion

	}
	
	// Update is called once per frame
	void Update () {
		if (GameManager.gameState == GameManager.State.InGame ||
		    GameManager.manager.currentLevel == "GameOver") {
			ComboSpeedBonus = (gameMain.bonus - 1)/8f;
			currentPos = thisSegment.transform.position;

			//		thisSegment.transform.Translate(new Vector3 (0,0, -1) * Time.deltaTime * timeToPos);

			if(thisSegment.transform.position.z <= -6f){
				//Debug.Log("Spawning new Segment & Deleting Old");
				Destroy (thisSegment);
				gameMain.NewSegment();
			}

			if (characterController.onSidewalk){
				timeToPos = sidewalkInitialSpeed + ComboSpeedBonus;
			} else {
				timeToPos = roadInitialSpeed + ComboSpeedBonus;
			}

//		if (characterController.forward){
			GameObject[] segs = GameObject.FindGameObjectsWithTag("Segment");
			GameObject[] obss = GameObject.FindGameObjectsWithTag("Obstacle");
			foreach (GameObject seg in segs) {
//				seg.transform.position -= new Vector3(0,0,3f);
				seg.transform.Translate(new Vector3 (0,0, -1) * Time.deltaTime * timeToPos);
			}
			foreach (GameObject obs in obss) {
//				obs.transform.position -= new Vector3(0,0,3f);
				if (obs.name.Contains("PoliceCarL") || obs.name.Contains("TankL") ||
				    obs.name.Contains("Human_")) {
					obs.transform.Translate(new Vector3 (0,0, 1) * Time.deltaTime * timeToPos);
				} else if (obs.name.Contains("PoliceCarR") || obs.name.Contains("TankR")) {
					obs.transform.Translate(new Vector3 (1,0,0) * Time.deltaTime * timeToPos);
				} else {
					obs.transform.Translate(new Vector3 (0,0, -1) * Time.deltaTime * timeToPos);
				}
				if(obs.transform.position.z <= -6f){
					Destroy (obs);
				}
			}
//				characterController.forward = false;
//		}
		}
		gameMain.moveSpeed = timeToPos;
	}

	//
//	void OnTriggerEnter(Collider other){
//		if(other.tag == "SegmentKill"){
//			Debug.Log ("Destroy Segment");
//			Destroy (thisSegment);
//			gameMain.NewSegment();
//		}
//	}

}
