using UnityEngine;
using System.Collections;
using Google2u;

public class CheckPoint : MonoBehaviour {
	public int                  goal;
	public GameManager          gameMain;
    public GameObject           arm;
    public GameObject           checkBg;
    public GameObject           checkText;
    public bool                 isFlicker;
    public bool                 isCheck;

	// Use this for initialization
	void Start () {
		gameMain = GameManager.manager;
        checkBg = gameObject.transform.FindChild("CheckBg").gameObject;
        checkText = gameObject.transform.FindChild("CheckText").gameObject;
        arm = GameObject.Find("Arm");
        iTween.Init(arm);
        //Debug.Log ("Found CheckPointLine");
        isCheck = false;
        isFlicker = false;
        gameObject.GetComponent<ObjectManager>().value = goal/100;
	}
	
	// Update is called once per frame
	void Update () {
        //if (gameObject.transform.position.z < (gameMain.characterPos.z - 1)) {
        //    Destroy(this.gameObject);
        //}
		if (goal > 0) {
			//Checks if goal has been met. If not Display Goal message
			if(gameMain.score > gameMain.levelGoal) {
                checkText.GetComponent<TextMesh>().text = "Good Job!";
                checkBg.GetComponent<Renderer>().material.color = new Color(0, 1, 0);
			} else {
                checkText.GetComponent<TextMesh>().text = goal.ToString("Goal $ " + "0");
			}
		}
        //Warn player that the goal hasn't been met
        if ((gameObject.transform.position.z <= (gameMain.characterPos.z + 2.5f)) && (gameMain.score < goal)) {
            if (!isFlicker) {
                StartCoroutine(Flickering());
                isFlicker = true;
            }
        }
        //Turn Checkpoint into coins if player can meet the goal before reaches the checkpoint
        if ((gameObject.transform.position.z > (gameMain.characterPos.z + .5f))
            && (gameObject.transform.position.z <= (gameMain.characterPos.z + 2f))
            && (gameMain.score >= goal)) {
            CheckGoal();
        }
        //Player reaches checkpoint
        if (gameObject.transform.position.z <= (gameMain.characterPos.z + .5f)) {
            if (!isCheck) {
                CheckGoal();
                isCheck = true;
            }
        }
	}

    //flicker to warn player
    IEnumerator Flickering() {
        iTween.RotateTo(arm, new Vector3(0,0,0), .5f);
        checkText.SetActive(false);
        yield return new WaitForSeconds(.15f);
        checkText.SetActive(true);
        yield return new WaitForSeconds(.15f);
        checkText.SetActive(false);
        yield return new WaitForSeconds(.15f);
        checkText.SetActive(true);
    }

    //check whether the player meet the goal
    void CheckGoal() {
        //Goal for the level hasn't been met
        if (gameMain.score < goal) {
            Debug.Log("Goals not met. Time Up");
            gameMain.GameOver();
            Destroy(this.gameObject);
        }

        //Goal for the level has been met
        if (gameMain.score >= goal) {
            Debug.Log("Goals Met. Adding time. Raise goal.");
            gameMain.levelCount++;

            //Move the arm up
            if (isFlicker) {
                iTween.RotateTo(arm, new Vector3(0, 0, -90), .2f);
            }

            //Load level goal from database
            if (gameMain.traffic.trafficDatabase == null) {
                gameMain.traffic.trafficDatabase = Google2u.TrafficDesign.Instance;
            }
            string levelID = "Level_" + gameMain.levelCount;
            gameMain.levelGoal = gameMain.traffic.trafficDatabase.GetRow(levelID)._Gold;

            gameObject.GetComponent<ObjectManager>().hitPoints = 0;
        }
    }
}
