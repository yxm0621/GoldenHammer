using UnityEngine;
using System.Collections;

public class NewSegTrigger : MonoBehaviour {
    //generate new segment
    void OnTriggerExit(Collider other)
    {
        GameManager.manager.NewSegment();
    }
}
