using UnityEngine;
using System.Collections;

public class NewSegTrigger : MonoBehaviour {
    int segNum;
    bool canDestroy;
    GameObject desObj;

    //generate new segment
    void OnTriggerExit(Collider other)
    {
        canDestroy = false;
        desObj = null;
        segNum = 0;
        GameObject[] segs = GameObject.FindGameObjectsWithTag("Segment");
        foreach (GameObject seg in segs)
        {
            if (seg.name == "spawnPoint") {
                segNum++;
                if (seg.transform.position.z < -5.5f) {
                    canDestroy = true;
                    desObj = seg;
                }
            }
        }
        if (segNum < 5) {
            GameManager.manager.NewSegment();
        } else if (segNum == 5) {
            if (canDestroy) {
                Destroy(desObj);
                GameManager.manager.NewSegment();
            }
        }
    }
}
