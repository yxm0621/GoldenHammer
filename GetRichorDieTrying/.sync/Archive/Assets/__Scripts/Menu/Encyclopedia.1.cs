using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Encyclopedia : MonoBehaviour {
    
    public GameManager gameMain; 

    public Google2u.ObjList_G2U objData;

    public GameObject pediaItem;

    //

    private List<GameObject> anyButOceanObjList;
    private List<GameObject> anyDuringRainObjList;
    private List<GameObject> cityObjsList;
    private List<GameObject> battleFieldObjList;
    private List<GameObject> countrySideObjList;
    private List<GameObject> wildernessObjList;
    private List<GameObject> OceanObjList;
    private List<GameObject> SpaceObjList;
    private List<GameObject> NightObjList;

	void Start () 
    {
        gameMain = GameManager.manager;

        objData = Google2u.ObjList_G2U.Instance;

        if (objData != null)
        {
            for (int i = 0; i < objData.Rows.Count; i++)
            {
                Debug.Log(objData.Rows[i]._Name + " ");// + objData.Rows[i].);

                //if (objData.Rows[i]._Occurance)

                //if (thisObject.name == objData.Rows[i]._Name)
                //{
                //    if (objData.Rows[i]._Name == "Raijin")
                //    {
                //        if (gameMain.raijinHP == -1)
                //        {
                //            hitPoints = objData.Rows[i]._HitPoints;
                //            gameMain.raijinHP = hitPoints;
                //        }
                //        else
                //        {
                //            hitPoints = gameMain.raijinHP;
                //        }
                //    }
                //    else
                //    {
                //        hitPoints = objData.Rows[i]._HitPoints;
                //    }
                //    value = objData.Rows[i]._Gold;
                //}
            }
        }

	}
	
	void Update () 
    {
	    
	}
}
