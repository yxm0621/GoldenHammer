using UnityEngine;
using System.Collections;

public enum Swipe { None, Up, Down, Left, Right};

public class characterController : MonoBehaviour {
	public GameManager				gameMain; //Singleton Ref to GameManager Script
	public bool						canControl = false;
	public static Swipe				swipeDirection;
	
	public float					minSwipeLength = 10f;
	
	public Vector2 					firstPressPos; //Position of 1st press
	public Vector2					continuePressPos; //Position of continue pressing
	public Vector2					secondPressPos; //Position of 2nd press
	public Vector2					currentSwipe;
	public Vector2					currentTouch;
	
	public GameObject               character;
	public Vector3                  characterPos;

	public Vector3					movePos0;
	public Vector3					movePos1;
	public Vector3					movePos2;
	public Vector3					movePos3;
	public Vector3					movePos4;

//	public bool                     canForward;
//	public bool                     canBackward;
//	public bool                     canMoveLeft;
//	public bool                     canMoveRight;
//	public bool						continueForward;

	public static bool              forward = false;
	public static bool              onSidewalk = false;
	
	public float                    moveOffset = 1;
	public float                    jumpOffset = .9f;
	public int						jumpForce = 350;
	public float                    dodgeOffset = .2f;

	public int                    	curPosX;
	public int                   	curPosZ;

	public bool 					touching = true;
	
	// Use this for initialization
	void Start () {
		gameMain = GameManager.manager; //Set Ref to GameManager Script
		if (!gameMain.firstRun) {
			if(Application.loadedLevelName != "GameOver"){
				gameMain.Start ();
			}

		}

		movePos0 = GameObject.Find ("Pos0").transform.position;
		movePos1 = GameObject.Find ("Pos1").transform.position;
		movePos2 = GameObject.Find ("Pos2").transform.position;
		movePos3 = GameObject.Find ("Pos3").transform.position;
		movePos4 = GameObject.Find ("Pos4").transform.position;

		characterPos = new Vector3 (.5f, .5f, -1.4f);
		character = GameObject.FindGameObjectWithTag("Character");
		character.transform.position = characterPos;
		curPosX = Mathf.Clamp (2,-1,4); //(Current Value, Min, Max)
		curPosZ = 3;
//		continueForward = false;
	}
	
	// Update is called once per frame
	void Update () {
		if (canControl) {

		//move camera when character move left or right
		Camera.main.gameObject.transform.position = new Vector3(character.transform.position.x,
		                                                        Camera.main.gameObject.transform.position.y,
		                                                        Camera.main.gameObject.transform.position.z);

		SwipeCheck();

		//check whether the character can go to 4 directions
//		canForward = gameMain.getMovement(curPosX, curPosZ + 1);
//		canBackward = gameMain.getMovement(curPosX, curPosZ - 1);
//		canMoveLeft = gameMain.getMovement(curPosX - 1, curPosZ);
//		canMoveRight = gameMain.getMovement(curPosX + 1, curPosZ);

//		Debug.Log ("curX: " + curPosX + ", curZ: "+curPosZ+", FBLR: "+canForward + canBackward+canMoveLeft+canMoveRight);

//		//constant moving
//		if (continueForward) {
//			character.transform.localEulerAngles = new Vector3 (0,0,0);
//			if (canForward && curPosZ == 5) {
//				curPosZ = 0;
//				forward = true;
//				character.transform.position -= new Vector3(0,0,2);
//			} else if (canForward && curPosZ == 2) {
//				curPosZ = 3;
//				forward = true;
//				character.transform.position -= new Vector3(0,0,2);
//			} else if (canForward) {
//				moveForward();
//				curPosZ++;
//			}
//		}

		//moving step by step
		if (swipeDirection != Swipe.None) {
//			Debug.Log (swipeDirection);
//			character.animation.CrossFade("run");
			switch (swipeDirection) {
			case Swipe.Up:
					forceJump();
//					StartCoroutine(jump());
					/*
				//moving forward
				character.transform.localEulerAngles = new Vector3 (0, 0, 0);
				//reach the edge of the grid
				if (canForward && curPosZ == 5) {
					curPosZ = 0;
					forward = true;
					character.transform.position -= new Vector3 (0, 0, 2);
					break;
				}
				//reach the middle of the grid
				if (canForward && curPosZ == 2) {
					curPosZ = 3;
					forward = true;
					character.transform.position -= new Vector3 (0, 0, 2);
					break;
				}
				//other conditions
				if (canForward) {
					//moveForward ();
					curPosZ++;
					break;
				}*/
				break;
			case Swipe.Down:
//					character.rigidbody.AddForce(Vector3.down*500);
					//StartCoroutine(dodge());
					/*
				//moving backward
				character.transform.localEulerAngles = new Vector3 (0, 180, 0);
				if (canBackward && curPosZ != 3) {
					//moveBack ();
					curPosZ--;
				}*/
				break;
			case Swipe.Left:
				//moving left
				//character.transform.localEulerAngles = new Vector3 (0, -90, 0);
				//if (canMoveLeft) {
					//moveLeft ();
					curPosX--;
					move();
				//}
				break;
			case Swipe.Right:
				//moving right
				//character.transform.localEulerAngles = new Vector3 (0, 90, 0);
				//if (canMoveRight) {
					//moveRight ();
					curPosX++;
					move ();
				//}
				break;
			default:
				break;
			}
		} else {
//			character.animation.CrossFade("idle");
		}

//		Animation anim = character.GetComponent<Animation>();
//		foreach(AnimationState state in anim)
//		{
//			Debug.Log(state.name);
//		}

		if (curPosX == 0 || curPosX == 3) {
			onSidewalk = true;
		}

		}
	}
	
