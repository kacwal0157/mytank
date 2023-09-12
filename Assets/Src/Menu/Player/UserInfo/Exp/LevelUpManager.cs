using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelUpManager
{
    private LevelUpRewards levelUpRewards;
    private LevelUpRewardsManager levelUpRewardsManager;

    private GameObject levelUpGO;
    private GameObject lobbyGO;
    private Transform rewardsParent;
    private Button continueBtn;

    public LevelUpManager(PublicGameObjects publicGameObjects, LevelUpRewardsManager levelUpRewardsManager)
    {
        this.levelUpRewardsManager = levelUpRewardsManager;
        levelUpRewards = JsonReader.loadLevelUpRewards();

        levelUpGO = publicGameObjects.levelUpGO;
        lobbyGO = publicGameObjects.lobby;
        rewardsParent = levelUpGO.transform.GetChild(3).transform.GetChild(1);

        continueBtn = levelUpGO.GetComponentInChildren<Button>();
        continueBtn.onClick.AddListener(hideLevelUpScreen);
    }

    public void handleLevelUp(int nextLevel)
    {
        TextMeshProUGUI level = levelUpGO.transform.GetChild(2).transform.GetChild(2).GetComponent<TextMeshProUGUI>();
        level.text = nextLevel.ToString();

        int listIndex = nextLevel - 2;
        for(int i = 0; i < levelUpRewards.levels[listIndex].rewardsCount; i++)
        {
            GameObject rewardPrefab = GameObject.Instantiate(Utilities.getPrefabFromResources("Menu/Levels/Reward_Item"), rewardsParent);

            Image icon = rewardPrefab.transform.GetChild(0).GetComponent<Image>();
            RectTransform iconGO = rewardPrefab.transform.GetChild(0).GetComponent<RectTransform>();
            TextMeshProUGUI rewardValue = rewardPrefab.transform.GetChild(2).GetComponent<TextMeshProUGUI>();

            if(levelUpRewards.levels[listIndex].rewards[i].rewardName.Equals("MaxEnergy"))
            {
                icon.sprite = Resources.Load<Sprite>("Textures/Icons/Items/Icon_Energy");
                rewardValue.text = "MAX + " + levelUpRewards.levels[listIndex].rewards[i].value.ToString();
            }
            else
            {
                icon.sprite = Resources.Load<Sprite>("Textures/Icons/Items/Icon_" + levelUpRewards.levels[listIndex].rewards[i].rewardName);
                rewardValue.text = levelUpRewards.levels[listIndex].rewards[i].value.ToString();
            }

            iconGO.sizeDelta = new Vector2(icon.sprite.texture.width, icon.sprite.texture.height);
        }

        levelUpRewardsManager.addRewardsToUserItems(levelUpRewards.levels[listIndex]);
        showLevelUpScreen();
    }

    private void showLevelUpScreen()
    {
        lobbyGO.transform.GetChild(1).gameObject.SetActive(false);
        levelUpGO.SetActive(true);
    }

    private void hideLevelUpScreen()
    {
        lobbyGO.transform.GetChild(1).gameObject.SetActive(true);
        levelUpGO.SetActive(false);

        foreach(Transform t in rewardsParent)
        {
            GameObject.Destroy(t.gameObject);
        }
    }
}
