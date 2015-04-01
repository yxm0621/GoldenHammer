using UnityEngine;
using System.Collections;

public class TrafficObjects : MonoBehaviour {
    public GameObject[] lengthways;
    public GameObject[] creature;
    public GameObject[] crosswise;
    public GameObject[] crossfly;

    public GameObject[] lengthwaysCity;
    public GameObject[] creatureCity;
    public GameObject[] crosswiseCity;
    public GameObject[] crossflyCity;

    public GameObject[] lengthwaysCountry;
    public GameObject[] creatureCountry;
    public GameObject[] crosswiseCountry;
    public GameObject[] crossflyCountry;

    public GameObject[] lengthwaysWilder;
    public GameObject[] creatureWilder;
    public GameObject[] crosswiseWilder;
    public GameObject[] crossflyWilder;

    public GameObject[] lengthwaysSpace;
    public GameObject[] creatureSpace;
    public GameObject[] crosswiseSpace;
    public GameObject[] crossflySpace;

    public GameObject[] lengthwaysOcean;
    public GameObject[] creatureOcean;
    public GameObject[] crosswiseOcean;
    public GameObject[] crossflyOcean;

    public TrafficController traffic;

	// Use this for initialization
	void Start () {
        lengthways = lengthwaysCity;
        creature = creatureCity;
        crosswise = crosswiseCity;
        crossfly = crossflyCity;
	}
	
	// Update is called once per frame
	void Update () {
        
	}

    public void changeTraffic() {
        switch (SceneManager.currentScene) {
            case SceneManager.scene.city:
                lengthways = lengthwaysCity;
                creature = creatureCity;
                crosswise = crosswiseCity;
                crossfly = crossflyCity;
                break;
            case SceneManager.scene.countryside:
                lengthways = lengthwaysCountry;
                creature = creatureCountry;
                crosswise = crosswiseCountry;
                crossfly = crossflyCountry;
                break;
            case SceneManager.scene.wilderness:
                lengthways = lengthwaysWilder;
                creature = creatureWilder;
                crosswise = crosswiseWilder;
                crossfly = crossflyWilder;
                break;
            case SceneManager.scene.space:
                lengthways = lengthwaysSpace;
                creature = creatureSpace;
                crosswise = crosswiseSpace;
                crossfly = crossflySpace;
                break;
            case SceneManager.scene.ocean:
                lengthways = lengthwaysOcean;
                creature = creatureOcean;
                crosswise = crosswiseOcean;
                crossfly = crossflyOcean;
                break;
            case SceneManager.scene.battlefield:
                lengthways = null;
                creature = null;
                crosswise = null;
                crossfly = null;
                break;
            default:
                break;
        }
        gameObject.GetComponent<TrafficController>().car = lengthways;
        gameObject.GetComponent<TrafficController>().people = creature;
        gameObject.GetComponent<TrafficController>().animals = crosswise;
        gameObject.GetComponent<TrafficController>().flying = crossfly;
    }
}
