using UnityEngine;
using System.Collections;

public class TrafficData : MonoBehaviour {
    public TrafficController traffic;

	// Use this for initialization
	void Start () {
        traffic = gameObject.GetComponent<TrafficController>();
	}

    public void loadData(int level) {
        if (traffic == null) {
            traffic = gameObject.GetComponent<TrafficController>();
        }
        switch (level) {
            case 1:
                //difficulty for level 1
                traffic.easyPer = .2f;
                traffic.mediumPer = .6f;
                traffic.hardPer = .2f;
                traffic.superHardPer = 0f;
                traffic.timeInterval = 3;
                break;
            case 2:
                //difficulty for level 2
                traffic.easyPer = .1f;
                traffic.mediumPer = .6f;
                traffic.hardPer = .3f;
                traffic.superHardPer = 0f;
                traffic.timeInterval = 3;
                break;
            case 3:
                //difficulty for level 3
                traffic.easyPer = .05f;
                traffic.mediumPer = .6f;
                traffic.hardPer = .3f;
                traffic.superHardPer = .05f;
                traffic.timeInterval = 3;
                break;
            case 4:
                //difficulty for level 4
                traffic.easyPer = 0f;
                traffic.mediumPer = .5f;
                traffic.hardPer = .45f;
                traffic.superHardPer = .05f;
                traffic.timeInterval = 3;
                break;
            case 5:
                //difficulty for level 5
                traffic.easyPer = .4f;
                traffic.mediumPer = .6f;
                traffic.hardPer = 0f;
                traffic.superHardPer = 0f;
                traffic.timeInterval = 3;
                break;
            default:
                traffic.easyPer = 0f;
                traffic.mediumPer = 0f;
                traffic.hardPer = 0f;
                traffic.superHardPer = 0f;
                traffic.timeInterval = 3;
                break;
        }
    }
}
