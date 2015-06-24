using UnityEngine;
using System.Collections;

public class HideDetail : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        //if (Input.GetMouseButtonDown(0))
        //    Destroy(gameObject);
            //Debug.Log("Pressed left click.");
	}

    void OnMouseDown() {
        Destroy(gameObject);
    }
}
