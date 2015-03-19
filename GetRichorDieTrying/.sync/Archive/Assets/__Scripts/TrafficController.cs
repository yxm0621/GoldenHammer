using UnityEngine;
using System.Collections;

public class TrafficController : MonoBehaviour {

	public GameManager					gameMain;

	public GameObject					car;

	public GameObject					human;
<<<<<<< HEAD
=======
	public GameObject[]					people;
>>>>>>> origin/master

	public float						humanSpawnTimer = 20.0f;
	public float						carSpawnTimer = 10.0f;

<<<<<<< HEAD
=======
	public float						carTimeLow = 10.0f;
	public float						carTimeHigh = 20.0f;

>>>>>>> origin/master
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

<<<<<<< HEAD
	public GameObject[]					animals;
	public GameObject[]					animalPoint;
=======
	//animals info
	public GameObject[]					animals;
	public GameObject					animalRSpawn;
	public GameObject					animalLSpawn;
	public Vector3[]					animalPos;

>>>>>>> origin/master
	public float                     	animalSpawnTimer;
	public int							animalNum;
	public bool							spawnAnimal = false;

<<<<<<< HEAD
=======
	//fly things info
	public GameObject[]					flying;
	public GameObject					flyRSpawn;
	public GameObject					flyLSpawn;
	public Vector3[]					flyPos;

	public float                     	flySpawnTimer;
	public int							flyNum;
	public bool							spawnFly = false;

	//feedback for killing people
	public bool							spawnPolice = false;
	public bool							spawnTank = false;
	public bool							spawnHelicopter = false;

	public GameObject					policeCar;
	public GameObject					tank;
	public GameObject					helicopter;

>>>>>>> origin/master

	// Use this for initialization
	void Start () {

<<<<<<< HEAD
=======
		gameMain = GameManager.manager;

>>>>>>> origin/master
		lLSpawnPos = leftLaneSpawn.transform.position;
		rLSpawnPos = rightLaneSpawn.transform.position;

		upStreetPos = upStreetSpawn.transform.position;
		downStreetPos = downStreetSpawn.transform.position;

<<<<<<< HEAD
		animalSpawnTimer = Random.Range (2f, 10f);
		animalNum = animals.Length;
=======
		animalPos[0] = animalRSpawn.transform.position;
		animalPos[1] = animalLSpawn.transform.position;

		flyPos[0] = flyRSpawn.transform.position;
		flyPos[1] = flyLSpawn.transform.position;

		animalSpawnTimer = Random.Range (2f, 10f);
		animalNum = animals.Length;

		flySpawnTimer = Random.Range (2f, 10f);
		flyNum = flying.Length;
>>>>>>> origin/master
	}
	
