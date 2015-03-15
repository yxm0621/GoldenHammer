using UnityEngine;
using System.Collections;

public class CarBehavior : MonoBehaviour {

	public GameManager				gameMain;
	
	public GameObject				car;
	
	public float					movementSpeed = 100.0f; //20 Seconds from end to end
	
	public GameObject				character;
	
	public GameObject				rightDownPos; //Down the street Right Lane
	public GameObject				rightUpPos; //Up the street Right Lane

	public GameObject				leftDownPos; //Down the street Left Lane
	public GameObject				leftUpPos; //Up the street Left Lane
	
	public bool						rightLane;
	public bool						leftLane;
	
	// Use this for initialization
	void Start () {
		
		gameMain = GameManager.manager;
		
		car = this.gameObject;
		iTween.Init (car);
		
		if(car.transform.position.x < 0){
			leftLane = true;
			Debug.Log ("Left Lane");
			movementSpeed = 50.0f;
			iTween.MoveTo(car, iTween.Hash ("z", leftDownPos.transform.position.z, "time", movementSpeed, "easetype", iTween.EaseType.linear));
		}
		if(car.transform.position.x > 0){
			rightLane = true;
			//[TODO] Spawn car in right lane staggered from left lane via Movementspeed
			movementSpeed = 100.0f;
			Debug.Log ("Right Lane");
			iTween.MoveTo (car, iTween.Hash ("z", rightUpPos.transform.position.z, "time", movementSpeed, "easetype", iTween.EaseType.linear));
		}
	}
	
	// Update is called once per frame
	void Update () {
		
		
		
	}
	
	void OnTriggerEnter(){
		
		
		
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
