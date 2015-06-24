using UnityEngine;
using System.Collections;

public class DestroyObjs : MonoBehaviour {
    //bool isDestroy = true;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnCollisionStay(Collision other) {
        if (other.gameObject.name.Contains("Sidewalk")||
            other.gameObject.name.Contains("Road") ||
            other.gameObject.name.Contains("Ground") ||
            other.gameObject.name.Contains("spawnPoint") ||
            other.gameObject.name.Contains("checkPoint") ||
            other.gameObject.name.Contains("Booth") ||
            other.gameObject.name.Contains("spin")||
            other.gameObject.name.Contains("damage")) {
                //isDestroy = false;
        } else {
            Destroy(other.gameObject);
        }
    }
}
