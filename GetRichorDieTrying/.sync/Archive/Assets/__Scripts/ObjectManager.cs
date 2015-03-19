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
			value = 80;
			hitPoints = 4;
			//Add to HP based on Combos
			hitPoints += gameMain.hitPointAdd;
		}
		if(thisObject.name == "Cube_2"){
			value = 160;
			hitPoints = 8;
			//Add to HP based on Combos
			hitPoints += gameMain.hitPointAdd;
		}

		if(thisObject.name == "Tree"){
			value = 15;
			hitPoints = 1;
			//Add to HP based on Combos
			hitPoints += gameMain.hitPointAdd;
		}

		if(thisObject.name == "Cloud"){
			value = 200;
			hitPoints = 10;
			//Add to HP based on Combos
			hitPoints += gameMain.hitPointAdd;
		}
		if((thisObject.name == "Sun")||(thisObject.name == "Moon")){
			value = 200;
			hitPoints = 10;
			//Add to HP based on Combos
			hitPoints += gameMain.hitPointAdd;
		}
		if(thisObject.name == "Cat"){
			value = 5;
			hitPoints = 1;
			//Add to HP based on Combos
			hitPoints += gameMain.hitPointAdd;
		}
	}
	
	// Update is called once per frame
	void Update () {
		currentPos = thisObject.transform.position;

		if(hitPoints <= 0){

			gameMain.objectName = thisObject.name;

			gameMain.score += value * gameMain.bonus; //Add value of item to score

			for (int i = 0; i < objLength ; i++) {
				for (int j = 0; j < objWidth ; j++) {
					thisObject.transform.parent.GetComponent<LevelSegments>().segEmpty[objPosX + i, objPosZ + j] = true;
				}
			}


			if (thisObject.name != "Cat") {
				//tell gamemain pos to Spawn coins
				gameMain.coinPos = thisObject.transform.position;

				//Spawn Coins Here
				gameMain.CashOut();

				//Destroy
				Destroy(thisObject);
			} else {
				thisObject.GetComponent<catsBehavior>().catAction = catsBehavior.CatState.Smash;
				gameMain.killAnimal = true;
				//tell gamemain pos to Spawn coins
				gameMain.coinPos = thisObject.transform.position;
				//Spawn Coins Here
				gameMain.CashOut();
			}
		}
	}

	void OnMouseDown(){

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
