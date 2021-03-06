﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System;
using Google2u;

public class TrafficController : MonoBehaviour {
	public GameManager					gameMain;
    public bool                         trafficStop = false;

    //read and load obstacle info
    public GameObject                   obstacles;
    public TrafficObjects               trafficObjs;
    public Google2u.TrafficDesign       trafficDatabase;
    public List<string>                 obstacleData; //all of obstacle data
    public List<string>                 obstacleLevel; //obstacle texts in current level
    public FileInfo                     sourceFile = null;
    public StreamReader                 reader = null;
    public string                       text = " "; // assigned to allow first line to be read below
    public int                          lineIndex = 0;
    public int                          timeInterval;
    public int                          patternLevel;

    //percentage of different types of patterns
    public float                        easyPer;
    public float                        mediumPer;
    public float                        hardPer;
    public float                        superHardPer;

    //number of different types of patterns
    public int                          easyNum;
    public int                          mediumNum;
    public int                          hardNum;
    public int                          superHardNum;
    public int                          obsNum;

    //obstacle collide info
    public float                        collideZ = -1;
    public float                        hitDis = .1f; //Distance between player and obstacle when player hits the obstacle
    public List<float>                  laneTimer;

    public List<float>                  firstLaneTimer;
    public List<float>                  secondLaneTimer;
    public List<float>                  thirdLaneTimer;
    public List<float>                  forthLaneTimer;

    //car info
    public GameObject					car;
	public GameObject[]					cars;
    public float                        lastOffset = -1;
    public float                        carSpeedLow = 0f;
    public float                        carSpeedHigh = 3f;
    public float[]                      carSpeedGroup;
    public float                        carHitOffsetZ = .4f;

    //public GameObject                   leftLaneSpawn;
    //public GameObject                   rightLaneSpawn;
    //public Vector3                      lLSpawnPos;
    //public Vector3                      rLSpawnPos;

    //human info
	public GameObject					human;
	public GameObject[]					people;
	public int							peopleNum;

    //public GameObject					lStreetSpawn;
    //public GameObject					rStreetSpawn;
    //public Vector3						lStreetPos;
    //public Vector3						rStreetPos;

	//animals info
	public GameObject[]					animals;
	public int							animalNum;
    public float                        animalSpeedLow = 0f;
    public float                        animalSpeedHigh = 1f;

    //public GameObject					animalRSpawn;
    //public GameObject					animalLSpawn;
    //public Vector3[]					animalPos;
    
	//fly things info
	public GameObject[]					flying;
	public GameObject					flyRSpawn;
	public GameObject					flyLSpawn;
	public Vector3[]					flyPos;

	public float                     	flySpawnTimer;
	public int							flyNum;

	//feedback for killing people
	public bool							spawnPolice = false;
	public bool							spawnTank = false;
	public bool							spawnHelicopter = false;

	public GameObject					policeCar;
	public GameObject					tank;
	public GameObject					helicopter;

	// Use this for initialization
	IEnumerator Start () {
		gameMain = GameManager.manager;

        //Create the obstacle parent object
        obstacles = (GameObject)Instantiate(obstacles, obstacles.transform.position, Quaternion.identity);
        gameMain.obstacleGroup = obstacles;

        //Get traffic data from file
        //sourceFile = new FileInfo(@"D:\GitHub\GoldenHammer\GetRichorDieTrying\Assets\__Scripts\data\Obstacles.txt");
        //reader = sourceFile.OpenText();

        //Get traffic data from static database
        trafficDatabase = Google2u.TrafficDesign.Instance;

        //Set traffic Objects
        trafficObjs = gameObject.GetComponent<TrafficObjects>();
        yield return new WaitForSeconds(.1f);
        if (trafficObjs.lengthways == null) {
            StartCoroutine(InitialObjLoadFinish());
        }
        cars = trafficObjs.lengthways;
        people = trafficObjs.creature;
        animals = trafficObjs.crosswise;
        flying = trafficObjs.crossfly;

        //Load level patterns
        patternLevel = 1;
        LoadLevel(patternLevel);
        
        //load initial 5 obstacles
        for (int i = 0; i < 5; i++) {
            ReadPattern();
            LoadPattern(obstacleData[lineIndex]);
        }

        //get spawn positions
        //lLSpawnPos = leftLaneSpawn.transform.position;
        //rLSpawnPos = rightLaneSpawn.transform.position;

        //lStreetPos = lStreetSpawn.transform.position;
        //rStreetPos = rStreetSpawn.transform.position;

        //animalPos[0] = animalLSpawn.transform.position;
        //animalPos[1] = animalRSpawn.transform.position;

		flyPos[0] = flyLSpawn.transform.position;
		flyPos[1] = flyRSpawn.transform.position;

        flySpawnTimer = RandomFloat(2f, 10f);
	}

