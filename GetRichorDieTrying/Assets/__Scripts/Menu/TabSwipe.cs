using UnityEngine;
using System.Collections;

public class TabSwipe : MonoBehaviour {

    public Swipe swipeDirection;

    public float minSwipeLength = 10;
    public float minSwipeSpeed = 100;

    public Vector2 firstPressPos; //Position of 1st press
    public Vector2 secondPressPos; //Position of 2nd press
    public Vector2 currentSwipe;
    public Vector2 currentTouch;

    private float length;
    public bool touching = false;
    public Vector3 focusSize = new Vector3(.07f, .07f, .07f);
    public Vector3 idleSize = new Vector3(.04f, .04f, .04f);

    private GameObject mostLeftTabItem;

    private GameObject[] tabs;
    private int tabIndex;

	void Start () 
    {
        tabIndex = 0;
        tabs = GameObject.FindGameObjectsWithTag("PediaTab");
	}
	
	void Update () 
    {
        CheckDirection();

        if (swipeDirection == Swipe.Left)
        {
            if (gameObject.transform.position.x > -1.5f)
            {
                //mostLeftTabItem = tabs[tabIndex];
                //tabIndex++;
                
                iTween.MoveBy(gameObject, new Vector3(-3.0f, 0, 0), .2f);
            }
        }
        if (swipeDirection == Swipe.Right)
        {
            if (gameObject.transform.position.x < 6.5f)
            {
                //tabIndex--;
                //mostLeftTabItem = tabs[tabIndex];

                iTween.MoveBy(gameObject, new Vector3(3.0f, 0, 0), .2f);
            }
        }
	}

    void CalculateDir(Vector3 first, Vector3 second)
    {
        currentSwipe = new Vector2(second.x - first.x, second.y - first.y);
        length = currentSwipe.magnitude;
        Debug.Log("touch magnitude: " + length);

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

    public void CheckDirection()
    {
        //only recognize when 1 finger touch
        if (Input.touchCount == 1)
        {
            switch (Input.touches[0].phase)
            {
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
                    if (touching && touchSpeed > minSwipeSpeed)
                    {
                        secondPressPos = new Vector2(Input.GetTouch(0).position.x, Input.GetTouch(0).position.y);
                        CalculateDir(firstPressPos, secondPressPos);
                    }
                    else
                    {
                        //when one direction has already been checked
                        swipeDirection = Swipe.None;
                        return;
                    }
                    break;
                case TouchPhase.Ended:
                    Debug.Log("touch end");
                    if (touching)
                    {
                        //when player ups the finger too fast and the "moved" state didn't have time to check the direction
                        secondPressPos = new Vector2(Input.GetTouch(0).position.x, Input.GetTouch(0).position.y);
                        CalculateDir(firstPressPos, secondPressPos);
                    }
                    else
                    {
                        swipeDirection = Swipe.None;
                        return;
                    }
                    break;
                default:
                    swipeDirection = Swipe.None;
                    break;
            }
        }
        else
        {
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
}
