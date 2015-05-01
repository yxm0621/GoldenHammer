using UnityEngine;
using System.Collections;

public class Moving : MonoBehaviour {
    float moveSpeed;
	// Use this for initialization
	void Start () {
        moveSpeed = GameManager.manager.moveSpeed;
	}
	
	// Update is called once per frame
	void Update () {
        gameObject.transform.Translate(new Vector3(0, 0, -1) * Time.deltaTime * moveSpeed);
	}
}
