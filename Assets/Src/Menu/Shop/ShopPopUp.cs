using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopPopUp
{
    private PlayerMetadataManager playerMetadataManager;
    private PublicGameObjects publicGameObjects;

    private GameObject popUp;
    private TextMeshProUGUI goldAmount;
    private TextMeshProUGUI gemAmount;

    public ShopPopUp(PublicGameObjects publicGameObjects, PlayerMetadataManager playerMetadataManager)
    {
        this.playerMetadataManager = playerMetadataManager;
        this.publicGameObjects = publicGameObjects;

        popUp = publicGameObjects.shopPopUp;
    }

    public void onClick(Transform offert, TextMeshProUGUI goldAmount, TextMeshProUGUI gemAmount)
    {
        this.goldAmount = goldAmount;
        this.gemAmount = gemAmount;

        if (offert.name.Contains("Gold"))
        {
            initializePopUpValuesForGold(offert);
        }

        //TODO: CREATE SERVICE FOR BUYING OTHER STUFF IN SHOP
    }

    private void initializePopUpValuesForGold(Transform offert)
    {
        var frame = popUp.GetComponent<Image>();
        var icon = popUp.transform.GetChild(5).GetComponent<Image>();
        var info = popUp.transform.GetChild(6).GetComponentInChildren<TextMeshProUGUI>();
        var btnClose = popUp.transform.GetChild(7);
        var btnSell = popUp.transform.GetChild(8);

        frame.sprite = offert.GetComponent<Image>().sprite;
        icon.sprite = offert.GetChild(1).GetComponentInChildren<Image>().sprite; 

        info.text = " + " + offert.GetChild(2).GetComponent<TextMeshProUGUI>().text + " gold";
        info.color = offert.GetChild(2).GetComponent<TextMeshProUGUI>().color;

        btnClose.GetComponent<Image>().color = new Color32(242, 164, 15, 255);

        btnSell.GetComponentInChildren<TextMeshProUGUI>().text = offert.GetChild(3).GetComponentInChildren<TextMeshProUGUI>().text;
        btnSell.GetComponent<Image>().color = new Color32(242, 164, 15, 255);

        int cost = int.Parse(btnSell.GetComponentInChildren<TextMeshProUGUI>().text);
        int value = GoldManager.getGoldFromString(offert.GetChild(2).GetComponent<TextMeshProUGUI>().text);

        btnSell.GetComponent<Button>().onClick.AddListener(delegate { buyOffer(cost, value); });
    }

    private void buyOffer(int cost, int value)
    {
        if (int.Parse(gemAmount.text) >= cost)
        {
            var gold = GoldManager.getGoldFromString(goldAmount.text) + value;
            goldAmount.text = GoldManager.getGoldToDisplay(gold);
            playerMetadataManager.goldAmount = gold;
            StatusBarManager.updateGoldOnStatusBars(publicGameObjects, playerMetadataManager);

            var gem = int.Parse(gemAmount.text) - cost;
            gemAmount.text = GemManager.getGemToDisplay(gem);
            playerMetadataManager.gemAmount = gem;
            StatusBarManager.updateGemsOnStatusBars(publicGameObjects, playerMetadataManager);
        }
        else
        {
            Debug.Log("You don't have enough gem to purchase that!");
        }
    }
}
