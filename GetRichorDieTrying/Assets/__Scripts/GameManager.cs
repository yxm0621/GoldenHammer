using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour {
	public enum State {Menu, InGame, Pause, Over};

	public static GameManager			manager;
//	public Camera						mainCam;
//	public GameObject					camPath;

	public State	    				gameState;
	public string						currentLevel;
	public string						mainLevel;
	public bool							firstRun = true;

	public GameObject					cam;
	public Vector3						camStartPos;
    public GameObject                   characterObj;
    public GameObject                   startMenuScene;
    public GameObject                   startMenu;
    public GameObject                   underground;
    public GameObject                   obstacleGroup;
    public GameObject                   itweenPath;

    //characterController controls the main character
    public characterController          character;

    //TrafficController controls obstacles
    public TrafficController            traffic;

    //GlobalObjects controls the light and global objects 
    public GlobalObjects                globalObj;

    //scene manager controls the scene change and load new assets
    public SceneManager                 sceneManager;

    //2D UI for buttons and texts
    public GUISkin                      menuSkin;
    public bool                         isItemShow = false;
    public bool                         isButtonClick = false;

//	public GameObject					uiTextObj;
//	public GameObject					goalTextObj;

	public int							score = 0;
	public TextMesh						scoreText;
	public TextMesh						scoreRecap;
	public TextMesh						highScoreText;
	public int							highScore;

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

	//parameters to spawn segments
	public GameObject					spawnPoint;
	public Vector3						segmentSpawnPos;
	public int							segmentLength = 12; //grid length
	public int                          segmentOffset = 6; //grid width
	public int							initialSegNum = 5;
    public GameObject[]                 segments;
	public List<bool[,]>				characterMove = new List<bool[,]>();
    //GameObject                          spawnTrigger;
    public int                          segNum = 0;

	//coins
	public GameObject					cube_coin;
	public GameObject					cube2_coin;
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

	//audio sources
	public AudioSource					audioSource;
	public AudioClip					finalSmashAudio;
	public AudioClip					coinDropAudio;
	public AudioClip					woodSmash;
	public AudioClip					cloudSmash;
	public AudioClip					bump;
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

		cam = Camera.main.gameObject;
		iTween.Init (cam);
		camStartPos = cam.transform.position;

		audioSource = GameObject.Find ("Audio Source").GetComponent<AudioSource>();
//		bgmBeat = GameObject.Find ("GH-BGM-Beat").GetComponent<AudioSource>();
		bgmBeat = Camera.main.GetComponent<AudioSource>();
		bgmSmash = GameObject.Find ("GH-BGM-Smashing").GetComponent<AudioSource>();
		bgmCombo = GameObject.Find ("GH-BGM-Combo").GetComponent<AudioSource>();
//		Debug.Log (audioSource + " Found");

		if((currentLevel != "GameOver") && (currentLevel != "Menu")){
            startMenu = (GameObject)Instantiate(startMenuScene, startMenuScene.transform.position, startMenuScene.transform.rotation);

            gameState = State.Menu;
			score = 0;
			comboMax = 0;
			levelCount = 1;
			
			levelGoal = 500;
			
			levelTimer = 30;
		}

        if (currentLevel != "Menu") {
            scoreText = GameObject.Find("Score").GetComponent<TextMesh>();
            timerText = GameObject.Find("Timer").GetComponent<TextMesh>();
            comboText = GameObject.Find("Combo").GetComponent<TextMesh>();
            comboTimerText = GameObject.Find("ComboTimer").GetComponent<TextMesh>();
            goalText = GameObject.Find("Goal").GetComponent<TextMesh>();
            distanceText = GameObject.Find("Distance").GetComponent<TextMesh>();
        }
		//feedback for killing!
		killPeople = 0;
		killPolice = 0;
		killTank = 0;
		killHelicopter = 0;
		//feedbackText = GameObject.Find("Feedback").GetComponent<TextMesh>();

		//Add coin groups to list
		coinGroups.Add (cube_coin);
		coinGroups.Add (cube2_coin);
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
            characterObj = GameObject.Find("Player");
            character = characterObj.GetComponent<characterController>();
            characterObj.transform.position = new Vector3(1.2f, .5f, -5.5f);
            characterObj.transform.eulerAngles = new Vector3(0f, 180f, 0f);
            Debug.Log(characterObj.animation.clip.name);
            characterObj.animation.Play("idle");
        }

		//try space scene
        //		globalObj.sunDown();
        //		globalObj.moonDown();
        //spawnTrigger= GameObject.Find("SpawnTrigger");
        if (currentLevel != "Menu")
        {
            segmentsInitialize();
            underground = GameObject.Find("Underground");
            //segments = new GameObject[initialSegNum];
        }
	}

	// Update is called once per frame
	void Update () {
        //int i = Random.Range(0, 100);
        //Debug.Log(i);
//		if(currentLevel != "GameOver") {
		switch(gameState) {
		case State.Menu:
            if (characterObj != null) {
                character.canControl = false;
            }
			if(comboStarted) {
				ComboEnd ();
			}
			//temp trigger
            if (characterObj != null && characterObj.transform.position.z >= -1.4f) {
                gameState = State.InGame;
                Destroy(startMenu);
            }
			break;
		case State.InGame:
            SegMove();
			if (sceneManager.reloadScene) {
				loadScene();
				sceneManager.reloadScene = false;
			}
			if (bgmBeat != null && !bgmBeat.isPlaying && Application.loadedLevelName != "GameOver"
                && SceneManager.currentScene == SceneManager.scene.city)
            {
//				Debug.Log("cityBGM");
				bgmBeat.Play();
				bgmBeat.loop = true;
			}
            if (SceneManager.currentScene == SceneManager.scene.city && bgmBeat.clip != cityBGM)
            {
				bgmBeat.Stop();
				bgmBeat.clip = cityBGM;
				bgmBeat.Play();
				bgmBeat.loop = true;
				//bgmSmash.clip = citySmashBGM;
			}
            if (SceneManager.currentScene == SceneManager.scene.space && bgmBeat.clip != spaceBGM)
            {
//				Debug.Log("spaceBGM");
				bgmBeat.Stop();
				bgmBeat.clip = spaceBGM;
				bgmBeat.Play();
				bgmBeat.loop = true;
				//bgmSmash.Stop();
				//bgmSmash.clip = null;
			}
			if ((score > 1) && (bgmSmash!=null) && (!bgmSmash.isPlaying)
                && (SceneManager.currentScene == SceneManager.scene.city))
            {
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
			character.canControl = true;
//			uiTextObj.SetActive(true);

			levelTimer -= 1 * Time.deltaTime;
			comboTimer -= 1 * Time.deltaTime;
            if(comboTimer <= 0 && comboStarted) {
				ComboEnd ();
			}
            
			goalText.text = levelGoal.ToString("Goal " + "$0");
			//update distance text
			if(distance < 1000){
                distanceText.text = distance.ToString("0");
			} else{
                distanceText.text = distance.ToString("0,000");
			}

			//update score text
			if(score < 1000){
				scoreText.text = score.ToString ("$ " + "0");
			} else{
				scoreText.text = score.ToString ("$ " + "0,000");
			}

			
			//update level timer
//			timerText.text = levelTimer.ToString ("n2");
			timerText.text = "";

			//update combo timer
            if (comboCount > 0) {
                comboText.text = comboCount.ToString("0" + " Combo");
                comboTimerText.text = comboTimer.ToString("n2");
            } else {
                comboText.text = "";
                comboTimerText.text = "";
            }
			
			if(score > highScore){ //Set HighScore
				highScore = score;
			}

            //if(levelTimer <= 10){
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
			character.canControl = false;
			break;
		case State.Over:
            //cam = Camera.main.gameObject;
            //cam.transform.position = new Vector3(-1f, 1.5f, -7f);
            //cam.transform.localEulerAngles = new Vector3(0f, 90f, 0f);

            SegMove();
            if (GameObject.Find("Player") != null)
            {
                character.canControl = false;
            }
			if(comboStarted){
				ComboEnd ();
			}
			break;
		}

		if(Application.loadedLevelName == "GameOver"){
			ScoreScreen ();
		}


		if(cam == null){
			Start ();
		}
	}

    IEnumerator GameStart() {
        //cam.transform.position = new Vector3(-1f, 1.5f, -7f);
        //cam.transform.localEulerAngles = new Vector3(0f, 90f, 0f);
        //characterObj.transform.position -= new Vector3(.4f, 0f, 0f);
        //characterObj.rigidbody.AddForce(Vector3.up * 1000);
        iTween.MoveTo(characterObj, iTween.Hash("x", .8f, "y", 1.4f, "z", -6.3f, "time", .5f, "easetype", iTween.EaseType.easeInCubic));
        characterObj.animation.Play("run");

        yield return new WaitForSeconds(.5f);
        iTween.MoveTo(cam, iTween.Hash("x", .5f, "y", 1.5f, "z", -2f, "time", 2f, "easetype", iTween.EaseType.easeInCubic));
        iTween.RotateTo(cam, iTween.Hash("x", 20f, "y", 0f, "z", 0f, "time", 2f, "easetype", iTween.EaseType.easeInCubic));
        iTween.MoveTo(characterObj, iTween.Hash("x", .5f, "y", .5f, "z", -1.4f, "time", 2f, "easetype", iTween.EaseType.easeInQuad));
        iTween.RotateTo(characterObj, new Vector3(0f, 0f, 0f), 1f);
        characterObj.animation.Play("run");

        iTween.RotateTo(globalObj.lightController, iTween.Hash("x", 50f, "y", 330f, "z", 0f, "time", 2f, "easetype", iTween.EaseType.easeInCubic));
        //globalObj.lightController.transform.eulerAngles = new Vector3(50, 330, 0);

        camStartPos = new Vector3(.5f, 1.5f, -2f);
        //GameObject light = GameObject.Find("Light");
        //if (light != null) {
        //    light.transform.localEulerAngles = new Vector3(60f, 70f, 0f);
        //}
    }

    void segmentsInitialize()
    {
        segmentSpawnPos = new Vector3(0.0f, 0.0f, 0.0f);

        for (int i = 0; i < initialSegNum; ++i){
            segments[i] = GameObject.Find("Grid").GetComponent<Grid>().generateGrid(spawnPoint, segmentSpawnPos, segmentLength, segmentOffset);
            segNum++;
            segmentSpawnPos.z += segmentOffset;
        }
        segmentSpawnPos.z -= segmentOffset;
        //if (spawnTrigger != null)
        //{
        //    spawnTrigger.SetActive(true);
        //}
    }

    //Spawn segment
    public void NewSegment()
    {
        //		Debug.Log ("Spawning at " + segmentSpawnPos);
        GameObject newSeg = GameObject.Find("Grid").GetComponent<Grid>().generateGrid(spawnPoint, segmentSpawnPos, segmentLength, segmentOffset);
        segNum++;
        if ((segNum - 1) % 5 == 0) {
            checkPoint = (GameObject)Instantiate(checkPointObj, checkPointObj.transform.position, Quaternion.identity);
            //checkPoint.GetComponent<CheckPoint>().goal = levelGoal;
            checkPoint.transform.parent = newSeg.transform;
        }
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
                distance += 8;
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
            segments[0].transform.Translate(new Vector3(0, 0, -1) * Time.deltaTime * moveSpeed);
            
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
            obstacleGroup.transform.Translate(new Vector3(0, 0, -1) * Time.deltaTime * moveSpeed);
        }
    }
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

	//feesback based on player's behavior
    //IEnumerator displayFeedback() {
    //    feedbackText.text = "How dare you! You just killed a kitty!";
    //    yield return new WaitForSeconds (1.5f);
    //    feedbackText.text = "Now you lose 20 dollars!";
    //    yield return new WaitForSeconds(1.5f);
    //    feedbackText.text = "";
    //}

	//Spawn coins to the world
	public void CashOut(int value){
		addScore (value);
        //Debug.Log (objectName + " Monitized");

<<<<<<< HEAD
		//Check which item was destroyed - [TODO] Save value for amount of screenshake
		if((objectName.Contains("Cube")) && (objectName != "Cube_2")){
			audioSource.PlayOneShot (finalSmashAudio);
			listItem = 0;

			camShakePower = 0.3f;
=======
		// check which item is 

		//Check which item was destroyed - [TODO] Save value for amount of screenshake
		if(objectName == "Cube" || objectName == "Cube_1"){
			audioSource.PlayOneShot (finalSmashAudio);
			listItem = 0;

			camShakePower = 0.4f;
>>>>>>> cf122640eb188b48cba94deee09b4f3739b98a51

			//Spawning Humans from building - Data Checked in TrafficController
			buildingDestroyed = true;
			buildingType = objectName;
			spawnHumanPos = coinPos;
		}
		if(objectName == "Cube_2"){

			audioSource.PlayOneShot (finalSmashAudio);
			listItem = 1;

<<<<<<< HEAD
			camShakePower = 0.4f;
=======
			camShakePower = 0.5f;
>>>>>>> cf122640eb188b48cba94deee09b4f3739b98a51

			//Spawning Humans from building - Data Checked in TrafficController
			buildingDestroyed = true;
			buildingType = objectName;
			spawnHumanPos = coinPos;
		}
		if(objectName == "Cat" || objectName == "Dog" || objectName == "Bird"|| objectName == "Road"){
			//audioSource.PlayOneShot ();
			listItem = 2;

<<<<<<< HEAD
			camShakePower = 0.05f;
=======
			camShakePower = 0.1f;
>>>>>>> cf122640eb188b48cba94deee09b4f3739b98a51

		}
		if(objectName.Contains ("Tree") || objectName.Contains ("obj") || objectName.Contains ("env")){
			audioSource.PlayOneShot (woodSmash);
			listItem = 2;

<<<<<<< HEAD
			camShakePower = 0.05f;
=======
			camShakePower = 0.1f;
>>>>>>> cf122640eb188b48cba94deee09b4f3739b98a51

		}
		if(objectName == "Cloud" || objectName.Contains("Planet") || objectName == "Stars"){
			audioSource.PlayOneShot (cloudSmash);
			listItem = 3;

<<<<<<< HEAD
			camShakePower = 0.7f;
=======
			camShakePower = 0.75f;
>>>>>>> cf122640eb188b48cba94deee09b4f3739b98a51

		}
		if(objectName == "BlackHole"){
			audioSource.PlayOneShot (cloudSmash);
			listItem = 3;

			camShakePower = 0.75f;

<<<<<<< HEAD
            globalObj.sunUp();
=======
			GameObject.Find("GlobalObjects").GetComponent<GlobalObjects>().sunUp();
>>>>>>> cf122640eb188b48cba94deee09b4f3739b98a51
		}
		if(objectName == "Sun"){
			audioSource.PlayOneShot (cloudSmash);
			listItem = 3;

<<<<<<< HEAD
			camShakePower = 0.7f;

            globalObj.sunDown();
=======
			camShakePower = 0.75f;

            GameObject.Find("GlobalObjects").GetComponent<GlobalObjects>().sunDown();
>>>>>>> cf122640eb188b48cba94deee09b4f3739b98a51
		}
		if(objectName == "Moon"){
			//change scene to space
			audioSource.PlayOneShot (cloudSmash);
			listItem = 3;

<<<<<<< HEAD
			camShakePower = 0.7f;

            globalObj.moonDown();
=======
			camShakePower = 0.75f;

            GameObject.Find("GlobalObjects").GetComponent<GlobalObjects>().moonDown();
>>>>>>> cf122640eb188b48cba94deee09b4f3739b98a51
		}
		if(objectName.Contains("car") || objectName.Contains("Police") || objectName.Contains("Tank")){
			audioSource.PlayOneShot (finalSmashAudio);

			if(objectName.Contains("car") || objectName.Contains("Police")){
				camShakePower = 0.33f;
			}
			if(objectName.Contains("Tank")){
				camShakePower = 0.4f;
			}

			listItem = 4;
            if (objectName.Contains("car") || objectName.Contains("Police")) {
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
<<<<<<< HEAD
=======

>>>>>>> cf122640eb188b48cba94deee09b4f3739b98a51
		}
		if(objectName == "Start" || objectName == "Special" || objectName == "Bomb"){
			audioSource.PlayOneShot (cloudSmash);
			listItem = 2;
<<<<<<< HEAD
=======

			camShakePower = 0.0001f;

		}
>>>>>>> cf122640eb188b48cba94deee09b4f3739b98a51

			camShakePower = 0.05f;

		}
        
        //Camera Shake
        if(objectName == "Start"){
            StartCoroutine(GameStart());
		} else if ((objectName == "Encyclopedia")
                ||(objectName == "Achievement")
                ||(objectName == "Inventory")
                ||(objectName == "Store")
                ||(objectName == "Setting")
                ||(objectName == "Credits")
                ||(objectName == "BackToMenu")) {
            //menu items
        } else{
            CamShake ();
        }

		//Spawn Coins
		GameObject coinsGroup = (GameObject)Instantiate (coinGroups[listItem], coinPos, Quaternion.identity);
		coinsGroup.GetComponent<CoinsGroup> ().value = value;
		audioSource.PlayOneShot (coinDropAudio);

		//Combo Count
		Combo ();
	}

	public void addScore(int addValue) {
		score += addValue * bonus;
	}

	//[X] iTween's camera shake
	public void CamShake(){
		float xPos = Random.Range (-1 * camShakePower, camShakePower);

		iTween.ShakePosition(cam, iTween.Hash ("x", xPos, "y", camShakePower, "time", duration, "oncompletetarget", this.gameObject,"oncomplete", "CamFix")); //X & Y shake
		//iTween.ShakePosition(cam, iTween.Hash ("x", xPos, "time", duration)); //X shake only
	}

    //Make sure cam is in proper X & Y
	public void CamFix(){
        Debug.Log ("Moving Camera Back");
        iTween.MoveTo(cam, iTween.Hash("x", camStartPos.x, "y", camStartPos.y, "time", 0));
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
        //spawnTrigger.SetActive(false);
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

		characterMove.Clear ();

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

    public void OnGUI() {
        //GUI.skin.label.normal.background = Color.black;
		GUI.skin = menuSkin;
        //GUI.Label (new Rect(Screen.width - 110 , 10, 100, 30), "$"+score);
		if(gameState == State.InGame) {
            //GUI.Label (new Rect(Screen.width/4 , Screen.height/4, Screen.width/2, Screen.height/2), "");
            if (isItemShow) {
                GUI.Label(new Rect(Screen.width - 70, 0, 70, Screen.height), "items");
                if (GUI.Button(new Rect(Screen.width - 60, 100, 50, 50), "1")) {
                    GameObject line = (GameObject)Instantiate(lineEffect, characterObj.transform.position, Quaternion.identity);
                    line.transform.eulerAngles = new Vector3(0,1,0);
                    line.name = lineEffect.name;
                    audioSource.PlayOneShot(lineSFX);
                }
                if (GUI.Button(new Rect(Screen.width - 60, 170, 50, 50), "2")) {
                }
                if (GUI.Button(new Rect(Screen.width - 60, 240, 50, 50), "3")) {
                }
                if (GUI.Button(new Rect(Screen.width - 60, 310, 50, 50), "4")) {
                }
            }
			if(GUI.Button(new Rect(Screen.width-60, 10, 50, 50), "=")) {
                isItemShow = !isItemShow;
            }
		}
	}
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
