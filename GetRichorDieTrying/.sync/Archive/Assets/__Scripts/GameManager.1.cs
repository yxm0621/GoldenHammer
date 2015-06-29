using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class GameManager : MonoBehaviour {
	public enum State {Menu, InGame, Pause, Over};
    public enum MenuPage {Main, Store, Inventory, Encyclopedia, Achievement, Setting, Credits};

	public static GameManager			manager;
//	public Camera						mainCam;
//	public GameObject					camPath;

	public State	    				gameState;
	public string						currentLevel;
	public string						mainLevel;
	public bool							firstRun = true;

	public GameObject					cam;
	public Vector3						camStartPos;
    public Vector3						camStartRot;
    public Vector3						camCurrentPos;
    public Vector3                      camCurrentRot;
    //public GameObject                   startMenuScene;
    //public GameObject                   startMenu;
    public GameObject                   obstacleGroup;
    public GameObject                   itweenPath;

    //Daikokuten data
    //public GameObject                   daikokutenObj;
    //public GameObject                   daikokuten;
    public int                          daikokutenState;

    //Character and its data
    public GameObject                   character;
    public Vector3                      characterPos;
    //characterController controls the main character
    public characterController          characterCon;

    //TrafficController controls obstacles
    public TrafficController            traffic;

    //GlobalObjects controls the light and global objects 
    public GlobalObjects                globalObj;

    //scene manager controls the scene change and load new assets
    public SceneManager                 sceneManager;

    //Hammer
    public GameObject                   hammerObj;
    public GameObject                   hammer;
    public float                        smashAngle = 75;
    public Vector3                      hammerPos = new Vector3(0f, 4f, -2f);

    //2D UI for buttons and texts
    public GUISkin                      menuSkin;
    public bool                         isItemShow = false;
    public bool                         isButtonClick = false;

//	public GameObject					uiTextObj;
//	public GameObject					goalTextObj;

	public int							score = 0;
	public TextMesh						scoreText;
    public TextMesh						valueText;
	public TextMesh						scoreRecap;
	public TextMesh						highScoreText;
	public int							highScore;
    public float                        valueShowTimer = .5f;

	public int							levelGoal = 0;
	public TextMesh						goalText;
	public int							levelCount = 0; //This is changed in CheckPoint
	public float						moveSpeed;
    public float                        initialSpeed = 1;
    public int							distance = 0;
	public TextMesh						distanceText;

	public int							bonus = 1;

	public float						duration = 0.25f;
	public float						magnitude = 1.0f;

	public float						levelTimer = 30.0f;
	public TextMesh						timerText;
	public GameObject					checkPoint; //Check Point in the scene
    public GameObject                   checkPointObj; //Check Point prefab
    public Vector3                      checkPointPos;
	public float						timerAdd = 1.0f;
	//public TextMesh						levelCountText;


	public int							comboCount = 0;
	public float						comboTimer = 0.0f;
	public TextMesh						comboText;
	public TextMesh						comboTimerText;
	public bool							comboStarted = false;
	private int 						comboAddToTimerCount = 0;
	public TextMesh						comboMaxText;
	public int							comboMax;

	public int							hitPointAdd = 0;

    public int                          raijinHP = -1;

	//parameters to spawn segments
	public GameObject					spawnPoint;
	public Vector3						segmentSpawnPos;
	public int							segmentLength = 12; //grid length
	public int                          segmentOffset = 6; //grid width
	public int							initialSegNum = 5;
    public GameObject[]                 segments;
    //public List<bool[,]>				characterMove = new List<bool[,]>(); //the grid info for which space character can go
    public int                          segNum = 0;

	//coins
	public GameObject					building_coin;
	public GameObject					building2_coin;
	public GameObject					tree_coin;
	public GameObject					cloud_coin;
	public GameObject					car_coin;
	public GameObject					human_coin;
	public List<GameObject>				coinGroups = new List<GameObject>();
	public int							listItem; //Number for game object in list
	public string						objectName;

	public Vector3						coinPos; //Position in the world for coins to instantiate

	private float						camShakePower; //Intensity of camera shake.

	//Spawn Human after building Destruction -- Set in CashOut() - Checked in Traffic Controller
	public bool							buildingDestroyed = false;
	public Vector3						spawnHumanPos;
	public string						buildingType;

    //feedback based on player's behavior
    public bool                         killAnimal = false;
    public int                          killPeople;
    public int                          killPolice;
    public int                          killTank;
    public int                          killHelicopter;
    //public TextMesh						feedbackText;

    //Menu data
    public MenuPage                     currentMenu = MenuPage.Main;
    public GameObject                   itemPage;
    public GameObject                   storeItemsObj;
    public GameObject                   storeItems;
    public GameObject                   itemDetailsObj;
    public GameObject                   itemDetails;

    //Items in game
    public int                          menuItem = 0;
    public bool                         isPowerhammer = false;
    public bool                         isShield = false;
    public bool                         isClock = false;
    public float                        itemTimer = 5f;
    public GameObject                   powerHammer;
    public GameObject                   shield;

	//audio sources
	public AudioSource					audioSource;
	public AudioClip					finalSmashAudio;
	public AudioClip[]					coinDropAudio;
	public AudioClip					woodSmash;
	public AudioClip					cloudSmash;
	public AudioClip					bump;
    public AudioClip[]					swipe;
	public AudioSource					bgmBeat;
	public AudioSource					bgmSmash;
	public AudioSource					bgmCombo;

	public AudioClip					cityBGM;
	//public AudioClip					citySmashBGM; //Sounds more annoying than helpful so turned off
	public AudioClip					spaceBGM;

	//special effect data
	public GameObject					explodeEffect;
	public GameObject					lineEffect;
	public AudioClip					explodeSFX;
	public AudioClip					lineSFX;

	void Awake(){
//		DontDestroyOnLoad(camPath);

		//Makes sure this is the only Game Manager makes it persist between scenes
		if(manager == null){
			DontDestroyOnLoad(this.gameObject);
			manager = this;
			Instantiate (itweenPath, new Vector3(0,0,0), Quaternion.identity);
            mainLevel = Application.loadedLevelName;
            LoadData();
		} else if(manager != this){ //If there is another Game Manager destroy's this one
			Destroy(gameObject);
		}

//		//Makes sure this is the only Main Camera makes it persist between scenes
//		if(mainCam == null){
//			DontDestroyOnLoad(Camera.main);
//			mainCam = Camera.main;
//		} else if(mainCam != Camera.main){ //If there is another Main Camera destroy's other one
//			Destroy(Camera.main);
//		}
	}

	// Use this for initialization
	public void Start () {
//		uiTextObj.SetActive (false);
		currentLevel = Application.loadedLevelName;
        Time.timeScale = 1.0F;

        cam = Camera.main.gameObject;
        camStartPos = cam.transform.position;
        camCurrentPos = camStartPos;
        camStartRot = cam.transform.eulerAngles;
        camCurrentRot = camStartRot;
		iTween.Init (cam);

		audioSource = GameObject.Find ("Audio Source").GetComponent<AudioSource>();
//		bgmBeat = GameObject.Find ("GH-BGM-Beat").GetComponent<AudioSource>();
		bgmBeat = Camera.main.GetComponent<AudioSource>();
		bgmSmash = GameObject.Find ("GH-BGM-Smashing").GetComponent<AudioSource>();
		bgmCombo = GameObject.Find ("GH-BGM-Combo").GetComponent<AudioSource>();
//		Debug.Log (audioSource + " Found");

        if (currentLevel != "Menu") {
            scoreText = GameObject.Find("Score").GetComponent<TextMesh>();
            valueText = GameObject.Find("Value").GetComponent<TextMesh>();
            timerText = GameObject.Find("Timer").GetComponent<TextMesh>();
            comboText = GameObject.Find("Combo").GetComponent<TextMesh>();
            comboTimerText = GameObject.Find("ComboTimer").GetComponent<TextMesh>();
            goalText = GameObject.Find("Goal").GetComponent<TextMesh>();
            distanceText = GameObject.Find("Distance").GetComponent<TextMesh>();

            if (currentLevel != "GameOver") {
                //startMenu = (GameObject)Instantiate(startMenuScene, startMenuScene.transform.position, startMenuScene.transform.rotation);

                gameState = State.Menu;
                score = 0;
                comboMax = 0;
                levelCount = 1;
                levelGoal = 500;

                levelTimer = 30;

                scoreText.text = "";
                valueText.text = "";
                timerText.text = "";
                comboText.text = "";
                comboTimerText.text = "";
                goalText.text = "";
                distanceText.text = "";
            }
        }

		//feedback for killing!
		killPeople = 0;
		killPolice = 0;
		killTank = 0;
		killHelicopter = 0;
		//feedbackText = GameObject.Find("Feedback").GetComponent<TextMesh>();

		//Add coin groups to list
		coinGroups.Add (building_coin);
		coinGroups.Add (building2_coin);
		coinGroups.Add (tree_coin);
		coinGroups.Add (cloud_coin);
		coinGroups.Add (car_coin);
		coinGroups.Add (human_coin);

        if (GameObject.Find("SceneManager") != null) {
            sceneManager = GameObject.Find("SceneManager").GetComponent<SceneManager>();
            sceneManager.changeScene(SceneManager.scene.city);
        }
		if (GameObject.Find("Traffic") != null) {
			traffic = GameObject.Find("Traffic").GetComponent<TrafficController>();
		}
        if (GameObject.Find("GlobalObjects") != null) {
            globalObj = GameObject.Find("GlobalObjects").GetComponent<GlobalObjects>();
        }
        if (GameObject.Find("Player") != null) {
            character = GameObject.Find("Player");
            characterCon = character.GetComponent<characterController>();
            //characterPos = character.transform.position;
            characterPos = new Vector3(.5f, 0f, 0f);
            character.GetComponent<Animation>().Play("idle");
            iTween.Init(character);

            shield = character.transform.FindChild("Shield").gameObject;
            shield.SetActive(false);
        }

		//try space scene
        //		globalObj.sunDown();
        //		globalObj.moonDown();

        if (currentLevel != "Menu") {
            segmentsInitialize();
            //segments = new GameObject[initialSegNum];
        }
	}

	// Update is called once per frame
	void Update () {
        //If hammer doesn't exist, create a hammer
        if (hammer == null) {
            HammerInit();
        }

		switch(gameState) {
		case State.Menu:
            if (currentMenu != MenuPage.Main) {
                if (gameObject.GetComponent<SwipeCheck>() == null)
                {
                    gameObject.AddComponent<SwipeCheck>();
                }
            } else {
                Destroy(gameObject.GetComponent<SwipeCheck>());
            }

            //Reset data
			if(comboStarted) {
				ComboEnd ();
			}
            //set player data
            if (character != null) {
                characterCon.canControl = false;
            }
			//temp trigger
            if (character != null && character.transform.position.z >= 0f) {
                gameState = State.InGame;
                //Destroy(startMenu);
            }
			break;
		case State.InGame:
			if (sceneManager.reloadScene) {
				loadScene();
				sceneManager.reloadScene = false;
			}
			if (bgmBeat != null && !bgmBeat.isPlaying && Application.loadedLevelName != "GameOver"
                && SceneManager.currentScene == SceneManager.scene.city) {
//				Debug.Log("cityBGM");
				bgmBeat.Play();
				bgmBeat.loop = true;
			}
            if (SceneManager.currentScene == SceneManager.scene.city && bgmBeat.clip != cityBGM) {
				bgmBeat.Stop();
				bgmBeat.clip = cityBGM;
				bgmBeat.Play();
				bgmBeat.loop = true;
				//bgmSmash.clip = citySmashBGM;
			}
            if (SceneManager.currentScene == SceneManager.scene.space && bgmBeat.clip != spaceBGM) {
//				Debug.Log("spaceBGM");
				bgmBeat.Stop();
				bgmBeat.clip = spaceBGM;
				bgmBeat.Play();
				bgmBeat.loop = true;
				//bgmSmash.Stop();
				//bgmSmash.clip = null;
			}
			if ((score > 1) && (bgmSmash!=null) && (!bgmSmash.isPlaying)
                && (SceneManager.currentScene == SceneManager.scene.city)) {
				//bgmSmash.Play();
				//bgmSmash.loop = true;
			}
			if((bonus>=5) && (bgmCombo!=null) && (!bgmCombo.isPlaying)) {
//				Debug.Log("combo");
				bgmCombo.Play();
				bgmCombo.loop = true;
			}
			if ((bonus<5)&& (bgmCombo!=null)&& (bgmCombo.isPlaying)) {
//				Debug.Log("comboStop");
				bgmCombo.Stop();
			}
			characterCon.canControl = true;
//			uiTextObj.SetActive(true);

            levelTimer -= 1 / (Mathf.Pow(2, daikokutenState)) * Time.deltaTime;
            comboTimer -= 1 / (Mathf.Pow(2, daikokutenState)) * Time.deltaTime;
            
            if (isPowerhammer || isShield || isClock) {
                itemTimer -= 1 / (Mathf.Pow(2, daikokutenState)) * Time.deltaTime;
                timerText.text = itemTimer.ToString("n2");
            } else {
                itemTimer = 5f;
                timerText.text = "";
            }
            if (itemTimer <= 0) {
                isPowerhammer = false;
                isShield = false;
                isClock = false;
                itemTimer = 5f;
                shield.SetActive(false);
                traffic.trafficStop = false;
            }
            
            if(comboTimer <= 0 && comboStarted) {
				ComboEnd ();
			}
            
            if (valueShowTimer > 0) {
                valueShowTimer -= 1 / (Mathf.Pow(2, daikokutenState)) * Time.deltaTime;
            } else {
                valueText.text = "";
            }

			goalText.text = levelGoal.ToString("Goal " + "$0");
			//update distance text
			if(distance < 1000) {
                distanceText.text = distance.ToString("0");
			} else{
                distanceText.text = distance.ToString("0,000");
			}

			//update score text
			if(score < 1000) {
				scoreText.text = score.ToString ("$ " + "0");
			} else{
				scoreText.text = score.ToString ("$ " + "0,000");
			}

			//update level timer
//			timerText.text = levelTimer.ToString ("n2");
            //timerText.text = "";

			//update combo timer
            if (comboCount > 0) {
                comboText.text = comboCount.ToString("0" + " Combo");
                comboTimerText.text = comboTimer.ToString("n2");
            } else {
                comboText.text = "";
                comboTimerText.text = "";
            }
			
			if(score > highScore) { //Set HighScore
				highScore = score;
			}

            //if(levelTimer <= 10) {
            //    checkPoint = (GameObject)Instantiate(checkPointObj, checkPointObj.transform.position, Quaternion.identity);
            //    checkPoint.GetComponent<CheckPoint>().goal = levelGoal;
            //    levelTimer  = 300; //Ensures that more checkpoints are not put into the ground until actually ready
            //}
			
			//kill Animal
			if (killAnimal) {
                //StartCoroutine(displayFeedback());
				killAnimal = false;
			}

			if (killPeople >= 3) {
				traffic.spawnPolice = true;
				killPeople = 0;
			}
			if (killPolice >= 1) {
				traffic.spawnTank = true;
				killPolice = 0;
			}
			if (killTank >= 1) {
				traffic.spawnHelicopter = true;
				killTank = 0;
			}
			break;
		case State.Pause:
			characterCon.canControl = false;
			break;
		case State.Over:
            //cam = Camera.main.gameObject;
            //cam.transform.position = new Vector3(-1f, 1.5f, -7f);
            //cam.transform.localEulerAngles = new Vector3(0f, 90f, 0f);

            if (GameObject.Find("Player") != null) {
                characterCon.canControl = false;
            }
			if(comboStarted) {
				ComboEnd ();
			}
			break;
		}

		if(Application.loadedLevelName == "GameOver") {
			ScoreScreen ();
		}

		if(cam == null) {
			Start ();
		}
	}

    //Fixed timestep == 0.02
    public void FixedUpdate() {
        //Incase gap appears between 2 segments
        if (gameState == State.InGame) {
            PlayerMove();
        }
        if (gameState == State.Over) {
            //SegMove();
        }
    }

    IEnumerator GameStart() {
        //Change camera data
        camStartPos = new Vector3(.5f, 1.5f, -.7f);
        camCurrentPos = camStartPos;
        camStartRot = new Vector3(30, 0, 0);
        camCurrentRot = camStartRot;

        //Steal hammer
        iTween.MoveTo(character, iTween.Hash("position", new Vector3(.8f, .5f, -6.5f), "time", .3f, "easetype", iTween.EaseType.easeInCubic));
        character.GetComponent<Animation>().Play("run");
        yield return new WaitForSeconds(.3f);

        //Camera's movement when character escapes
        iTween.MoveTo(cam, iTween.Hash("position", camStartPos, "time", 2f, "easetype", iTween.EaseType.easeInCubic));
        iTween.RotateTo(cam, iTween.Hash("x", camStartRot.x, "y", camStartRot.y, "z", camStartRot.z, "time", 2f, "easetype", iTween.EaseType.easeInCubic));

        //Character's movement when character escapes
        characterPos = new Vector3(.5f, 0f, 0f);
        iTween.MoveTo(character, iTween.Hash("position", characterPos, "time", 2f, "easetype", iTween.EaseType.easeInQuad));
        iTween.RotateTo(character, new Vector3(0f, 0f, 0f), 1f);

        //Rotate light
        iTween.RotateTo(globalObj.lightController, iTween.Hash("x", 25f, "y", 180f, "z", 180f, "time", 2f, "easetype", iTween.EaseType.easeInCubic));
        //globalObj.lightController.transform.eulerAngles = new Vector3(50, 330, 0);
        globalObj.lightController.GetComponent<Light>().intensity = 0.4f;
    }

    void segmentsInitialize() {
        segmentSpawnPos = new Vector3(0.0f, 0.0f, 0.0f);

        for (int i = 0; i < initialSegNum; ++i) {
            segments[i] = GameObject.Find("Grid").GetComponent<Grid>().generateGrid(spawnPoint, segmentSpawnPos, segmentLength, segmentOffset);
            segNum++;
            segmentSpawnPos.z += segmentOffset;
        }
    }

    //Spawn segment
    public void NewSegment() {
        //		Debug.Log ("Spawning at " + segmentSpawnPos);
        GameObject newSeg = GameObject.Find("Grid").GetComponent<Grid>().generateGrid(spawnPoint, segmentSpawnPos, segmentLength, segmentOffset);
        segNum++;
        if ((segNum - 1) % 5 == 0) {
            checkPointPos = new Vector3(checkPointObj.transform.position.x, 
                                        checkPointObj.transform.position.y,
                                        segmentSpawnPos.z + 1);
            checkPoint = (GameObject)Instantiate(checkPointObj, checkPointPos, Quaternion.identity);
            checkPoint.name = checkPointObj.name;
            checkPoint.GetComponent<CheckPoint>().goal = levelGoal;
            checkPoint.transform.parent = newSeg.transform;
        }
        segmentSpawnPos.z += segmentOffset;
        int i = 0;
        while(i != (initialSegNum-1)) {
            segments[i] = segments[++i];
        }
        segments[i] = newSeg;
    }

    //Segment movement
    public void SegMove() {
        //change the speed based on the combo
        if (bonus >= 10) {
            moveSpeed = initialSpeed * 4;
            if (gameState == State.InGame) {
                distance += 4;
            }
        } else if (bonus >= 5) {
            moveSpeed = initialSpeed * 2;
            if (gameState == State.InGame) {
                distance += 2;
            }
        } else {
            moveSpeed = initialSpeed;
            if (gameState == State.InGame) {
                distance ++;
            }
        }

        //move segments
        if (segments[0] != null) {
            segments[0].transform.position += new Vector3(0, 0, -1) * Time.deltaTime * moveSpeed;
            //segments[0].transform.Translate(new Vector3(0, 0, -1) * Time.deltaTime * moveSpeed);
            
            for (int i = 1; i < initialSegNum; i++) {
                segments[i].transform.position = new Vector3(segments[i].transform.position.x,
                                                             segments[i].transform.position.y,
                                                             segments[i - 1].transform.position.z + segmentOffset);
            }

            if (segments[0].transform.position.z <= -6f) {
                Destroy(segments[0]);
                NewSegment();
            }
        }

        //move obstacles
        if (obstacleGroup != null) {
            obstacleGroup.transform.position += new Vector3(0, 0, -1) * Time.deltaTime * moveSpeed;

            //obstacleGroup.transform.Translate(new Vector3(0, 0, -1) * Time.deltaTime * moveSpeed);
        }
    }

    //Player movement
    public void PlayerMove() {
        if (!isClock) {
            //change the speed based on the combo
            if (bonus >= 10) {
                //moveSpeed = initialSpeed * 4;
                daikokutenState = 2;
                //distance += 4;
                Time.timeScale = 4.0F;
            } else if (bonus >= 5) {
                //moveSpeed = initialSpeed * 2;
                daikokutenState = 1;
                //distance += 2;
                Time.timeScale = 2.0F;
            } else {
                //moveSpeed = initialSpeed;
                daikokutenState = 0;
                //distance ++;
                Time.timeScale = 1.0F;
            }
            moveSpeed = initialSpeed;
            distance++;
        } else {
            Time.timeScale = 1.0F;
            moveSpeed = 0f;
        }
        //Character position adjustment
        characterPos.x = character.transform.position.x;
        characterPos.y = character.transform.position.y;
        characterPos.z += (Time.deltaTime * moveSpeed);
        character.transform.position = characterPos;
        //character.transform.Translate(Vector3.forward * Time.deltaTime * moveSpeed);

        //float angle = Mathf.Tan(20*(3.14f/180));
        //cam.transform.Translate(new Vector3(0, angle, 1) * Time.deltaTime);

        //Camare positon adjustment
        camCurrentPos.x = cam.transform.position.x;
        camCurrentPos.y = cam.transform.position.y;
        camCurrentPos.z += (Time.deltaTime * moveSpeed);
        cam.transform.position = camCurrentPos;

        //GlobalObjects position adjustment
        GameObject.Find("GlobalObjects").transform.position += new Vector3(0, 0, Time.deltaTime * moveSpeed);

        //Daikoukuten's behavior
        /*
        if ((daikokutenState > 0) && (daikokuten == null)) {
            //Daikoukuten finds out the character
            daikokuten = Instantiate(daikokutenObj, new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
            daikokuten.transform.parent = character.transform;
            daikokuten.transform.localPosition = new Vector3(0, 0f, ((daikokutenState == 1) ? -.9f : -.5f));
            //daikokuten.transform.localScale = new Vector3(.2f, .2f, .2f);
        } else if ((daikokutenState > 0) && (daikokuten != null)){
            //When player has more combos, daikokuten will be closer to the character
            daikokuten.transform.localPosition = new Vector3(0, 0f, ((daikokutenState == 1) ? -.9f : -.5f));
        } else if ((daikokutenState == 0) && (daikokuten != null)){
            //When combo ends, daikokuten will leave the character
            Destroy(daikokuten);
        }
        */

        //Destroy segment and generate new segment
        if (segments[0] == null) {
            NewSegment();
        }
    }

    public void playerSwipe() {
        int i = UnityEngine.Random.Range(0, swipe.Length);
        audioSource.PlayOneShot(swipe[i]);
    }
    
    //Hammer initialization
    public void HammerInit() {
        hammer = (GameObject)Instantiate(hammerObj, hammerPos, Quaternion.identity);
        hammer.transform.localEulerAngles = new Vector3(0, smashAngle, 0);
        hammer.name = hammerObj.name;
        DontDestroyOnLoad(hammer);
    }

    //Hammer smash
    public void HammerSmash(Vector3 pos) {
        if (hammer == null) {
            HammerInit();
        }
        StartCoroutine(hammer.GetComponent<HammerBehavior>().Movement(pos));
    }

	//Spawn coins to the world
	public void CashOut(int value){
		addScore (value);
        //Debug.Log (objectName + " Monitized");

		//Check which item was destroyed - [TODO] Save value for amount of screenshake
        //Default Value
        listItem = 2;
        camShakePower = 0.05f;

        //Different Objects
		if((objectName.Contains("Building")) && (objectName != "Building_3")){
			audioSource.PlayOneShot (finalSmashAudio);
			listItem = 0;

			camShakePower = 0.3f;

			//Spawning Humans from building - Data Checked in TrafficController
			buildingDestroyed = true;
			buildingType = objectName;
			spawnHumanPos = coinPos;
		}
		if(objectName == "Building_3"){

			audioSource.PlayOneShot (finalSmashAudio);
			listItem = 1;

			camShakePower = 0.4f;

			//Spawning Humans from building - Data Checked in TrafficController
			buildingDestroyed = true;
			buildingType = objectName;
			spawnHumanPos = coinPos;
		}
		if(objectName.Contains ("Cat") || objectName.Contains ("Dog") || objectName.Contains ("Bird") || objectName.Contains ("City_Road")){
			//audioSource.PlayOneShot ();
			listItem = 2;

			camShakePower = 0.05f;

		}
		if(objectName.Contains ("Tree") || objectName.Contains ("obj") || objectName.Contains ("env")){
			audioSource.PlayOneShot (woodSmash);
			listItem = 2;

			camShakePower = 0.05f;

		}
		if(objectName.Contains ("Cloud") || objectName.Contains("Planet") || objectName == "Stars"){
			audioSource.PlayOneShot (cloudSmash);
			listItem = 3;

			camShakePower = 0.7f;

		}
		if(objectName == "BlackHole"){
			audioSource.PlayOneShot (cloudSmash);
			listItem = 3;

			camShakePower = 0.75f;

            globalObj.sunUp();
		}
		if(objectName == "Sun"){
			audioSource.PlayOneShot (cloudSmash);
			listItem = 3;

			camShakePower = 0.7f;

            globalObj.sunDown();
		}
		if(objectName == "Moon"){
			//change scene to space
			audioSource.PlayOneShot (cloudSmash);
			listItem = 3;

			camShakePower = 0.7f;

            globalObj.moonDown();
		}
		if(objectName.Contains("Car") || objectName.Contains("Tank")){
			audioSource.PlayOneShot (finalSmashAudio);
			listItem = 4;
            if (objectName.Contains("Car")) {
                camShakePower = 0.3f;
            }
            if (objectName.Contains("Tank")) {
                camShakePower = 0.4f;
            }
		}
		if(objectName.Contains("Human")){
			audioSource.PlayOneShot (cloudSmash);
			listItem = 5;

			camShakePower = 0.05f;
		}
		if(objectName == "Start" || objectName == "Special" || objectName == "Bomb"){
			audioSource.PlayOneShot (cloudSmash);
			listItem = 2;

			camShakePower = 0.05f;

		}
        
        //Camera Shake
        if(objectName == "Start"){
            StartCoroutine(GameStart());
		} else if (gameState == State.InGame) {
            CamShake ();
        }

		//Spawn Coins
        //GameObject coinsGroup = (GameObject)Instantiate (coinGroups[listItem], coinPos, Quaternion.identity);
        //coinsGroup.GetComponent<CoinsGroup> ().value = value;
        Instantiate(coinGroups[listItem], coinPos, Quaternion.identity);
		audioSource.PlayOneShot (coinDropAudio[(listItem <= 1) ? 0 : 1]);

		//Combo Count
		Combo ();
	}

	public void addScore(int addValue) {
        int collectCoin = addValue * bonus;
        score += collectCoin;
        if (gameState == State.InGame) {
            valueText.text = collectCoin.ToString("+$" + "0");
            valueShowTimer = .5f;
        } else if (valueText != null){
            valueText.text = "";
        }
	}

	//[X] iTween's camera shake
	public void CamShake(){
        camShakePower = camShakePower * 20;
		float xPos = UnityEngine.Random.Range (-1 * camShakePower, camShakePower);
        camCurrentRot = cam.transform.eulerAngles;
        iTween.ShakeRotation(cam, iTween.Hash("x", xPos, "y", camShakePower, "time", duration, "oncompletetarget", this.gameObject, "oncomplete", "CamFix")); //X & Y shake
        //iTween.ShakePosition(cam, iTween.Hash ("x", xPos, "y", camShakePower, "time", duration, "oncompletetarget", this.gameObject,"oncomplete", "CamFix")); //X & Y shake
		//iTween.ShakePosition(cam, iTween.Hash ("x", xPos, "time", duration)); //X shake only
	}

    //Make sure cam is in proper X & Y
	public void CamFix(){
        //Debug.Log ("Moving Camera Back");
        //iTween.MoveTo(cam, iTween.Hash("x", camCurrentPos.x, "y", camCurrentPos.y, "time", 0));
        
        cam.transform.eulerAngles = camCurrentRot;
        if (gameState == State.InGame) {
            cam.transform.eulerAngles = new Vector3(20f, 0f, 0f);
        }
	}

	public void Combo(){
		comboStarted = true;
        //Debug.Log ("Combo Started");

		comboAddToTimerCount++;
        //Debug.Log (comboAddToTimerCount);

		comboCount++;
        //Debug.Log ("Combo" + comboCount);
		comboTimer = 1.5f; //Combo time is always this value

		if(comboCount > 10) {
			bonus = 10;
		} else {
			bonus = comboCount;
		}

		if(comboCount > comboMax){ //Set Combo Max tracks highest combos
			comboMax = comboCount;
		}

		//[TODO] Give players feedback that combo is going




		if(comboAddToTimerCount >= 5){
			//levelTimer += 5.0f;
            //Debug.Log ("5 Seconds Added to Level Timer");
			//[TODO] Give players feedback that they have more time


			//hitPointAdd += 1;
			comboAddToTimerCount = 0;
		}
	}

	public void ComboEnd(){
		comboStarted = false;
        //Debug.Log ("Combo Over");
		comboCount = 0;
		bonus = 1;
	} 

	public void loadScene() {
		GameObject[] segs = GameObject.FindGameObjectsWithTag("Segment");
		GameObject[] obss = GameObject.FindGameObjectsWithTag("Obstacle");
		foreach (GameObject seg in segs) {
			Destroy(seg);
		}
		foreach (GameObject obs in obss) {
			Destroy (obs);
		}
		segmentsInitialize ();
	}

	public void GameOver(){
		mainLevel = Application.loadedLevelName;
		audioSource.PlayOneShot (bump);
		gameState = State.Over;
		firstRun = false;
		hitPointAdd = 0;

        //characterMove.Clear ();

		if(comboStarted){
			ComboEnd ();
		}

		Debug.Log ("Restarting Level " + currentLevel);
		
		//iTween.MoveTo(cam, iTween.Hash ("x", 0.0f, "y", 1.0f, "z", -10.0f));
		
		//scoreRecap.text = score.ToString ("$ " + "0,000");
		//comboMaxText.text = comboMax.ToString ("Combo Max \t" + 0);
		//highScoreText.text = highScore.ToString ("High Score \t" + "$" + "0,000");
		//levelCountText.text = levelCount.ToString ("Goals met \t" + 0);
		//Debug.Log ("Setting Text");
	
		//iTween.RotateTo(cam, iTween.Hash ("x", -90.0f, "time", 1));
		//Debug.Log ("Rotating Camera");

        SaveData();
		//End level
		Application.LoadLevel ("GameOver");
	}

	public void ScoreScreen(){
		if(Application.loadedLevelName == "GameOver"){
			highScoreText = GameObject.Find("Best Score").GetComponent<TextMesh>();
			//	levelCountText = GameObject.Find("Time Added").GetComponent<TextMesh>();
			comboMaxText = GameObject.Find("ComboMax").GetComponent<TextMesh>();
			scoreRecap = GameObject.Find("ScoreRecap").GetComponent<TextMesh>();


			if(score < 1000){
				scoreRecap.text = score.ToString ("$ " + "0");
			} else{
				scoreRecap.text = score.ToString ("$ " + "0,000");
			}

			comboMaxText.text = comboMax.ToString ("0");

			if(highScore < 1000){
				highScoreText.text = highScore.ToString ("$ " + "0");
			} else{
				highScoreText.text = highScore.ToString ("$ " + "0,000");
			}

			//levelCountText.text = levelCount.ToString ("Goals met \t" + 0);
			//Debug.Log ("Setting Text");
		}
		else{
			ScoreScreen ();
		}

	}

    //Save persistent data
    public void SaveData() {
        //Create file, serialize data and save date as a binary format file
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/playerInfo.dat");
        
        PlayerData data = new PlayerData();
        data.raijinHP = raijinHP;
        data.highScore = highScore;

        bf.Serialize(file, data);
        file.Close();
        Debug.Log("Save data to: " + Application.persistentDataPath);
    }

    //Load persistent data
    public void LoadData() {
        //Check whether the file exists
        if (File.Exists(Application.persistentDataPath + "/playerInfo.dat")) {
            //Open file and deserialize binary format file
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/playerInfo.dat", FileMode.Open);
            
            PlayerData data = (PlayerData)bf.Deserialize(file);
            file.Close();
            Debug.Log("Load data from: " + Application.persistentDataPath);

            //Load data from file
            raijinHP = data.raijinHP;
            highScore = data.highScore;
        }
    }

    public void UseSaw() {
        GameObject line = (GameObject)Instantiate(lineEffect, character.transform.position, Quaternion.identity);
        line.transform.eulerAngles = new Vector3(0, 1, 0);
        line.name = lineEffect.name;
        audioSource.PlayOneShot(lineSFX);
    }

    public void UseBomb() {
        GameObject explode = (GameObject)Instantiate(explodeEffect, character.transform.position, Quaternion.identity);
        explode.name = explodeEffect.name;
        audioSource.PlayOneShot(explodeSFX);
    }

    //public void OnGUI() {
    //    //GUI.skin.label.normal.background = Color.black;
    //    GUI.skin = menuSkin;
    //    //GUI.Label (new Rect(Screen.width - 110 , 10, 100, 30), "$"+score);
    //    if(gameState == State.InGame) {
    //        //GUI.Label (new Rect(Screen.width/4 , Screen.height/4, Screen.width/2, Screen.height/2), "");
    //        if (isItemShow) {
    //            GUI.Label(new Rect(Screen.width - 70, 0, 70, Screen.height), "items");
    //            if (GUI.Button(new Rect(Screen.width - 60, 100, 50, 50), "1")) {
    //                UseSaw();
    //            }
    //            if (GUI.Button(new Rect(Screen.width - 60, 170, 50, 50), "2")) {
    //            }
    //            if (GUI.Button(new Rect(Screen.width - 60, 240, 50, 50), "3")) {
    //            }
    //            if (GUI.Button(new Rect(Screen.width - 60, 310, 50, 50), "4")) {
    //            }
    //        }
    //        if(GUI.Button(new Rect(Screen.width-60, 10, 50, 50), "=")) {
    //            isItemShow = !isItemShow;
    //        }
    //    }
    //}

    //Draw mouse position with gizmoz
    void OnDrawGizmos() {
        Vector3 p = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 1));
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(p, 0.05F);
	}

    /*
    //Add grid info
    public void addMovement(bool[,] boolGrid){
        characterMove.Add (boolGrid);
        Debug.Log ("add grid. Now: "+characterMove.Count);
    }
    //delete grid info
    public void removeMovement(){
        characterMove.RemoveAt (0);
        Debug.Log ("delete grid. Now: "+characterMove.Count);
    }

    //get grid info -- whether the character can move towards
    public bool getMovement(int posX, int posZ){
        if (gameState == State.Over)
            return false;
        if (posX < 0 || posX >= segmentLength || posZ < 0 || posZ > segmentOffset) {
            return false;
        } else if(posZ == segmentOffset){
            return characterMove[1][posX, 0];
        } else if(posZ < segmentOffset/2){
            return characterMove[1][posX, posZ];
        } else {
            return characterMove[0][posX, posZ];
        }
    }
    */
    //feesback based on player's behavior
    //IEnumerator displayFeedback() {
    //    feedbackText.text = "How dare you! You just killed a kitty!";
    //    yield return new WaitForSeconds (1.5f);
    //    feedbackText.text = "Now you lose 20 dollars!";
    //    yield return new WaitForSeconds(1.5f);
    //    feedbackText.text = "";
    //}

	/*         -------Bad Camera Shake-----
	IEnumerator Shake(){
		Debug.Log ("Begin Camera Shake");


		float elapsed = 0.0f;

		Vector3 startCamPos = Camera.main.transform.position;
		Debug.Log (startCamPos);

		while(elapsed < duration){

			elapsed += Time.deltaTime;

			float percentComplete = elapsed / duration;
			float damper = 1.0f - Mathf.Clamp (4.0f * percentComplete - 3.0f, 0.0f, 1.0f);

			//map value to (-1, 1)
			float x = Random.value * 2.0f - 1.0f;
			float y = Random.value * 2.0f - 1.0f;
			x *= magnitude * damper;
			y *= magnitude * damper;

			Camera.main.transform.position = new Vector3(x, y, startCamPos.z);

			yield return null;
		}

		Camera.main.transform.position = startCamPos;
	} */
}

[System.Serializable]
class PlayerData {
    public int raijinHP;
    public int highScore;
}