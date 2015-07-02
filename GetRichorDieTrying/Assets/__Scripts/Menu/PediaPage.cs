/*******
 * attached to PediaPage in the scene
 * ****/

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PediaPage : MonoBehaviour
{

    public GameManager gameMain;

    public Google2u.ObjList_G2U objData;

    public GameObject pediaItemPrefab;

    public GameObject icon;


    public GameObject allObjParent;
    public GameObject cityObjParent;
    public GameObject rainObjParent;
    public GameObject battleFieldObjParent;
    public GameObject countrySideObjParent;
    public GameObject wildernessObjParent;
    public GameObject oceanObjParent;
    public GameObject spaceObjParent;
    public GameObject nightObjParent;
    public GameObject anyObjParent;

    public List<GameObject> allObjList;
    public List<GameObject> cityObjList;
    public List<GameObject> rainObjList;
    public List<GameObject> battleFieldObjList;
    public List<GameObject> countrySideObjList;
    public List<GameObject> wildernessObjList;
    public List<GameObject> oceanObjList;
    public List<GameObject> spaceObjList;
    public List<GameObject> nightObjList;
    public List<GameObject> anyObjList;

    void Awake()
    {
        Initialize();
        CreateLayout();
    }

    void Initialize()
    {
        gameMain = GameManager.manager;
        objData = Google2u.ObjList_G2U.Instance;

        //allObjList = new GameObject();
        //cityObjList = new GameObject();
        //rainObjList = new GameObject();
        //battleFieldObjList = new GameObject();
        //countrySideObjList = new GameObject();
        //wildernessObjList = new GameObject();
        //oceanObjList = new GameObject();
        //spaceObjList = new GameObject();
        //nightObjList = new GameObject();
        //anyObjList = new GameObject();

        allObjList = new List<GameObject>();
        cityObjList = new List<GameObject>();
        rainObjList = new List<GameObject>();
        battleFieldObjList = new List<GameObject>();
        countrySideObjList = new List<GameObject>();
        wildernessObjList = new List<GameObject>();
        oceanObjList = new List<GameObject>();
        spaceObjList = new List<GameObject>();
        nightObjList = new List<GameObject>();
        anyObjList = new List<GameObject>();
    }

    void CreateLayout()
    {
        //CreateHeader();
        CreateContent();
    }

    void CreateHeader()
    {
        GameObject pedia = Instantiate(icon);
    }

    void CreateContent()
    {
        if (objData != null)
        {
            int cityCount = 0;
            int rainCount = 0;
            int battleCount = 0;
            int countryCount = 0;
            int wilderCount = 0;
            int oceanCount = 0;
            int spaceCount = 0;
            int nightCount = 0;
            int anyCount = 0;

            for (int i = 0; i < objData.Rows.Count; i++)
            {

                /*** Eliminating things that are not supposed to be on the list ***/
                if (objData.Rows[i]._Name == "Checkpoint")
                {
                    continue;
                }

                /************************************************************************/
                

                if (objData.Rows[i]._Location.Contains("City"))
                {
                    GameObject pediaItem = Instantiate(i);
                    pediaItem.transform.localPosition += new Vector3(0, -cityCount * 1.3f, 0);
                    pediaItem.transform.SetParent(cityObjParent.transform);
                    cityObjList.Add(pediaItem);
                    cityCount++;
                }
                if (objData.Rows[i]._Location.Contains("Rain"))
                {
                    GameObject pediaItem = Instantiate(i);
                    pediaItem.transform.localPosition += new Vector3(10, -rainCount * 1.3f, 0);
                    pediaItem.transform.SetParent(rainObjParent.transform);
                    rainObjList.Add(pediaItem);
                    rainCount++;
                }
                if (objData.Rows[i]._Location.Contains("Battlefield"))
                {
                    GameObject pediaItem = Instantiate(i);
                    pediaItem.transform.localPosition += new Vector3(12, -battleCount * 1.3f, 0);
                    pediaItem.transform.SetParent(battleFieldObjParent.transform);
                    battleFieldObjList.Add(pediaItem);
                    battleCount++;
                }
                if (objData.Rows[i]._Location.Contains("Countryside"))
                {
                    GameObject pediaItem = Instantiate(i);
                    pediaItem.transform.localPosition += new Vector3(14, -countryCount * 1.3f, 0);
                    pediaItem.transform.SetParent(countrySideObjParent.transform);
                    countrySideObjList.Add(pediaItem);
                    countryCount++;
                }
                if (objData.Rows[i]._Location.Contains("Wilderness"))
                {
                    GameObject pediaItem = Instantiate(i);
                    pediaItem.transform.localPosition += new Vector3(16, -wilderCount * 1.3f, 0);
                    pediaItem.transform.SetParent(wildernessObjParent.transform);
                    wildernessObjList.Add(pediaItem);
                    wilderCount++;
                }
                if (objData.Rows[i]._Location.Contains("Ocean"))
                {
                    GameObject pediaItem = Instantiate(i);
                    pediaItem.transform.localPosition += new Vector3(18, -oceanCount * 1.3f, 0);
                    pediaItem.transform.SetParent(oceanObjParent.transform);
                    oceanObjList.Add(pediaItem);
                    oceanCount++;
                }
                if (objData.Rows[i]._Location.Contains("Space"))
                {
                    GameObject pediaItem = Instantiate(i);
                    pediaItem.transform.localPosition += new Vector3(20, -spaceCount * 1.3f, 0);
                    pediaItem.transform.SetParent(spaceObjParent.transform);
                    spaceObjList.Add(pediaItem);
                    spaceCount++;
                }
                if (objData.Rows[i]._Location.Contains("Night"))
                {
                    GameObject pediaItem = Instantiate(i);
                    pediaItem.transform.localPosition += new Vector3(22, -nightCount * 1.3f, 0);
                    pediaItem.transform.SetParent(nightObjParent.transform);
                    nightObjList.Add(pediaItem);
                    nightCount++;
                }
                if (objData.Rows[i]._Location.Contains("Any"))
                {
                    GameObject pediaItem = Instantiate(i);
                    pediaItem.transform.localPosition += new Vector3(24, -anyCount * 1.3f, 0);
                    pediaItem.transform.SetParent(anyObjParent.transform);
                    anyObjList.Add(pediaItem);
                    anyCount++;
                }
            }
        }

        //Debug.Log("City obstacles count = " + cityObjList.Count);
        //Debug.Log("Rain obstacles count = " + rainObjList.Count);
        //Debug.Log("Battlefield obstacles count = " + battleFieldObjList.Count);
        //Debug.Log("Countryside obstacles count = " + countrySideObjList.Count);
        //Debug.Log("Wilderness obstacles count = " + wildernessObjList.Count);
        //Debug.Log("Ocean obstacles count = " + oceanObjList.Count);
        //Debug.Log("Space obstacles count = " + spaceObjList.Count);
        //Debug.Log("Night obstacles count = " + nightObjList.Count);
        //Debug.Log("Any obstacles count = " + anyObjList.Count);
    }

    GameObject Instantiate(int i)
    {
        GameObject pediaItem = Instantiate(pediaItemPrefab);
        pediaItem.name = "PediaItem_" + objData.Rows[i]._Name;
        //pediaItem.transform.SetParent(GameObject.Find("PediaItemsStartPoint").gameObject.transform);
        //////no need//pediaItem.transform.position = GameObject.Find("PediaItemsStartPoint").transform.position;

        GameObject tmpItemNameObj = GameObject.Find("ItemName");
        tmpItemNameObj.GetComponent<TextMesh>().text = objData.Rows[i]._Name;
        tmpItemNameObj.name = "ItemName_" + objData.Rows[i]._Name;

        GameObject tmpItemDscrObj = GameObject.Find("ItemDescription");
        //TODO//tmpItemDscrObj.GetComponent<TextMesh>().text = objData.Rows[i]._Description;
        tmpItemDscrObj.name = "ItemDescription_" + objData.Rows[i]._Name;

        GameObject tmpItemHPObj = GameObject.Find("ItemHP");
        tmpItemHPObj.GetComponent<TextMesh>().text = "HP: " + objData.Rows[i]._HitPoints.ToString();
        tmpItemHPObj.name = "ItemHP_" + objData.Rows[i]._Name;

        GameObject tmpItemValuObj = GameObject.Find("ItemValue");
        tmpItemValuObj.GetComponent<TextMesh>().text = "$$: " + objData.Rows[i]._Gold.ToString();
        tmpItemValuObj.name = "ItemValue_" + objData.Rows[i]._Name;

        return pediaItem;
    }

    void Update()
    {
    }
}
