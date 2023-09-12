using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class EndGameScreenManager
{
    private const string imageBackground = "Preview/Frame/Frame_ItemFrame03_Navy";
    private const string playerInventory = "PLAYER_INVENTORY";

    private GameObject stars;
    private GameObject rewards;
    private TMP_FontAsset LilitaOne40;

    public EndGameScreenManager(PublicGameObjects publicGameObjects)
    {
        LilitaOne40 = publicGameObjects.LilitaOne40;

        onInit(publicGameObjects.gameResultScreen);
    }

    private void onInit(GameObject resultScreen)
    {
        foreach(Transform t in resultScreen.transform)
        {
            switch(t.name)
            {
                case "Stars":
                    stars = t.gameObject;
                    break;
                case "Rewards":
                    rewards = t.gameObject;
                    break;
            }
        }
    }

    public void getEndedLevelProperties(LevelPrefabs levelPrefabs, List<string> rewardItems)
    {
        handleStars(levelPrefabs);
        handleRewards(rewardItems);
    }

    private void handleStars(LevelPrefabs levelPrefabs)
    {
        switch (levelPrefabs.starNumber)
        {
            case 1:
                stars.transform.GetChild(1).gameObject.SetActive(true);
                stars.transform.GetChild(2).gameObject.SetActive(false);
                stars.transform.GetChild(3).gameObject.SetActive(false);
                break;
            case 2:
                stars.transform.GetChild(1).gameObject.SetActive(true);
                stars.transform.GetChild(2).gameObject.SetActive(true);
                stars.transform.GetChild(3).gameObject.SetActive(false);
                break;
            case 3:
                stars.transform.GetChild(1).gameObject.SetActive(true);
                stars.transform.GetChild(2).gameObject.SetActive(true);
                stars.transform.GetChild(3).gameObject.SetActive(true);
                break;
        }
    }

    private void handleRewards(List<string> rewardItems)
    {
        List<Item> rewardsList = initializeListOfRewards(rewardItems);
        Transform rewardsParent = rewards.transform.GetChild(1).transform;

        foreach(Item item in rewardsList)
        {
            var index = rewardsList.IndexOf(item);
            var go = Utilities.getPrefabFromResources("Menu/Items/" + item.name);

            go = GameObject.Instantiate(go, Vector3.zero, Quaternion.identity, rewardsParent);
            go.transform.localPosition = Vector3.zero;

            go.GetComponent<RectTransform>().pivot = new Vector2(0.5f, 0.5f);
            go.GetComponent<RectTransform>().sizeDelta = new Vector2(182f, 200f);
            go.GetComponent<Image>().sprite = Resources.Load<Sprite>(imageBackground);
            go.GetComponent<Image>().type = Image.Type.Sliced;

            go.transform.GetChild(0).GetComponent<RectTransform>().transform.localPosition = new Vector3(0f, 40f, 0f);
            go.transform.GetChild(0).GetComponent<RectTransform>().localScale = new Vector3(1.1f, 1.1f, 1.1f);

            go.transform.GetChild(1).gameObject.SetActive(true);
            go.transform.GetChild(1).GetComponent<RectTransform>().transform.localPosition = new Vector3(0f, -54.3f, 0f);
            go.transform.GetChild(1).GetComponent<RectTransform>().sizeDelta = new Vector2(171f, 50f);

            go.GetComponentInChildren<TextMeshProUGUI>().font = LilitaOne40;
            go.GetComponentInChildren<TextMeshProUGUI>().fontSize = 40f;
            go.GetComponentInChildren<TextMeshProUGUI>().alignment = TextAlignmentOptions.Center;

            go.transform.GetChild(3).gameObject.SetActive(true);

            if(item.name.Contains("Gold") || item.name.Contains("Energy"))
            {
                go.GetComponentInChildren<TextMeshProUGUI>().text = rewardItems[index];
            }
            else
            {
                go.GetComponentInChildren<TextMeshProUGUI>().text = "1";
            }
        }
    }

    private List<Item> initializeListOfRewards(List<string> rewardItems)
    {
        string items = PlayerPrefs.GetString(playerInventory);
        List<Item> itemsList = Utilities.convertStringToListOfItem(items);
        List<Item> tempList = new List<Item>();
        List<Item> rewards = new List<Item>();

        rewards.Add(new Item(ItemDict.ENERGY_ITEM, "Award", ColorUtility.ToHtmlStringRGBA(Color.white), "Description", 0, 0, string.Empty, 0));
        rewards.Add(new Item(ItemDict.GOLD_ITEM, "Award", ColorUtility.ToHtmlStringRGBA(Color.white), "Description", 0, 0, string.Empty, 0));

        if(rewardItems.Count == 2) //it means lvl is played more than once soo only rewards avaible are gold and energy
        {
            return rewards;
        }

        for(int index = 2; index < rewardItems.Count; index++)
        {
            rewards.Add(ItemDict.getItem(rewardItems[index]));
            tempList.Add(rewards[index]);
        }

        //SAVING REWARDS TO ITEMS IN EQ
        for (int index = 0; index < itemsList.Count; index++)
        {
            if (tempList.Count != 0 && itemsList[index].name.Contains("Empty"))
            {
                itemsList[index] = tempList[tempList.Count - 1];
                tempList.RemoveAt(tempList.Count - 1);
            }
        }

        PlayerPrefs.SetString(playerInventory, Utilities.convertListOfItemToString(itemsList));
        return rewards;
    }
}
