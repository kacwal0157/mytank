using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TankChooseController
{
    private PlayerMetadataManager playerMetadataManager;

    private Dictionary<int, string> tankPrefabs = new Dictionary<int, string>();
    private List<GameObject> tanksList; //tanks
    private List<GameObject> focusedTankList = new List<GameObject>();
    private GameObject focus;
    private GameObject content;

    private string activeTankPrefab;
   
    public TankChooseController(PublicGameObjects publicGameObjects, PlayerMetadataManager playerMetadataManager)
    {
        this.playerMetadataManager = playerMetadataManager;

        activeTankPrefab = playerMetadataManager.tankPrefabName;
        tanksList = publicGameObjects.tanksList;
        focus = publicGameObjects.focusGameObject;
        content = publicGameObjects.tanksListContent;

        onInit();
    }

    private void onInit()
    {
        initializeDictOfPrefabs();
        setFocusGameObject(PlayerPrefs.GetString("TANK_PREFAB"));
    }

    public void onClick(GameObject obj)
    {
        if (obj.name != activeTankPrefab)
        {
            activeTankPrefab = tankPrefabs[obj.GetInstanceID()];
            playerMetadataManager.tankPrefabName = activeTankPrefab;
            setFocusGameObject(activeTankPrefab);
            selectTank(obj);
        }
    }

    private void initializeDictOfPrefabs()
    {
        //adding every button to equal prefab name
        tankPrefabs.Add(tanksList[0].GetInstanceID(), "Panzer1");
        tankPrefabs.Add(tanksList[2].GetInstanceID(), "Panzer2");
        tankPrefabs.Add(tanksList[4].GetInstanceID(), "Panzer3");
        tankPrefabs.Add(tanksList[6].GetInstanceID(), "Panzer4");
        tankPrefabs.Add(tanksList[8].GetInstanceID(), "Panzer5");
    }

    private void setFocusGameObject(string tankName)
    {
        if(focusedTankList.Count != 0)
        {
            destroyExistingFocusFrames(focusedTankList[0].transform);
            destroyExistingFocusFrames(focusedTankList[1].transform);
            focusedTankList.Clear();
        }

        foreach (GameObject go in tanksList)
        {
            if(go.name.Contains(tankName))
            {
                createFocusFrames(go);
            }
        }
    }

    private void destroyExistingFocusFrames(Transform previousFocusedTank)
    {
        foreach (Transform t in previousFocusedTank)
        {
            if (t.name == "Frame_Focus")
            {
                GameObject.Destroy(t.gameObject);
            }
        }
    }

    private void createFocusFrames(GameObject parent)
    {
        GameObject focusGo = GameObject.Instantiate(focus);
        focusGo.transform.SetParent(parent.transform);
        focusGo.transform.SetSiblingIndex(9);

        focusGo.name = "Frame_Focus";
        focusGo.transform.localScale = new Vector3(1, 1, 1);
        focusGo.transform.localPosition = new Vector3(0, 0, 0);

        focusGo.GetComponent<RectTransform>().offsetMin = new Vector2(-38, -28);
        focusGo.GetComponent<RectTransform>().offsetMax = new Vector2(38, 38);

        focusedTankList.Add(parent);
    }

    private void selectTank(GameObject tank)
    {
        PlayerPrefs.SetString("TANK_PREFAB", tank.name);
    }

    public void onDisable()
    {
        content.GetComponent<RectTransform>().transform.localPosition = new Vector3(0f, -1000f, 0f);
    }
}
