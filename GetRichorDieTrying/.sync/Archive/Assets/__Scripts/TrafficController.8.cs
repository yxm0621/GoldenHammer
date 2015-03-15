using UnityEngine;
using System.Collections;

public class TrafficController : MonoBehaviour {

	public GameManager					gameMain;

	public GameObject					car;

	public GameObject					human;

	public float						humanSpawnTimer = 20.0f;
	public float						carSpawnTimer = 10.0f;

	public float						carTimeLow = 10.0f;
	public float						carTimeHigh = 20.0f;

	public bool							spawnHuman = false;
	public bool							spawnCar = false;

	public GameObject					leftLaneSpawn;
	public GameObject					rightLaneSpawn;

	public GameObject					upStreetSpawn;
	public GameObject					downStreetSpawn;

	public Vector3						lLSpawnPos;
	public Vector3						rLSpawnPos;

	public Vector3						upStreetPos;
	public Vector3						downStreetPos;

	//animals info
	public GameObject[]					animals;
	public GameObject					animalRSpawn;
	public GameObject					animalLSpawn;
	public Vector3[]					animalPos;

	public float                     	animalSpawnTimer;
	public int							animalNum;
	public bool							spawnAnimal = false;

	//fly things info
	public GameObject[]					flying;
	public GameObject					flyRSpawn;
	public GameObject					flyLSpawn;
	public Vector3[]					flyPos;

	public float                     	flySpawnTimer;
	public int							flyNum;
	public bool							spawnFly = false;


	// Use this for initialization
	void Start () {

		lLSpawnPos = leftLaneSpawn.transform.position;
		rLSpawnPos = rightLaneSpawn.transform.position;

		upStreetPos = upStreetSpawn.transform.position;
		downStreetPos = downStreetSpawn.transform.position;

		animalPos[0] = animalRSpawn.transform.position;
		animalPos[1] = animalLSpawn.transform.position;

		flyPos[0] = flyRSpawn.transform.position;
		flyPos[1] = flyLSpawn.transform.position;

		animalSpawnTimer = Random.Range (2f, 10f);
		animalNum = animals.Length;

		flySpawnTimer = Random.Range (2f, 10f);
		flyNum = flying.Length;
	}
	
	// Update is called once per frame
	void Update () {

		if (GameManager.gameState == GameManager.State.InGame) {
			humanSpawnTimer -= 1 * Time.deltaTime;
			carSpawnTimer -= 1 * Time.deltaTime;
			animalSpawnTimer -= 1 * Time.deltaTime;
			flySpawnTimer -= 1 * Time.deltaTime;

			//[TODO]Spawn Human at top or bottom of street every 20 seconds
			if(humanSpawnTimer <= 0){
				//Debug.Log ("Human Timer Up");
				spawnHuman = true;
				humanSpawnTimer = Random.Range(10.0f, 20.0f);
				Human ();
			}

			//[TODO] Spawn Car in left lane every 5 seconds (or don't spawn)
			if(carSpawnTimer <= 0){
				spawnCar = true;
				carSpawnTimer = Random.Range(carTimeLow, carTimeHigh);
				Car ();
				//Debug.Log ("Car timer up. Next timer: " + carSpawnTimer);
			}

			//[TODO] Spawn Animal
			if(animalSpawnTimer <= 0){
				spawnAnimal = true;
				animalSpawnTimer = Random.Range(3f, 10f);
				Animal ();
				//Debug.Log ("Animal timer up. Next timer: " + animalSpawnTimer);
			}

			//[TODO] Spawn Flying things
			if(flySpawnTimer <= 0){
				spawnFly = true;
				flySpawnTimer = Random.Range(3f, 10f);
				Fly ();
			}
		}

		if(gameMain.levelCount == 2){
			humanSpawnTimer = humanSpawnTimer * 0.2f;
			carSpawnTimer = carSpawnTimer * 0.2f;
			animalSpawnTimer = animalSpawnTimer * 0.2f;
			flySpawnTimer = flySpawnTimer * 0.2f;
		}
		if(gameMain.levelCount == 3){
			humanSpawnTimer = humanSpawnTimer * 0.35f;
			carSpawnTimer = carSpawnTimer * 0.35f;
			animalSpawnTimer = animalSpawnTimer * 0.35f;
			flySpawnTimer = flySpawnTimer * 0.35f;
		}
		if(gameMain.levelCount == 4){
			humanSpawnTimer = humanSpawnTimer * 0.45f;
			carSpawnTimer = carSpawnTimer * 0.45f;
			animalSpawnTimer = animalSpawnTimer * 0.45f;
			flySpawnTimer = flySpawnTimer * 0.45f;
		}

	}

	public void Car(){
		if(spawnCar){
			Debug.Log ("Creating Cars");
			Instantiate (car, lLSpawnPos, Quaternion.identity);
			Instantiate (car, rLSpawnPos, Quaternion.identity);
			spawnCar = false;
		}


	}

	public void Human(){
		if(spawnHuman){
			Debug.Log ("Creating Humans");
			//[TODO] Spawn humans randomly up or down street
			Instantiate (human, downStreetPos, Quaternion.identity);
			spawnHuman = false;
		}
	}

	public void Animal(){
		if(spawnAnimal){
//			Debug.Log ("Creating Animal");
			//[TODO] Spawn animal
			int i = Random.Range (0, animalNum); //which type of animals
			int j = Random.Range(0, 2); //which direction
			GameObject newAnimal = (GameObject)Instantiate(animals[i], animalPos[j], Quaternion.identity);
			newAnimal.name = animals[i].name;
			spawnAnimal = false;
		}
	}

	public void Fly(){
		if(spawnFly){
			//			Debug.Log ("Creating Fly");
			//[TODO] Spawn fly things
			int i = Random.Range (0, flyNum); //which type of animals
			int j = Random.Range(0, 2); //which direction
			GameObject newFly = (GameObject)Instantiate(flying[i], flyPos[j], Quaternion.identity);
			newFly.name = flying[i].name;
			spawnFly = false;
		}
	}
}