    IEnumerator InitialObjLoadFinish() {
        if (trafficObjs.lengthways == null) {
            yield return new WaitForSeconds(.1f);
            StartCoroutine(InitialObjLoadFinish());
        }
    }
	
	// Update is called once per frame
	void Update () {
        peopleNum = people.Length;
        animalNum = animals.Length;
        flyNum = flying.Length;

        if (gameMain.gameState == GameManager.State.InGame) {
            //keep 5 obstacles on the road, when laneTimer reach 0, delete obstacle timer data and generate new obstacle pattern
            if (laneTimer.Count != 0) {
                for (int i = 0; i < laneTimer.Count; i++) {
                    if (!trafficStop) {
                        laneTimer[i] -= Time.deltaTime;
                    }
                }
                if (laneTimer[0] <= 0) {
                    laneTimer.RemoveAt(0);
                    ReadPattern();
                    LoadPattern(obstacleData[lineIndex]);
                }
            }

            /*
			flySpawnTimer -= 1 * Time.deltaTime;

			//[TODO] Spawn Cars
			if(carSpawnTimer <= 0){
				spawnCar = true;
                carSpawnTimer = RandomFloat(carTimeLow, carTimeHigh);
				//spawn normal cars
				if(spawnCar && !spawnPolice && !spawnTank){
                    if (cars.Length > 0)
                    {
                        //Car(cars[0], "car", lLSpawnPos);
                        //Car(cars[0], "car", rLSpawnPos);
                    }
					spawnCar = false;
				}
				//spawn police cars
				if(spawnCar && spawnPolice && !spawnTank){
                    Car(policeCar, "PoliceCar", lLSpawnPos);
                    Car(policeCar, "PoliceCar", rLSpawnPos);
					spawnCar = false;
					spawnPolice = false;
				}
				//spawn tanks
				if(spawnCar && !spawnPolice && spawnTank){
					Car(tank, "Tank", lLSpawnPos);
                    Car(tank, "Tank", rLSpawnPos);
					spawnCar = false;
					spawnTank = false;
				}

				//Debug.Log ("Car timer up. Next timer: " + carSpawnTimer);
			}

			//[TODO] Spawn Flying things
			if(flySpawnTimer <= 0){
				spawnFly = true;
                flySpawnTimer = RandomFloat(3f, 10f);
                if (flyNum > 0)
                {
                    Fly();
                }
			}
            */
			//Spawn Person from Destroyed building
            //if(gameMain.buildingDestroyed){
            //    gameMain.buildingDestroyed = false;

            //    Vector3 humanPos = gameMain.spawnHumanPos;
            //    GameObject newHuman;

            //    if(gameMain.buildingType == "Building_1"){
            //        newHuman = (GameObject) Instantiate(human, humanPos, Quaternion.identity);
            //        newHuman.transform.parent = obstacles.transform;
            //        Debug.Log ("1 Human fell from building");
            //    }
            //    if(gameMain.buildingType == "Building_2"){
            //        newHuman = (GameObject)Instantiate(human, humanPos, Quaternion.identity);
            //        newHuman.transform.parent = obstacles.transform;
            //        //Debug.Log ("Human fell from building");
            //        newHuman = (GameObject)Instantiate(human, humanPos, Quaternion.identity);
            //        newHuman.transform.parent = obstacles.transform;
            //        //Debug.Log ("Human fell from building");
            //        newHuman = (GameObject)Instantiate(human, humanPos, Quaternion.identity);
            //        newHuman.transform.parent = obstacles.transform;
            //        Debug.Log ("3 Human fell from building");
            //    }
            //    if(gameMain.buildingType == "Building_3"){
            //        newHuman = (GameObject)Instantiate(human, humanPos, Quaternion.identity);
            //        newHuman.transform.parent = obstacles.transform;
            //        //Debug.Log ("Human fell from building");
            //        newHuman = (GameObject)Instantiate(human, humanPos, Quaternion.identity);
            //        newHuman.transform.parent = obstacles.transform;
            //        Debug.Log ("2 Human fell from building");
            //    }
            //}
		}
	}

