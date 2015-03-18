using UnityEngine;
using System.Collections;

public class Coins : MonoBehaviour {

	public GameObject				coin;

	public float					timer = 99;

	// Use this for initialization
	void Start () {
		coin = this.gameObject;

		iTween.Init (coin);


		timer = 3;
	}
	
	// Update is called once per frame
	void Update () {

		timer -= 1 * Time.deltaTime;

		if(timer <= 1){
			//Flashing coins
//			iTween.ColorFrom (coin, iTween.Hash ("a", 0, "time", 3));

			//Debug.Log ("changing coin alpha");
		}
		//destroy coins
		if(timer <= 0){
			Debug.Log ("coin destroyed");
			Destroy (coin);
		}
	}

//	void OnCollisionEnter(Collision other) {
//		if(other.collider.tag == "Character"){
//			coin.transform.parent.GetComponent<CoinsGroup>().PickUp();
//		}
//	}
}
