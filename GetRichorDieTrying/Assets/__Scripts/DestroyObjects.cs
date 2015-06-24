using UnityEngine;
using System.Collections;

public class DestroyObjects : MonoBehaviour {
    public float maxDistance;
    GameManager gameMain;
    Vector3 characterPos;

	// Use this for initialization
	void Start () {
        gameMain = GameManager.manager;
	}
	
	// Update is called once per frame
	void Update () {
        if (gameMain.gameState == GameManager.State.InGame) {
            characterPos = GameManager.manager.characterPos;
            if (characterPos.z - gameObject.transform.position.z >= maxDistance) {
                Destroy(gameObject);
            }
        }
	}
}
