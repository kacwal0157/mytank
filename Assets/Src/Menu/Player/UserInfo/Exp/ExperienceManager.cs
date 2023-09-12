using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ExperienceManager
{
    private PlayerMetadataManager playerMetadataManager;
    private LevelUpManager levelUpManager;

    private TextMeshProUGUI expTextOnSlider;
    private TextMeshProUGUI userLevel;
    private Slider expValueSlider;

    private int currentExp;
    private int expForPreviousLevel;
    private int expToLevelUp;
    private int playerLevel;
    private int lastCompletedLevel;

    //"Arbitrary numbers(Multipliers)"
    public float additionMultiplier = 300;
    public float powerMultiplier = 2;
    public float divisionMultiplier = 7;

    public ExperienceManager(PlayerMetadataManager playerMetadataManager, PublicGameObjects publicGameObjects, LevelUpManager levelUpManager)
    {
        this.playerMetadataManager = playerMetadataManager;
        this.levelUpManager = levelUpManager;

        expValueSlider = publicGameObjects.userExpSlider;
        expTextOnSlider = publicGameObjects.userExpTextOnSlider;
        userLevel = publicGameObjects.userLevel;

        currentExp = playerMetadataManager.currentExp;
        expForPreviousLevel = playerMetadataManager.previousExpNeeded;
        expToLevelUp = playerMetadataManager.expToLevelUp;
        playerLevel = playerMetadataManager.playerLevel;
        lastCompletedLevel = playerMetadataManager.lastCompletedLevel;
    }

    public void getUserExperienceInfo()
    {
        if(expToLevelUp == 0 && playerLevel == 1)
        {
            expToLevelUp = calculateRequiredExp();
        }

        if(lastCompletedLevel != 0)
        {
            addUserExperience(lastCompletedLevel);
            lastCompletedLevel = 0;
        }

        expValueSlider.minValue = expForPreviousLevel;
        expValueSlider.maxValue = expToLevelUp;
        expValueSlider.value = currentExp;

        expTextOnSlider.text = currentExp.ToString() + " / " + expToLevelUp.ToString();
        userLevel.text = playerLevel.ToString();
    }

    //READY FOR FUTURE IMPORT
    private void addUserExperience(float expGained, int passedLevel)
    {
        gainExperienceScalable(expGained, passedLevel);

        if (currentExp >= expToLevelUp)
        {
            levelUp();
            levelUpManager.handleLevelUp(playerLevel);
        }

        setExpSlider();
    }

    private void addUserExperience(int passedLevel)
    {
        float expGained = (1 + (7 * Mathf.Sqrt(passedLevel))) * 2;
        gainExperienceScalable(expGained, passedLevel);

        if (currentExp >= expToLevelUp)
        {
            levelUp();
            levelUpManager.handleLevelUp(playerLevel);
        }

        setExpSlider();
    }

    private void setExpSlider()
    {
        expTextOnSlider.text = currentExp.ToString() + " / " + expToLevelUp.ToString();
        expValueSlider.value = currentExp;
    }

    private void gainExperienceScalable(float expGained, int passedLevel)
    {
        if (playerLevel < passedLevel)
        {
            float multiplier = 1 + (playerLevel - passedLevel) * 0.1f;
            currentExp += Mathf.RoundToInt(expGained * multiplier);
        }
        else
        {
            currentExp += Mathf.RoundToInt(expGained);
        }
    }

    private void levelUp()
    {
        playerLevel++;
        userLevel.text = playerLevel.ToString();

        expValueSlider.minValue = expToLevelUp;
        expForPreviousLevel = expToLevelUp;

        expToLevelUp = calculateRequiredExp();
        expValueSlider.maxValue = expToLevelUp;
    }

    private int calculateRequiredExp()
    {
        //https://oldschool.runescape.wiki/w/Experience
        int solveForRequiredExp = 0;
        for(int levelCycle = 1; levelCycle <= playerLevel; levelCycle++)
        {
            solveForRequiredExp += (int)Mathf.Floor(levelCycle + additionMultiplier * Mathf.Pow(powerMultiplier, levelCycle / divisionMultiplier));
        }

        return solveForRequiredExp / 4;
    }

    public void saveUserExpInfo()
    {
        playerMetadataManager.currentExp = currentExp;
        playerMetadataManager.previousExpNeeded = expForPreviousLevel;
        playerMetadataManager.expToLevelUp = expToLevelUp;
        playerMetadataManager.playerLevel = playerLevel;
        playerMetadataManager.lastCompletedLevel = lastCompletedLevel;
    }
}