    //Change difficulty based on level count
    public void LoadLevel(int level) {
        //level = gameMain.levelCount;
        if (trafficDatabase == null) {
            trafficDatabase = Google2u.TrafficDesign.Instance;
        }
        string levelID = "Level_" + level;
        //Debug.Log(levelID);
        easyPer = trafficDatabase.GetRow(levelID)._Easy_Per;
        mediumPer = trafficDatabase.GetRow(levelID)._Med_Per;
        hardPer = trafficDatabase.GetRow(levelID)._Hard_Per;
        superHardPer = trafficDatabase.GetRow(levelID)._SuHard_Per;
        timeInterval = trafficDatabase.GetRow(levelID)._Time_Interval;
        GetPatternNum();
        GeneratePattern();
    }

    //Calculate pattern amount in the level
    public void GetPatternNum() {
        obsNum = 30 / timeInterval;
        easyNum = Convert.ToInt32(obsNum * easyPer);
        mediumNum = Convert.ToInt32(obsNum * mediumPer);
        hardNum = Convert.ToInt32(obsNum * hardPer);
        superHardNum = Convert.ToInt32(obsNum * superHardPer);
        int totalNum = easyNum + mediumNum + hardNum + superHardNum;

        if (totalNum != obsNum) {
            mediumNum = mediumNum - (obsNum - totalNum);
        }
    }

    //generate pattern text for the level
    public void GeneratePattern() {
        //time interval between each pattern
        float time = timeInterval;
        
        //repeat times for the pattern
        int repeatTimes = 0;
        
        //generate text for the pattern
        int i;
        int laneId;
        for (i = 0; i < obsNum; i++) {
            string newText = "";
            if (i < easyNum) {
                //generate easy pattern text
                laneId = RandomInt(1, 5);
                newText = "E " + laneId + " " + time + " " + repeatTimes;
            } else if (i < easyNum + mediumNum) {
                //generate medium pattern text
                laneId = RandomInt(1, 4);
                newText = "M " + laneId;
                laneId = RandomInt(laneId + 1, 5);
                newText += laneId + " " + time + " " + repeatTimes;
            } else if (i < easyNum + mediumNum + hardNum) {
                //generate hard pattern text
                laneId = RandomInt(1, 5);
                switch (laneId) {
                case 1:
                    newText = "H 234 " + time + " " + repeatTimes;
                    break;
                case 2:
                    newText = "H 134 " + time + " " + repeatTimes;
                    break;
                case 3:
                    newText = "H 124 " + time + " " + repeatTimes;
                    break;
                case 4:
                    newText = "H 123 " + time + " " + repeatTimes;
                    break;
                default:
                    break;
                }
            } else {
                //generate super hard pattern text
                newText = "S 1234 " + time + " " + repeatTimes;
            }

            //add texts to obstacleLevel list
            obstacleLevel.Add(newText);
        }
    }

    //[provious]read pattern from line
    //[now]generate pattern text based on the percentage
    public void ReadPattern() {
        //if (text != null) {
            //text = reader.ReadLine(); //read next line
            //obstacleData.Add(text); //add to list
            //print(text);
        //} else {

        if (obstacleLevel.Count <= 0) {
            LoadLevel(++patternLevel);
        }
        int i = RandomInt(0, obstacleLevel.Count);
        
        //add to list
        obstacleData.Add(obstacleLevel[i]);
        obstacleLevel.RemoveAt(i);

        //}
    }

