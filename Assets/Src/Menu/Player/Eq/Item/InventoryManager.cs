using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager
{
    public Transform clickedItem;
    private enum INVENTORY { MENU = 0, TANK = 1};
    private enum PURCHASE_METHOD { GOLD = 0, AD = 1 };
    private const int slotsBuyingCost = 2500;
    
    private PublicGameObjects publicGameObjects;
    private PlayerMetadataManager playerMetadataManager;
    private InventoryButtonsManager inventoryButtonsManager;
    private EquipmentCreationManager equipmentCreationManager;

    private List<Transform> slotsSkills = new List<Transform>();
    private List<Transform> slotsBullets = new List<Transform>();
    private List<Transform> navItems = new List<Transform>();
    private List<Item> items = new List<Item>();
    private List<Bullet> bullets = new List<Bullet>();
    private List<int> activeEqIndexes = new List<int>();

    private Transform activeItem;
    private Transform navFocus;
    private Transform activeNavItem;
    private Transform errorMessage;
    private ScrollRect activeScrollRect;
    //private TextMeshProUGUI goldAmount;
    private TextMeshProUGUI slotsCount;

    private int inventoryNum; //needed to properly save items
    private int itemsCount;
    private int bulletsCount;
    private string tankName;
    private bool openItemInfo; //open item info only when menuInventory is open

    private Color inActiveNavItemColor = new Color32(74, 172, 247, 255);
    private Color activeNavItemColor = new Color32(255, 255, 255, 255);
 

    public InventoryManager(PublicGameObjects publicGameObjects, PlayerMetadataManager playerMetadataManager, InventoryButtonsManager buttonSellManager, EquipmentCreationManager equipmentCreationManager)
    {
        this.publicGameObjects = publicGameObjects;
        this.playerMetadataManager = playerMetadataManager;
        this.inventoryButtonsManager = buttonSellManager;
        this.equipmentCreationManager = equipmentCreationManager;

        //goldAmount = publicGameObjects.goldAmountInventory;
        errorMessage = publicGameObjects.equipmentAddSlotsPopUp.transform;

        onInit();
    }

    private void onInit()
    {
        if (playerMetadataManager.getInventory().Equals(string.Empty))
        {
            //give player 2 start items and add empty item to rest of slots for correct work of dragService
            playerMetadataManager.setInventory(Utilities.convertListOfItemToString(ItemDict.getStartItems()));
        }

        if (playerMetadataManager.getInventoryBullets().Equals(string.Empty))
        {
            playerMetadataManager.setInventoryBullets(Utilities.convertListOfBulletItemToString(ItemDict.getStartBullets()));
        }
    }

    private void getNavItemsAndInitializeEq()
    {
        var slotsQuantity = playerMetadataManager.inventorySlotsQuantity;

        if ((INVENTORY)inventoryNum == INVENTORY.MENU)
        {
            navItems = publicGameObjects.navItemsInventory;
            slotsCount = publicGameObjects.slotCountInventory;

            equipmentCreationManager.initializeSlotsInEquipments(slotsQuantity, publicGameObjects.equipmentsContent[0]);
            equipmentCreationManager.initializeSlotsInEquipments(slotsQuantity, publicGameObjects.equipmentsContent[1]);

            activeEqIndexes.Add(0); 
            activeEqIndexes.Add(1);
        }
        else
        {
            switch(tankName)
            {
                case "Panzer1":
                    navItems = publicGameObjects.navItemsPanzer1;
                    slotsCount = publicGameObjects.slotCountPanzer1;

                    equipmentCreationManager.initializeSlotsInEquipments(slotsQuantity, publicGameObjects.equipmentsContent[2]);
                    equipmentCreationManager.initializeSlotsInEquipments(slotsQuantity, publicGameObjects.equipmentsContent[3]);

                    activeEqIndexes.Add(2);
                    activeEqIndexes.Add(3);
                    break;
                case "Panzer2":
                    navItems = publicGameObjects.navItemsPanzer2;
                    slotsCount = publicGameObjects.slotCountPanzer2;

                    equipmentCreationManager.initializeSlotsInEquipments(slotsQuantity, publicGameObjects.equipmentsContent[4]);
                    equipmentCreationManager.initializeSlotsInEquipments(slotsQuantity, publicGameObjects.equipmentsContent[5]);

                    activeEqIndexes.Add(4);
                    activeEqIndexes.Add(5);
                    break;
                case "Panzer3":
                    navItems = publicGameObjects.navItemsPanzer3;
                    slotsCount = publicGameObjects.slotCountPanzer3;

                    equipmentCreationManager.initializeSlotsInEquipments(slotsQuantity, publicGameObjects.equipmentsContent[6]);
                    equipmentCreationManager.initializeSlotsInEquipments(slotsQuantity, publicGameObjects.equipmentsContent[7]);

                    activeEqIndexes.Add(6);
                    activeEqIndexes.Add(7);
                    break;
                case "Panzer4":
                    navItems = publicGameObjects.navItemsPanzer4;
                    slotsCount = publicGameObjects.slotCountPanzer4;

                    equipmentCreationManager.initializeSlotsInEquipments(slotsQuantity, publicGameObjects.equipmentsContent[8]);
                    equipmentCreationManager.initializeSlotsInEquipments(slotsQuantity, publicGameObjects.equipmentsContent[9]);

                    activeEqIndexes.Add(8);
                    activeEqIndexes.Add(9);
                    break;
                case "Panzer5":
                    navItems = publicGameObjects.navItemsPanzer5;
                    slotsCount = publicGameObjects.slotCountPanzer5;

                    equipmentCreationManager.initializeSlotsInEquipments(slotsQuantity, publicGameObjects.equipmentsContent[10]);
                    equipmentCreationManager.initializeSlotsInEquipments(slotsQuantity, publicGameObjects.equipmentsContent[11]);

                    activeEqIndexes.Add(10);
                    activeEqIndexes.Add(11);
                    break;
            }
        }

        foreach (Transform navItem in navItems)
        {
            navItem.GetComponent<Button>().onClick.AddListener(delegate { changeFocus(navItem, navFocus); });

            if(navItem.childCount != 0)
            {
                navFocus = navItem.GetChild(0);
            }
        }
    }

    private void changeFocus(Transform destinationObject, Transform focus)
    {
        focus.SetParent(destinationObject);
        focus.localPosition = new Vector3(0f, focus.localPosition.y, 0f);

        if(activeNavItem == null)
        {
            activeNavItem = navItems[0];
            activeNavItem.GetComponent<TextMeshProUGUI>().color = activeNavItemColor;
            navItems[1].GetComponent<TextMeshProUGUI>().color = inActiveNavItemColor;
        }
        else
        {
            destinationObject.GetComponent<TextMeshProUGUI>().color = activeNavItemColor;
            activeNavItem.GetComponent<TextMeshProUGUI>().color = inActiveNavItemColor;
            activeNavItem = destinationObject;
        }

        if ((INVENTORY)inventoryNum == INVENTORY.MENU)
        {
            setActiveScrollRect("Inventory_" + destinationObject.name);
        }
        else
        {
            setActiveScrollRect(tankName + "_" + destinationObject.name);
        }

        if(destinationObject.name.Equals("Skills"))
        {
            slotsCount.text = itemsCount.ToString() + " / " + items.Count;
        }
        else
        {
            slotsCount.text = bulletsCount.ToString() + " / " + bullets.Count;
        }
    }

    public void getInventory(int inventoryNum)
    {
        this.inventoryNum = inventoryNum;
        getNavItemsAndInitializeEq();

        var skillsSlots = getInventorySlots("Skills", (INVENTORY)inventoryNum);
        var bulletSlots = getInventorySlots("Bullets", (INVENTORY)inventoryNum);
 
        bullets = Utilities.convertStringToListOfBullets(playerMetadataManager.getInventoryBullets());
        items = Utilities.convertStringToListOfItem(playerMetadataManager.getInventory());

        var skillsScroll = getScrollRect("Inventory_Skills");
        var bulletsScroll = getScrollRect("Inventory_Bullets");

        renderBulletInventory(bulletSlots, bullets, bulletsScroll);
        renderItemInventory(skillsSlots, items, skillsScroll);

        setSlotListToDragService(bulletSlots);
        setSlotListToDragService(skillsSlots);

        //Always set first nav as active
        changeFocus(navItems[0], navFocus);
    }

    public void setTankName(string tankName)
    {
        this.tankName = tankName;
    }

    private List<Transform> getInventorySlots(string nav, INVENTORY inventoryType)
    {
        var inventoryContent = new List<Transform>();
        List<Transform> inventorySkills = new List<Transform>();
        
        if(inventoryType == INVENTORY.MENU)
        {
            switch (nav)
            {
                case "Skills":
                    inventoryContent = publicGameObjects.inventoryContent.GetComponentsInChildren<Transform>().ToList<Transform>();
                    break;
                case "Bullets":
                    inventoryContent = publicGameObjects.inventoryBulletContent.GetComponentsInChildren<Transform>().ToList<Transform>();
                    break;
            }
            openItemInfo = true;
        }
        else
        {
            inventoryContent = getTankContent(tankName, nav);
            openItemInfo = false;
        }

        foreach (Transform content in inventoryContent)
        {
            if (content.name.Equals("Slot"))
            {
                inventorySkills.Add(content);
            }
        }

        switch (nav)
        {
            case "Skills":
                slotsSkills = inventorySkills;
                break;
            case "Bullets":
                slotsBullets = inventorySkills;
                break;
        }

        return inventorySkills;
    }

    private List<Transform> getTankContent(string name, string nav)
    {
        if(nav.Equals("Skills"))
        {
            switch (name)
            {
                case "Panzer1":
                    return publicGameObjects.panzerEquipment[0].transform.GetChild(2).transform.GetChild(0).transform.GetChild(0).GetComponentsInChildren<Transform>().ToList<Transform>();
                case "Panzer2":
                    return publicGameObjects.panzerEquipment[1].transform.GetChild(2).transform.GetChild(0).transform.GetChild(0).GetComponentsInChildren<Transform>().ToList<Transform>();
                case "Panzer3":
                    return publicGameObjects.panzerEquipment[2].transform.GetChild(2).transform.GetChild(0).transform.GetChild(0).GetComponentsInChildren<Transform>().ToList<Transform>();
                case "Panzer4":
                    return publicGameObjects.panzerEquipment[3].transform.GetChild(2).transform.GetChild(0).transform.GetChild(0).GetComponentsInChildren<Transform>().ToList<Transform>();
                case "Panzer5":
                    return publicGameObjects.panzerEquipment[4].transform.GetChild(2).transform.GetChild(0).transform.GetChild(0).GetComponentsInChildren<Transform>().ToList<Transform>();
            }
        }
        else
        {
            switch (name)
            {
                case "Panzer1":
                    return publicGameObjects.panzerEquipment[0].transform.GetChild(2).transform.GetChild(1).transform.GetChild(0).GetComponentsInChildren<Transform>().ToList<Transform>();
                case "Panzer2":
                    return publicGameObjects.panzerEquipment[1].transform.GetChild(2).transform.GetChild(1).transform.GetChild(0).GetComponentsInChildren<Transform>().ToList<Transform>();
                case "Panzer3":
                    return publicGameObjects.panzerEquipment[2].transform.GetChild(2).transform.GetChild(1).transform.GetChild(0).GetComponentsInChildren<Transform>().ToList<Transform>();
                case "Panzer4":
                    return publicGameObjects.panzerEquipment[3].transform.GetChild(2).transform.GetChild(1).transform.GetChild(0).GetComponentsInChildren<Transform>().ToList<Transform>();
                case "Panzer5":
                    return publicGameObjects.panzerEquipment[4].transform.GetChild(2).transform.GetChild(1).transform.GetChild(0).GetComponentsInChildren<Transform>().ToList<Transform>();
            }
        }

        return null;
    }

    private void renderItemInventory(List<Transform> slots, List<Item> items, ScrollRect scrollRect)
    {
        //render player items in proper inventory slots
        var slotIndex = 0;
        itemsCount = 0;

        foreach (Item item in items)
        {
            var itemToRender = instantiateItemToRender("Menu/Items/", item.name, slots[slotIndex], scrollRect);

            //get item info after clicking on it
            if (openItemInfo)
            {
                itemToRender.GetComponent<Button>().onClick.AddListener(delegate { getItemInfo(itemToRender.transform, item); });
            }
            else
            {
                itemToRender.GetComponent<Button>().onClick.AddListener(delegate { getItemDetail(itemToRender.transform, item); });
            }

            if(!item.name.Equals("Empty_Item"))
            {
                itemsCount++;
            }

            slotIndex++;
        }
    }

    private void renderBulletInventory(List<Transform> slots, List<Bullet> bullets, ScrollRect scrollRect)
    {
        //render player items in proper inventory slots
        var slotIndex = 0;
        bulletsCount = 0;

        foreach (Bullet bullet in bullets)
        {
            var itemToRender = instantiateItemToRender("Menu/Bullets/", bullet.name, slots[slotIndex], scrollRect);
            
            if(!bullet.name.Equals("EMPTY")) 
            {
                bulletsCount++;
            }
            
            //get item info after clicking on it
            if (openItemInfo)
            {
           //     itemToRender.GetComponent<Button>().onClick.AddListener(delegate { getItemInfo(itemToRender.transform, item); });
            }

            slotIndex++;
        }
    }

    private GameObject instantiateItemToRender(string prefabPrefix, string name, Transform slot, ScrollRect scrollRect)
    {
        // get item from prefabs and set good position in worldSpace
        var itemToRender = Utilities.getPrefabFromResources(prefabPrefix + name);
        itemToRender = GameObject.Instantiate(itemToRender, Vector3.zero, Quaternion.identity, slot);
        itemToRender.transform.localPosition = Vector3.zero;

        //add dragService for moving items in inventory feature
        itemToRender.AddComponent<DragService>();
        itemToRender.GetComponent<DragService>().activeScrollRect = scrollRect;

        return itemToRender;
    }

    private void setSlotListToDragService(List<Transform> slotItems)
    {
        foreach(Transform item in slotItems)
        {
            item.GetChild(0).GetComponent<DragService>().itemSlotList = slotItems;
        }
    }

    private void getItemInfo(Transform itemTransform, Item item)
    {
        clickedItem = itemTransform;

        //focus on clicked item handler
        if(activeItem == null)
        {
            activeItem = itemTransform;
        }
        else if(activeItem.GetInstanceID() != clickedItem.GetInstanceID())
        {
            activeItem.GetChild(2).gameObject.SetActive(false);
            activeItem = clickedItem;
        }

        itemTransform.GetChild(2).gameObject.SetActive(true);

        //open right side info
        publicGameObjects.inventoryGroupRight.transform.GetChild(0).gameObject.SetActive(true);
        publicGameObjects.inventoryGroupLeft.GetComponent<RectTransform>().anchorMax = new Vector2(0.66f, 1f);

        getBackgroundInfo(itemTransform, item);
    }

    private void getItemDetail(Transform itemTransform, Item item)
    {
        Transform popUp = publicGameObjects.itemDetailsPopUp.transform.GetChild(1);
        ColorUtility.TryParseHtmlString("#" + item.typeColor, out Color typeColor);

        popUp.GetChild(3).transform.GetChild(0).GetComponent<Image>().sprite = itemTransform.GetComponent<Image>().sprite;
        popUp.GetChild(3).transform.GetChild(0).transform.GetChild(0).GetComponent<Image>().sprite = itemTransform.GetChild(0).GetComponent<Image>().sprite;
        popUp.GetChild(3).transform.GetChild(0).transform.GetChild(0).GetComponent<RectTransform>().sizeDelta = itemTransform.GetChild(0).GetComponent<RectTransform>().sizeDelta;

        popUp.GetChild(3).transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = item.type.ToUpper();
        popUp.GetChild(3).transform.GetChild(1).GetComponent<TextMeshProUGUI>().color = typeColor;
        popUp.GetChild(3).transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = item.name;

        popUp.transform.GetChild(4).transform.GetChild(0).transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = item.itemPropertyName;
        popUp.transform.GetChild(4).transform.GetChild(0).transform.GetChild(1).GetComponent<Image>().sprite = Resources.Load<Sprite>("Textures/Icons/ItemProperties/Icon_PropertyIcon_" + item.itemPropertyName);
        popUp.transform.GetChild(4).transform.GetChild(0).transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = "+" + item.itemPropertyValue.ToString();

        popUp.transform.GetChild(5).transform.GetChild(1).transform.GetChild(0).transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = GoldManager.getGoldToDisplay(item.value); ;
        inventoryButtonsManager.setButtonInfo(item, slotsSkills, items, popUp.transform.GetChild(5).transform.GetChild(1).GetComponent<Button>());
        inventoryButtonsManager.setButtonInfo(item, activeItem, popUp.transform.GetChild(5).transform.GetChild(0).transform.GetChild(0).GetComponentInChildren<TextMeshProUGUI>(), popUp.transform.GetChild(5).transform.GetChild(0).GetComponent<Button>());

        publicGameObjects.itemDetailsPopUp.SetActive(true);
    }

    private void getBackgroundInfo(Transform itemTransform, Item item)
    {
        Transform background = publicGameObjects.inventoryGroupRight.transform.GetChild(0);
        ColorUtility.TryParseHtmlString("#" + item.typeColor, out Color typeColor);

        var textType = background.GetChild(0).GetComponent<TextMeshProUGUI>();
        var textItemName = background.GetChild(1).GetComponent<TextMeshProUGUI>();
        var textItemInfo = background.GetChild(2).GetComponent<TextMeshProUGUI>();
        var properties = background.GetChild(4).transform.GetChild(0);
        var itemObj = background.GetChild(5);
        var bottomMenu = background.GetChild(6);
        
        textType.text = item.type.ToUpper();
        textType.color = typeColor;
        textItemName.text = item.name;
        textItemInfo.text = item.info;

        itemObj.GetComponent<Image>().sprite = itemTransform.GetComponent<Image>().sprite;
        itemObj.GetChild(0).GetComponent<Image>().sprite = itemTransform.GetChild(0).GetComponent<Image>().sprite;
        itemObj.GetChild(0).GetComponent<RectTransform>().sizeDelta = itemTransform.GetChild(0).GetComponent<RectTransform>().sizeDelta;

        if(item.upgradesCounter != 0)
        {
            itemObj.GetComponentInChildren<TextMeshProUGUI>().text = item.upgradesCounter.ToString();
        }
        
        properties.GetChild(0).GetComponent<Image>().sprite = Resources.Load<Sprite>("Textures/Icons/ItemProperties/Icon_PropertyIcon_" + item.itemPropertyName);
        properties.GetChild(1).GetComponent<TextMeshProUGUI>().text = item.itemPropertyName;
        properties.GetChild(2).GetComponent<TextMeshProUGUI>().text = "+" + item.itemPropertyValue.ToString();

        bottomMenu.transform.GetChild(0).GetComponentInChildren<TextMeshProUGUI>().text = GoldManager.getGoldToDisplay(item.value);
        inventoryButtonsManager.setButtonInfo(item, slotsSkills, items, background.transform.GetChild(6).transform.GetChild(0).GetComponent<Button>());
        inventoryButtonsManager.setButtonInfo(item, activeItem, background.transform.GetChild(6).transform.GetChild(1).transform.GetChild(0).GetComponentInChildren<TextMeshProUGUI>(), background.transform.GetChild(6).transform.GetChild(1).GetComponent<Button>());
    }

    //handling item bug where item goes "under" scroll rect
    private void setActiveScrollRect(string inventoryName)
    {
        if (activeScrollRect != null)
        {
            activeScrollRect.gameObject.SetActive(false);
        }

        activeScrollRect = getScrollRect(inventoryName);
     
        if (activeScrollRect != null)
        {
            activeScrollRect.gameObject.SetActive(true);
        }
    }

    public ScrollRect getScrollRect(string inventoryName)
    {
        switch (inventoryName)
        {
            case "Inventory_Skills":
                return publicGameObjects.inventoryScrollRects[0];
            case "Inventory_Bullets":
                return publicGameObjects.inventoryScrollRects[1];
            case "Panzer1_Skills":
                return publicGameObjects.inventoryScrollRects[2];
            case "Panzer1_Bullets":
                return publicGameObjects.inventoryScrollRects[3];
            case "Panzer2_Skills":
                return publicGameObjects.inventoryScrollRects[4];
            case "Panzer2_Bullets":
                return publicGameObjects.inventoryScrollRects[5];
            case "Panzer3_Skills":
                return publicGameObjects.inventoryScrollRects[6];
            case "Panzer3_Bullets":
                return publicGameObjects.inventoryScrollRects[7];
            case "Panzer4_Skills":
                return publicGameObjects.inventoryScrollRects[8];
            case "Panzer4_Bullets":
                return publicGameObjects.inventoryScrollRects[9];
            case "Panzer5_Skills":
                return publicGameObjects.inventoryScrollRects[10];
            case "Panzer5_Bullets":
                return publicGameObjects.inventoryScrollRects[11];
        }
        return null;
    }

    public void buyEquipementSlots(int purchaseMethodNum)
    {
        //TODO: SAVING ITEMS DO NOT WORK, FIX IT!
        switch ((PURCHASE_METHOD)purchaseMethodNum)
        {
            case PURCHASE_METHOD.GOLD:
                var userGoldAmount = playerMetadataManager.goldAmount;
                if(userGoldAmount >= slotsBuyingCost) 
                {
                    userGoldAmount -= slotsBuyingCost;
                    playerMetadataManager.goldAmount = userGoldAmount;
                    StatusBarManager.updateGoldOnStatusBars(publicGameObjects, playerMetadataManager);

                    equipmentCreationManager.createNewSlotsForEquipements(publicGameObjects.equipmentsContent[activeEqIndexes[0]], slotsSkills);
                    equipmentCreationManager.createNewSlotsForEquipements(publicGameObjects.equipmentsContent[activeEqIndexes[1]], slotsBullets);
                    for (int i = 0; i < 2; i++)
                    {
                        items.Add(ItemDict.getItem(ItemDict.EMPTY_ITEM));
                        bullets.Add(new Bullet()); //add bulletsDict maybe
                    }

                    slotsCount.text = slotsCount.text.Replace((items.Count() - 2).ToString(), items.Count().ToString()); //we have the same amount of slots for bullet and skills - at least for now
                    errorMessage.gameObject.SetActive(false);

                    var slotsQuantity = playerMetadataManager.inventorySlotsQuantity;
                    slotsQuantity += 2;
                    playerMetadataManager.inventorySlotsQuantity = slotsQuantity;
                }
                else
                {
                    errorMessage.GetChild(1).gameObject.SetActive(true);
                    errorMessage.GetChild(1).GetComponentInChildren<TextMeshProUGUI>().text = "You do not have enough gold to buy more slots.";
                }
                break;
            case PURCHASE_METHOD.AD:
                Debug.Log("Display ad here");
                break;
        }

    }

    public void saveItems()
    {
        //in case when player leaves game after exiting inventory (we dont want to save empty list right? :P)
        if (slotsSkills.Count == 0)
        {
            return;
        }

        for (int slotContent = 0; slotContent < slotsSkills.Count; slotContent++)
        {
            if (!slotsSkills[slotContent].GetChild(0).name.Contains(items[slotContent].name))
            {
                var index = searchForProprieteItem(items[slotContent].name, slotsSkills);
                var temp = items[slotContent];
                var temp2 = items[index];

                items[index] = temp;
                items[slotContent] = temp2;
            }
        }

        for (int slotContent = 0; slotContent < slotsBullets.Count; slotContent++)
        {
            if (!slotsBullets[slotContent].GetChild(0).name.Contains(items[slotContent].name))
            {
                var index = searchForProprieteItem(bullets[slotContent].name, slotsBullets);
                var temp = bullets[slotContent];
                var temp2 = bullets[index];

                bullets[index] = temp;
                bullets[slotContent] = temp2;
            }
        }

        playerMetadataManager.setInventoryBullets(Utilities.convertListOfBulletItemToString(bullets));
        playerMetadataManager.setInventory(Utilities.convertListOfItemToString(items));

        clearVariables();
    }
    private int searchForProprieteItem(string searchedItemName, List<Transform> slots)
    {
        var index = 0;
        for (int i = 0; i < slots.Count; i++)
        {
            if (slots[i].GetChild(0).name.Contains(searchedItemName))
            {
                index = i;
            }
        }

        return index;
    }

    private void clearVariables()
    {
        //destroy current objects in case when player come back
        foreach (Transform item in slotsSkills)
        {
            GameObject.Destroy(item.GetChild(0).gameObject);
        }

        foreach (Transform bullet in slotsBullets)
        {
            GameObject.Destroy(bullet.GetChild(0).gameObject);
        }

        foreach(Transform slot in publicGameObjects.equipmentsContent[activeEqIndexes[0]])
        {
            GameObject.Destroy(slot.gameObject);
        }

        foreach (Transform slot in publicGameObjects.equipmentsContent[activeEqIndexes[1]])
        {
            GameObject.Destroy(slot.gameObject);
        }

        foreach (Transform navItem in navItems)
        {
            navItem.GetComponent<Button>().onClick.RemoveAllListeners();
        }

        items.Clear();
        bullets.Clear();

        slotsSkills.Clear();
        slotsBullets.Clear();

        activeEqIndexes.Clear();
        activeNavItem = null;
        tankName = string.Empty;
    }
}
