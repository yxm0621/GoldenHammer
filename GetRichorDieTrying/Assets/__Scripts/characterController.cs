using UnityEngine;
using System.Collections;

public enum Swipe { None, Up, Down, Left, Right};

public class characterController : MonoBehaviour {
	public GameManager				gameMain; //Singleton Ref to GameManager Script
	public bool						canControl = false;
	public static Swipe				swipeDirection;
	
	public float					minSwipeLength = 10;
	public float					minSwipeSpeed = 100;
	
	public Vector2 					firstPressPos; //Position of 1st press
	public Vector2					secondPressPos; //Position of 2nd press
	public Vector2					currentSwipe;
	public Vector2					currentTouch;
	
	public GameObject               character;
	public Vector3                  characterPos;

    public Vector3[]                movePos;

	public static bool              forward = false;
	public static bool              onSidewalk = false;
	
    //public float                    jumpOffset = .9f;
	public int						jumpForce = 350;
	public float                    dodgeOffset = .2f;

	public int                    	curPosX;

	public bool 					touching = false;
    public bool                     onTheGround = false;
	
	// Use this for initialization
	void Start () {
		gameMain = GameManager.manager; //Set Ref to GameManager Script

        //Run gameManager.Start() when restart game
		if (!gameMain.firstRun) {
			if(Application.loadedLevelName != "GameOver"){
				gameMain.Start ();
			}
		}

        movePos[0] = GameObject.Find("PlayerLanes/Pos0").transform.position;
        movePos[1] = GameObject.Find("PlayerLanes/Pos1").transform.position;
        movePos[2] = GameObject.Find("PlayerLanes/Pos2").transform.position;
        movePos[3] = GameObject.Find("PlayerLanes/Pos3").transform.position;
        movePos[4] = GameObject.Find("PlayerLanes/Pos4").transform.position;

		characterPos = new Vector3 (.5f, 0f, 0f);
        character = this.gameObject;
        //character.transform.position = characterPos;
		curPosX = Mathf.Clamp (2,-1,4); //(Current Value, Min, Max)

        //fix Camera position problem
        //Camera.main.gameObject.transform.position = new Vector3(character.transform.position.x,
        //                                                        Camera.main.gameObject.transform.position.y,
        //                                                        Camera.main.gameObject.transform.position.z);
        //gameMain.camStartPos.x = character.transform.position.x;
	}
	
