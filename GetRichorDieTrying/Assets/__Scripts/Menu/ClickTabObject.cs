// attached to Tab_City, Tab_Rain, etc...

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ClickTabObject : MonoBehaviour {

    private GameObject pediaPage;

    private GameObject pediaItemsStartPoint;

    private List<GameObject> objList;

    private GameObject objParent;


    public enum TabItemName
    {
        City,
        Rain,
        Battlefield,
        Countryside,
        Wilderness,
        Ocean,
        Space,
        Night,
        Any
    }

    public TabItemName tabItemName;

	void Start ()
    {
        pediaPage = GameObject.Find("PediaPage");

        switch (tabItemName)
        {
            case TabItemName.Battlefield:
                objList = pediaPage.GetComponent<PediaPage>().battleFieldObjList;
                objParent = pediaPage.GetComponent<PediaPage>().battleFieldObjParent;
                break;
            case TabItemName.City:
                objList = pediaPage.GetComponent<PediaPage>().cityObjList;
                objParent = pediaPage.GetComponent<PediaPage>().cityObjParent;
                break;
            case TabItemName.Countryside:
                objList = pediaPage.GetComponent<PediaPage>().countrySideObjList;
                objParent = pediaPage.GetComponent<PediaPage>().countrySideObjParent;
                break;
            case TabItemName.Night:
                objList = pediaPage.GetComponent<PediaPage>().nightObjList;
                objParent = pediaPage.GetComponent<PediaPage>().nightObjParent;
                break;
            case TabItemName.Ocean:
                objList = pediaPage.GetComponent<PediaPage>().oceanObjList;
                objParent = pediaPage.GetComponent<PediaPage>().oceanObjParent;
                break;
            case TabItemName.Rain:
                objList = pediaPage.GetComponent<PediaPage>().rainObjList;
                objParent = pediaPage.GetComponent<PediaPage>().rainObjParent;
                break;
            case TabItemName.Space:
                objList = pediaPage.GetComponent<PediaPage>().spaceObjList;
                objParent = pediaPage.GetComponent<PediaPage>().spaceObjParent;
                break;
            case TabItemName.Wilderness:
                objList = pediaPage.GetComponent<PediaPage>().wildernessObjList;
                objParent = pediaPage.GetComponent<PediaPage>().wildernessObjParent;
                break;
            case TabItemName.Any:
                objList = pediaPage.GetComponent<PediaPage>().anyObjList;
                objParent = pediaPage.GetComponent<PediaPage>().anyObjParent;
                break;
        }
    }


    void OnMouseDown()
    {
        Debug.Log("Clicked " + gameObject.name);

        objParent.transform.localPosition = new Vector3(0, 0, 0);//GameObject.Find("PediaItemsStartPoint").transform.position;

        for (int i = 0; i < objList.Count; i++)
        {
            objList[i].transform.localPosition = new Vector3(1.02f, -1.02f + -i * 1.3f, 0);
        }

        GameObject.Find("PediaTab").GetComponent<TabManager>().ChangeTabitem(gameObject.name);
    }

    public void PutAway()
    {
        for (int i = 0; i < objList.Count; i++)
        {
            objList[i].transform.localPosition = new Vector3(50, 50, 0);
        }

        //notworking//objParent.transform.localPosition = new Vector3(50, 50, 0);
    }
}
