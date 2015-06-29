using UnityEngine;
using System.Collections;
using Google2u;

public class ObjectManager : MonoBehaviour {
	public GameObject			thisObject;

	public GameManager			gameMain; //Singleton Ref to GameManager Script

	public Vector3				currentPos;
    public Vector3              localPos;

    public Google2u.ObjList_G2U objData;
	public int					value; //Value of object
	public int					hitPoints; //Hits to break object

	public AudioClip			hitAudio;

	//public GameObject			human;


	// Use this for initialization
	void Start () {
        gameMain = GameManager.manager; //Set Ref to GameManager Script

		thisObject = this.gameObject;
		iTween.Init (thisObject);

        localPos = thisObject.transform.localPosition;

        objData = Google2u.ObjList_G2U.Instance;
        if (objData != null) {
            for (int i = 0; i < objData.Rows.Count; i++) {
                if(thisObject.name == objData.Rows[i]._Name){
                    if (objData.Rows[i]._Name == "Raijin"){
                        if (gameMain.raijinHP == -1) {
                            hitPoints = objData.Rows[i]._HitPoints;
                            gameMain.raijinHP = hitPoints;
                        } else {
                            hitPoints = gameMain.raijinHP;
                        }
                    } else {
                        hitPoints = objData.Rows[i]._HitPoints;
                    }
                    value = objData.Rows[i]._Gold;
                }
            }
        }

		if(thisObject.name.Contains("Building")){
			value = 20;
			hitPoints = 2;
		}

		if(thisObject.name == "Building_3"){
			value = 25;
			hitPoints = 3;
		}

		if((thisObject.name == "Tree") || (thisObject.name.Contains("tree")) || 
		   (thisObject.name.Contains("sign")) ||(thisObject.name == "Stars")){
			value = 5;
			hitPoints = 1;
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
			value = 200;
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

            if (!thisObject.name.Contains("Gray")) {
                //tell gamemain pos to Spawn coins
                if (thisObject.name.Contains("checkPoint")) {
                    gameMain.coinPos = thisObject.transform.position + new Vector3(2f, .5f, 0f);
                } else {
                    gameMain.coinPos = thisObject.transform.position;
                }

                //Spawn Coins Here
                gameMain.CashOut(value);

            }

            if (thisObject.name.Contains("Cat") ||
                thisObject.name.Contains("Dog")) {
                thisObject.GetComponent<AnimalBehavior>().AnimalDead();
                //gameMain.killAnimal = true;
            }

            if (thisObject.tag == "Building") {
                //Trigger Person falling out of Building
                gameMain.spawnHumanPos = thisObject.transform.position;
                gameMain.buildingDestroyed = true;
                gameMain.buildingType = thisObject.name;
            }

            //kill people, polices come
            if (thisObject.name.Contains("Person")) {
                gameMain.killPeople++;
            }
            //kill police, tanks come
            if (thisObject.name.Contains("PoliceCar")) {
                gameMain.killPolice++;
            }
            //kill tank, helicopters come
            if (thisObject.name.Contains("Tank")) {
                gameMain.killTank++;
            }

            //smash gray cloud, it'll rain
            if (thisObject.name.Contains("RainClouds")) {
                if ((gameMain.globalObj != null)&&(gameMain.globalObj.isRaining == false)) {
                    gameMain.globalObj.Rain(thisObject.transform.position);
                }
            }
            
            //Raijin has dead, rain will stop
            if (thisObject.name.Contains("Raijin")) {
                gameMain.globalObj.isRaining = false;
            }

            if ((thisObject.name != "BackToMenu")
                &&(thisObject.name != "Encyclopedia")
                && (thisObject.name != "Achievement")
                && (thisObject.name != "Inventory")
                && (thisObject.name != "Store")
                && (thisObject.name != "Setting")
                && (thisObject.name != "Credits")
                //&& (thisObject.name != "Start")
                ) {
            //if (thisObject.name == "City_Road")
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

			if ((thisObject.name == "GameOverStart")||(thisObject.name == "Back")) {
                Application.LoadLevel(gameMain.mainLevel);
			}

            if ((thisObject.name == "Encyclopedia")
                ||(thisObject.name == "Achievement")
                ||(thisObject.name == "Inventory")
                ||(thisObject.name == "Store")
                ||(thisObject.name == "Setting")
                ||(thisObject.name == "Credits")){

                //Camera.main.gameObject.transform.position += new Vector3(10.5f, 0f, 0f);
                iTween.MoveTo(Camera.main.gameObject, new Vector3(6.5f, 1f, -10f), .5f);
                hitPoints++;
                if (gameMain.itemPage == null) {
                    gameMain.itemPage = GameObject.Find("Menu");
                }
                gameMain.itemPage.transform.FindChild("Title").GetComponent<TextMesh>().text = thisObject.name;
                switch (thisObject.name) {
                    case "Encyclopedia":
                        gameMain.currentMenu = GameManager.MenuPage.Encyclopedia;
                        
                        break;
                    case "Achievement":
                        gameMain.currentMenu = GameManager.MenuPage.Achievement;
                        break;
                    case "Inventory":
                        gameMain.currentMenu = GameManager.MenuPage.Inventory;
                        break;
                    case "Store":
                        gameMain.currentMenu = GameManager.MenuPage.Store;
                        gameMain.storeItems = Instantiate(gameMain.storeItemsObj, gameMain.storeItemsObj.transform.position, Quaternion.identity) as GameObject;
                        gameMain.storeItems.name = gameMain.storeItemsObj.name;
                        gameMain.storeItems.transform.parent = gameMain.itemPage.transform;
                        break;
                    case "Setting":
                        gameMain.currentMenu = GameManager.MenuPage.Setting;
                        break;
                    case "Credits":
                        gameMain.currentMenu = GameManager.MenuPage.Credits;
                        break;
                    default:
                        break;

                }
			}

            if (thisObject.name == "BackToMenu"){
                    iTween.MoveTo(Camera.main.gameObject, new Vector3(0f, 1f, -10f), .5f);
                    Destroy(gameMain.storeItems);
                    if (gameMain.itemDetails != null) {
                        Destroy(gameMain.itemDetails);
                    }
                    gameMain.currentMenu = GameManager.MenuPage.Main;
                hitPoints++;
                    //Camera.main.transform.position -= new Vector3(10.5f, 0f, 0f);
                //Application.LoadLevel("Menu");
			}

            if (thisObject.name == "Start"){
                hitPoints++;
			}

            if (thisObject.name == "TreasureBox") {
                Debug.Log("menu");
                if (GameObject.Find("Start") != null) {
                    Application.LoadLevel("Menu");
                    gameMain.currentMenu = GameManager.MenuPage.Main;
                }
            }
		}
	}

	void OnMouseDown(){
        //if (gameMain.gameState == GameManager.State.InGame
        //    || thisObject.name == "GameOverStart"
        //    || thisObject.name == "Start") {
        if (gameMain.gameState != GameManager.State.Over || thisObject.name == "GameOverStart") {
            if (thisObject.name.Contains("checkPoint")) {
                gameMain.HammerSmash(thisObject.transform.position + new Vector3(2f, .5f, 0f));
            } else {
                gameMain.HammerSmash(thisObject.transform.position);
            }
            
            //Debug.Log(thisObject + " hit!!");
            gameMain.audioSource.PlayOneShot(hitAudio);
            //iTween.MoveFrom (cam, iTween.Hash("z", -0.001f, "time", 0.5f)); //Give the camera a little kick in Z
            
            //Vector3 pos = thisObject.transform.localPosition;
            Debug.Log(currentPos + ", local" + localPos);
            if (!thisObject.name.Contains("Car")) {
                thisObject.transform.position = new Vector3(currentPos.x,
                                                            currentPos.y - 0.25f,
                                                            currentPos.z + 0.25f);
                iTween.MoveTo(thisObject, iTween.Hash("position", localPos, "islocal", true, "time", .5f));
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

            if ((thisObject.name == "TreasureBox") && (GameObject.Find("Start") == null)) {
                //Player is stealing the hammer
            } else {
                if (gameMain.isPowerhammer) {
                    hitPoints -= 5;
                } else {
                    hitPoints--;
                }
            }
            if (thisObject.name == "Raijin") {
                if (gameMain.isPowerhammer) {
                    gameMain.raijinHP -= 3;
                } else {
                    gameMain.raijinHP--;
                }
            }
		}
	}

	void OnCollisionEnter(Collision other){
		if((other.gameObject.name.Contains("damage")) || (other.gameObject.name.Contains("spinCutter"))) {
			hitPoints--;
		}
	}
    void OnTriggerEnter(Collider other){
		if(other.gameObject.name == "Lightning") {
			hitPoints--;
            Destroy(other.gameObject);
		}
	}
}
