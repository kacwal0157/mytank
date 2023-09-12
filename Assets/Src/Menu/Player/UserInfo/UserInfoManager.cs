using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UserInfoManager
{
    private PlayerMetadataManager meta;
    private ExperienceManager experienceManager;

    private TextMeshProUGUI usernameText;
    private TextMeshProUGUI energyValue;

    private int energyAmount;
    private int maxEnergyAmount;
    private string username;

    public UserInfoManager(PlayerMetadataManager meta, PublicGameObjects publicGameObjects, ExperienceManager experienceManager)
    {
        this.meta = meta;
        this.experienceManager = experienceManager;
;
        usernameText = publicGameObjects.lobbyUsername;
        energyValue = publicGameObjects.lobbyEnergyValue;

        username = meta.username;
        energyAmount = meta.energyAmount;
        maxEnergyAmount = meta.maxEnergyAmount;
    }

    public void setDefaultUserInfoStatement()
    {
        experienceManager.getUserExperienceInfo();
        usernameText.text = username;
        energyValue.text = energyAmount.ToString() + " / " + maxEnergyAmount.ToString();
    }

    public void takeEnergy(int levelEnergyCost)
    {
        if (energyAmount == maxEnergyAmount)
        {
            //TODO: stop regenerating energy
        }
        else
        {
            //TODO: start regenerating energy
        }

        updateEnergyAmountAndText(levelEnergyCost);
    }

    public bool checkEnergyAmount(int levelEnergyCost)
    {
        var enoughEnergy = true;

        if (levelEnergyCost > energyAmount)
        {
            //TODO: Show popUp with DebugLog info
            Debug.Log("You don't have enough energy to play this level!");
            enoughEnergy = false;
        }

        return enoughEnergy;
    }

    private void updateEnergyAmountAndText(int levelEnergyCost)
    {
        energyAmount -= levelEnergyCost;
        energyValue.text = energyAmount.ToString() + " / " + maxEnergyAmount.ToString();
    }

    public void saveAllPrefs()
    {
        experienceManager.saveUserExpInfo();
        meta.energyAmount = energyAmount;
    }
}
