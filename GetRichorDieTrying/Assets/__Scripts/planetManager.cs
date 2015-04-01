using UnityEngine;
using System.Collections;

public class PlanetManager : MonoBehaviour {

	// Use this for initialization
	void Start () {
		gameObject.transform.position += new Vector3 (0,Random.Range(-5f, 5f),0);
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