	// Update is called once per frame
	void Update () {
        if (character.transform.position.y > movePos[4].y) {
            character.transform.position = new Vector3(character.transform.position.x,
                                                       movePos[4].y,
                                                       character.transform.position.z);
        }

		if (canControl) {

            //When player is not collidinng with ground, fall down. Otherwise keep y == 0
            if (!onTheGround) {
                //character.transform.Translate(Vector3.down * Time.deltaTime * .98f);
                character.gameObject.GetComponent<Rigidbody>().useGravity = true;
            } else {
                character.gameObject.GetComponent<Rigidbody>().useGravity = false;
                character.transform.position = new Vector3(character.transform.position.x,
                                                           0f,
                                                           character.transform.position.z);
                onTheGround = false;
            }



            SwipeCheck();

		//moving step by step
		if (swipeDirection != Swipe.None) {
//			Debug.Log (swipeDirection);
//			character.animation.CrossFade("run");
			switch (swipeDirection) {
			case Swipe.Up:
                //if (character.transform.position.y <= .515f) {
                forceJump();
                //}
				break;
			case Swipe.Down:
                //dodge
                //character.rigidbody.AddForce(Vector3.down*500);
                //StartCoroutine(dodge());
				break;
			case Swipe.Left:
				//moving left
                if (curPosX > 0) {
                    curPosX--;
                    Move();
                }
				break;
			case Swipe.Right:
				//moving right
                if (curPosX < 3) {
                    curPosX++;
                    Move();
                }
				break;
			default:
				break;
			}
		} else {
//			character.animation.CrossFade("idle");
            if (character.transform.position.x != movePos[curPosX].x) {
                character.transform.position = new Vector3(movePos[curPosX].x,
                                                           character.transform.position.y,
                                                           character.transform.position.z);
            }
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

    //calculate direction with 2 points
    void CalculateDir(Vector3 first, Vector3 second) {
        currentSwipe = new Vector2(second.x - first.x, second.y - first.y);
        float length = currentSwipe.magnitude;
        Debug.Log("touch magnitude: "+length);

        //make sure it was a legit swipe not a tap
        if (currentSwipe.magnitude < minSwipeLength)
        {
            swipeDirection = Swipe.None;
            return;
        }
        currentSwipe.Normalize();

        //swipe left
        if (currentSwipe.x < 0 && currentSwipe.y > (-Mathf.Sqrt(3) / 2f) && currentSwipe.y < (Mathf.Sqrt(3) / 2f))
        {
            //Debug.Log("Left Swipe" + length);
            swipeDirection = Swipe.Left;
            touching = false;
            return;
        }

        //swipe right
        if (currentSwipe.x > 0 && currentSwipe.y > (-Mathf.Sqrt(3) / 2f) && currentSwipe.y < (Mathf.Sqrt(3) / 2f))
        {
            //Debug.Log("Right Swipe" + length);
            swipeDirection = Swipe.Right;
            touching = false;
            return;
        }

        //swipe upwards
        if (currentSwipe.y > 0 && currentSwipe.x > -0.5f && currentSwipe.x < 0.5f)
        {
            //Debug.Log("Up Swipe" + length);
            swipeDirection = Swipe.Up;
            touching = false;
            return;
        }

        //swipe down
        if (currentSwipe.y < 0 && currentSwipe.x > -0.5f && currentSwipe.x < 0.5f)
        {
            //Debug.Log("Down Swipe" + length);
            swipeDirection = Swipe.Down;
            touching = false;
            return;
        }
    }

    //input swipe check
    public void SwipeCheck() {
        //only recognize when 1 finger touch
        if (Input.touchCount == 1) {
            switch (Input.touches[0].phase) {
                case TouchPhase.Began:
                    Debug.Log("touch start");
                    firstPressPos = new Vector2(Input.GetTouch(0).position.x, Input.GetTouch(0).position.y);
                    touching = true;
                    swipeDirection = Swipe.None;
                    break;
                case TouchPhase.Moved:
                    float touchSpeed = Input.GetTouch(0).deltaPosition.magnitude / Input.GetTouch(0).deltaTime;
                    Debug.Log("touchMovingSpeed: " + touchSpeed);
                    //Debug.Log("Touch index " + Input.GetTouch(0).fingerId + " has moved by " + Input.GetTouch(0).deltaPosition);
                    if (touching && touchSpeed > minSwipeSpeed) {
                        secondPressPos = new Vector2(Input.GetTouch(0).position.x, Input.GetTouch(0).position.y);
                        CalculateDir(firstPressPos, secondPressPos);
                    } else {
                        //when one direction has already been checked
                        swipeDirection = Swipe.None;
                        return;
                    }
                    break;
                case TouchPhase.Ended:
                    Debug.Log("touch end");
                    if (touching) {
                        //when player ups the finger too fast and the "moved" state didn't have time to check the direction
                        secondPressPos = new Vector2(Input.GetTouch(0).position.x, Input.GetTouch(0).position.y);
                        CalculateDir(firstPressPos, secondPressPos);
                    } else {
                        swipeDirection = Swipe.None;
                        return;
                    }
                    break;
                default:
                    swipeDirection = Swipe.None;
                    break;
            }
        } else {
/* --------comment from here if test with mouse or
 * cancel comment from here if test with touch-------- */
    //        swipeDirection = Swipe.None;
    //        return;
    //    }
    //}
/* --------comment until here if test with mouse or
 * cancel comment until here if test with touch-------- */



/* --------comment from here if test with touch
 * or cancel comment from here if test with mouse-------- */

            if (Input.GetMouseButtonDown(0))
            {
                //save began touch 2d point
                firstPressPos = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
                touching = true;
            }

            if (Input.GetMouseButton(0))
            {
                if (!touching)
                {
                    swipeDirection = Swipe.None;
                    return;
                }
                //save ended touch 2d point
                secondPressPos = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
                CalculateDir(firstPressPos, secondPressPos);
            }
            else
            {
                swipeDirection = Swipe.None;
                return;
            }
        }
    }

/* --------comment until here if test with touch
 * or cancel comment until here if test with mouse-------- */

	void Move() {
        iTween.MoveTo(character, iTween.Hash("x", movePos[curPosX].x, "easeType", "easeOutCubic", "time", .1f));
        iTween.MoveTo(Camera.main.gameObject, iTween.Hash("x", movePos[curPosX].x, "easeType", "linear", "time", .12f));
        gameMain.camCurrentPos.x = movePos[curPosX].x;
        gameMain.playerSwipe();
	}

	void forceJump(){
//		rigidbody.velocity = new Vector3(0, 10, 0);
		GetComponent<Rigidbody>().AddForce(Vector3.up * jumpForce);
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
        //iTween.MoveTo(character, iTween.Hash("y", characterPos.y - dodgeOffset, "easeType", "easeInOutExpo", "loopType", "none", "time", .3));
        yield return new WaitForSeconds (.5f);
        //iTween.MoveTo(character, iTween.Hash("y", characterPos.y, "easeType", "easeInOutExpo", "loopType", "none", "time", .3));
		character.transform.localScale += new Vector3 (0, .3f, 0);
    }

    //When the floor has smashed, player fall down
    void OnCollisionStay(Collision other) {
        if ((other.collider.name.Contains("Road")) || (other.collider.name.Contains("Sidewalk"))) {
            onTheGround = true;
        }
    }

	void OnTriggerEnter(Collider other){
		if(other.CompareTag("SegmentKill")){
			Destroy (gameObject);
			gameMain.GameOver();
		}
	}
}
