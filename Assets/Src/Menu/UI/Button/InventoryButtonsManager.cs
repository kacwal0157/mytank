using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryButtonsManager
{
    private PublicGameObjects publicGameObjects;
    private PlayerMetadataManager playerMetadataManager;

    private List<Transform> slots;
    private List<Item> items;
    private Item item;
    private Transform activeItem;
    private int upgradeCost;
    private bool upgradeExecuted = false;

    private const int upgradeMultiplier = 50;
   
    public InventoryButtonsManager(PublicGameObjects publicGameObjects, PlayerMetadataManager playerMetadataManager)
    {
        this.publicGameObjects = publicGameObjects;
        this.playerMetadataManager = playerMetadataManager;
    }

    public void setButtonInfo(Item item, List<Transform> slots, List<Item> items, Button btn)
    {
        this.item = item;
        this.slots = slots;
        this.items = items;

        btn.onClick.AddListener(sellItem);
    }

    public void setButtonInfo(Item item, Transform activeItem, TextMeshProUGUI upgradeGoldAmount, Button btn)
    {
        this.item = item;
        this.activeItem = activeItem;

        if(upgradeExecuted)
        {
            btn.onClick.RemoveListener(upgradeItem);
            upgradeExecuted = false;
        }

        handleUpgradeCost(upgradeGoldAmount);
        btn.onClick.AddListener(upgradeItem);

        upgradeExecuted = true;
    }

    private void sellItem()
    {
        var emptyItem = ItemDict.getItem(ItemDict.EMPTY_ITEM);
        var gold = playerMetadataManager.goldAmount;

        gold += item.value;
        playerMetadataManager.goldAmount = gold;
        StatusBarManager.updateGoldOnStatusBars(publicGameObjects, playerMetadataManager);

        searchAndRemoveItemForSell(item, slots, emptyItem, items);
        resetInventoryLook();

        playerMetadataManager.setInventory(Utilities.convertListOfItemToString(items));
    }

    private void upgradeItem()
    {
        var gold = playerMetadataManager.goldAmount;

        if (gold >= upgradeCost)
        {
            gold -= upgradeCost;
            playerMetadataManager.goldAmount = gold;
            StatusBarManager.updateGoldOnStatusBars(publicGameObjects, playerMetadataManager);

            resetInventoryLook();
            activeItem.GetChild(2).gameObject.SetActive(false);

            item.upgradesCounter += 1;
            activeItem.GetChild(1).GetComponent<TextMeshProUGUI>().text = item.upgradesCounter.ToString();
        }
        else
        {
            Debug.Log("You don't have enough gold to upgrade an item!");
        }
    }

    private void handleUpgradeCost(TextMeshProUGUI upgradeGoldAmount)
    {
        var upgradeCost = 0;

        switch (item.type)
        {
            case "Legendary":
                upgradeCost = 500;
                break;
            case "Epic":
                upgradeCost = 350;
                break;
            case "Rare":
                upgradeCost = 300;
                break;
            case "Unusual":
                upgradeCost = 250;
                break;
            case "Common":
                upgradeCost = 200;
                break;
            case "Normal":
                upgradeCost = 100;
                break;
        }


        if (item.upgradesCounter != 0)
        {
            upgradeCost = upgradeCost + (upgradeMultiplier * item.upgradesCounter);
        }

        upgradeGoldAmount.text = GoldManager.getGoldToDisplay(upgradeCost);
        this.upgradeCost = upgradeCost;
    }

    private void searchAndRemoveItemForSell(Item item, List<Transform> slots, Item emptyItem, List<Item> items)
    {
        var index = 0;
        foreach(Transform t in slots)
        {
            if(t.GetChild(0).name.Contains(item.name))
            {
                GameObject.Destroy(t.GetChild(0).gameObject);
                items[index] = emptyItem;

                var go = Utilities.getPrefabFromResources("Menu/Items/" + emptyItem.name);
                go = GameObject.Instantiate(go, Vector3.zero, Quaternion.identity, t);
                go.transform.localPosition = Vector3.zero;

                break;
            }

            index++;
        }

        playerMetadataManager.setInventory(Utilities.convertListOfItemToString(items));
    }

    private void resetInventoryLook()
    {
        var background = publicGameObjects.inventoryGroupRight.transform.GetChild(0).gameObject;
        var detailsPopUp = publicGameObjects.itemDetailsPopUp;

        if(background.activeSelf)
        {
            background.SetActive(false);
            publicGameObjects.inventoryGroupLeft.GetComponent<RectTransform>().anchorMax = new Vector2(1f, 1f);
        }
        else if(detailsPopUp.activeSelf)
        {
            detailsPopUp.SetActive(false);
        }
    }
}
