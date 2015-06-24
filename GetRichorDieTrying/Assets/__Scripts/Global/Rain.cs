using UnityEngine;
using System.Collections;

public class Rain : MonoBehaviour {
    public float            minTimer = .1f;
    public float            maxTimer = 1f;
    float                   timer;
    bool                    timerStart;

	// Use this for initialization
	void Start () {
        timer = Random.Range(minTimer, maxTimer);
        timerStart = false;
	}
	
	// Update is called once per frame
	void Update () {
        if (timerStart) {
            timer -= Time.deltaTime;
        }
        if (timer <= 0) {
            Destroy(gameObject);
        }
	}

    void OnCollisionEnter(Collision other){
        timerStart = true;
	}
}
