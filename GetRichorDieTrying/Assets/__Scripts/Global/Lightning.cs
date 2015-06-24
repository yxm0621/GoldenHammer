using UnityEngine;
using System.Collections;

public class Lightning : MonoBehaviour {
    public float timer;

	// Use this for initialization
	void Start () {
        timer = Random.Range(.1f, .5f);
        StartCoroutine(Flash());
	}
	
	//Lightning flash
    IEnumerator Flash() {
        gameObject.SetActive(true);
        yield return new WaitForSeconds(timer);
        gameObject.SetActive(false);
        yield return new WaitForSeconds(timer/2);
        gameObject.SetActive(true);
        yield return new WaitForSeconds(timer);
        gameObject.SetActive(false);
        Destroy(gameObject);
    }
}
