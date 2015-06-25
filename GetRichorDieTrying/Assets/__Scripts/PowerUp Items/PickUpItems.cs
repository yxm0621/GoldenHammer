using UnityEngine;
using System.Collections;

public class PickUpItems : MonoBehaviour {
    GameManager             gameMain;

	// Use this for initialization
	void Start () {
        gameMain = GameManager.manager;
	}
	
    void OnTriggerEnter(Collider other) {
        if(other.gameObject.CompareTag("Character")){
            if (gameObject.name.Contains("Saw")) {
                gameMain.UseSaw();
            }
            if (gameObject.name.Contains("Bomb")) {
                gameMain.UseBomb();
            }
            if (gameObject.name.Contains("Shield")) {
                gameMain.shield.SetActive(true);
            }
            if (gameObject.name.Contains("Clock")) {
                gameMain.isClock = true;
                gameMain.traffic.trafficStop = true;
            }

            Destroy(gameObject);
		}
    }
}