	public void SwipeCheck(){
		
		#if UNITY_ANDROID || UNITY_IOS || UNITY_EDITOR
		//		Debug.Log ("Touches Recognized");
		
		if(Input.touches.Length > 0){
			Touch t = Input.GetTouch(0);
			
			if (t.phase == TouchPhase.Began){
				firstPressPos = new Vector2(t.position.x, t.position.y);
				touching = true;
			}
			
			if(t.phase == TouchPhase.Ended){
				secondPressPos = new Vector2(t.position.x, t.position.y);
				currentSwipe = new Vector2(secondPressPos.x - firstPressPos.x, secondPressPos.y - firstPressPos.y);
				
				//make sure it was a legit swipe not a tap
				if(currentSwipe.magnitude < minSwipeLength || !touching){
					swipeDirection = Swipe.None;
					return;
				}
				currentSwipe.Normalize ();
				
				//swipe upwards
				if(currentSwipe.y > 0 && currentSwipe.x > -0.5f && currentSwipe.x < 0.5f){
					Debug.Log("Up Swipe");
					swipeDirection = Swipe.Up;
					touching = false;
				}
				
				//swipe down
				if(currentSwipe.y < 0 && currentSwipe.x > -0.5f && currentSwipe.x < 0.5f){
					Debug.Log ("Down Swipe");
					swipeDirection = Swipe.Down;
					touching = false;
				}
				
				//swipe left
				if(currentSwipe.x < 0 && currentSwipe.y > -0.5f && currentSwipe.y < 0.5f){
					Debug.Log ("Left Swipe");
					swipeDirection = Swipe.Left;
					touching = false;
				}
				
				//swipe right
				if(currentSwipe.x > 0 && currentSwipe.y > -0.5f && currentSwipe.y < 0.5f){
					Debug.Log ("Right Swipe");
					swipeDirection = Swipe.Right;
					touching = false;
				}
			}
		} else{
			swipeDirection = Swipe.None;
		}
		#endif
		
		if(Input.GetMouseButtonDown(0)){
			//save began touch 2d point
			firstPressPos = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
//            touching = true;
		}

		if(Input.GetMouseButton(0)){
			if(!touching) {
				swipeDirection = Swipe.None;
				return;
			}
			//save ended touch 2d point
//			continuePressPos = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
			
			//creat vector from the two points
//			currentTouch = new Vector2(continuePressPos.x - firstPressPos.x, continuePressPos.y - firstPressPos.y);
			
			//normalize the 2d vector
//			currentTouch.Normalize ();
			
			//touch upwards
//			if(currentTouch.y > 0 && currentTouch.x > -0.5f && currentTouch.x < 0.5f){
//				Debug.Log("Up Touch");
//				continueForward = true;
//			}
		//}
		//if(Input.GetMouseButtonUp(0)){
//			continueForward = false;

			//save ended touch 2d point
			secondPressPos = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
			
			//creat vector from the two points
			currentSwipe = new Vector2(secondPressPos.x - firstPressPos.x, secondPressPos.y - firstPressPos.y);
			
			//normalize the 2d vector
			currentSwipe.Normalize ();
			
			//swipe upwards
			if((currentSwipe.y > 0 && currentSwipe.x > -0.5f && currentSwipe.x < 0.5f)
			&&touching){
				Debug.Log("Up Swipe");
				swipeDirection = Swipe.Up;
				touching  = false;
			}
			
			//swipe down
			if((currentSwipe.y < 0 && currentSwipe.x > -0.5f && currentSwipe.x < 0.5f)
			   &&touching){
				Debug.Log ("Down Swipe");
				swipeDirection = Swipe.Down;
				touching = false;
			}
			
			//swipe left
			if((currentSwipe.x < 0 && currentSwipe.y > -0.5f && currentSwipe.y < 0.5f)
			   &&touching){
				Debug.Log ("Left Swipe");
				swipeDirection = Swipe.Left;
				touching = false;
			}
			
			//swipe right
			if((currentSwipe.x > 0 && currentSwipe.y > -0.5f && currentSwipe.y < 0.5f)
			   &&touching){
				Debug.Log ("Right Swipe");
				swipeDirection = Swipe.Right;
				touching = false;
			}
		}
        if (Input.GetMouseButtonUp(0)) {
            touching = true;
        }
	}

