using UnityEngine;
using System.Collections;

public class HumanBehavior : MonoBehaviour {

	public GameManager				gameMain;

	public GameObject				human;

	public float					movementSpeed = 20.0f; //20 Seconds from end to end

	public GameObject				character;

	public Vector3					currentPos;

	public GameObject				downPos; //Down the street
	public GameObject				upPos; //Up the street

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

		if(human.transform.position.x < 0){
			startedLeft = true;
//			Debug.Log ("Started Down");
			sameDir = true;
//			iTween.MoveBy(human, iTween.Hash ("z", 100, "time", movementSpeed, "easetype", iTween.EaseType.linear));
//			iTween.MoveTo(human, iTween.Hash ("z", upPos.transform.position.z, "time", movementSpeed, "easetype", iTween.EaseType.linear));
		}
		if(human.transform.position.x > 0){
			startedRight = true;
//			Debug.Log ("Started Up");
			sameDir = false;
//			iTween.MoveTo (human, iTween.Hash ("z", downPos.transform.position.z, "time", movementSpeed, "easetype", iTween.EaseType.linear));
		}

		//if(human.name == "Human" || thisObject.name == "Human(Clone)"){
			value = 7;
			hitPoints = 3;
		//}
	}
	
	// Update is called once per frame
	void Update () {
		//people move
		if (sameDir) {
			human.transform.Translate(Vector3.forward * Time.deltaTime * .5f);
		} else {
			human.transform.Translate(Vector3.back * Time.deltaTime * .5f);
		}

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

	void OnCollisionEnter(Collision other) {
		if(other.collider.tag == "Character"){

			Debug.Log("hit human!");
			//gameMain.audioSource.PlayOneShot (humanHit);
			
			//TODO end game when obstacle hit
			gameMain.GameOver ();
		}
		if(other.collider.name == "car(Clone)" || other.collider.name == "car" ){
			//[TODO] launch human when hit
			//gameMain.audioSource.PlayOneShot (humanHit);
		}
	}

	void OnTriggerEnter(){



		if(startedLeft){ //Checks which side game object started from and destroys it at end

				Debug.Log ("At end");
				Destroy (human);
		}
		if(startedRight){ //Checks which side game object started from and destroys it at end
			Debug.Log ("At end");
			Destroy (human);
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
