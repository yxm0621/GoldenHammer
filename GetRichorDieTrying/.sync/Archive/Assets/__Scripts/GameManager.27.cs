using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour {
	public enum State {Menu, InGame, Pause, Over};

	public static GameManager			manager;

	public static State					gameState;
	public string						currentLevel;
	public bool							firstRun = true;

	public GameObject					cam;
	public Vector3						camStartPos;

//	public GameObject					uiTextObj;
//	public GameObject					goalTextObj;

	public int							score = 0;
	public TextMesh						scoreText;
	public TextMesh						scoreRecap;
	public TextMesh						highScoreText;
	public int							highScore;

	public int							levelGoal = 0;
	public TextMesh						goalText;
	public int							levelCount = 0;

	public int							bonus = 1;

	public float						duration = 0.25f;
	public float						magnitude = 1.0f;

	public float						levelTimer = 30.0f;
	public TextMesh						timerText;
	public GameObject					checkPoint;
	public float						timerAdd = 1.0f;
	//public TextMesh						levelCountText;


	public int							comboCount = 0;
	public float						comboTimer = 0.0f;
	public TextMesh						comboText;
	public TextMesh						comboTimerText;
	public bool							comboStarted = false;
	private int 						comboAddToTimerCount = 0;
	//public TextMesh						comboMaxText;
	public int							comboMax;

	public int							hitPointAdd = 0;

	//parameters to spawn segments
	public GameObject					spawnPoint;
	public Vector3						segmentSpawnPos;
	public int							segmentLength = 12; //grid length
	public int                          segmentOffset = 6; //grid width
	public int							initialSegNum = 5;
	public List<bool[,]>				characterMove = new List<bool[,]>();

	//coins
	public GameObject					cube_coin;
	public GameObject					cube2_coin;
	public GameObject					tree_coin;
	public GameObject					cloud_coin;
	public GameObject					car_coin;
	public GameObject					human_coin;
	public List<GameObject>				coinGroups = new List<GameObject>();
	public int							listItem;
	public string						objectName;

	public Vector3						coinPos; //Position in the world for coins to instantiate

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

	//feedback based on player's behavior
	public bool                         killAnimal = false;
	//public TextMesh						feedbackText;

	//scene manager to control the scene change and load new assets
	public SceneManager                 sceneManager;

	void Awake(){

		//Makes sure this is the only Game Manager makes it persist between scenes
		if(manager == null){
			DontDestroyOnLoad(this.gameObject);
			manager = this;
		} else if(manager != this){ //If there is another Game Manager destroy's this one
			Destroy(gameObject);
		}
	}

	// Use this for initialization
	public void Start () {
		gameState = State.Menu;
//		uiTextObj.SetActive (false);

		currentLevel = Application.loadedLevelName;

		cam = Camera.main.gameObject;
		iTween.Init (cam);

		camStartPos = cam.transform.position;

		audioSource = GameObject.Find ("Audio Source").GetComponent<AudioSource>();
		bgmBeat = GameObject.Find ("GH-BGM-Beat").GetComponent<AudioSource>();
		bgmSmash = GameObject.Find ("GH-BGM-Smashing").GetComponent<AudioSource>();
		bgmCombo = GameObject.Find ("GH-BGM-Combo").GetComponent<AudioSource>();
//		Debug.Log (audioSource + " Found");

		score = 0;
		comboMax = 0;
		levelCount = 0;

		levelGoal = 100;

		levelTimer = 10;

		scoreText = GameObject.Find("Score").GetComponent<TextMesh>();
		timerText = GameObject.Find("Timer").GetComponent<TextMesh>();
		comboText = GameObject.Find("Combo").GetComponent<TextMesh>();
		comboTimerText = GameObject.Find("ComboTimer").GetComponent<TextMesh>();
		goalText = GameObject.Find ("Goal").GetComponent<TextMesh>();

		//highScoreText = GameObject.Find("HighScore").GetComponent<TextMesh>();
	//	levelCountText = GameObject.Find("Time Added").GetComponent<TextMesh>();
		//comboMaxText = GameObject.Find("MaxCombo").GetComponent<TextMesh>();
		//scoreRecap = GameObject.Find("ScoreRecap").GetComponent<TextMesh>();

		//feedbackText = GameObject.Find("Feedback").GetComponent<TextMesh>();

		//Add coin groups to list
		coinGroups.Add (cube_coin);
		coinGroups.Add (cube2_coin);
		coinGroups.Add (tree_coin);
		coinGroups.Add (cloud_coin);
		coinGroups.Add (car_coin);
		coinGroups.Add (human_coin);

		sceneManager = cam.GetComponent<SceneManager>();
		sceneManager.changeScene (SceneManager.scene.city);
		
		segmentSpawnPos = new Vector3(0.0f,0.0f, 0.0f);

		for (int i = 0; i < initialSegNum; ++i) {
			NewSegment ();
			segmentSpawnPos.z += segmentOffset;
		}
		segmentSpawnPos.z -= segmentOffset;
	}
	
	// Update is called once per frame
	void Update () {
		switch(gameState) {
		case State.Menu:
			GameObject.Find("Player").GetComponent<characterController>().canControl = false;
			//temp trigger
			gameState = State.InGame;
			break;
		case State.InGame:
			if (bgmBeat != null && !bgmBeat.isPlaying) {
				bgmBeat.Play();
				bgmBeat.loop = true;
			}
			if ((score > 0) && (bgmSmash!=null) && (!bgmSmash.isPlaying)) {
				bgmSmash.Play();
				bgmBeat.loop = true;
			}
			if((bonus>=5) && (bgmCombo!=null) && (!bgmCombo.isPlaying)) {
//				Debug.Log("combo");
				bgmCombo.Play();
				bgmBeat.loop = true;
			}
			if ((bonus<5)&& (bgmCombo!=null)&& (bgmCombo.isPlaying)) {
//				Debug.Log("comboStop");
				bgmCombo.Stop();
			}
			GameObject.Find("Player").GetComponent<characterController>().canControl = true;
//			uiTextObj.SetActive(true);

			levelTimer -= 1 * Time.deltaTime;
			comboTimer -= 1 * Time.deltaTime;

			comboText.text = comboCount.ToString("Combo " + "0");
			goalText.text = levelGoal.ToString("Goal " + "$0");

			if(comboTimer <= 0 && comboStarted){
				ComboEnd ();
			}
			
			//update score text
			scoreText.text = score.ToString ("$ " + "0,000");
			
			//update level timer
//			timerText.text = levelTimer.ToString ("n2");
			timerText.text = "";

			//update combo timer
			if (comboTimer > 0) {
				comboTimerText.text = comboTimer.ToString("n2");
			} else {
				comboTimerText.text = "";
			}
			
			if(score > highScore){ //Set HighScore
				highScore = score;
			}

			if(levelTimer <= 10){
				GameObject checkGoal = (GameObject) Instantiate(checkPoint, checkPoint.transform.position, Quaternion.identity);
				checkGoal.GetComponent<CheckPoint>().goal = levelGoal;
				levelTimer  = 300; //Ensures that more checkpoints are not put into the ground until actually ready
			}
			
			//kill Animal
			if (killAnimal) {
				StartCoroutine(displayFeedback());
				killAnimal = false;
			}

			break;
		case State.Pause:
			GameObject.Find("Player").GetComponent<characterController>().canControl = false;
			break;
		case State.Over:
			GameObject.Find("Player").GetComponent<characterController>().canControl = false;
			break;
		}




		if(cam == null){
			Start ();
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
	IEnumerator displayFeedback() {
		//feedbackText.text = "How dare you! You just killed a kitty!";
		yield return new WaitForSeconds (1.5f);
//		feedbackText.text = "Now you lose 20 dollars!";
//		yield return new WaitForSeconds (1.5f);
		//feedbackText.text = "";
	}

	//Spawn coins to the world
	public void CashOut(int value){
		addScore (value);
		Debug.Log (objectName + " Monitized");

		//Check which item was destroyed
		if(objectName == "Cube" || objectName == "Cube_1"){
			audioSource.PlayOneShot (finalSmashAudio);
			listItem = 0;
		}
		if(objectName == "Cube_2"){
			audioSource.PlayOneShot (finalSmashAudio);
			listItem = 1;
		}
		if(objectName == "Tree" || objectName == "Tree_1" || objectName == "Cat" ||
		   objectName == "Dog" || objectName == "Bird" ||
		   objectName == "Mshr1Prefab" || objectName == "Mshr2Prefab" ||
		   objectName == "Mshr3Prefab" || objectName == "Mshr4Prefab" ||
		   objectName == "Mshr5Prefab"){
			audioSource.PlayOneShot (woodSmash);
			listItem = 2;
		}
		if(objectName == "Cloud"){
			audioSource.PlayOneShot (cloudSmash);
			listItem = 3;
		}
		if(objectName == "Sun"){
			audioSource.PlayOneShot (cloudSmash);
			listItem = 3;
			GameObject.Find("globalObjects").GetComponent<globalObjects>().sunDown();
		}
		if(objectName == "Moon"){
			//change scene to space
			audioSource.PlayOneShot (cloudSmash);
			listItem = 3;
			GameObject.Find("globalObjects").GetComponent<globalObjects>().moonDown();
		}
		if(objectName == "car" || objectName == "car(Clone)"){
			audioSource.PlayOneShot (finalSmashAudio);
			listItem = 4;
		}
		if(objectName == "Human" || objectName == "Human(Clone)"){
			audioSource.PlayOneShot (cloudSmash);
			listItem = 5;
		}

		//StartCoroutine (Shake ()); //Camera Shake
		CamShake ();

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

	//Spawn segment
	public void NewSegment(){
//		Debug.Log ("Spawning at " + segmentSpawnPos);
		GameObject.Find("Grid").GetComponent<Grid> ().generateGrid(spawnPoint, segmentSpawnPos, segmentLength, segmentOffset);
	}

	//[X] iTween's camera shake
	public void CamShake(){
		float xPos = Random.Range (-1, 1);

		iTween.ShakePosition(cam, iTween.Hash ("x", xPos, "y", 0.5f, "time", duration, "oncompletetarget", this.gameObject,"oncomplete", "CamFix")); //X & Y shake
		//iTween.ShakePosition(cam, iTween.Hash ("x", xPos, "time", duration)); //X shake only
	}

	//- No longer called as causes camera issues if a new move is started before cam is back to home pos
	public void CamKick(){ //Kicks cam back in Z 

		iTween.ShakePosition(cam, iTween.Hash("z", -0.25f, "time", duration - 0.2f));
		CamFix ();
	} 

	public void CamFix(){ //Make sure cam is in proper X & Y 
		if(cam.transform.position.y < 1.5 || cam.transform.position.y > 1.5){
			Debug.Log ("Moving Camera Back");
			iTween.MoveTo(cam, iTween.Hash("x", camStartPos.x, "y", camStartPos.y, "time", 0));
		}

	}

	public void Combo(){

		comboStarted = true;
		Debug.Log ("Combo Started");


		comboAddToTimerCount++;
		Debug.Log (comboAddToTimerCount);

		comboCount++;
		Debug.Log ("Combo" + comboCount);
		comboTimer = 1.5f; //Combo time is always this value

		bonus = comboCount;

		if(comboCount > comboMax){ //Set Combo Max tracks highest combos
			comboMax = comboCount;
		}

		//[TODO] Give players feedback that combo is going




		if(comboAddToTimerCount >= 5){
			//levelTimer += 5.0f;
			Debug.Log ("5 Seconds Added to Level Timer");
			//[TODO] Give players feedback that they have more time


			//hitPointAdd += 1;
			comboAddToTimerCount = 0;
		}
	}

	public void ComboEnd(){
		comboStarted = false;
		Debug.Log ("Combo Over");
		comboCount = 0;
		bonus = 1;
	} 

	public void GameOver(){
		audioSource.PlayOneShot (bump);
		gameState = State.Over;
		firstRun = false;
		hitPointAdd = 0;

		characterMove.Clear ();

		Debug.Log ("Restarting Level " + currentLevel);

		
		//iTween.MoveTo(cam, iTween.Hash ("x", 0.0f, "y", 1.0f, "z", -10.0f));
		
		//scoreRecap.text = score.ToString ("$ " + "0,000");
		//comboMaxText.text = comboMax.ToString ("Combo Max \t" + 0);
		//highScoreText.text = highScore.ToString ("High Score \t" + "$" + "0,000");
		//levelCountText.text = levelCount.ToString ("Goals met \t" + 0);
		//Debug.Log ("Setting Text");
	
		iTween.RotateTo(cam, iTween.Hash ("x", -90.0f, "time", 1));
		Debug.Log ("Rotating Camera");
		//End level
		Application.LoadLevel (currentLevel);
	}

	public void OnGUI() {
		GUI.backgroundColor = Color.black;
		GUI.Label (new Rect(Screen.width - 22 , 5, 20, 10), "$"+score);
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
