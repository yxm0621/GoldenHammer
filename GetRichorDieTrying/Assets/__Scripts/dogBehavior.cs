using UnityEngine;
using System.Collections;

public class DogBehavior : MonoBehaviour {
	public GameManager			gameMain; //Singleton Ref to GameManager Script
	
	public GameObject character;
	public GameObject dog;
	//	public Vector3 dogPos;
	public enum dogState {Wait, Walk, Hit, Smash, Pass};
	public dogState dogAction;
	bool startWalk = false;
	
	public float startDis = 5f;
	public float firstWalkDis = -.7f;
	public float secondWalkDis = -3.4f;
	public float firstWalkSec = 1f;
	public float secondWalkSec = 5f;
	public float waitSec = 1f;
	
	public AudioClip dogJump;
	public AudioClip dogSmash;
	public AudioClip dogHit;
	bool             hasPlayed = false; // Whether dogSmash has played
	
	// Use this for initialization
	void Start () {
		gameMain = GameManager.manager; //Set Ref to GameManager Script
		character = GameObject.FindGameObjectWithTag("Character");
		dog = this.gameObject;
		dogAction = dogState.Wait;
		startDis = Random.Range (4f,7f);
		if (dog.transform.position.x < 0) {
			firstWalkDis = .7f;
			secondWalkDis = 3.4f;
		}
	}
	
	// Update is called once per frame
	void Update () {
		float dist = Vector3.Distance(character.transform.position, dog.transform.position);
		//		Debug.Log("Distance to other: " + dist);
		if (dist <= startDis && !startWalk) {
			dogAction = dogState.Walk;
			StartCoroutine(walk());
			startWalk = true;
			gameMain.audioSource.PlayOneShot (dogJump);
		}
		if (dog.transform.position.z < character.transform.position.z){
			dogAction = dogState.Pass;
		}
		
		switch(dogAction) {
		case dogState.Wait:
			//			Debug.Log("dogWait");
			break;
		case dogState.Walk:
			//			Debug.Log("dogWalk");
			break;
		case dogState.Hit:
			//			Debug.Log("dogHit");
			break;
		case dogState.Smash:
			//play death animation;
			dogDead();
			break;
		case dogState.Pass:
			//			Debug.Log("dogPass");
			break;
		}
	}
	
	public IEnumerator walk(){
		//		Debug.Log("startWalk");
		iTween.MoveBy(dog, iTween.Hash("x", firstWalkDis, "easeType", "easeInOutExpo", "time", firstWalkSec));
		//		Debug.Log("stopWalk");
		yield return new WaitForSeconds (waitSec);
		//		Debug.Log("startWalkAgain");
		iTween.MoveBy(dog, iTween.Hash ("x", secondWalkDis, "easeType", "easeOutSine", "time", secondWalkSec));
	}
	
	void OnCollisionEnter(Collision other) {
		if(other.collider.tag == "Character"){
			dogAction = dogState.Hit;
			Debug.Log("hit dog!");
			gameMain.audioSource.PlayOneShot (dogHit);
			
			//TODO end game when obstacle hit
			gameMain.GameOver ();
		}
		if(other.collider.name.Contains("car") || other.collider.name.Contains("Police") || other.collider.name.Contains("Tank")){
			dogAction = dogState.Smash;
			gameMain.audioSource.PlayOneShot (dogHit);
		}
	}
	
	void dogDead(){
		if (!hasPlayed) {
			gameMain.audioSource.PlayOneShot (dogSmash);
			hasPlayed = true;
		}
		Destroy (gameObject);
	}
}