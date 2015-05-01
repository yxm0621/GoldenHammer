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

	//public GameObject			human;


	// Use this for initialization
	void Start () {
		thisObject = this.gameObject;

		iTween.Init (thisObject);

		gameMain = GameManager.manager; //Set Ref to GameManager Script

		if(thisObject.name.Contains("Cube")){
			value = 5;
			hitPoints = 4;
		}

		if(thisObject.name == "Cube_2"){
			value = 10;
			hitPoints = 8;
		}

		if((thisObject.name == "Tree") || (thisObject.name.Contains("tree")) || 
		   (thisObject.name.Contains("sign")) ||(thisObject.name == "Stars")){
			value = 5;
			hitPoints = 1;
		}

		if(thisObject.name.Contains("Cloud")){
			value = 100;
			hitPoints = 10;
		}
		if(thisObject.name.Contains("Planet")){
			value = 500;
			hitPoints = 5;
		}
		if(thisObject.name == "BlackHole"){
			value = 200;
			hitPoints = 5;
		}
		if((thisObject.name == "Sun") || (thisObject.name == "Moon")){
			value = 1000;
			hitPoints = 5;
		}

		if((thisObject.name == "Cat")||(thisObject.name == "Dog")||(thisObject.name == "Bird")){
			value = 2;
			hitPoints = 1;
		}
        if (thisObject.name == "Road")
        {
            value = 10;
            hitPoints = 5;
        }

		//For GameOver and game start
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

            //tell LevelSegments one space becomes empty so that player can move to that space
            //for (int i = 0; i < objLength ; i++) {
            //    for (int j = 0; j < objWidth ; j++) {
            //        thisObject.transform.parent.GetComponent<LevelSegments>().segEmpty[objPosX + i, objPosZ + j] = true;
            //    }
            //}

            if (!thisObject.name.Contains("Gray"))
            {

                //tell gamemain pos to Spawn coins
                gameMain.coinPos = thisObject.transform.position;

                //Spawn Coins Here
                gameMain.CashOut(value);

            }

            if (thisObject.name.Contains("Cat") ||
                thisObject.name.Contains("Dog"))
            {
                thisObject.GetComponent<AnimalBehavior>().AnimalDead();
                //gameMain.killAnimal = true;
            }

            if (thisObject.tag == "Building")
            {
                //Trigger Person falling out of Building
                gameMain.spawnHumanPos = thisObject.transform.position;
                gameMain.buildingDestroyed = true;
                gameMain.buildingType = thisObject.name;
            }

            //bomb explode effect
            if (thisObject.name == "Bomb")
            {
                Debug.Log("explode");
                GameObject explode = (GameObject)Instantiate(gameMain.explodeEffect, thisObject.transform.position, Quaternion.identity);
                explode.name = gameMain.explodeEffect.name;
                gameMain.audioSource.PlayOneShot(gameMain.explodeSFX);
            }

            //special effect to smash the whole line
            if (thisObject.name == "Special")
            {
                Debug.Log("special");
                GameObject line = (GameObject)Instantiate(gameMain.lineEffect, thisObject.transform.position, Quaternion.identity);
                line.name = gameMain.lineEffect.name;
                gameMain.audioSource.PlayOneShot(gameMain.lineSFX);
            }

            //kill people, polices come
            if (thisObject.name.Contains("Human"))
            {
                gameMain.killPeople++;
            }
            //kill police, tanks come
            if (thisObject.name.Contains("PoliceCar"))
            {
                gameMain.killPolice++;
            }
            //kill tank, helicopters come
            if (thisObject.name.Contains("Tank"))
            {
                gameMain.killTank++;
            }

            //smash gray cloud, it'll rain
            if (thisObject.name.Contains("Gray")) {
                GameObject rain = GameObject.Find("GlobalObjects").GetComponent<GlobalObjects>().rain;
                Instantiate(rain, thisObject.transform.position, Quaternion.identity);
            }

            if ((thisObject.name != "BackToMenu")
                &&(thisObject.name != "Encyclopedia")
                && (thisObject.name != "Achievement")
                && (thisObject.name != "Inventory")
                && (thisObject.name != "Store")
                && (thisObject.name != "Setting")
                && (thisObject.name != "Credits")
                && (thisObject.name != "Start")) {
            //if (thisObject.name == "Road")
            //{
            //    if (GameObject.Find("Grid") != null)
            //    {
            //        GameObject holeObj = GameObject.Find("Grid").GetComponent<Grid>().holeObj;
            //        GameObject hole = (GameObject)Instantiate(holeObj, thisObject.transform.position, Quaternion.identity);
            //        hole.name = holeObj.name;
            //    }
            //    hitPoints = 1000;
            //} else {
                //Destroy
                Destroy(thisObject);
            //}
            }

			if ((thisObject.name == "GameOverStart")||(thisObject.name == "Back")){
                Application.LoadLevel(gameMain.mainLevel);
			}

            if ((thisObject.name == "Encyclopedia")
                ||(thisObject.name == "Achievement")
                ||(thisObject.name == "Inventory")
                ||(thisObject.name == "Store")
                ||(thisObject.name == "Setting")
                ||(thisObject.name == "Credits")){
                    //Camera.main.gameObject.transform.position += new Vector3(10.5f, 0f, 0f);
                    iTween.MoveTo(Camera.main.gameObject, new Vector3(10.5f, 1f, -10f), .5f);
                    hitPoints++;
			}

            if (thisObject.name == "BackToMenu"){
                iTween.MoveTo(Camera.main.gameObject, new Vector3(0f, 1f, -10f), .5f);
                hitPoints++;
                    //Camera.main.transform.position -= new Vector3(10.5f, 0f, 0f);
                //Application.LoadLevel("Menu");
			}

            if (thisObject.name == "Start"){
                hitPoints++;
			}

            if (thisObject.name == "TreasureBox") {
                Debug.Log("menu");
                Application.LoadLevel ("Menu");
            }
		}
	}

	void OnMouseDown(){
        //if (gameMain.gameState == GameManager.State.InGame
        //    || thisObject.name == "GameOverStart"
        //    || thisObject.name == "Start") {
        if (gameMain.gameState != GameManager.State.Over
            || thisObject.name == "GameOverStart") {
            GameObject.Find("Hammer").GetComponent<HammerBehavior>().hammerSmash(thisObject.transform.position);
            //Debug.Log(thisObject + " hit!!");
            gameMain.audioSource.PlayOneShot(hitAudio);
            //iTween.MoveFrom (cam, iTween.Hash("z", -0.001f, "time", 0.5f)); //Give the camera a little kick in Z
            
            Vector3 pos = thisObject.transform.localPosition;
            Debug.Log(currentPos + ", local" + pos);
            if (!thisObject.name.Contains("car")) {
                thisObject.transform.position = new Vector3(currentPos.x,
                                                            currentPos.y - 0.25f,
                                                            currentPos.z + 0.25f);
                iTween.MoveTo(thisObject, iTween.Hash("position", pos, "islocal", true, "time", .5f));
            } else {
                //thisObject.transform.position = new Vector3(currentPos.x,
                //                                            currentPos.y - 0.25f,
                //                                            currentPos.z);
            //    float posZ = pos.z;
            //    float carSpeed = gameObject.GetComponent<CarBehavior>().runSpeed;
            //    posZ -= Time.deltaTime * carSpeed;
            //    pos.z = posZ;
                //iTween.MoveTo(thisObject, iTween.Hash("y", pos.y, "islocal", true, "time", .5f));
            }
            //iTween.MoveFrom(thisObject, iTween.Hash("y", currentPos.y - 0.25f, "time", 0.5f));
            //iTween.MoveFrom(thisObject, iTween.Hash("z", currentPos.z + 0.25f, "y", currentPos.y - 0.25f, "time", 0.5f));
            hitPoints--;
		}
	}

	void OnCollisionEnter(Collision other){
		if(other.gameObject.name == "damage"){
			hitPoints--;
		}
		if(other.gameObject.name == "spinCutter"){
			hitPoints--;
		}
	}
}
