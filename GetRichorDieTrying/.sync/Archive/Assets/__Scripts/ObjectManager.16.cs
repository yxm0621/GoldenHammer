using UnityEngine;
using System.Collections;

public class ObjectManager : MonoBehaviour {

	public GameObject			thisObject;

	public GameManager			gameMain; //Singleton Ref to GameManager Script


	public Vector3				currentPos;
	public int					objPosX;
	public int					objPosZ;
	public int 					objLength;
	public int 					objWidth;

	public int					value; //Value of object
	public int					hitPoints; //Hits to break object

	public AudioClip			hitAudio;

	// Use this for initialization
	void Start () {
		thisObject = this.gameObject;

		iTween.Init (thisObject);

		gameMain = GameManager.manager; //Set Ref to GameManager Script

		if(thisObject.name == "Cube"){
			value = 5;
			hitPoints = 4;
		}

		if(thisObject.name == "Cube_2"){
			value = 10;
			hitPoints = 8;
		}

		if((thisObject.name == "Tree")|| (thisObject.name == "Stars")){
			value = 5;
			hitPoints = 1;
		}

		if(thisObject.name == "Cloud"){
			value = 20;
			hitPoints = 10;
		}
		if(thisObject.name.Contains("Planet")){
			value = 20;
			hitPoints = 5;
		}
		if(thisObject.name == "BlackHole"){
			value = 20;
			hitPoints = 5;
		}
		if((thisObject.name == "Sun") || (thisObject.name == "Moon")){
			value = 20;
			hitPoints = 5;
		}

		if((thisObject.name == "Cat")||(thisObject.name == "Dog")||(thisObject.name == "Bird")){
			value = 2;
			hitPoints = 1;
		}

		//For GameOver
		if(thisObject.name == "GameOverStart" || thisObject.name == "Start"){
			value = 0;
			hitPoints = 1;
		}

	}
	
	// Update is called once per frame
	void Update () {
		currentPos = thisObject.transform.position;

		if(hitPoints <= 0){

			gameMain.objectName = thisObject.name;

//			gameMain.score += value * gameMain.bonus; //Add value of item to score

			for (int i = 0; i < objLength ; i++) {
				for (int j = 0; j < objWidth ; j++) {
					thisObject.transform.parent.GetComponent<LevelSegments>().segEmpty[objPosX + i, objPosZ + j] = true;
				}
			}


			if (thisObject.name != "Cat") {
				//tell gamemain pos to Spawn coins
				gameMain.coinPos = thisObject.transform.position;

				//Spawn Coins Here
				gameMain.CashOut(value);

				//Destroy
				Destroy(thisObject);
			} else {
				thisObject.GetComponent<catsBehavior>().catAction = catsBehavior.CatState.Smash;
				gameMain.killAnimal = true;
				//tell gamemain pos to Spawn coins
				gameMain.coinPos = thisObject.transform.position;
				//Spawn Coins Here
				gameMain.CashOut(value);
			}

			if(thisObject.name == "GameOverStart"){
			Application.LoadLevel(gameMain.mainLevel);
			}
		}
	}

	void OnMouseDown(){
		if (GameManager.gameState == GameManager.State.InGame || thisObject.name == "GameOverStart"
		    || thisObject.name == "Start") {
			GameObject cam = Camera.main.gameObject;
			GameObject hammer = GameObject.Find ("Hammer");
			hammer.transform.position = thisObject.transform.position + new Vector3 (.5f,1,0);
			//		hammer.transform.localRotation = Quaternion.Euler(0,0,90);
			hammer.transform.localEulerAngles = new Vector3 (0, 0, 90);
			iTween.RotateTo(hammer, iTween.Hash ("z", 0, "time", 0.2f));
			//gameMain.CamKick ();

			Debug.Log(thisObject + " hit!!");
			gameMain.audioSource.PlayOneShot (hitAudio);
			//iTween.MoveFrom (cam, iTween.Hash("z", -0.001f, "time", 0.5f)); //Give the camera a little kick in Z
			iTween.MoveFrom(thisObject, iTween.Hash ("z", currentPos.z + 0.25f, "y", currentPos.y + -0.25f, "time", 0.5f));
			hitPoints--;
		}
	}
}
