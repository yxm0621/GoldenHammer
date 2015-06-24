using UnityEngine;
using System.Collections;

public class RainClouds : MonoBehaviour {
    public float minTimer = .1f;
    public float maxTimer = 1f;
    float timer;
    bool isRaining;

	// Use this for initialization
	void Start () {
        timer = Random.Range(minTimer, maxTimer);
	}
	
	// Update is called once per frame
	void Update () {
        isRaining = GameObject.Find("GlobalObjects").GetComponent<GlobalObjects>().isRaining;
        if (!isRaining) {
            timer -= Time.deltaTime;
        }
        if (timer <= 0) {
            Destroy(gameObject);
        }
	}
}
