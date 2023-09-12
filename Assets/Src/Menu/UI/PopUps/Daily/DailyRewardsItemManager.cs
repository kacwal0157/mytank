using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DailyRewardsItemManager
{
    private PlayerMetadataManager playerMetadataManager;
    private PublicGameObjects publicGameObjects;

    private const string playerInventory = "PLAYER_INVENTORY";

    public DailyRewardsItemManager(PlayerMetadataManager playerMetadataManager, PublicGameObjects publicGameObjects)
    {
        this.playerMetadataManager = playerMetadataManager;
        this.publicGameObjects = publicGameObjects;
    }

    public void getClaimedDailyItem(DailyRewards.Days dailyReward)
    {
        if(dailyReward.rewardName.Equals("Gold"))
        {
            var gold = playerMetadataManager.goldAmount;
            gold += dailyReward.value;
            playerMetadataManager.goldAmount = gold;

            StatusBarManager.updateGoldOnStatusBars(publicGameObjects, playerMetadataManager);
            return;
        }

        if(dailyReward.rewardName.Equals("Gems"))
        {
            var gem = playerMetadataManager.gemAmount;
            gem += dailyReward.value;
            playerMetadataManager.goldAmount = gem;

            StatusBarManager.updateGemsOnStatusBars(publicGameObjects, playerMetadataManager);
            return;
        }

        addRewardToItems(dailyReward);
    }

    private void addRewardToItems(DailyRewards.Days dailyReward)
    {
        string items = PlayerPrefs.GetString(playerInventory);
        List<Item> itemsList = Utilities.convertStringToListOfItem(items);
        Item reward = ItemDict.getItem(dailyReward.rewardName);

        for (int index = 0; index < itemsList.Count; index++)
        {
            if (itemsList[index].name.Equals("Empty_Item"))
            {
                itemsList[index] = reward;
                break;
            }
        }

        PlayerPrefs.SetString(playerInventory, Utilities.convertListOfItemToString(itemsList));
    }
}
