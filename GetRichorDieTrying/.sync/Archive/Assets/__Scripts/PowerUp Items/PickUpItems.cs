using UnityEngine;
using System.Collections;

public class PickUpItems : MonoBehaviour {
    GameManager             gameMain;
    bool                    triggerEffect = false;

	// Use this for initialization
	void Start () {
        gameMain = GameManager.manager;
	}
	
	// Update is called once per frame
	void Update () {
        
	}

    void OnCollisionEnter(Collision other) {
		if(other.collider.CompareTag("Character")){
            triggerEffect = true;

            if (gameObject.name.Contains("Saw")) {
                gameMain.UseSaw();
            }
            if (gameObject.name.Contains("Bomb")) {
                gameMain.UseBomb();
            }
            if (gameObject.name.Contains("PowerHammer")) {
                if ((!gameMain.isClock) && (!gameMain.isPowerhammer) && (!gameMain.isShield)) {
                    gameMain.isPowerhammer = true;
                }
            }
            if (gameObject.name.Contains("Shield")) {
                if ((!gameMain.isClock) && (!gameMain.isPowerhammer) && (!gameMain.isShield)) {
                    gameMain.isShield = true;
                    gameMain.shield.SetActive(true);
                }
            }
            if (gameObject.name.Contains("Clock")) {
                if ((!gameMain.isClock) && (!gameMain.isPowerhammer) && (!gameMain.isShield)) {
                    gameMain.isClock = true;
                    gameMain.traffic.trafficStop = true;
                }
            }

            Destroy(gameObject);
		}
	}
}
