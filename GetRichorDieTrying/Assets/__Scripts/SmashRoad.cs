using UnityEngine;
using System.Collections;

public class SmashRoad : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnMouseDown()
    {
        Debug.Log("Smash Road!!");
        //if (GameManager.gameState == GameManager.State.InGame)
        //{
        //    GameObject.Find("Hammer").GetComponent<HammerBehavior>().hammerSmash(gameObject.transform.position);
        //    iTween.MoveFrom(gameObject, iTween.Hash("z", gameObject.transform.position.z + 0.25f, "y", gameObject.transform.position.y + -0.25f, "time", 0.5f));
        //}
    }
}