    //[TODO] Load pattern based on the data
    /* FORMAT: [E/M/H/S][ ][obstacle lane(s)][ ][time][ ][repeat times]
     * Eg. "M 13 3.5 2" means medium obstacles will happen on lane1&3, after 3.5 seconds, repeat twice
     */
    public void LoadPattern(string line) {
        string[] words = line.Split(' '); //split line by ' ' and save as string array
        string lane = words[1]; //get lane(s)
        float time = Convert.ToSingle(words[2]); //get time interval
        int times = Convert.ToInt32(words[3]); //get repeat times
        float totalTime = time; //total time the pattern will be met from now

        //calculate total time that the obstacles will be met and put into laneTimer[]
        //change 5 timers in laneTimer[] based on the 5 obstacles on the road
        if (lineIndex > 4){
            laneTimer.Add(totalTime + laneTimer[3]);
            totalTime = laneTimer[4];
            lineIndex++;
        }else if (lineIndex > 0){
            laneTimer.Add(totalTime+laneTimer[lineIndex-1]);
            totalTime = laneTimer[lineIndex];
            lineIndex++;
        } else {
            laneTimer.Add(totalTime);
            lineIndex++;
        }

        //load pattern based on difficulty mode
        switch (line[0]) {
            case 'E':
                //load easy mode pattern
                char obsLane = lane[0];
                print("Easy obstacles on " + obsLane + ".");
                GeneratePattern(obsLane, totalTime, time, times);
                break;
            case 'M':
                //load medium mode pattern
                char first = lane[0];
                char second = lane[1];
                print("Medium obstacles on " + first + " and " + second + ".");
                GeneratePattern(first, totalTime, time, times);
                GeneratePattern(second, totalTime, time, times);
                break;
            case 'H':
                //load hard mode pattern
                char one = lane[0];
                char two = lane[1];
                char three = lane[2];
                print("Hard obstacles on " + one + " " + two + " and " + three + ".");
                GeneratePattern(one, totalTime, time, times);
                GeneratePattern(two, totalTime, time, times);
                GeneratePattern(three, totalTime, time, times);
                break;
            case 'S':
                //load super hard mode pattern
                print("Super Hard obstacles on all lanes.");
                GeneratePattern('1', totalTime, time, times);
                GeneratePattern('2', totalTime, time, times);
                GeneratePattern('3', totalTime, time, times);
                GeneratePattern('4', totalTime, time, times);
                break;
            default:
                break;
        }
        
    }

    //[TODO] Generate pattern on each lane
    /*lane - which lane the obstacle will be on
     * totalTime - the time player will meet the obstacle
     * offsetTime - the time interval between this obstacle with last obstacle
     * times - repeat times of this obstacle pattern
     */
    public void GeneratePattern(char lane, float totalTime, float offsetTime, int times) {
        switch (lane) {
            case '1':
                firstLane(totalTime, offsetTime, times);
                break;
            case '2':
                secondLane(totalTime, offsetTime, times);
                break;
            case '3':
                thirdLane(totalTime, offsetTime, times);
                break;
            case '4':
                forthLane(totalTime, offsetTime, times);
                break;
            default:
                break;
        }
    }

    //Generate obstacle on first lane
    public void firstLane(float totalTime, float offsetTime, int times) {
        int type = RandomInt(0, 2);
        float posZ = totalTime + gameMain.characterPos.z + hitDis;
        Vector3 pos = new Vector3(-1.5f, 0f, posZ);
        switch (type) {
            case 0:
                //animal obstacle
                //if (totalTime > 6) {
                //    //generate when 6s left
                //} else if (totalTime == 6) {
                //    //generate and wait on sidewalk
                //} else {
                //    //generate and run to the player OR generate and wait
                //    //after x seconds reach z=-1
                //    int run = RandomInt(0, 2);
                //    if(run == 0) {
                //        //wait
                //        float z = totalTime + collideZ;
                //        Vector3 pos = new Vector3(animalPos[0].x,
                //                                  animalPos[0].y,
                //                                  z);
                //        Animal(pos, 0, 'n', 0, 0);
                //    } else {
                //        //walk
                //        Animal(animalPos[0], 0, 'z', 6-totalTime, -1);
                //    }
                //}

                Animal(pos);
                break;
            case 1:
                //human obstacle
                //if (totalTime > 24) {
                //    //generate when 24s left
                //} else if (totalTime == 24) {
                //    //generate and wait on sidewalk
                //} else if (totalTime > 6) {
                //    //generate and run to the player
                //    //after x seconds reach z=-1
                //}
                Human(pos);
                break;
        }
    }

