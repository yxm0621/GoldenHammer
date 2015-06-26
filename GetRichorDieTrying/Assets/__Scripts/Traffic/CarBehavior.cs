using UnityEngine;
using System.Collections;

public class CarBehavior : MonoBehaviour {

	public GameManager				gameMain;
	public GameObject				car;
	public GameObject				human;
    public ObjectManager            objManager;

    public float                    runTime = 1f;
    public float                    runDistance;
    public float                    destination = -1f;
    public Vector3                  carPos;
    public float                    runSpeed;
    public float                    startX;

    public float                    characterDis; //distance between character and car

    public bool                     addGravity = false;
	
//	public float					movementSpeed = 100.0f; //Seconds from end to end
	
//	public GameObject				character;

//	public Vector3					currentPos;
	
//	public GameObject				rightDownPos; //Down the street Right Lane
//	public GameObject				rightUpPos; //Up the street Right Lane

//	public GameObject				leftDownPos; //Down the street Left Lane
//	public GameObject				leftUpPos; //Up the street Left Lane
	
//	public bool						rightLane;
//	public bool						leftLane;
	
	// Use this for initialization
	void Start () {
		gameMain = GameManager.manager;
		car = this.gameObject;
        carPos = car.transform.position;
        objManager = car.GetComponent<ObjectManager>();

        objManager.value = 10;
        objManager.hitPoints = 7;

        startX = carPos.x;

        //iTween.Init(car);
        //iTween.MoveBy(car, iTween.Hash("z", -30, "time", 10, "easetype", "linear"));

//		if(car.transform.position.x < 0){
//			leftLane = true;
//			Debug.Log ("Left Lane");
//			movementSpeed = 5.0f;
//			iTween.MoveTo(car, iTween.Hash ("position", leftDownPos.transform.position, "time", movementSpeed, "easetype", iTween.EaseType.linear));
//		}
//		if(car.transform.position.x > 0){
//			rightLane = true;
			//[TODO] Spawn car in right lane staggered from left lane via Movementspeed
//			movementSpeed = 10.0f;
//			Debug.Log ("Right Lane");
//			iTween.MoveTo (car, iTween.Hash ("position", rightDownPos.transform.position, "time", movementSpeed, "easetype", iTween.EaseType.linear));
//		}
		
		
		//if(thisObject.name == "car" || thisObject.name == "car(Clone)"){
            //value = 10;
            //hitPoints = 7;
		//}
        
	}
	
	// Update is called once per frame
	void Update () {
        //runSpeed = (carPos.z - destination) / runTime;
        //runSpeed = runSpeed - gameMain.moveSpeed;
        runSpeed = runDistance / runTime;

        characterDis = car.transform.position.z - gameMain.characterPos.z;
        if (characterDis < -4f) {
            Destroy(gameObject);
		}
        if (gameObject.transform.position.y > .08f) {
            gameObject.transform.position = new Vector3(gameObject.transform.position.x, .08f, gameObject.transform.position.z);
        }
        if (gameObject.transform.position.x != startX) {
            gameObject.transform.position = new Vector3(startX, gameObject.transform.position.y, gameObject.transform.position.z);
        }
        if ((characterDis < 18) && !addGravity){
            gameObject.GetComponent<Rigidbody>().useGravity = true;
            gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotation;
            addGravity = true;
        }

        if (gameMain.gameState == GameManager.State.InGame) {
            //Stop police cars and tanks
            if (car.name.Contains("Police") || car.name.Contains("Tank")) {
                if (car.name.Contains("R") && characterDis <= 2.8f) {
                    //Stop and turn
                    car.transform.localEulerAngles = new Vector3(0, 90, 0);
                } else if (car.name.Contains("L") && characterDis <= 2.5f) {
                    //Stop
                } else {
                    //cars move
                    if(!gameMain.traffic.trafficStop) {
                        car.transform.Translate(Vector3.forward * Time.deltaTime * runSpeed);
                    }
                }
            } else {
                //cars move
                if (!gameMain.traffic.trafficStop) {
                    car.transform.Translate(Vector3.forward * Time.deltaTime * runSpeed);
                }
            }
        }
		/*
		if (rightLane) {
			car.transform.Translate(Vector3.back * Time.deltaTime * .7f);
		} else {
			car.transform.Translate(Vector3.back * Time.deltaTime * .7f);
		}
		*/

//		currentPos = car.transform.position;

		//Stop police cars and tanks
        //if (!car.name.Contains("car") && car.name.Contains("R") && car.transform.position.z <= 1.5f) {
        //    iTween.Stop(car);
        //        car.transform.localEulerAngles = new Vector3 (0, 90, 0);
        //}
        //if (!car.name.Contains("car") && car.name.Contains("L") && car.transform.position.z <= 1.2f) {
        //    iTween.Stop(car);
        //}
		

		if(objManager.hitPoints <= 0) {
			//Person falls out of car
            Vector3 humanPos = new Vector3(car.transform.position.x,
                                           car.transform.position.y - .07f,
                                           car.transform.position.z);
            GameObject newHuman = (GameObject)Instantiate(human, humanPos, Quaternion.identity);
            newHuman.transform.parent = gameMain.obstacleGroup.transform;

		}
	}

    public void SetCar(float timer, float dis) {
        runTime = timer;
        runDistance = dis;
    }

	void OnCollisionEnter(Collision other) {
		if(other.collider.CompareTag("Character")){
            //Debug.Log("hit by Car!");
            gameMain.audioSource.PlayOneShot(objManager.hitAudio);
			
			//TODO end game when obstacle hit
            gameMain.hitObj = gameObject;
			gameMain.GameOver ();
		}
	}

	void OnTriggerEnter(Collider other){
		if(other.GetComponent<Collider>().CompareTag("Car")){
//			if(leftLane){ //Checks which side game object started from and destroys it at end
				
//				Debug.Log ("At end");
//				Destroy (car);
//			}
//			if(rightLane){ //Checks which side game object started from and destroys it at end
//				Debug.Log ("At end");
				Destroy (car);
//			}
		}
        if(other.GetComponent<Collider>().CompareTag("SegmentKill")){
            //kill the car
            objManager.hitPoints = 0;
		}
	}
}
