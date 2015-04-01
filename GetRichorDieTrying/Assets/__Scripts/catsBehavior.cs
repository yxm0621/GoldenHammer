using UnityEngine;
using System.Collections;

public class CatsBehavior : MonoBehaviour {
	public GameManager			gameMain; //Singleton Ref to GameManager Script

	public GameObject character;
	public GameObject cat;
	public enum CatState {Wait, Walk, Hit, Smash, Pass};
	public CatState catAction;
	bool startWalk = false;

	public float startDis = 5f;
	public float firstWalkDis = -.7f;
	public float secondWalkDis = -3.4f;
	public float firstWalkSec = 1f;
	public float secondWalkSec = 5f;
	public float waitSec = 1f;

	public AudioClip catJump;
	public AudioClip catSmash;
	public AudioClip catHit;
	bool             hasPlayed = false; // Whether catSmash has played

	// Use this for initialization
	void Start () {
		gameMain = GameManager.manager; //Set Ref to GameManager Script
		character = GameObject.FindGameObjectWithTag("Character");
		cat = this.gameObject;
		catAction = CatState.Wait;
		startDis = Random.Range (4f,7f);
		if (cat.transform.position.x < 0) {
			firstWalkDis = .7f;
			secondWalkDis = 3.4f;
		}
	}
	
	// Update is called once per frame
	void Update () {
		float dist = Vector3.Distance(character.transform.position, cat.transform.position);
//		Debug.Log("Distance to other: " + dist);
		if (dist <= startDis && !startWalk) {
			catAction = CatState.Walk;
			StartCoroutine(walk());
			startWalk = true;
			gameMain.audioSource.PlayOneShot (catJump);
		}
		if (cat.transform.position.z < character.transform.position.z){
			catAction = CatState.Pass;
		}

		switch(catAction) {
		case CatState.Wait:
//			Debug.Log("catWait");
			break;
		case CatState.Walk:
//			Debug.Log("catWalk");
			break;
		case CatState.Hit:
//			Debug.Log("catHit");
			break;
		case CatState.Smash:
			//play death animation;
			catDead();
			break;
		case CatState.Pass:
//			Debug.Log("catPass");
			break;
		}
	}

	public IEnumerator walk(){
//		Debug.Log("startWalk");
		iTween.MoveBy(cat, iTween.Hash("x", firstWalkDis, "easeType", "easeInOutExpo", "time", firstWalkSec));
//		Debug.Log("stopWalk");
		yield return new WaitForSeconds (waitSec);
//		Debug.Log("startWalkAgain");
		iTween.MoveBy(cat, iTween.Hash ("x", secondWalkDis, "easeType", "easeOutSine", "time", secondWalkSec));
	}

	void OnCollisionEnter(Collision other) {
		if(other.collider.tag == "Character"){
			catAction = CatState.Hit;
			Debug.Log("hit cat!");
			gameMain.audioSource.PlayOneShot (catHit);

			//TODO end game when obstacle hit
			gameMain.GameOver ();
		}
		if(other.collider.name.Contains("car") || other.collider.name.Contains("Police") || other.collider.name.Contains("Tank")){
			catAction = CatState.Smash;
			gameMain.audioSource.PlayOneShot (catHit);
		}
	}

	void catDead(){
		if (!hasPlayed) {
			gameMain.audioSource.PlayOneShot (catSmash);
			hasPlayed = true;
		}
		Destroy (gameObject);
	}
}