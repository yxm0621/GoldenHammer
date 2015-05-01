using UnityEngine;
using System.Collections;

public class ObstacleMoving : MonoBehaviour {
    public float moveSpeed;

	// Use this for initialization
	void Start () {
        moveSpeed = GameManager.manager.moveSpeed;
	}
	
	// Update is called once per frame
	void Update () {
        //moving obstacles with the segments
        if (gameObject.name.Contains("PoliceCarL") || gameObject.name.Contains("TankL") ||
                    gameObject.name.Contains("Human_")) {
            gameObject.transform.Translate(new Vector3(0, 0, 1) * Time.deltaTime * moveSpeed);
        } else if (gameObject.name.Contains("PoliceCarR") || gameObject.name.Contains("TankR")) {
            gameObject.transform.Translate(new Vector3(1, 0, 0) * Time.deltaTime * moveSpeed);
        }  else  {
            //gameObject.transform.Translate(new Vector3(0, 0, -1) * Time.deltaTime * moveSpeed);
        }

        //delete obstacles at the end of the road
        if (gameObject.transform.position.z <= -3) {
            Destroy(gameObject);
        }
	}
}
