using UnityEngine;
using System.Collections;

public class HumanBehavior : MonoBehaviour {

	public GameManager				gameMain;

	public GameObject				human;

	public float					movementSpeed = 200.0f; //20 Seconds from end to end

	public GameObject				character;

	public GameObject				downPos; //Down the street
	public GameObject				upPos; //Up the street

	public bool						startedUp;
	public bool						startedDown;

	// Use this for initialization
	void Start () {
	
		gameMain = GameManager.manager;

		human = this.gameObject;
		iTween.Init (human);

		if(human.transform.position.z < 0){
			startedDown = true;
			Debug.Log ("Started Down");
			iTween.MoveTo(human, iTween.Hash ("z", upPos.transform.position.z, "time", movementSpeed, "easetype", iTween.EaseType.linear));
		}
		if(human.transform.position.z > 0){
			startedUp = true;
			Debug.Log ("Started Down");
			iTween.MoveTo (human, iTween.Hash ("z", downPos.transform.position.z, "time", movementSpeed, "easetype", iTween.EaseType.linear));
		}
	}
	
	// Update is called once per frame
	void Update () {



	}

	void OnTriggerEnter(){



		if(startedDown){ //Checks which side game object started from and destroys it at end

				Debug.Log ("At end");
				Destroy (human);
		}
		if(startedUp){ //Checks which side game object started from and destroys it at end
			Debug.Log ("At end");
			Destroy (human);
		}
	}
}
