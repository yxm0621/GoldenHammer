using UnityEngine;
using System.Collections;

public class UseItems : MonoBehaviour {
    public GameManager gameMain;
    public bool showList = false;
    public GameObject itemList;
    bool isChanging = false;

    // Use this for initialization
    void Start() {
        gameMain = GameManager.manager;
        if (gameObject.name.Contains("item")) {
            itemList = gameObject.transform.FindChild("itemList").gameObject;
            itemList.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update() {
        if (isChanging) {
            if (showList) {
                itemList.SetActive(true);
                isChanging = false;
            } else {
                itemList.SetActive(false);
                isChanging = false;
            }
        }
    }
    void OnMouseDown() {
        if (gameObject.name.Contains("item")) {
            showList = !showList;
            isChanging = true;
        }
        if (gameObject.name.Contains("Saw")) {
            gameMain.UseSaw();
        }
        if (gameObject.name.Contains("Bomb")) {
            gameMain.UseBomb();
        }
        //if (gameObject.name.Contains("PowerHammer")) {
        //    if ((!gameMain.isClock) && (!gameMain.isPowerhammer) && (!gameMain.isShield)) {
        //        gameMain.isPowerhammer = true;
        //    }
        //}
        //if (gameObject.name.Contains("Shield")) {
        //    if ((!gameMain.isClock) && (!gameMain.isPowerhammer) && (!gameMain.isShield)) {
        //        gameMain.isShield = true;
        //        gameMain.shield.SetActive(true);
        //    }
        //}
        //if (gameObject.name.Contains("Clock")) {
        //    if ((!gameMain.isClock) && (!gameMain.isPowerhammer) && (!gameMain.isShield)) {
        //        gameMain.isClock = true;
        //        gameMain.traffic.trafficStop = true;
        //    }
        //}
        
    }
}
