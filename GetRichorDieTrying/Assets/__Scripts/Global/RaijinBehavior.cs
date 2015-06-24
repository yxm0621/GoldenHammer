using UnityEngine;
using System.Collections;

public class RaijinBehavior : MonoBehaviour {
    public float minStayTimer = 2f;
    public float maxStayTimer = 10f;
    public float staytimer;
    public bool leave = false;
    public Vector3 appearPos = new Vector3(-1.5f, 1.8f, 5f);
    public Vector3 leavePos = new Vector3(-5f, 3f, 0f);

	// Use this for initialization
	void Start () {
        staytimer = Random.Range(minStayTimer, maxStayTimer);
        iTween.Init(gameObject);
        iTween.MoveTo(gameObject, iTween.Hash("position", appearPos, "islocal", true, "time", Random.Range(0f, 1f)));
	}
	
	// Update is called once per frame
	void Update () {
        staytimer -= Time.deltaTime;
        if (staytimer <= 0) {
            leave = true;
        }
        if (leave) {
            iTween.MoveTo(gameObject, iTween.Hash("position", leavePos, "islocal", true, "time", 1f));
            leave = false;
        }
        if (gameObject.transform.position.x < -3) {
            Destroy(gameObject);
            GameObject.Find("GlobalObjects").GetComponent<GlobalObjects>().Sunny();
        }
	}
}
