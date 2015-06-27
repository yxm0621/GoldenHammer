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

    private GameObject backArrow;

    private Vector3 pediaStartPoint;

    //

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
        pediaStartPoint = new Vector3(1.0f, -4.0f, 0);
        gameMain = GameManager.manager;
        objData = Google2u.ObjList_G2U.Instance;

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
        backArrow = Instantiate(Resources.Load("Menu/General/BackToMenu")) as GameObject;
        backArrow.transform.position = pediaStartPoint - new Vector3(3.0f, 0, 0);
        backArrow.name = "BackArrowFromPediaToMainMenu";
    }

    void CreateContent()
    {
        if (objData != null)
        {
            for (int i = 0; i < objData.Rows.Count; i++)
            {
                /*** Eliminating things that are not supposed to be on the list ***/
                if (objData.Rows[i]._Name == "Checkpoint")
                {
                    continue;
                }

                /************************************************************************/
                GameObject pediaItem = Instantiate(pediaItemPrefab);//Resources.Load("Menu/Encyclopedia/PediaItem")) as GameObject;
                pediaItem.name = "PediaItem_" + objData.Rows[i]._Name;
                pediaItem.transform.SetParent(GameObject.Find("PediaPage").transform);
                pediaItem.transform.position = pediaStartPoint;

                GameObject tmpItemNameObj = GameObject.Find("ItemName");
                if (tmpItemNameObj == null) Debug.Log("null!");
                else Debug.Log("not null");
                tmpItemNameObj.GetComponent<TextMesh>().text = objData.Rows[i]._Name;
                tmpItemNameObj.name = "ItemName_" + objData.Rows[i]._Name;

                GameObject tmpItemDscrObj = GameObject.Find("ItemDescription");
                //TODO//tmpItemDscrObj.GetComponent<TextMesh>().text = objData.Rows[i]._Description;
                tmpItemDscrObj.name = "ItemDescription_" + objData.Rows[i]._Name;

                GameObject tmpItemHPObj = GameObject.Find("ItemHP");
                tmpItemHPObj.GetComponent<TextMesh>().text = objData.Rows[i]._HitPoints.ToString();
                tmpItemHPObj.name = "ItemHP_" + objData.Rows[i]._Name;

                GameObject tmpItemValuObj = GameObject.Find("ItemValue");
                tmpItemValuObj.GetComponent<TextMesh>().text = objData.Rows[i]._Gold.ToString();
                tmpItemValuObj.name = "ItemValue_" + objData.Rows[i]._Name;

                if (objData.Rows[i]._Location.Contains("City"))
                {
                    pediaItem.transform.position += new Vector3(0, -i * 3.0f, 0);
                    cityObjList.Add(pediaItem);
                }
                if (objData.Rows[i]._Location.Contains("Rain"))
                {
                    pediaItem.transform.position += new Vector3(0, -i * 3.0f, 0);
                    rainObjList.Add(pediaItem);
                }
                if (objData.Rows[i]._Location.Contains("Battlefield"))
                {
                    pediaItem.transform.position += new Vector3(0, -i * 3.0f, 0);
                    battleFieldObjList.Add(pediaItem);
                }
                if (objData.Rows[i]._Location.Contains("Countryside"))
                {
                    pediaItem.transform.position += new Vector3(0, -i * 3.0f, 0);
                    countrySideObjList.Add(pediaItem);
                }
                if (objData.Rows[i]._Location.Contains("Wilderness"))
                {
                    pediaItem.transform.position += new Vector3(0, -i * 3.0f, 0);
                    wildernessObjList.Add(pediaItem);
                }
                if (objData.Rows[i]._Location.Contains("Ocean"))
                {
                    pediaItem.transform.position += new Vector3(0, -i * 3.0f, 0);
                    oceanObjList.Add(pediaItem);
                }
                if (objData.Rows[i]._Location.Contains("Space"))
                {
                    pediaItem.transform.position += new Vector3(0, -i * 3.0f, 0);
                    spaceObjList.Add(pediaItem);
                }
                if (objData.Rows[i]._Location.Contains("Night"))
                {
                    pediaItem.transform.position += new Vector3(0, -i * 3.0f, 0);
                    nightObjList.Add(pediaItem);
                }
                if (objData.Rows[i]._Location.Contains("Any"))
                {
                    pediaItem.transform.position += new Vector3(0, -i * 3.0f, 0);
                    anyObjList.Add(pediaItem);
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

    void Update()
    {

    }
}
