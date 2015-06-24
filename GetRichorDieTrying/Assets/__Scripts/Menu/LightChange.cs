using UnityEngine;
using System.Collections;

public class LightChange : MonoBehaviour {
    public GameManager gameMain;

	// Use this for initialization
	void Start () {
        gameMain = GameManager.manager;
	}
	
	// Update is called once per frame
	void Update () {
        if (gameMain.currentMenu == GameManager.MenuPage.Main) {
            if (gameObject.GetComponent<Light>().intensity == .5f) {
                gameObject.GetComponent<Light>().intensity = 1f;
            }
        } else if (gameMain.currentMenu == GameManager.MenuPage.Store) {
            if (gameObject.GetComponent<Light>().intensity == 1f) {
                gameObject.GetComponent<Light>().intensity = .5f;
            }
        }
	}
}
