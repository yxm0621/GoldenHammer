using UnityEngine;
using System.Collections;

public class HumanBehavior : MonoBehaviour {
	public GameManager				gameMain;
	public GameObject				human;
    public ObjectManager            objManager;
	
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
        objManager = human.GetComponent<ObjectManager>();
        objManager.value = 7;
        objManager.hitPoints = 1;
        character = GameObject.FindGameObjectWithTag("Character");

        //When people get off from the car, move to the sidewalk
        if ((human.transform.position.x > -1) && (human.transform.position.x < 1)) {
            iTween.Init(human);
            if (human.transform.position.x < 0) {
                human.transform.Translate(Vector3.left);
                iTween.MoveTo(human, iTween.Hash("x", -1.5f, "time", 1f, "easetype", iTween.EaseType.linear));
            } else {
                iTween.MoveTo(human, iTween.Hash("x", 1.5f, "time", 1f, "easetype", iTween.EaseType.linear));
            }
        }

        //Fix person move a lot
        if (gameObject.transform.position.x < -1.5f) {
            gameObject.transform.position = new Vector3(-1.5f, gameObject.transform.position.y, gameObject.transform.position.z);
        }
        if (gameObject.transform.position.x > 1.5f) {
            gameObject.transform.position = new Vector3(1.5f, gameObject.transform.position.y, gameObject.transform.position.z);
        }

        ////human.transform.localEulerAngles = new Vector3 (0, 180, 0);
        //iTween.Init (human);
		
        ////[TODO] Make Humans walk to -1.3x or 1.3xonce complete move to one of the End points
		
        //if(human.transform.position.x < 0){
        //    startedLeft = true;
        //    leftDownPos = GameObject.Find ("Left-HumanDownStreet").transform.position;
        //    leftUpPos = GameObject.Find ("Left-HumanUpStreet").transform.position;
        //    //			Debug.Log ("Started Down");
        //    //sameDir = true;
        //    iTween.MoveTo(human, iTween.Hash("x", -1.3, "time", 1f, "easetype", iTween.EaseType.linear, "oncomplete", "Move", "oncompletetarget", human));
        //    //			iTween.MoveBy(human, iTween.Hash ("z", 100, "time", movementSpeed, "easetype", iTween.EaseType.linear));
        //    //			iTween.MoveTo(human, iTween.Hash ("z", upPos.transform.position.z, "time", movementSpeed, "easetype", iTween.EaseType.linear));
        //}
        //if(human.transform.position.x > 0){
        //    startedRight = true;
        //    rightDownPos = GameObject.Find ("Right-HumanDownStreet").transform.position;
        //    rightUpPos = GameObject.Find ("Right-HumanUpStreet").transform.position;
			
        //    //			Debug.Log ("Started Up");
        //    //sameDir = false;
        //    iTween.MoveTo(human, iTween.Hash("x", 1.3, "time", 0.5f, "easetype", iTween.EaseType.linear, "oncomplete", "Move", "oncompletetarget", human));
        //    //			iTween.MoveTo (human, iTween.Hash ("z", downPos.transform.position.z, "time", movementSpeed, "easetype", iTween.EaseType.linear));
        //}
		
		//if(human.name == "Human" || thisObject.name == "Human(Clone)"){
        //value = 7;
        //hitPoints = 1;
		//}
	}
	
	// Update is called once per frame
	void Update () {
		if (human.transform.position.z - character.transform.position.z < -2f) {
            Destroy(gameObject);
		}
		/*
		//people move
		if (sameDir) {
			human.transform.Translate(Vector3.forward * Time.deltaTime * .5f);
		} else {
			human.transform.Translate(Vector3.back * Time.deltaTime * .5f);
		}
		*/
		
	}
	
	public void Move(){
		Debug.Log ("Human On Sidewalk");
		
		//Move using translate
		//human.transform.Translate(Vector3.back * Time.deltaTime * .5f);
		//Debug.Log ("Moving down road");
		
		//Move using itween
		
		if(startedLeft){
			Debug.Log ("Moving down road");
			iTween.MoveTo(human, iTween.Hash ("z", leftDownPos.z, "time", 4.0f, "easetype", iTween.EaseType.linear));
			
		}
		
		if(startedRight){
			Debug.Log ("Moving down road");
			iTween.MoveTo(human, iTween.Hash ("z", rightDownPos.z, "time", 4.0f, "easetype", iTween.EaseType.linear));
		}
		
	}
	
	void OnCollisionEnter(Collision other) {
		if(other.collider.CompareTag("Character")){
			Debug.Log("hit human!");
			//gameMain.audioSource.PlayOneShot (humanHit);
			
			//TODO end game when obstacle hit
            gameMain.hitObj = gameObject;
			gameMain.GameOver ();
		}
		if(other.collider.name.Contains("Car") || other.collider.name.Contains("Police") || other.collider.name.Contains("Tank")){
			//[TODO] launch human when hit
			human.GetComponent<Rigidbody>().AddForce (0, 10, 50, ForceMode.VelocityChange);
			gameMain.audioSource.PlayOneShot (hitAudio);
			Destroy (human);
		}
	}
	
	void OnTriggerEnter(Collider other){
		if(startedLeft){ //Checks which side game object started from and destroys it at end
			if(other.CompareTag("human")){
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
}
