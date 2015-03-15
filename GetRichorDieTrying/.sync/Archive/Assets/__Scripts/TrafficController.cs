using UnityEngine;
using System.Collections;

public class TrafficController : MonoBehaviour {

	public GameManager					gameMain;

	public GameObject					car;

	public GameObject					human;

	public float						humanSpawnTimer = 20.0f;
	public float						carSpawnTimer = 10.0f;

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

	public GameObject[]					animals;
	public GameObject[]					animalPoint;
	public float                     	animalSpawnTimer;
	public int							animalNum;
	public bool							spawnAnimal = false;


	// Use this for initialization
	void Start () {

		lLSpawnPos = leftLaneSpawn.transform.position;
		rLSpawnPos = rightLaneSpawn.transform.position;

		upStreetPos = upStreetSpawn.transform.position;
		downStreetPos = downStreetSpawn.transform.position;

		animalSpawnTimer = Random.Range (2f, 10f);
		animalNum = animals.Length;
	}
	
	// Update is called once per frame
	void Update () {

		humanSpawnTimer -= 1 * Time.deltaTime;

		carSpawnTimer -= 1 * Time.deltaTime;

		animalSpawnTimer -= 1 * Time.deltaTime;

		//[TODO]Spawn Human at top or bottom of street every 20 seconds
		if(humanSpawnTimer <= 0){
//			Debug.Log ("Human Timer Up");
			spawnHuman = true;
			humanSpawnTimer = 20.0f;
			Human ();
		}

		//[TODO] Spawn Car in left lane every 5 seconds (or don't spawn)
		if(carSpawnTimer <= 0){
			spawnCar = true;
			carSpawnTimer = Random.Range(10f, 20f);
			Car ();
//			Debug.Log ("Car timer up. Next timer: " + carSpawnTimer);
		}

		//[TODO] Spawn Animal
		if(animalSpawnTimer <= 0){
			spawnAnimal = true;
			animalSpawnTimer = Random.Range(5f, 10f);
			Animal ();
//			Debug.Log ("Animal timer up. Next timer: " + animalSpawnTimer);
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
			int j = Random.Range(0, animalPoint.Length); //which positioin
			GameObject newAnimal = (GameObject)Instantiate(animals[i], animalPoint[j].transform.position, Quaternion.identity);
			newAnimal.name = animals[i].name;
			spawnAnimal = false;
		}

	}
}