	// Update is called once per frame
	void Update () {
<<<<<<< HEAD

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


=======
		human = people[Random.Range(0,people.Length)];
		if (GameManager.gameState == GameManager.State.InGame) {
			humanSpawnTimer -= 1 * Time.deltaTime;
			carSpawnTimer -= 1 * Time.deltaTime;
			animalSpawnTimer -= 1 * Time.deltaTime;
			flySpawnTimer -= 1 * Time.deltaTime;

			//[TODO]Spawn Human at top or bottom of street every 20 seconds
//			if(humanSpawnTimer <= 0){
				//Debug.Log ("Human Timer Up");
//				spawnHuman = true;
//				humanSpawnTimer = Random.Range(10.0f, 20.0f);
//				Human ();
//			}

			//[TODO] Spawn Cars
			if(carSpawnTimer <= 0){
				spawnCar = true;
				carSpawnTimer = Random.Range(carTimeLow, carTimeHigh);
				//spawn normal cars
				if(spawnCar && !spawnPolice && !spawnTank){
					Car(car, "car");
					spawnCar = false;
				}
				//spawn police cars
				if(spawnCar && spawnPolice && !spawnTank){
					Car(policeCar, "PoliceCar");
					spawnCar = false;
					spawnPolice = false;
				}
				//spawn tanks
				if(spawnCar && !spawnPolice && spawnTank){
					Car(tank, "Tank");
					spawnCar = false;
					spawnTank = false;
				}

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

			//Spawn Person from Destroyed building
			if(gameMain.buildingDestroyed){
				gameMain.buildingDestroyed = false;

				Vector3 humanPos = gameMain.spawnHumanPos;

				if(gameMain.buildingType == "Cube"){
					Instantiate (human, humanPos, Quaternion.identity);
					Debug.Log ("1 Human fell from building");
				}
				if(gameMain.buildingType == "Cube_1"){
					Instantiate (human, humanPos, Quaternion.identity);
					//Debug.Log ("Human fell from building");
					Instantiate (human, humanPos, Quaternion.identity);
					//Debug.Log ("Human fell from building");
					Instantiate (human, humanPos, Quaternion.identity);
					Debug.Log ("3 Human fell from building");
				}
				if(gameMain.buildingType == "Cube_2"){
					Instantiate (human, humanPos, Quaternion.identity);
					//Debug.Log ("Human fell from building");
					Instantiate (human, humanPos, Quaternion.identity);
					Debug.Log ("2 Human fell from building");
				}
			}

			if(gameMain.levelCount == 2){
				humanSpawnTimer = humanSpawnTimer * 0.2f;
				carSpawnTimer = carSpawnTimer * 0.2f;
				animalSpawnTimer = animalSpawnTimer * 0.2f;
				flySpawnTimer = flySpawnTimer * 0.2f;
			}
			if(gameMain.levelCount == 3){
				humanSpawnTimer = humanSpawnTimer * 0.21f;
				carSpawnTimer = carSpawnTimer * 0.25f;
				animalSpawnTimer = animalSpawnTimer * 0.21f;
				flySpawnTimer = flySpawnTimer * 0.21f;
			}
			if(gameMain.levelCount == 4){
				humanSpawnTimer = humanSpawnTimer * 0.3f;
				carSpawnTimer = carSpawnTimer * 0.3f;
				animalSpawnTimer = animalSpawnTimer * 0.3f;
				flySpawnTimer = flySpawnTimer * 0.3f;
			}
		}
	}

	public void Car(GameObject obj, string name){
		GameObject newCarL = (GameObject) Instantiate (obj, lLSpawnPos, Quaternion.identity);
		newCarL.transform.localEulerAngles = new Vector3 (0, 180, 0);
		newCarL.name = name+"L";
		iTweenEvent.GetEvent(newCarL, "leftCar").Play();
		
		GameObject newCarR = (GameObject) Instantiate (obj, rLSpawnPos, Quaternion.identity);
		newCarR.transform.localEulerAngles = new Vector3 (0, 180, 0);
		newCarR.name = name+"R";
		iTweenEvent.GetEvent(newCarR, "rightCar").Play();
>>>>>>> origin/master
	}

	public void Human(){
		if(spawnHuman){
			Debug.Log ("Creating Humans");
			//[TODO] Spawn humans randomly up or down street
<<<<<<< HEAD
			Instantiate (human, downStreetPos, Quaternion.identity);
=======
			Instantiate ((GameObject)human, upStreetPos, Quaternion.identity);
>>>>>>> origin/master
			spawnHuman = false;
		}
	}

	public void Animal(){
		if(spawnAnimal){
//			Debug.Log ("Creating Animal");
			//[TODO] Spawn animal
			int i = Random.Range (0, animalNum); //which type of animals
<<<<<<< HEAD
			int j = Random.Range(0, animalPoint.Length); //which positioin
			GameObject newAnimal = (GameObject)Instantiate(animals[i], animalPoint[j].transform.position, Quaternion.identity);
			newAnimal.name = animals[i].name;
			spawnAnimal = false;
		}

=======
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
>>>>>>> origin/master
	}
}
