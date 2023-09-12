using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PublicGameObjects : MonoBehaviour
{
    [Header("MENU")]
    [Header("- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -")]

    [Header("Main")]
    public Canvas canvas;
    public GameObject panel;

    [Header("Lobby")]
    public GameObject lobby;
    public GameObject userInfo;
    public List<Transform> statusBarLobby;
    public List<GameObject> tanksInMenu;

    [Header("UserInfo")]
    public Slider userExpSlider;
    public TextMeshProUGUI userExpTextOnSlider;
    public TextMeshProUGUI userLevel;
    public TextMeshProUGUI lobbyUsername;
    public TextMeshProUGUI lobbyEnergyValue;

    [Header("StatusBars")]
    public List<Transform> statusBars;

    [Header("Inventory")]
    public GameObject invnetoryGO;
    public GameObject inventoryContent;
    public GameObject inventoryBulletContent;
    public GameObject inventoryGroupRight;
    public GameObject inventoryGroupLeft;
    public TextMeshProUGUI goldAmountInventory;
    public List<Transform> statusBarInventory;
    public List<ScrollRect> inventoryScrollRects;
    public List<Transform> navItemsInventory;
    public TextMeshProUGUI slotCountInventory;
    public List<Transform> equipmentsContent;

    [Header("InventoryTanks")]
    public List<Transform> navItemsPanzer1;
    public List<Transform> navItemsPanzer2;
    public List<Transform> navItemsPanzer3;
    public List<Transform> navItemsPanzer4;
    public List<Transform> navItemsPanzer5;
    public TextMeshProUGUI slotCountPanzer1;
    public TextMeshProUGUI slotCountPanzer2;
    public TextMeshProUGUI slotCountPanzer3;
    public TextMeshProUGUI slotCountPanzer4;
    public TextMeshProUGUI slotCountPanzer5;

    [Header("Shop")]
    public List<Transform> statusBarShop;
    public List<GameObject> shopOffert;
    public GameObject shopPopUp;
    public List<GameObject> shopTapMenuCategories;
    public TextMeshProUGUI goldAmountShop;
    public TextMeshProUGUI gemAmountShop;

    [Header("RouletteSlots")]
    public List<GameObject> slotGold;
    public GameObject rouletteGameObject;

    [Header("RouletteAnimationCurves")]
    public List<AnimationCurve> animationCurves;

    [Header("StatusGroup")]
    public TextMeshProUGUI goldAmountText;

    [Header("LanguageButtons")]
    public GameObject buttonLanguageGroup;
    public GameObject settingIconLanguageGameObject;
    public List<Image> settingIconImage;

    [Header("Stages")]
    public List<Button> stagesBtn;
    public List<Transform> statusBarStages;
    public List<Transform> stages;

    [Header("TanksList")]
    public List<GameObject> tanksList;
    public GameObject focusGameObject;
    public GameObject tanksListContent;
    public GameObject tankListGO;

    [Header("LevelSelect")]
    public List<Transform> statusBarLevelSelect;
    public List<Transform> levelsList;

    [Header("TankInfoCardsParent")]
    public GameObject tankInfoParent;

    [Header("TankInfoCardsParent")]
    public List<Sprite> fillTankCardsImages;

    [Header("EquipmentStatsSliders")]
    public Slider tankHealthSlider;
    public Slider tankAttackSlider;
    public Slider tankDefenceSlider;

    [Header("EquipmentTanks")]
    public List<GameObject> panzerEquipment;
    public List<Transform> statusBarPanzerEq_1;
    public List<Transform> statusBarPanzerEq_2;
    public List<Transform> statusBarPanzerEq_3;
    public List<Transform> statusBarPanzerEq_4;
    public List<Transform> statusBarPanzerEq_5;

    [Header("PopUps")]
    public GameObject userInfoPopUp;
    public GameObject changeNamePopUp;
    public GameObject namePopUp;
    public GameObject itemDetailsPopUp;
    public GameObject dailyRewardsPopUp;
    public List<GameObject> firstTimeAwardsPopUps;
    public List<GameObject> awardsPopUps;
    public GameObject equipmentAddSlotsPopUp;

    [Header("DailyRewards")]
    public GameObject tomorrowItemMessageBox;

    [Header("LevelUp")]
    public GameObject levelUpGO;

    [Header("IN-GAME")]
    [Header("- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -")]

    [Header("cannon fire smoke. Particle render just after shooting")]
    public GameObject cannonFireSmoke;

    [Header("normal bullet")]
    public GameObject normalBullet;

    [Header("targetPreview")]
    public GameObject targetPreview;

    [Header("targetPreviewUzi")]
    public GameObject targetPreviewUzi;

    [Header("targetCursor")]
    public GameObject targetCursor;

    [Header("aimTarget")]
    public GameObject aimTarget;

    [Header("shootButton")]
    public Button shootButton;

    [Header("projectileArc")]
    public ProjectileArc projectileArc;

    [Header("timeOfFlight")]
    public TextMeshProUGUI timeOfFlight;

    [Header("wind")]
    public GameObject windArrow;
    public TextMeshProUGUI windDirectionBasedOnValue;

    [Header("damagePopUp")]
    public GameObject damagePopUp;

    [Header("cannon interface canvas")]
    public Transform cannonInterfaceCanvas;

    [Header("Explosion Particles")]
    public GameObject bigExplosionParticle;
    public GameObject smallExplosionParticle;

    [Header("ListOfPlayers")]
    public Animator listOpenerAnimator;
    public GameObject listOfPlayersContent;
    public GameObject listOfBulletsContent;
    public GameObject listContentPrefab;

    [Header("EndGameTitles")]
    public GameObject gameResultScreen;
    public GameObject deadScreen;

    [Header("PauseMenu")]
    public GameObject pauseMenuGO;
    public List<Button> pauseMenuButtons;

    [Header("FONTS")]
    [Header("- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -")]

    public TMP_FontAsset LilitaOne40;
}
