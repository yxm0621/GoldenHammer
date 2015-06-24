using UnityEngine;
using System.Collections;

public class ItemMenu : MonoBehaviour {
    public GameObject itemMenu;
    bool isShowing = false;

	// Use this for initialization
	void Start () {
        itemMenu = gameObject.transform.FindChild("item").gameObject;
        itemMenu.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () {
        if (GameManager.manager.gameState == GameManager.State.InGame) {
            if (!isShowing) {
                itemMenu.SetActive(true);
                isShowing = true;
            }
        }
	}
}
