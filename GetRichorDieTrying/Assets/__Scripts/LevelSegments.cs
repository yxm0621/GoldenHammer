using UnityEngine;
using System.Collections;

public class LevelSegments : MonoBehaviour {

	public GameManager 				gameMain;

	public GameObject				thisSegment;

	//TODO create Array of potential objects to be input and their position to be created. Randomize their generation.

	public Vector3					currentPos;

//	private float					moveToPos = -10.0f;
	float					timeToPos;

	public bool[,]					segEmpty;

	float					roadInitialSpeed = 1f;
	float					sidewalkInitialSpeed = .5f;
	float					ComboSpeedBonus = 1f;

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
			ComboSpeedBonus = (gameMain.bonus - 1)/2f;
			currentPos = thisSegment.transform.position;

            //delete segment and generate new segment
            //if(thisSegment.transform.position.z <= -6f){
            //    //Debug.Log("Spawning new Segment & Deleting Old");
            //    Destroy (thisSegment);
            //    gameMain.NewSegment();
            //}

            //change the moving speed. Slow down when walking on sidewalk
			if (characterController.onSidewalk){
				timeToPos = sidewalkInitialSpeed + ComboSpeedBonus;
			} else {
				timeToPos = roadInitialSpeed + ComboSpeedBonus;
			}

            //move segment
            thisSegment.transform.Translate(new Vector3(0, 0, -1) * Time.deltaTime * timeToPos);
		}
		gameMain.moveSpeed = timeToPos;
	}

    //delete segment and generate new segment
    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "SegmentKill")
        {
            Debug.Log("Destroy Segment");
            Destroy(thisSegment);
            gameMain.NewSegment();
        }
    }

}
