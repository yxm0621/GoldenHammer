using UnityEngine;
using System.Collections;

public class MyFirstTouch : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	//Subscribe to events
	void OnEnable(){
		EasyTouch.On_TouchStart += On_TouchStart;
	}

	//Unsubscribe
	void Disable(){
		EasyTouch.On_TouchStart -= On_TouchStart;
	}

	//Destroy
	void OnDestroy(){
		EasyTouch.On_TouchStart -= On_TouchStart;
	}

	//Touch Start Event
	public void On_TouchStart(Gesture gesture){
		Debug.Log ("Touch in" + gesture.position);
	}

}

