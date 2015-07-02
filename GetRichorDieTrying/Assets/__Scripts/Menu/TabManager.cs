using UnityEngine;
using System.Collections;

public class TabManager : MonoBehaviour {

    public GameObject tabCity;
    public GameObject tabRain;
    public GameObject tabBattlefield;
    public GameObject tabCountryside;
    public GameObject tabWilderness;
    public GameObject tabOcean;
    public GameObject tabSpace;
    public GameObject tabNight;
    public GameObject tabAny;

	
    void Start () 
    {
	}
	
	void Update () 
    {
	}

    public void ChangeTabitem(string gameObjectName)
    {
        if (gameObjectName == tabCity.name)
        {
            //tabCity.GetComponent<ClickTabObject>().PutAway();
            tabRain.GetComponent<ClickTabObject>().PutAway();
            tabBattlefield.GetComponent<ClickTabObject>().PutAway();
            tabCountryside.GetComponent<ClickTabObject>().PutAway();
            tabWilderness.GetComponent<ClickTabObject>().PutAway();
            tabOcean.GetComponent<ClickTabObject>().PutAway();
            tabSpace.GetComponent<ClickTabObject>().PutAway();
            tabNight.GetComponent<ClickTabObject>().PutAway();
            tabAny.GetComponent<ClickTabObject>().PutAway();
        }
        else if (gameObjectName == tabRain.name)
        {
            tabCity.GetComponent<ClickTabObject>().PutAway();
            //tabRain.GetComponent<ClickTabObject>().PutAway();
            tabBattlefield.GetComponent<ClickTabObject>().PutAway();
            tabCountryside.GetComponent<ClickTabObject>().PutAway();
            tabWilderness.GetComponent<ClickTabObject>().PutAway();
            tabOcean.GetComponent<ClickTabObject>().PutAway();
            tabSpace.GetComponent<ClickTabObject>().PutAway();
            tabNight.GetComponent<ClickTabObject>().PutAway();
            tabAny.GetComponent<ClickTabObject>().PutAway();
        }
        else if (gameObjectName == tabBattlefield.name)
        {
            tabCity.GetComponent<ClickTabObject>().PutAway();
            tabRain.GetComponent<ClickTabObject>().PutAway();
            //tabBattlefield.GetComponent<ClickTabObject>().PutAway();
            tabCountryside.GetComponent<ClickTabObject>().PutAway();
            tabWilderness.GetComponent<ClickTabObject>().PutAway();
            tabOcean.GetComponent<ClickTabObject>().PutAway();
            tabSpace.GetComponent<ClickTabObject>().PutAway();
            tabNight.GetComponent<ClickTabObject>().PutAway();
            tabAny.GetComponent<ClickTabObject>().PutAway();
        }
        else if(gameObjectName == tabCountryside.name)
        {
            tabCity.GetComponent<ClickTabObject>().PutAway();
            tabRain.GetComponent<ClickTabObject>().PutAway();
            tabBattlefield.GetComponent<ClickTabObject>().PutAway();
            //tabCountryside.GetComponent<ClickTabObject>().PutAway();
            tabWilderness.GetComponent<ClickTabObject>().PutAway();
            tabOcean.GetComponent<ClickTabObject>().PutAway();
            tabSpace.GetComponent<ClickTabObject>().PutAway();
            tabNight.GetComponent<ClickTabObject>().PutAway();
            tabAny.GetComponent<ClickTabObject>().PutAway();
        }
        else if (gameObjectName == tabWilderness.name)
        {
            tabCity.GetComponent<ClickTabObject>().PutAway();
            tabRain.GetComponent<ClickTabObject>().PutAway();
            tabBattlefield.GetComponent<ClickTabObject>().PutAway();
            tabCountryside.GetComponent<ClickTabObject>().PutAway();
            //tabWilderness.GetComponent<ClickTabObject>().PutAway();
            tabOcean.GetComponent<ClickTabObject>().PutAway();
            tabSpace.GetComponent<ClickTabObject>().PutAway();
            tabNight.GetComponent<ClickTabObject>().PutAway();
            tabAny.GetComponent<ClickTabObject>().PutAway();
        }
        else if (gameObjectName == tabOcean.name)
        {
            tabCity.GetComponent<ClickTabObject>().PutAway();
            tabRain.GetComponent<ClickTabObject>().PutAway();
            tabBattlefield.GetComponent<ClickTabObject>().PutAway();
            tabCountryside.GetComponent<ClickTabObject>().PutAway();
            tabWilderness.GetComponent<ClickTabObject>().PutAway();
            //tabOcean.GetComponent<ClickTabObject>().PutAway();
            tabSpace.GetComponent<ClickTabObject>().PutAway();
            tabNight.GetComponent<ClickTabObject>().PutAway();
            tabAny.GetComponent<ClickTabObject>().PutAway();
        }
        else if (gameObjectName == tabSpace.name)
        {
            tabCity.GetComponent<ClickTabObject>().PutAway();
            tabRain.GetComponent<ClickTabObject>().PutAway();
            tabBattlefield.GetComponent<ClickTabObject>().PutAway();
            tabCountryside.GetComponent<ClickTabObject>().PutAway();
            tabWilderness.GetComponent<ClickTabObject>().PutAway();
            tabOcean.GetComponent<ClickTabObject>().PutAway();
            //tabSpace.GetComponent<ClickTabObject>().PutAway();
            tabNight.GetComponent<ClickTabObject>().PutAway();
            tabAny.GetComponent<ClickTabObject>().PutAway();
        }
        else if (gameObjectName == tabNight.name)
        {
            tabCity.GetComponent<ClickTabObject>().PutAway();
            tabRain.GetComponent<ClickTabObject>().PutAway();
            tabBattlefield.GetComponent<ClickTabObject>().PutAway();
            tabCountryside.GetComponent<ClickTabObject>().PutAway();
            tabWilderness.GetComponent<ClickTabObject>().PutAway();
            tabOcean.GetComponent<ClickTabObject>().PutAway();
            tabSpace.GetComponent<ClickTabObject>().PutAway();
            //tabNight.GetComponent<ClickTabObject>().PutAway();
            tabAny.GetComponent<ClickTabObject>().PutAway();
        }
        else if (gameObjectName == tabAny.name)
        {
            tabCity.GetComponent<ClickTabObject>().PutAway();
            tabRain.GetComponent<ClickTabObject>().PutAway();
            tabBattlefield.GetComponent<ClickTabObject>().PutAway();
            tabCountryside.GetComponent<ClickTabObject>().PutAway();
            tabWilderness.GetComponent<ClickTabObject>().PutAway();
            tabOcean.GetComponent<ClickTabObject>().PutAway();
            tabSpace.GetComponent<ClickTabObject>().PutAway();
            tabNight.GetComponent<ClickTabObject>().PutAway();
            //tabAny.GetComponent<ClickTabObject>().PutAway();
        }
    }
}
