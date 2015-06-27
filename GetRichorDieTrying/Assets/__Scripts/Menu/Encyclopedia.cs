/*******
 * attached to PediaPage in the scene
 * ****/

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Encyclopedia : MonoBehaviour
{

    public GameManager gameMain;

    public Google2u.ObjList_G2U objData;

    public GameObject pediaItemPrefab;

    public GameObject icon;

    public static bool showEncyclopedia;


    //

    private List<GameObject> allObjList;
    private List<GameObject> cityObjList;
    private List<GameObject> rainObjList;
    private List<GameObject> battleFieldObjList;
    private List<GameObject> countrySideObjList;
    private List<GameObject> wildernessObjList;
    private List<GameObject> oceanObjList;
    private List<GameObject> spaceObjList;
    private List<GameObject> nightObjList;
    private List<GameObject> anyObjList;

    void Start()
    {
        Initialize();
        CreateLayout();
    }

    void Initialize()
    {
        gameMain = GameManager.manager;
        objData = Google2u.ObjList_G2U.Instance;
        showEncyclopedia = false;

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
                    pediaItem.transform.position += new Vector3(0, -cityCount * 1.3f, 0);
                    cityObjList.Add(pediaItem);
                    cityCount++;
                }
                if (objData.Rows[i]._Location.Contains("Rain"))
                {
                    GameObject pediaItem = Instantiate(i);
                    pediaItem.transform.position += new Vector3(0, -rainCount * 1.3f, 0);
                    rainObjList.Add(pediaItem);
                    rainCount++;
                }
                if (objData.Rows[i]._Location.Contains("Battlefield"))
                {
                    GameObject pediaItem = Instantiate(i);
                    pediaItem.transform.position += new Vector3(0, -battleCount * 1.3f, 0);
                    battleFieldObjList.Add(pediaItem);
                    battleCount++;
                }
                if (objData.Rows[i]._Location.Contains("Countryside"))
                {
                    GameObject pediaItem = Instantiate(i);
                    pediaItem.transform.position += new Vector3(0, -countryCount * 1.3f, 0);
                    countrySideObjList.Add(pediaItem);
                    countryCount++;
                }
                if (objData.Rows[i]._Location.Contains("Wilderness"))
                {
                    GameObject pediaItem = Instantiate(i);
                    pediaItem.transform.position += new Vector3(0, -wilderCount * 1.3f, 0);
                    wildernessObjList.Add(pediaItem);
                    wilderCount++;
                }
                if (objData.Rows[i]._Location.Contains("Ocean"))
                {
                    GameObject pediaItem = Instantiate(i);
                    pediaItem.transform.position += new Vector3(0, -oceanCount * 1.3f, 0);
                    oceanObjList.Add(pediaItem);
                    oceanCount++;
                }
                if (objData.Rows[i]._Location.Contains("Space"))
                {
                    GameObject pediaItem = Instantiate(i);
                    pediaItem.transform.position += new Vector3(0, -spaceCount * 1.3f, 0);
                    spaceObjList.Add(pediaItem);
                    spaceCount++;
                }
                if (objData.Rows[i]._Location.Contains("Night"))
                {
                    GameObject pediaItem = Instantiate(i);
                    pediaItem.transform.position += new Vector3(0, -nightCount * 1.3f, 0);
                    nightObjList.Add(pediaItem);
                    nightCount++;
                }
                if (objData.Rows[i]._Location.Contains("Any"))
                {
                    GameObject pediaItem = Instantiate(i);
                    pediaItem.transform.position += new Vector3(0, -anyCount * 1.3f, 0);
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
        pediaItem.transform.SetParent(GameObject.Find("PediaPage").transform);
        pediaItem.transform.position = GameObject.Find("MenuStartPoint").transform.position;

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
        if (showEncyclopedia)
        {
            this.gameObject.transform.position = new Vector3(-3.36f, -4.08f, 0);
            Debug.Log("!!!!!!");
        }
        else
        {
            this.gameObject.transform.position = new Vector3(-100.0f, -100.0f, 0);
            Debug.Log("333333");
        }
    }
}
