using UnityEngine;
using System.Collections;

public class MenuFrame : MonoBehaviour {
    public GameObject[] frame;
    public int width;
    public int height;
    public float offset = .08f;
    GameObject frames;
    int i;
    int j;
    Vector3 offsetPos = new Vector3(0f, 0f, 0f);

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void CreateMenu() {
        offsetPos = new Vector3(0f, 0f, 0f);
        frames = Instantiate(frame[0], gameObject.transform.position, Quaternion.identity) as GameObject;
        adjust(offsetPos, frame[0].name);

        offsetPos = new Vector3(offset * (width - 1), 0f, 0f);
        frames = Instantiate(frame[2], offsetPos, Quaternion.identity) as GameObject;
        adjust(offsetPos, frame[2].name);

        offsetPos = new Vector3(0f, -offset * (height - 1), 0f);
        frames = Instantiate(frame[6], offsetPos, Quaternion.identity) as GameObject;
        adjust(offsetPos, frame[6].name);

        offsetPos = new Vector3(offset * (width - 1), -offset * (height - 1), 0f);
        frames = Instantiate(frame[8], offsetPos, Quaternion.identity) as GameObject;
        adjust(offsetPos, frame[8].name);
        
        for (i = 1; i < width - 1; i++) {
            offsetPos = new Vector3(offset * i, 0f, 0f);
            frames = Instantiate(frame[1], offsetPos, Quaternion.identity) as GameObject;
            adjust(offsetPos, frame[1].name);

            offsetPos = new Vector3(offset * i, -offset * (height - 1), 0f);
            frames = Instantiate(frame[7], offsetPos, Quaternion.identity) as GameObject;
            adjust(offsetPos, frame[7].name);
        }

        for (j = 1; j < height - 1; j++) {
            offsetPos = new Vector3(0f, -offset * j, 0f);
            frames = Instantiate(frame[3], offsetPos, Quaternion.identity) as GameObject;
            adjust(offsetPos, frame[3].name);

            offsetPos = new Vector3(offset * (width - 1), -offset * j, 0f);
            frames = Instantiate(frame[5], offsetPos, Quaternion.identity) as GameObject;
            adjust(offsetPos, frame[5].name);
        }

        for (i = 1; i < width - 1; i++) {
            for (j = 1; j < height - 1; j++) {
                offsetPos = new Vector3(offset * i, -offset * j, 0f);
                frames = Instantiate(frame[4], offsetPos, Quaternion.identity) as GameObject;
                adjust(offsetPos, frame[4].name);
            }
        }
    }

    void adjust(Vector3 offset, string name) {
        frames.transform.parent = gameObject.transform;
        frames.transform.localScale = new Vector3(1f, 1f, 1f);
        frames.transform.localPosition = offset;
        frames.name = name;
    }
}
