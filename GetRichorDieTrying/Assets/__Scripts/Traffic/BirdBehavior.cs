using UnityEngine;
using System.Collections;

public class BirdBehavior : MonoBehaviour {
	public GameManager			gameMain; //Singleton Ref to GameManager Script
	
	public GameObject character;
	public GameObject bird;
	//	public Vector3 birdPos;
	public enum birdState {Wait, Fly, Hit, Smash, Pass};
	public birdState birdAction;
	bool startFly = false;
	
	public float startDis = 5f;
	public float flyDis = -10f;
	public float flySec = 10f;
	
	public AudioClip[] birdJump;
	public AudioClip birdSmash;
	public AudioClip birdHit;
	bool             hasPlayed = false; // Whether birdSmash has played
	int				 birdType = 0;
	
	// Use this for initialization
	void Start () {
		gameMain = GameManager.manager; //Set Ref to GameManager Script
		character = GameObject.FindGameObjectWithTag("Character");
		bird = this.gameObject;
		birdAction = birdState.Wait;
		startDis = Random.Range (40f,100f);
		if (bird.transform.position.x < 0) {
			flyDis = 10f;
		}
		birdType = Random.Range (0, 2);
	}
	
	// Update is called once per frame
	void Update () {
//		float dist = Vector3.Distance(character.transform.position, bird.transform.position);
//				Debug.Log("Distance to other: " + dist);
//		if (dist <= startDis && !startFly) {
		if (!startFly) {
			birdAction = birdState.Fly;
//			fly();
			iTween.MoveBy(bird, iTween.Hash("x", flyDis, "easeType", "easeInOutExpo", "time", flySec));
//			iTween.MoveBy(bird, iTween.Hash("y", -.2f, "easeType", "linearTween","loopType", "pingPong ", "time", 1));
			startFly = true;
		}
//		if (bird.transform.position.x <= .5f || bird.transform.position.x >= -.5f ) {
//			gameMain.audioSource.PlayOneShot (birdJump[birdType]);
//		}
		if (bird.transform.position.x < 2 || bird.transform.position.x > -2) {
			bird.transform.Translate(Vector3.down*Time.deltaTime*0.2f);
//			iTween.MoveTo(bird, iTween.Hash("y", 1, "easeType", "easeInOutExpo", "time", .2f));
		}
		if (bird.transform.position.x > 2 || bird.transform.position.x < -2) {
//			iTween.MoveTo(bird, iTween.Hash("y", 2.5, "easeType", "easeInOutExpo", "time", .2f));
		}

		if (bird.transform.position.z < character.transform.position.z){
			birdAction = birdState.Pass;
		}
		
		switch(birdAction) {
		case birdState.Wait:
			//			Debug.Log("birdWait");
			break;
		case birdState.Fly:
			//			Debug.Log("birdFly");
			break;
		case birdState.Hit:
			//			Debug.Log("birdHit");
			birdDead();
			break;
		case birdState.Smash:
			//play death animation;
			birdDead();
			break;
		case birdState.Pass:
			//			Debug.Log("birdPass");
			Destroy (gameObject);
			break;
		}
	}
	
	public void fly(){
		iTween.MoveBy(bird, iTween.Hash("x", flyDis, "easeType", "easeInOutExpo", "time", flySec));
		iTween.MoveBy(bird, iTween.Hash("y", -.2f, "easeType", "linearTween","loopType", "pingPong ", "time", 1));
	}
	
	void OnCollisionEnter(Collision other) {
		if(other.collider.tag == "Character"){
			birdAction = birdState.Hit;
			Debug.Log("hit bird!");
			gameMain.audioSource.PlayOneShot (birdJump[birdType]);
			
			//TODO end game when obstacle hit
			gameMain.GameOver ();
		} else {
			birdAction = birdState.Hit;
			gameMain.audioSource.PlayOneShot (birdJump[birdType]);
		}
	}

	void OnTriggerEnter(Collider other) {
		if (other.GetComponent<Collider>().tag == "BirdTrigger") {
			gameMain.audioSource.PlayOneShot (birdJump[birdType]);
		}
	}

	void birdDead(){
		if (!hasPlayed) {
			gameMain.audioSource.PlayOneShot (birdJump[birdType]);
			hasPlayed = true;
		}
		Destroy (gameObject);
	}
}