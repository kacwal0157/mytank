using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UserEquipmentController
{
    private PublicGameObjects publicGameObjects;
    private PlayerMetadataManager playerMetadataManager;

    private Dictionary<string, GameObject> tankEquipementDict = new Dictionary<string, GameObject>();
    private List<int> panzer = new List<int>();
    private string tankName;

    private Slider healthSlider;
    private Slider attackSlider;
    private Slider defenceSlider;

    private int healthAmount;
    private int attackAmount;
    private int defenceAmount;

    private TextMeshProUGUI healthLevelText;
    private TextMeshProUGUI attackLevelText;
    private TextMeshProUGUI defenceLevelText;

    private int healthLevel;
    private int attackLevel;
    private int defenceLevel;

    public UserEquipmentController(PublicGameObjects publicGameObjects, PlayerMetadataManager playerMetadataManager)
    {
        this.publicGameObjects = publicGameObjects;
        this.playerMetadataManager = playerMetadataManager;

        onInit();
    }

    private void onInit()
    {
        initializeTankEqDict();
    }

    public void onClick(Button btn)
    {
        tankName = btn.name;
        GameObject groupLeft = tankEquipementDict[tankName].transform.GetChild(1).gameObject;
        foreach(Transform t in groupLeft.transform)
        {
            if(t.name == "SliderHealth")
            {
                healthSlider = t.GetComponent<Slider>();
                healthLevelText = healthSlider.transform.GetChild(1).gameObject.transform.GetChild(1).GetComponent<TextMeshProUGUI>();
            }

            if (t.name == "SliderAttack")
            {
                attackSlider = t.GetComponent<Slider>();
                attackLevelText = attackSlider.transform.GetChild(1).gameObject.transform.GetChild(1).GetComponent<TextMeshProUGUI>();
            }

            if (t.name == "SliderDefence")
            {
                defenceSlider = t.GetComponent<Slider>();
                defenceLevelText = defenceSlider.transform.GetChild(1).gameObject.transform.GetChild(1).GetComponent<TextMeshProUGUI>();
            }
        }

        getStatsFromPrefs(tankName);
    }

    private void getStatsFromPrefs(string tankName)
    {
        switch (tankName)
        {
            case "Panzer1":
                panzer = Utilities.convertTankStringToList(playerMetadataManager.panzer1);
                break;
            case "Panzer2":
                panzer = Utilities.convertTankStringToList(playerMetadataManager.panzer2);
                break;
            case "Panzer3":
                panzer = Utilities.convertTankStringToList(playerMetadataManager.panzer3);
                break;
            case "Panzer4":
                panzer = Utilities.convertTankStringToList(playerMetadataManager.panzer4);
                break;
            case "Panzer5":
                panzer = Utilities.convertTankStringToList(playerMetadataManager.panzer5);
                break;
        }

        healthAmount = panzer[0];
        attackAmount = panzer[1];
        defenceAmount = panzer[2];

        healthLevel = panzer[3];
        attackLevel = panzer[4];
        defenceLevel = panzer[5];

        healthLevelText.text = healthLevel.ToString();
        attackLevelText.text = attackLevel.ToString();
        defenceLevelText.text = defenceLevel.ToString();

        if(healthAmount % 50 != 0)
        {
            healthSlider.minValue = healthAmount - (healthAmount % 50f);
            healthSlider.maxValue = healthAmount - (healthAmount % 50f) + 50f;
        }
        else
        {
            healthSlider.minValue = healthAmount;
            healthSlider.maxValue = healthAmount + 50f;
        }

        if (attackAmount % 50 != 0)
        {
            attackSlider.minValue = attackAmount - (attackAmount % 50f);
            attackSlider.maxValue = attackAmount - (attackAmount % 50f) + 50f;
        }
        else
        {
            attackSlider.minValue = attackAmount;
            attackSlider.maxValue = attackAmount + 50f;
        }

        if (healthAmount % 50 != 0)
        {
            defenceSlider.minValue = defenceAmount - (defenceAmount % 50f);
            defenceSlider.maxValue = defenceAmount - (defenceAmount % 50f) + 50f;
        }
        else
        {
            defenceSlider.minValue = defenceAmount;
            defenceSlider.maxValue = defenceAmount + 50f;
        }

        healthSlider.value = healthAmount;
        attackSlider.value = attackAmount;
        defenceSlider.value = defenceAmount;

        checkMaxStatsValue();

        healthSlider.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = healthAmount.ToString() + " / " + healthSlider.maxValue.ToString();
        attackSlider.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = attackAmount.ToString() + " / " + attackSlider.maxValue.ToString();
        defenceSlider.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = defenceAmount.ToString() + " / " + defenceSlider.maxValue.ToString();
    }

    //TO IMPROVE
    public void addStats()
    {
        healthSlider.value += 5;
        attackSlider.value += 5;
        defenceSlider.value += 5;

        checkMaxStatsValue();

        healthSlider.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = healthSlider.value.ToString() + " / " + healthSlider.maxValue.ToString();
        attackSlider.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = attackSlider.value.ToString() + " / " + attackSlider.maxValue.ToString();
        defenceSlider.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = defenceSlider.value.ToString() + " / " + defenceSlider.maxValue.ToString();
    }

    private void checkMaxStatsValue()
    {
        if (healthSlider.value >= healthSlider.maxValue)
        {
            healthSlider.maxValue += 50;
            healthLevel++;
            healthLevelText.text = healthLevel.ToString();
        }

        if (attackSlider.value >= attackSlider.maxValue)
        {
            attackSlider.maxValue += 50;
            attackLevel++;
            attackLevelText.text = attackLevel.ToString();
        }

        if (defenceSlider.value >= defenceSlider.maxValue)
        {
            defenceSlider.maxValue += 50;
            defenceLevel++;
            defenceLevelText.text = defenceLevel.ToString();
        }
    }

    public void saveAllPrefs()
    {
        panzer[0] = (int)healthSlider.value;
        panzer[1] = (int)attackSlider.value;
        panzer[2] = (int)defenceSlider.value;

        switch (tankName)
        {
            case "Panzer1":
                playerMetadataManager.panzer1 = Utilities.convertListToString(panzer);
                break;
            case "Panzer2":
                playerMetadataManager.panzer2 = Utilities.convertListToString(panzer);
                break;
            case "Panzer3":
                playerMetadataManager.panzer3 = Utilities.convertListToString(panzer);
                break;
            case "Panzer4":
                playerMetadataManager.panzer4 = Utilities.convertListToString(panzer);
                break;
            case "Panzer5":
                playerMetadataManager.panzer5 = Utilities.convertListToString(panzer);
                break;
        }
    }

    private void initializeTankEqDict()
    {
        tankEquipementDict.Add("Panzer1", publicGameObjects.panzerEquipment[0]);
        tankEquipementDict.Add("Panzer2", publicGameObjects.panzerEquipment[1]);
        tankEquipementDict.Add("Panzer3", publicGameObjects.panzerEquipment[2]);
        tankEquipementDict.Add("Panzer4", publicGameObjects.panzerEquipment[3]);
        tankEquipementDict.Add("Panzer5", publicGameObjects.panzerEquipment[4]);
    }
}
