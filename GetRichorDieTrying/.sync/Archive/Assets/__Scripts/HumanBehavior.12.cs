using UnityEngine;
using System.Collections;

public class HumanBehavior : MonoBehaviour {

	public GameManager				gameMain;

	public GameObject				human;

	public float					movementSpeed = 5.0f; //20 Seconds from end to end

	public GameObject				character;

	public Vector3					currentPos;

	public Vector3					rightDownPos; //Down the street - Right
	public Vector3					rightUpPos; //Up the street - Right
	public Vector3					leftDownPos; //Down the street - Left
	public Vector3					leftUpPos; //Up the street - Left

	public bool						startedRight;
	public bool						startedLeft;

	public int						value; //Value of object
	public int						hitPoints; //Hits to break object
	
	public AudioClip				hitAudio;
	public bool						sameDir;

	// Use this for initialization
	void Start () {
	
		gameMain = GameManager.manager;

		human = this.gameObject;
		iTween.Init (human);

		//[TODO] Make Humans walk to -1.3x or 1.3xonce complete move to one of the End points

		if(human.transform.position.x < 0){
			startedLeft = true;
			rightDownPos = GameObject.Find ("Right-HumanDownStreet").transform.position;
			rightUpPos = GameObject.Find ("Right-HumanUpStreet").transform.position;
//			Debug.Log ("Started Down");
			//sameDir = true;
			iTween.MoveTo(human, iTween.Hash("x", -1.3, "time", movementSpeed, "easetype", iTween.EaseType.linear, "oncomplete", "Move"));
//			iTween.MoveBy(human, iTween.Hash ("z", 100, "time", movementSpeed, "easetype", iTween.EaseType.linear));
//			iTween.MoveTo(human, iTween.Hash ("z", upPos.transform.position.z, "time", movementSpeed, "easetype", iTween.EaseType.linear));
		}
		if(human.transform.position.x > 0){
			startedRight = true;
			leftDownPos = GameObject.Find ("Left-HumanDownStreet").transform.position;
			leftUpPos = GameObject.Find ("Left-HumanUpStreet").transform.position;
//			Debug.Log ("Started Up");
			//sameDir = false;
			iTween.MoveTo(human, iTween.Hash("x", 1.3, "time", movementSpeed, "easetype", iTween.EaseType.linear, "oncomplete", "Move"));
//			iTween.MoveTo (human, iTween.Hash ("z", downPos.transform.position.z, "time", movementSpeed, "easetype", iTween.EaseType.linear));
		}

		//if(human.name == "Human" || thisObject.name == "Human(Clone)"){
			value = 7;
			hitPoints = 3;
		//}
	}
	
	// Update is called once per frame
	void Update () {

		/*
		//people move
		if (sameDir) {
			human.transform.Translate(Vector3.forward * Time.deltaTime * .5f);
		} else {
			human.transform.Translate(Vector3.back * Time.deltaTime * .5f);
		}
		*/

		currentPos = human.transform.position;

		if(hitPoints <= 0){
			
			gameMain.objectName = human.name;
			
			//gameMain.score += value * gameMain.bonus; //Add value of item to score
			
			//tell gamemain pos to Spawn coins
			gameMain.coinPos = human.transform.position;
			//Spawn Coins Here
			gameMain.CashOut(7);
			
			//Destroy
			Destroy(human);
		}

	}

	public void Move(){
		//Move using translate
		human.transform.Translate(Vector3.back * Time.deltaTime * .5f);
		
		//Move using itween

			if(startedLeft){
				iTween.MoveTo(human, iTween.Hash ("z", leftDownPos, "time", movementSpeed, "easetype", iTween.EaseType.linear));

			}
			
		if(startedRight){
			iTween.MoveTo(human, iTween.Hash ("z", rightDownPos, "time", movementSpeed, "easetype", iTween.EaseType.linear));
		}

	}

	void OnCollisionEnter(Collision other) {
		if(other.collider.tag == "Character"){

			Debug.Log("hit human!");
			//gameMain.audioSource.PlayOneShot (humanHit);
			
			//TODO end game when obstacle hit
			gameMain.GameOver ();
		}
		if(other.collider.name == "car(Clone)" || other.collider.name == "car" ){
			//[TODO] launch human when hit
			human.rigidbody.AddForce (0, 10, 50, ForceMode.VelocityChange);
			gameMain.audioSource.PlayOneShot (hitAudio);
			Destroy (human);
		}
	}

	void OnTriggerEnter(Collider other){



		if(startedLeft){ //Checks which side game object started from and destroys it at end
			if(other.tag == "human"){
				Debug.Log ("Human At end");
				Destroy (human);
			}
				
		}
		if(startedRight){ //Checks which side game object started from and destroys it at end
			if(other.tag == "human"){
				Debug.Log ("Human At end");
				Destroy (human);
			}
		}
	}

	void OnMouseDown(){
		
		GameObject cam = Camera.main.gameObject;
		GameObject hammer = GameObject.Find ("Hammer");
		hammer.transform.position = human.transform.position + new Vector3 (.5f,1,0);
		//		hammer.transform.localRotation = Quaternion.Euler(0,0,90);
		hammer.transform.localEulerAngles = new Vector3 (0, 0, 90);
		iTween.RotateTo(hammer, iTween.Hash ("z", 0, "time", 0.2f));
		//gameMain.CamKick ();
		
		Debug.Log(human + " hit!!");
		gameMain.audioSource.PlayOneShot (hitAudio);
		//iTween.MoveFrom (cam, iTween.Hash("z", -0.001f, "time", 0.5f)); //Give the camera a little kick in Z
		//iTween.MoveFrom(human, iTween.Hash ("z", currentPos.z + 0.25f, "y", currentPos.y + -0.25f, "time", 0.5f));
		hitPoints--;
	}
}
