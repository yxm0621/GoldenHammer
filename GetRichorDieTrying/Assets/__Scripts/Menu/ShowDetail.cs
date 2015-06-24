using UnityEngine;
using System.Collections;

public class ShowDetail : MonoBehaviour {
    public GameManager gameMain;
    public GameObject menuFrame;
    public GameObject obj;

	// Use this for initialization
	void Start () {
        gameMain = GameManager.manager;
	}
	
	// Update is called once per frame
	void Update () {
        //Vector3 p = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 1));
	
	}

    public void OnMouseDown() {
        //menuFrame.GetComponent<MenuFrame>().CreateMenu();
        if (gameMain.itemDetails == null) {
            obj = Instantiate(menuFrame, menuFrame.transform.position, Quaternion.identity) as GameObject;
            obj.name = menuFrame.name;
            obj.transform.parent = gameObject.transform.parent.parent;
            gameMain.itemDetails = obj;
        }
    }
}
