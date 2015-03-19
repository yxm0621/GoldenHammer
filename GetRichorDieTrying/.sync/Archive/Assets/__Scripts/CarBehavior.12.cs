using UnityEngine;
using System.Collections;

public class CarBehavior : MonoBehaviour {

	public GameManager				gameMain;
	
	public GameObject				car;

	public GameObject				human;
	
	public float					movementSpeed = 100.0f; //Seconds from end to end
	
	public GameObject				character;

	public Vector3					currentPos;
	
	public GameObject				rightDownPos; //Down the street Right Lane
	public GameObject				rightUpPos; //Up the street Right Lane

	public GameObject				leftDownPos; //Down the street Left Lane
	public GameObject				leftUpPos; //Up the street Left Lane
	
	public bool						rightLane;
	public bool						leftLane;

	public int						value; //Value of object
	public int						hitPoints; //Hits to break object
	
	public AudioClip				hitAudio;
	
	// Use this for initialization
	void Start () {
		
		gameMain = GameManager.manager;
		
		car = this.gameObject;
		iTween.Init (car);
		
		if(car.transform.position.x < 0){
			leftLane = true;
			Debug.Log ("Left Lane");
			movementSpeed = 5.0f;
			iTween.MoveTo(car, iTween.Hash ("position", leftDownPos.transform.position, "time", movementSpeed, "easetype", iTween.EaseType.linear));
		}
		if(car.transform.position.x > 0){
			rightLane = true;
			//[TODO] Spawn car in right lane staggered from left lane via Movementspeed
			movementSpeed = 10.0f;
			Debug.Log ("Right Lane");
			iTween.MoveTo (car, iTween.Hash ("position", rightDownPos.transform.position, "time", movementSpeed, "easetype", iTween.EaseType.linear));
		}
		
		
		//if(thisObject.name == "car" || thisObject.name == "car(Clone)"){
			value = 10;
			hitPoints = 7;
		//}
	}
	
	// Update is called once per frame
	void Update () {
		//cars move
		/*
		if (rightLane) {
			car.transform.Translate(Vector3.back * Time.deltaTime * .7f);
		} else {
			car.transform.Translate(Vector3.back * Time.deltaTime * .7f);
		}
		*/

		currentPos = car.transform.position;
		
		if(hitPoints <= 0){
			
			gameMain.objectName = car.name;
			
			//gameMain.score += value * gameMain.bonus; //Add value of item to score

			//tell gamemain pos to Spawn coins
			gameMain.coinPos = car.transform.position;
			//Spawn Coins Here
			gameMain.CashOut(10);

			Instantiate (human, car.transform.position, Quaternion.identity);

			//Destroy
			Destroy(car);
		}
	}

	void OnCollisionEnter(Collision other) {
		if(other.collider.tag == "Character"){
			
			Debug.Log("hit by Car!");
			//gameMain.audioSource.PlayOneShot (humanHit);
			
			//TODO end game when obstacle hit
			gameMain.GameOver ();
		}
	}

	void OnTriggerEnter(Collider other){
		
		
		if(other.collider.tag == "car"){
			if(leftLane){ //Checks which side game object started from and destroys it at end
				
				Debug.Log ("At end");
				Destroy (car);
			}
			if(rightLane){ //Checks which side game object started from and destroys it at end
				Debug.Log ("At end");
				Destroy (car);
			}
		}

	}

	void OnMouseDown(){
		
		GameObject cam = Camera.main.gameObject;
		GameObject hammer = GameObject.Find ("Hammer");
		hammer.transform.position = car.transform.position + new Vector3 (.5f,1,0);
		//		hammer.transform.localRotation = Quaternion.Euler(0,0,90);
		hammer.transform.localEulerAngles = new Vector3 (0, 0, 90);
		iTween.RotateTo(hammer, iTween.Hash ("z", 0, "time", 0.2f));
		//gameMain.CamKick ();
		
		Debug.Log(car + " hit!!");
		gameMain.audioSource.PlayOneShot (hitAudio);
		//iTween.MoveFrom (cam, iTween.Hash("z", -0.001f, "time", 0.5f)); //Give the camera a little kick in Z
		//iTween.MoveFrom(car, iTween.Hash ("z", currentPos.z + 0.25f, "y", currentPos.y + -0.25f, "time", 0.5f));
		hitPoints--;
	}
}
