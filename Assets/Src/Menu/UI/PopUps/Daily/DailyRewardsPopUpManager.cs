using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DailyRewardsPopUpManager
{
    private DailyRewards dailyRewards;
    private PublicGameObjects publicGameObjects;
    private LastLoginDateManager lastLoginDateManager;
    private PlayerMetadataManager playerMetadataManager;
    private DailyRewardsItemManager dailyRewardsItemManager;

    private List<Transform> rewardList = new List<Transform>();
    private GameObject tomorrowItemMessageBox;

    private int claimedDailyRewards;

    public DailyRewardsPopUpManager(DailyRewards dailyRewards, PublicGameObjects publicGameObjects, LastLoginDateManager lastLoginDateManager, PlayerMetadataManager playerMetadataManager, DailyRewardsItemManager dailyRewardsItemManager)
    {
        this.dailyRewards = dailyRewards;
        this.publicGameObjects = publicGameObjects;
        this.lastLoginDateManager = lastLoginDateManager;
        this.playerMetadataManager = playerMetadataManager;
        this.dailyRewardsItemManager = dailyRewardsItemManager;

        onInit();
    }

    private void onInit()
    {
        rewardList.Clear();

        var rewardsList = publicGameObjects.dailyRewardsPopUp.transform.GetChild(1).transform.GetChild(4).transform.GetChild(0);
        tomorrowItemMessageBox = publicGameObjects.tomorrowItemMessageBox;
        claimedDailyRewards = playerMetadataManager.claimedDailyRewards;

        foreach (Transform reward in rewardsList)
        {
            rewardList.Add(reward);
        }

        initializeRewards();
        checkDailyPopUpAvailability();
        loadRewards();
    }

    private void initializeRewards()
    {
        var finalReward = publicGameObjects.dailyRewardsPopUp.transform.GetChild(1).transform.GetChild(4).transform.GetChild(1);
        rewardList.Add(finalReward);

        for(int index = 0; index < rewardList.Count; index++)
        {
            var rewardName = dailyRewards.weeks[0].days[index].rewardName;
            var icon = Resources.Load<Sprite>("Textures/Icons/Items/Icon_" + rewardName);
            var value = dailyRewards.weeks[0].days[index].value;

            rewardList[index].transform.GetChild(1).GetComponent<Image>().sprite = icon;
            rewardList[index].transform.GetChild(1).GetComponent<RectTransform>().sizeDelta = new Vector2(icon.texture.width, icon.texture.height);

            if (dailyRewards.weeks[0].days[index].rewardName.Contains("Chest"))
            {
                rewardList[index].transform.GetChild(1).GetComponent<RectTransform>().localScale = Vector3.one;
                rewardList[index].transform.GetChild(3).GetComponent<TextMeshProUGUI>().text = rewardName + " x" + value.ToString();
            }
            else
            {
                rewardList[index].transform.GetChild(1).GetComponent<RectTransform>().localScale = new Vector3(1.2f, 1.2f, 1.2f);
                rewardList[index].transform.GetChild(3).GetComponent<TextMeshProUGUI>().text = value.ToString();
            }
        }

        int counter = 0;
        foreach(Transform reward in rewardList)
        {
            reward.GetComponent<Button>().onClick.AddListener(delegate { getDailyRewardButtonProperties(reward, dailyRewards.weeks[0].days[counter]); });
        }

    }

    private void checkDailyPopUpAvailability()
    {
        if (lastLoginDateManager.enableDailyRewardPopUp)
        {
            MenuManager.getInstance.StartCoroutine(waitAndDisplay(3));
        }
    }

    private IEnumerator waitAndDisplay(float delay)
    {
        yield return new WaitForSeconds(delay);

        MenuTankActivator.setTankActivation(false, publicGameObjects);
        publicGameObjects.dailyRewardsPopUp.SetActive(true);
    }

    private void loadRewards()
    {
        Debug.Log("Day strike: " + playerMetadataManager.logInDayStrike);
        Debug.Log("Claimed daily rewards: " + claimedDailyRewards);
        var dayStrike = playerMetadataManager.logInDayStrike;

        if (playerMetadataManager.firstPlay || claimedDailyRewards == 0)
        {
            rewardList[0].GetComponent<Button>().interactable = true;
            return;
        }
    
        for (int index = 0; index < claimedDailyRewards; index++)
        {
            rewardList[index].GetChild(4).gameObject.SetActive(true);
            rewardList[index].GetChild(5).gameObject.SetActive(false);
        }

        if (claimedDailyRewards == dayStrike)
        {
            setTomorrowMessageBoxProperties(claimedDailyRewards);
        }
        else
        {
            rewardList[dayStrike - 1].GetComponent<Button>().interactable = true;
            rewardList[dayStrike - 1].GetChild(5).gameObject.SetActive(true);
        }
    }

    private void getDailyRewardButtonProperties(Transform reward, DailyRewards.Days dailyReward)
    {
        reward.GetChild(4).gameObject.SetActive(true);
        reward.GetChild(5).gameObject.SetActive(false);

        setTomorrowMessageBoxProperties(claimedDailyRewards + 1);
        dailyRewardsItemManager.getClaimedDailyItem(dailyReward);

        playerMetadataManager.claimedDailyRewards = claimedDailyRewards + 1;

        if(playerMetadataManager.claimedDailyRewards == 7)
        {
            playerMetadataManager.claimedDailyRewards = 1;
        }
    }

    private void setTomorrowMessageBoxProperties(int index)
    {
        tomorrowItemMessageBox.transform.SetParent(rewardList[index]);
        tomorrowItemMessageBox.SetActive(true);
        tomorrowItemMessageBox.GetComponent<RectTransform>().anchoredPosition3D = new Vector3(0f, 27f, 0f);
    }
}
