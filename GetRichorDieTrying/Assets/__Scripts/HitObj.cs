using UnityEngine;
using System.Collections;

public class HitObj : MonoBehaviour {
    GameManager gameMain;
    Vector3 objPos;

	// Use this for initialization
	void Start () {
        gameMain = GameManager.manager;
        objPos = gameObject.transform.position;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnMouseDown() {
        gameObject.transform.position += new Vector3(0, .2f, -.2f);
        iTween.MoveTo(gameObject, iTween.Hash("position", objPos, "time", .5f));
        //gameMain.objectName = gameObject.name;
        //gameMain.coinPos = gameObject.transform.position;
        //gameMain.CashOut(0);
        //Destroy(gameObject);
    }
}
