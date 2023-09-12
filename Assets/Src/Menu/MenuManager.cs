using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    private PlayerMetadataManager playerMetadataManager;
    private LastLoginDateManager lastLoginDateManager;
    private DailyRewards dailyRewards;
    private DailyRewardsItemManager dailyRewardsItemManager;
    private UsernamesDict usernamesDict;
    private TankChooseController tankChooseController;
    private TankExperienceController tankExperienceController;
    private UserEquipmentController userEquipmentController;
    private InventoryManager inventoryManager;
    private UserInfoPopUpHandler userInfoPopUpHandler;
    private ChangeNamePopUpHandler changeNamePopUpHandler;
    private AwardsManager awardsManager;
    private InventoryButtonsManager inventoryButtonsManager;
    private ShopPopUp shopPopUp;
    private ShopTapMenuManager shopTapMenuManager;
    private ExperienceManager experienceManager;
    private LevelUpManager levelUpManager;
    private LevelUpRewardsManager levelUpRewardsManager;
    private EquipmentCreationManager equipmentCreationManager;

    public UserInfoManager userInfo;
    public PublicGameObjects publicGameObjects;
    public GameObject levelMainParent;

    public static int levelToLoad;
    private static MenuManager INSTANCE;
    private static readonly object padlock = new object();

    private void Awake()
    {
        dailyRewards = JsonReader.loadDailyRewards();

        //REMEMBER THAT PREFS MUST BE FIRST TO SAVE DATE FOR THE FIRST TIME
        playerMetadataManager = new PlayerMetadataManager();
        lastLoginDateManager = new LastLoginDateManager(playerMetadataManager);
        new StatusBarManager(playerMetadataManager, publicGameObjects);
        dailyRewardsItemManager = new DailyRewardsItemManager(playerMetadataManager, publicGameObjects);
        new DailyRewardsPopUpManager(dailyRewards, publicGameObjects, lastLoginDateManager, playerMetadataManager, dailyRewardsItemManager);
        usernamesDict = new UsernamesDict(playerMetadataManager); //on cloud server in the future
        equipmentCreationManager = new EquipmentCreationManager();
        new PlayerStatsHandler(playerMetadataManager, publicGameObjects);
        levelUpRewardsManager = new LevelUpRewardsManager(playerMetadataManager, publicGameObjects);
        levelUpManager = new LevelUpManager(publicGameObjects, levelUpRewardsManager);
        experienceManager = new ExperienceManager(playerMetadataManager, publicGameObjects, levelUpManager);
        userInfo = new UserInfoManager(playerMetadataManager, publicGameObjects, experienceManager);
        new RouletteManager(publicGameObjects, playerMetadataManager);
        new ButtonLanguageManager(playerMetadataManager, publicGameObjects);
        new PlayerMetadataLimitRefresherService(playerMetadataManager);
        new StageSelectController(publicGameObjects, playerMetadataManager);
        new LevelSelectController(publicGameObjects, playerMetadataManager);
        inventoryButtonsManager = new InventoryButtonsManager(publicGameObjects, playerMetadataManager);
        inventoryManager = new InventoryManager(publicGameObjects, playerMetadataManager, inventoryButtonsManager, equipmentCreationManager);
        tankChooseController = new TankChooseController(publicGameObjects, playerMetadataManager);
        tankExperienceController = new TankExperienceController(publicGameObjects, playerMetadataManager);
        userEquipmentController = new UserEquipmentController(publicGameObjects, playerMetadataManager);
        userInfoPopUpHandler = new UserInfoPopUpHandler(publicGameObjects);
        changeNamePopUpHandler = new ChangeNamePopUpHandler(publicGameObjects, playerMetadataManager);
        awardsManager = new AwardsManager(publicGameObjects);
        shopPopUp = new ShopPopUp(publicGameObjects, playerMetadataManager);
        new ShopManager(publicGameObjects, shopPopUp);
        shopTapMenuManager = new ShopTapMenuManager(publicGameObjects);
    }

    void Start()
    {
        userInfo.setDefaultUserInfoStatement();
        tankExperienceController.onStart();
    }

    void Update()
    {
        tankExperienceController.onUpdate();
    }

    public void chooseTank(GameObject obj)
    {
        tankChooseController.onClick(obj);
    }

    public void resetTanksListPos()
    {
        tankChooseController.onDisable();
    }

    //TODO: THINK ABOUT TANK STATS AND HANDLE IT
    public void selectTankAndItsStats(Button button)
    {
        userEquipmentController.onClick(button);
    }

    public void getUserInfoPopUp()
    {
        userInfoPopUpHandler.onClick();
    }

    public void tryToChangeUsername()
    {
        changeNamePopUpHandler.onClick();
    }

    public void showAwards(int levelNumber)
    {
        awardsManager.showAwards(levelNumber - 1);
    }

    public void saveUserEquipementPrefs()
    {
        userEquipmentController.saveAllPrefs();
    }

    public void resetInventoryLook()
    {
        if (publicGameObjects.inventoryGroupRight.transform.GetChild(0).gameObject.activeSelf)
        {
            if (!inventoryManager.clickedItem.name.Contains("Empty"))
            {
                inventoryManager.clickedItem.GetChild(2).gameObject.SetActive(false);
            }

            publicGameObjects.inventoryGroupRight.transform.GetChild(0).gameObject.SetActive(false);
            publicGameObjects.inventoryGroupLeft.GetComponent<RectTransform>().anchorMax = new Vector2(1f, 1f);
        }
    }

    public void openGemShopSection()
    {
        shopTapMenuManager.setTapButtonProperties(publicGameObjects.shopTapMenuCategories[3]); //tapGemsPack
    }

    public void getTankFromPrefsToMenu()
    {
        var activeTank = playerMetadataManager.tankPrefabName;

        foreach (GameObject tank in publicGameObjects.tanksInMenu)
        {
            if (tank.name.Equals(activeTank))
            {
                tank.SetActive(true);
            }
        }
    }

    public void getInventory(int inventoryNum)
    {
        inventoryManager.getInventory(inventoryNum);
    }

    public void setTankNameForInventory(string tankName)
    {
        inventoryManager.setTankName(tankName);
    }

    public void setInventory()
    {
        inventoryManager.saveItems();
    }

    public void buyEquipementSlots(int purchaseMethodNum)
    {
        inventoryManager.buyEquipementSlots(purchaseMethodNum);
    }

    public void setTankActivation(bool active)
    {
        MenuTankActivator.setTankActivation(active, publicGameObjects);
    }

    public static MenuManager getInstance
    {
        get
        {
            lock (padlock)
            {
                if (INSTANCE == null)
                {
                    INSTANCE = FindObjectOfType(typeof(MenuManager)) as MenuManager;
                }
                return INSTANCE;
            }
        }
    }

    private void OnDisable()
    {
        tankExperienceController.saveAllPrefs();
        inventoryManager.saveItems();
        usernamesDict.saveUsernames();
        userInfo.saveAllPrefs();
        playerMetadataManager.saveAllPrefs();
    }
}