    //Generate obstacle on second lane
    public void secondLane(float totalTime, float offsetTime, int times) {
        int type = RandomInt(0, 2);
        float posZ = totalTime + gameMain.characterPos.z + hitDis;
        Vector3 pos = new Vector3(-.5f, 0f, posZ);
        switch (type) {
            case 0:
                //animal obstacle
                Animal(pos);
                break;
            case 1:
                //car obstacle
                pos.y = .08f;
                posZ += carHitOffsetZ;
                if (spawnTank) {
                    //spawn tanks
                    Car(tank, "Tank", pos, offsetTime, 0);
                    spawnTank = false;
                } else if (spawnPolice) {
                    //spawn police cars
                    Car(policeCar, "PoliceCar", pos, offsetTime, 0);
                    spawnPolice = false;
                } else {
                    //spawn normal cars
                    int i = RandomInt(0, cars.Length);
                    Car(cars[i], "Car", pos, offsetTime, i);
                }
                break;
        }
    }

    //Generate obstacle on third lane
    public void thirdLane(float totalTime, float offsetTime, int times) {
        int type = RandomInt(0, 2);
        float posZ = totalTime + gameMain.characterPos.z + hitDis;
        Vector3 pos = new Vector3(.5f, 0f, posZ);
        switch (type) {
            case 0:
                //animal obstacle
                Animal(pos);
                break;
            case 1:
                //car obstacle
                pos.y = .08f;
                posZ += carHitOffsetZ;
                if (spawnTank) {
                    //spawn tanks
                    Car(tank, "Tank", pos, offsetTime, 0);
                    spawnTank = false;
                } else if (spawnPolice) {
                    //spawn police cars
                    Car(policeCar, "PoliceCar", pos, offsetTime, 0);
                    spawnPolice = false;
                } else {
                    //spawn normal cars
                    int i = RandomInt(0, cars.Length);
                    Car(cars[i], "Car", pos, offsetTime, i);
                }
                break;
        }
    }

    //Generate obstacle on forth lane
    public void forthLane(float totalTime, float offsetTime, int times) {
        int type = RandomInt(0, 2);
        float posZ = totalTime + gameMain.characterPos.z + hitDis;
        Vector3 pos = new Vector3(1.5f, 0f, posZ);
        switch (type) {
            case 0:
                //animal obstacle
                Animal(pos);
                break;
            case 1:
                //human obstacle
                Human(pos);
                break;
        }
    }

    //Spawn car
	public void Car(GameObject obj, string name, Vector3 pos, float offsetTime, int iType){
        float totalTime = pos.z + carHitOffsetZ; //total time player will meet the car
        float offsetZ = 0; //how far the car is from the position if it doesn't need to move
        float carSpeed = 0; //car's moving speed

        //Change car's speed to be random
        /*
        //distance overlap fix
        if (lastOffset < 0) {
            //First car
            carSpeed = RandomFloat(carSpeedLow, 1);
        } else {
            //Rest cars
            float offsetDis = lastOffset - offsetTime;
            if (offsetDis > 0) {
                //Last car occupied the space of this car
                carSpeed = RandomFloat((offsetDis + 1) / totalTime, carSpeedHigh);
            } else {
                //Last car didn't occupy the space of this car
                carSpeed = RandomFloat(carSpeedLow, carSpeedHigh);
            }
        }

        //First 5 levels, cars don't move
        if (gameMain.levelCount <= 5) {
            carSpeed = 1f;
        }
        */

        carSpeed = carSpeedGroup[iType];

        offsetZ = totalTime * carSpeed; //how much relative distance the car needs to move
        lastOffset = offsetZ;

        //generate car
        GameObject newCar = (GameObject)Instantiate(obj, new Vector3(pos.x, pos.y, pos.z + offsetZ), obj.transform.rotation);
        if (pos.x > 0) {
            newCar.name = name + "R";
        } else {
            newCar.name = name + "L";
        }
        newCar.transform.parent = obstacles.transform;
        newCar.GetComponent<CarBehavior>().SetCar(totalTime, offsetZ);

        //iTweenEvent.GetEvent(newCarL, "leftCar").Play();
        //newCarR.transform.localEulerAngles = new Vector3 (0, 180, 0);
        //iTweenEvent.GetEvent(newCarR, "rightCar").Play();
	}

    //Spawn human
	public void Human(Vector3 pos){
        //Debug.Log("Creating Humans");
        //[TODO] Spawn human on the street
        int i = RandomInt(0, peopleNum); //which type of people
        GameObject newHuman = (GameObject)Instantiate((GameObject)people[i], pos, Quaternion.identity);
        newHuman.transform.parent = obstacles.transform;
        if ((i == peopleNum - 1) || (i == peopleNum - 2)) {
            if (pos.x > 0) {
                newHuman.transform.localPosition -= new Vector3(.1f, 0f, 0f);
                newHuman.transform.localEulerAngles = new Vector3(0f, -90f, 0f);
            } else {
                newHuman.transform.localPosition += new Vector3(.1f, 0f, 0f);
                newHuman.transform.localEulerAngles = new Vector3(0f, 90f, 0f);
            }
        }
	}

