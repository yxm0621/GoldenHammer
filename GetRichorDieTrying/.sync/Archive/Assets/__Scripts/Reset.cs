using UnityEngine;
using System.Collections;

public class Reset : MonoBehaviour {

	public GameObject			button;

	// Use this for initialization
	void Start () {
		button = this.gameObject;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnMouseDown(){
		Debug.Log ("Restarting Level");
		Application.LoadLevel ("1st");
	}
}
