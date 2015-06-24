using UnityEngine;
using System.Collections;

public class AnimalBehavior : MonoBehaviour {
    public GameManager gameMain; //Singleton Ref to GameManager Script
    public GameObject character;
    public GameObject animal;

    public float waitTime;
    public char direction;
    public bool walk;
    public bool jump = false;
    public float walkTime = 1f;
    public float destination = -1f;
    public float animalSpeed;
    public float walkDis;
    public bool toRight;

    public AudioClip animalAudio;
    public AudioClip animalSmash;
    public AudioClip animalHit;
    bool hasPlayed = false; // Whether animalSmash has played

	// Use this for initialization
	void Start () {
        gameMain = GameManager.manager; //Set Ref to GameManager Script
        character = GameObject.FindGameObjectWithTag("Character");
        animal = this.gameObject;
        walk = false;
	}
	
	// Update is called once per frame
	void Update () {
        if (character != null) {
            if (animal.transform.position.z - character.transform.position.z < -2f) {
                Destroy(gameObject);
            }
        }
        if (gameMain.gameState == GameManager.State.InGame) {
            if (!gameMain.traffic.trafficStop) {
                waitTime -= Time.deltaTime;
            }
            //if ((waitTime <= 0) && (!walk)) {
            if ((waitTime <= 1) && (direction == 'x') && (!jump)) {
                GetComponent<Rigidbody>().AddForce(Vector3.up * 100);
                jump = true;
            }

            if (waitTime <= 0) {
                if (direction == 'z') {
                    iTween.MoveTo(gameObject, iTween.Hash("z", destination, "islocal", true, "time", walkTime));
                }
                if (direction == 'x') {
                    //iTween.MoveBy(gameObject, iTween.Hash("x", -.2f, "islocal", true, "time", 3,
                    //    "oncomplete", "Move", "oncompletetarget", gameObject));
                    if (gameMain.traffic.trafficStop) {
                        gameObject.transform.Translate(Vector3.right * Time.deltaTime * animalSpeed);
                    }
                }
                walk = true;
            }
        }
	}

    public void SetAnimal(float time, char axis, float animTime, float dest) {
        waitTime = time;
        direction = axis;
        walkTime = animTime;
        destination = dest;
        if (dest < 0) {
            walkDis = destination - gameObject.transform.position.x;
            toRight = true;
        } else {
            walkDis = gameObject.transform.position.x - destination;
            toRight = false;
        }

        animalSpeed = walkDis / walkTime;
    }

    void Move() {
        iTween.MoveTo(gameObject, iTween.Hash("x", destination, "islocal", true, "time", walkTime));
    }

    void OnCollisionEnter(Collision other) {
        if (other.collider.CompareTag("Character")) {
            //Debug.Log("hit animal!");
            gameMain.audioSource.PlayOneShot(animalHit);

            //TODO end game when obstacle hit
            gameMain.GameOver();
        }

        if (other.collider.name.Contains("Car") || other.collider.name.Contains("Tank")) {
            gameMain.audioSource.PlayOneShot(animalHit);
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter(Collider other){
        if(other.GetComponent<Collider>().CompareTag("SegmentKill")){
            //kill the animal
            animal.GetComponent<ObjectManager>().hitPoints = 0;
		}
	}

    public void AnimalDead() {
        if (!hasPlayed)
        {
            gameMain.audioSource.PlayOneShot(animalSmash);
            hasPlayed = true;
        }
        Destroy(gameObject);
    }
}
