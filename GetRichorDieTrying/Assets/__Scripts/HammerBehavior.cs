using UnityEngine;
using System.Collections;

public class HammerBehavior : MonoBehaviour {
    public GameManager gameMain;

	// Use this for initialization
	void Start () {
        gameMain = GameManager.manager;
	}
	
	// Update is called once per frame
	void Update () {
        if ((gameObject.transform.eulerAngles.z == 0)&&(gameObject.transform.position != gameMain.hammerPos)) {
            iTween.MoveTo(gameObject, gameMain.hammerPos, .1f);
        }
	}

    //Hammer smash
    public IEnumerator Movement(Vector3 pos) {
        iTween.MoveTo(gameObject, pos + new Vector3(.5f, 1, 0), .01f);
        gameObject.transform.localEulerAngles = new Vector3(0, gameMain.smashAngle, 90);
        iTween.RotateTo(gameObject, iTween.Hash("z", 0, "time", 0.2f));
        yield return new WaitForSeconds(.2f);
        iTween.MoveTo(gameObject, gameMain.hammerPos, .01f);
    }
}
