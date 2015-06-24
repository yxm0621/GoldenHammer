using UnityEngine;
using System.Collections;

public class PlanetManager : MonoBehaviour {

	// Use this for initialization
	void Start () {
        randomPos();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void randomPos() {
        gameObject.transform.position += new Vector3(0, Random.Range(-5f, 5f), 0);
        if ((gameObject.transform.position.y >= -1) && (gameObject.transform.position.y <= 1)) {
            randomPos();
        }
    }
}