    //Spawn animal
	public void Animal(Vector3 pos){
        //Debug.Log ("Creating Animal");
        //[TODO] Spawn animal
        float totalTime = pos.z + 1; //total time player will meet the animal

        //which type of animals
        int i;
        if ((gameMain.levelCount % 5 == 1) && (gameMain.levelCount != 1)) {
            i = RandomInt(0, animalNum); //including the herd each 5 level
        } else {
            i = RandomInt(0, animalNum - 1); //excluding the herd
        }
        
        //Spawn animal and add under obstacleGroup
        GameObject newAnimal = (GameObject)Instantiate(animals[i], pos, Quaternion.identity);
        newAnimal.name = animals[i].name;
        newAnimal.transform.parent = obstacles.transform;
        if (pos.x > 0) {
            newAnimal.transform.eulerAngles = new Vector3(0, 180, 0);
        }

        //Set date for single animal and herd
        if (!newAnimal.name.Contains("Herd")) {
            newAnimal.GetComponent<AnimalBehavior>().SetAnimal(totalTime, 'n', 0, 0);
        } else {
            foreach (Transform child in newAnimal.transform) {
                child.GetComponent<AnimalBehavior>().SetAnimal(totalTime, 'n', 0, 0);
            }
        }

        //Animal starts moving
        if (gameMain.levelCount > 10) {
            if ((pos.x == 1.5f) || (pos.x == -1.5f)) {
                //animals on sidewalk don't move
                if (!newAnimal.name.Contains("Herd")) {
                    newAnimal.GetComponent<AnimalBehavior>().SetAnimal(totalTime, 'n', 0, 0);
                } else {
                    foreach (Transform child in newAnimal.transform) {
                        child.GetComponent<AnimalBehavior>().SetAnimal(totalTime, 'n', 0, 0);
                    }
                }
            } else {
                //animals on the road walk from the sidewalk
                float distance = .7f;
                float speed = RandomFloat(animalSpeedLow, animalSpeedHigh);
                float walkTime = distance / speed;
                if (walkTime > totalTime) {
                    walkTime = totalTime;
                }

                if (pos.x < 0f) {
                    newAnimal.transform.position -= new Vector3(0f, distance, 0f);
                    if (!newAnimal.name.Contains("Herd")) {
                        newAnimal.GetComponent<AnimalBehavior>().SetAnimal((totalTime - walkTime), 'x', walkTime, -.5f);
                    } else {
                        foreach (Transform child in newAnimal.transform) {
                            child.GetComponent<AnimalBehavior>().SetAnimal((totalTime - walkTime), 'x', walkTime, -.5f);
                        }
                    }
                } else if (pos.x > 0f) {
                    newAnimal.transform.position += new Vector3(0f, distance, 0f);
                    if (!newAnimal.name.Contains("Herd")) {
                        newAnimal.GetComponent<AnimalBehavior>().SetAnimal((totalTime - walkTime), 'x', walkTime, .5f);
                    } else {
                        foreach (Transform child in newAnimal.transform) {
                            child.GetComponent<AnimalBehavior>().SetAnimal((totalTime - walkTime), 'x', walkTime, .5f);
                        }
                    }
                }
            }
        }

        //newAnimal.GetComponent<AnimalBehavior>().setAnimal(waitTime, direction, walkTime, destination);
	}

    //Spawn fly things
	public void Fly(){
        //Debug.Log ("Creating Fly");
        //[TODO] Spawn fly things
        int i = RandomInt(0, flyNum); //which type of animals
        int j = RandomInt(0, 2); //which direction
        GameObject newFly = (GameObject)Instantiate(flying[i], flyPos[j], Quaternion.identity);
        newFly.name = flying[i].name;
        newFly.transform.parent = obstacles.transform;
	}

    int RandomInt(int min, int max) {
        return UnityEngine.Random.Range(min, max);
    }

    float RandomFloat(float min, float max)
    {
        return UnityEngine.Random.Range(min, max);
    }
}
