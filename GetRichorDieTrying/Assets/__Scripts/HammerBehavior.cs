using UnityEngine;
using System.Collections;

public class HammerBehavior : MonoBehaviour {
    public float smashAngle = 75;
    public GameObject mouse;

	// Use this for initialization
	void Start () {
        //mouse = (GameObject)Instantiate(mouse, mouse.transform.position, mouse.transform.rotation);
	}
	
	// Update is called once per frame
    void Update () {
        //mouse.transform.position = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 1));
    }
    
    void OnDrawGizmos()
    {
        //mouse.transform.position = Input.mousePosition;
        //Camera.main.ScreenPointToRay(Input.mousePosition);
        Vector3 p = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 1));
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(p, 0.05F);
	}

    public void hammerSmash(Vector3 pos) {
        gameObject.transform.position = pos + new Vector3(.5f, 1, 0);
        gameObject.transform.localEulerAngles = new Vector3(0, smashAngle, 90);
        iTween.RotateTo(gameObject, iTween.Hash("z", 0, "time", 0.2f));
    }
}
