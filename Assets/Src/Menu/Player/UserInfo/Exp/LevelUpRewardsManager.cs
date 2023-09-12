using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelUpRewardsManager
{
    private PlayerMetadataManager playerMetadataManager;
    private PublicGameObjects publicGameObjects;
    private List<Item> items = new List<Item>();

    private const string playerInventory = "PLAYER_INVENTORY";

    public LevelUpRewardsManager(PlayerMetadataManager playerMetadataManager, PublicGameObjects publicGameObjects)
    {
        this.playerMetadataManager = playerMetadataManager;
        this.publicGameObjects = publicGameObjects;
    }

    public void addRewardsToUserItems(LevelUpRewards.LevelRewards levelUpRews)
    {
        items = Utilities.convertStringToListOfItem(playerMetadataManager.getInventory());
        var listOfIndexes = new List<int>();

        foreach (Item item in items)
        {
            if(item.name.Contains("Empty") && levelUpRews.rewards.Count != listOfIndexes.Count)
            {
                var index = items.IndexOf(item);
                listOfIndexes.Add(index);
            }
            else if(levelUpRews.rewards.Count == listOfIndexes.Count)
            {
                break;
            }
        }

        for(int i = 0; i < listOfIndexes.Count; i++)
        {
            if (levelUpRews.rewards[i].rewardName.Equals("Gold") || levelUpRews.rewards[i].rewardName.Equals("Gems") || levelUpRews.rewards[i].rewardName.Equals("MaxEnergy"))
            {
                handleSpecialRewards(levelUpRews.rewards[i].rewardName, levelUpRews, i);
            }
            else
            {
                items[listOfIndexes[i]] = ItemDict.getItem(levelUpRews.rewards[i].rewardName);
            }
        }

        PlayerPrefs.SetString(playerInventory, Utilities.convertListOfItemToString(items));
    }

    private void handleSpecialRewards(string rewardName, LevelUpRewards.LevelRewards levelRewards, int index)
    {
        switch(rewardName)
        {
            case "Gold":
                playerMetadataManager.goldAmount += levelRewards.rewards[index].value;
                StatusBarManager.updateGoldOnStatusBars(publicGameObjects, playerMetadataManager);
                break;
            case "Gems":
                playerMetadataManager.gemAmount += levelRewards.rewards[index].value;
                StatusBarManager.updateGemsOnStatusBars(publicGameObjects, playerMetadataManager);
                break;
            case "MaxEnergy":
                playerMetadataManager.maxEnergyAmount += levelRewards.rewards[index].value;
                StatusBarManager.updateMaxEnergyOnStatusBars(publicGameObjects, playerMetadataManager);
                break;
        }
    }
}
