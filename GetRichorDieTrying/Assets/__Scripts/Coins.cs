using UnityEngine;
using System.Collections;

public class Coins : MonoBehaviour {

	public GameObject				coin;

	public float					timer = 1.5f;

	// Use this for initialization
	void Start () {
		coin = this.gameObject;
		iTween.Init (coin);
		timer = Random.Range(.1f, 1.5f);
	}
	
	// Update is called once per frame
	void Update () {
		timer -= 1 * Time.deltaTime;

        //Flashing coins
        //if(timer <= 1){
//			iTween.ColorFrom (coin, iTween.Hash ("a", 0, "time", 3));
			//Debug.Log ("changing coin alpha");
        //}

		//destroy coins
		if(timer <= 0){
            //Debug.Log ("coin destroyed");
            Destroy(gameObject);
		}

        if (gameObject.transform.position.z < GameManager.manager.characterPos.z) {
            Destroy(gameObject);
        }
	}

//	void OnCollisionEnter(Collision other) {
//		if(other.collider.tag == "Character"){
//			coin.transform.parent.GetComponent<CoinsGroup>().PickUp();
//		}
//	}
}
