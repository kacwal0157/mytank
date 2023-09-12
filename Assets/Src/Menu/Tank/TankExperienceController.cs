using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class TankExperienceController
{
    private PlayerMetadataManager playerMetadataManager;

    private Dictionary<string, GameObject> allTanksSlidersDict = new Dictionary<string, GameObject>();
    private List<WorldPrefabs> worldPrefabs;
    private List<Sprite> fillImages;
    private List<int> panzer = new List<int>();

    private string tankPrefabName;
    private bool upgradePosibilitty = false;

    public TankExperienceController(PublicGameObjects publicGameObjects, PlayerMetadataManager playerMetadataManager)
    {
        this.playerMetadataManager = playerMetadataManager;

        foreach (Transform t in publicGameObjects.tankInfoParent.transform)
        {
            allTanksSlidersDict.Add(t.name, t.GetChild(8).gameObject);
        }

        fillImages = publicGameObjects.fillTankCardsImages;
    }

    public void onStart()
    {
        initializeBasicValues();
    }

    public void onUpdate()
    {
        if(Input.GetMouseButtonDown(1) && upgradePosibilitty)
        {
            levelUp(allTanksSlidersDict[tankPrefabName], panzer);
            levelUp(allTanksSlidersDict[tankPrefabName + "_Clicked"], panzer);
        }
    }

    private void initializeBasicValues()
    {
        worldPrefabs = playerMetadataManager.worldPrefabs;
        tankPrefabName = playerMetadataManager.tankPrefabName;

        foreach (KeyValuePair<string, GameObject> entry in allTanksSlidersDict)
        {
            if(entry.Key.Contains("_Clicked"))
            {
                continue;
            }

            panzer = setStatsFromPrefs(entry.Key);
            getBasicValuesForSelectedTank(entry.Key, panzer);
            getBasicValuesForSelectedTank(entry.Key + "_Clicked", panzer);
        }
    }

    private void getBasicValuesForSelectedTank(string tankName, List<int> panzer)
    {
        GameObject activeTankSlider = allTanksSlidersDict[tankName];
        setLevelProperties(activeTankSlider, panzer);
    }

    private void setLevelProperties(GameObject activeTank, List<int> panzer)
    {
        GameObject fill = activeTank.transform.GetChild(0).gameObject;
        GameObject levelGO = activeTank.transform.GetChild(1).gameObject;
        GameObject levelText = levelGO.transform.GetChild(1).gameObject;
        GameObject expText = activeTank.transform.GetChild(2).gameObject;

        int level = panzer[0];
        int minValue = panzer[3];
        int maxValue = panzer[2];
        int value = panzer[1];

        levelText.GetComponent<TextMeshProUGUI>().text = level.ToString();
        activeTank.GetComponent<Slider>().minValue = minValue;
        activeTank.GetComponent<Slider>().maxValue = maxValue;
        activeTank.GetComponent<Slider>().value = value;

        //TODO: Make another variable - different exp for player and for tanks
        if (value + worldPrefabs[0].levelPrefabs[0].earnedExp >= maxValue)
        {
            if (level + 1 >= 20)
            {
                setMaxLevelStats(levelText, expText, activeTank, maxValue, fill, levelGO);
            }
            else
            {
                expText.GetComponent<TextMeshProUGUI>().text = "Lv." + (level + 1).ToString() + " Upgrade";
                activeTank.GetComponent<Slider>().value = maxValue;

                fill.GetComponent<Image>().sprite = fillImages[1];
                upgradePosibilitty = true;
            }
        }
        else
        {
            activeTank.GetComponent<Slider>().value = value + worldPrefabs[0].levelPrefabs[0].earnedExp;
            expText.GetComponent<TextMeshProUGUI>().text = (activeTank.GetComponent<Slider>().value).ToString() + "/" + maxValue.ToString();
        }
    }

    private void setMaxLevelStats(GameObject levelText, GameObject expText, GameObject activeTank, int maxValue, GameObject fill, GameObject levelGO)
    {
        levelText.GetComponent<TextMeshProUGUI>().text = "20";
        expText.GetComponent<TextMeshProUGUI>().text = "MAX!";

        activeTank.GetComponent<Slider>().value = maxValue;
        fill.GetComponent<Image>().sprite = fillImages[2];

        levelGO.transform.GetChild(0).gameObject.SetActive(true);
    }

    private void levelUp(GameObject activeTank, List<int> panzer)
    {
        int level = panzer[0];
        GameObject fill = activeTank.transform.GetChild(0).gameObject;
        GameObject levelGO = activeTank.transform.GetChild(1).gameObject;
        GameObject levelText = levelGO.transform.GetChild(1).gameObject;
        GameObject expText = activeTank.transform.GetChild(2).gameObject;

        levelText.GetComponent<TextMeshProUGUI>().text = (level + 1).ToString();
        activeTank.GetComponent<Slider>().maxValue += (int)(25 * Mathf.Pow(2, level));
        activeTank.GetComponent<Slider>().minValue = activeTank.GetComponent<Slider>().value;

        expText.GetComponent<TextMeshProUGUI>().text = (activeTank.GetComponent<Slider>().value).ToString() + "/" + (activeTank.GetComponent<Slider>().maxValue).ToString();
        fill.GetComponent<Image>().sprite = fillImages[0];
        upgradePosibilitty = false;
    }

    private List<int> setStatsFromPrefs(string tankName)
    {
        List<int> panzer = new List<int>();

        switch (tankName)
        {
            case "Panzer1":
                panzer = Utilities.convertTankStringToList(playerMetadataManager.lvlPanzer1);
                break;
            case "Panzer2":
                panzer = Utilities.convertTankStringToList(playerMetadataManager.lvlPanzer2);
                break;
            case "Panzer3":
                panzer = Utilities.convertTankStringToList(playerMetadataManager.lvlPanzer3);
                break;
            case "Panzer4":
                panzer = Utilities.convertTankStringToList(playerMetadataManager.lvlPanzer4);
                break;
            case "Panzer5":
                panzer = Utilities.convertTankStringToList(playerMetadataManager.lvlPanzer5);
                break;
        }

        return panzer;
    }

    public void saveAllPrefs()
    {
        foreach (KeyValuePair<string, GameObject> entry in allTanksSlidersDict)
        {
            if (entry.Key.Contains("_Clicked"))
            {
                continue;
            }

            GameObject levelGO = entry.Value.transform.GetChild(1).gameObject;
            GameObject levelText = levelGO.transform.GetChild(1).gameObject;

            panzer[0] = int.Parse(levelText.GetComponent<TextMeshProUGUI>().text);
            panzer[1] = (int)entry.Value.GetComponent<Slider>().value;
            panzer[2] = (int)entry.Value.GetComponent<Slider>().maxValue;
            panzer[3] = (int)entry.Value.GetComponent<Slider>().minValue;

            switch (entry.Key)
            {
                case "Panzer1":
                    playerMetadataManager.lvlPanzer1 = Utilities.convertListToString(panzer);
                    break;
                case "Panzer2":
                    playerMetadataManager.lvlPanzer2 = Utilities.convertListToString(panzer);
                    break;
                case "Panzer3":
                    playerMetadataManager.lvlPanzer3 = Utilities.convertListToString(panzer);
                    break;
                case "Panzer4":
                    playerMetadataManager.lvlPanzer4 = Utilities.convertListToString(panzer);
                    break;
                case "Panzer5":
                    playerMetadataManager.lvlPanzer5 = Utilities.convertListToString(panzer);
                    break;
            }
        }
    }
}
