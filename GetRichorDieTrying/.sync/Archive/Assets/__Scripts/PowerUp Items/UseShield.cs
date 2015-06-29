using UnityEngine;
using System.Collections;

public class UseShield : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnCollisionEnter(Collision other){
        //if(!(other.gameObject.CompareTag("Character"))
        //    &&!(other.gameObject.name.Contains("damage"))
        //    && !(other.gameObject.name.Contains("spinCutter"))
        //    && !(other.gameObject.name.Contains("PowerUp"))
        //    && !(other.gameObject.name.Contains("Coin"))) {
        if(other.gameObject.CompareTag("Obstacle")) {
            Destroy(other.gameObject);
            gameObject.SetActive(false);
            GameManager.manager.itemTimer = 0f;
		}
	}
}