	void move(){
				if(curPosX == 0){
					iTween.MoveTo(character, iTween.Hash("x", movePos0.x, "easeType", "easeInOutExpo", "time", .2f));
				}
				if(curPosX == 1){
					iTween.MoveTo(character, iTween.Hash("x", movePos1.x, "easeType", "easeInOutExpo", "time", .2f));
				}
				if(curPosX == 2){
					iTween.MoveTo(character, iTween.Hash("x", movePos2.x, "easeType", "easeInOutExpo", "time", .2f));
				}
				if(curPosX == 3){
					iTween.MoveTo(character, iTween.Hash("x", movePos3.x, "easeType", "easeInOutExpo", "time", .2f));
				}
	}

//	void moveForward(){
//		iTween.MoveTo(character, iTween.Hash("z", character.transform.position.z + moveOffset, "easeType", "easeInOutExpo", "loopType", "none", "time", .3));
//	}
//	
//	void moveBack(){
//		iTween.MoveTo(character, iTween.Hash("z", character.transform.position.z - moveOffset, "easeType", "easeInOutExpo", "loopType", "none", "time", .3));
//	}
//
//	void moveLeft(){
//		iTween.MoveTo(character, iTween.Hash("x", character.transform.position.x - moveOffset, "easeType", "easeInOutExpo", "loopType", "none", "time", .3));
//	}
//
//	void moveRight(){
//		iTween.MoveTo(character, iTween.Hash("x", character.transform.position.x + moveOffset, "easeType", "easeInOutExpo", "loopType", "none", "time", .3));
//	}

	void forceJump(){
//		rigidbody.velocity = new Vector3(0, 10, 0);
		rigidbody.AddForce(Vector3.up * jumpForce);
	}
	
//		IEnumerator jump(){
//			iTween.MoveBy(character, iTween.Hash("y", -.01f, "easeType", "easeInOutExpo", "loopType", "none", "time", .05f));
//			yield return new WaitForSeconds (.05f);
//			iTween.MoveBy(character, iTween.Hash("y", jumpOffset, "easeType", "easeInOutExpo", "loopType", "none", "time", .2f));
//			yield return new WaitForSeconds (.4f);
//			iTween.MoveBy(character, iTween.Hash("y", 0-jumpOffset, "easeType", "easeInOutExpo", "loopType", "none", "time", .1f));
//			yield return new WaitForSeconds (.1f);
//			iTween.MoveTo(character, iTween.Hash("y", movePos3.y, "easeType", "easeInOutExpo","time", .2f));
//		}
	
		IEnumerator dodge(){
		character.transform.localScale -= new Vector3(0,.3f,0);
//			iTween.MoveTo(character, iTween.Hash("y", characterPos.y - dodgeOffset, "easeType", "easeInOutExpo", "loopType", "none", "time", .3));
			yield return new WaitForSeconds (.5f);
//			iTween.MoveTo(character, iTween.Hash("y", characterPos.y, "easeType", "easeInOutExpo", "loopType", "none", "time", .3));
		character.transform.localScale += new Vector3 (0, .3f, 0);
		}

	void OnTriggerEnter(Collider other){
		if(other.tag == "SegmentKill"){
			Destroy (gameObject);
			gameMain.GameOver();
		}
	}
}
