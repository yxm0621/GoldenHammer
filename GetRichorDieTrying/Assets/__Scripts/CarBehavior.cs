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
        if (gameMain.gameState == GameManager.State.InGame)
        {
            //cars move
            car.transform.Translate(Vector3.back * Time.deltaTime * runSpeed);
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
		if (!car.name.Contains("car") && car.name.Contains("R") && car.transform.position.z <= 1.5f) {
			iTween.Stop(car);
				car.transform.localEulerAngles = new Vector3 (0, 90, 0);
		}
		if (!car.name.Contains("car") && car.name.Contains("L") && car.transform.position.z <= 1.2f) {
			iTween.Stop(car);
		}
		

		if(objManager.hitPoints <= 0) {
			//Person falls out of car
            Vector3 humanPos = new Vector3(car.transform.position.x,
                                           car.transform.position.y + .5f,
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
		if(other.collider.tag == "Character"){
            //Debug.Log("hit by Car!");
            gameMain.audioSource.PlayOneShot(objManager.hitAudio);
			
			//TODO end game when obstacle hit
			gameMain.GameOver ();
		}
	}

	void OnTriggerEnter(Collider other){
		if(other.collider.tag == "Car"){
//			if(leftLane){ //Checks which side game object started from and destroys it at end
				
//				Debug.Log ("At end");
//				Destroy (car);
//			}
//			if(rightLane){ //Checks which side game object started from and destroys it at end
//				Debug.Log ("At end");
				Destroy (car);
//			}
		}

	}
}
