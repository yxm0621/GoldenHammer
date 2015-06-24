using UnityEngine;
using System.Collections;

public class CoinsGroup : MonoBehaviour {
	public int value;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if (gameObject.transform.childCount == 0) {
            Destroy(gameObject);
        }
	}

	public void PickUp () {
//		GameManager.manager.addScore (value);
	}
}
