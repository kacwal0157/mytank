using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Newtonsoft.Json;
using System.IO;

public class GameManager : MonoBehaviour
{
    public enum Stage { PLAYER_TURN_AIMING, PLAYER_TURN_SHOOTING, OPONNENT_TURN_AIMING, OPONNENT_TURN_SHOOTING, AFTER_SHOOT, END_OF_TIME, CAMERA_MOVEMENT, WIN_GAME, LOSE_GAME };
    public GameObject cube;
    public GameObject terrainGo;
    public GameObject ballPrefab;
    public GameObject aimTarget;
    public int levelToLoad;
    public TextMeshProUGUI timeRemaining;
    public ButtonController buttonController;

    public static Stage currentStage = GameManager.Stage.CAMERA_MOVEMENT;
    private static Stage nextStage = GameManager.Stage.PLAYER_TURN_AIMING;
    private static Stage previousStage = GameManager.Stage.END_OF_TIME;


    public TimeManager timeManager;
    public CameraManager cameraManager;
    public TouchManager touchManager;
    public PlayerConfigurationService playerConfigurationService;
    public ScrollAndPinch scrollAndPinch;
    public PublicGameObjects publicGameObjects;

    private Level.Arena arena;
    private FocusOnTargetManager focusOnTargetManager;
    private PlayerController playerController;
    private AIController enemyAIManager;
    private LevelController levelController;
    private FireController fireController;
    private WindController windController;
    private SwitchingController switchingController;
    private PendulumEffectController pendulumEffectController;
    private EnemiesCreator enemiesCreator;
    private SpawnPlayer spawnPlayer;
    private GameRightPanelManager gameRightPanelManager;
    private EndGameScreenManager endGameScreenManager;
    private DeadScreenManager deadScreenManager;
    private PauseMenuManager pauseMenuManager;
    private PlayerMetadataManager playerMetadataManager;

    public static List<GameObject> enemies;
    public static GameObject activePlayer;
    private GameObject playerPrefab;
    private bool enableTouch = true;


    private static GameManager INSTANCE;
    private static readonly object padlock = new object();

    void Awake()
    {
        Application.targetFrameRate = 60;

        GameManager.currentStage = Stage.CAMERA_MOVEMENT;
        GameManager.previousStage = Stage.END_OF_TIME;
        GameManager.nextStage = Stage.PLAYER_TURN_AIMING;
        playerMetadataManager = new PlayerMetadataManager();
        //levelToLoad = MenuManager.levelToLoad == 0 ? 1 : MenuManager.levelToLoad;
        //arena = JsonReader.loadWorldConfig(1, levelToLoad);
        var _jsonFromFile = File.ReadAllText("Assets/Config/Worlds/World_1/level.json");
        arena = JsonConvert.DeserializeObject<Level.Arena>(_jsonFromFile);

        SceneCreator sceneCreator = new SceneCreator();
        sceneCreator.generateTerrainBorders(sceneCreator.terrainPosition, arena.scene);
        sceneCreator.createWindZone(sceneCreator.terrainPosition, arena.scene);

        //LandModel landModel = sceneCreator.createSceneLand(arena.scene, levelToLoad, cube);
        //sceneCreator.GenerateTerrainBorders(sceneCreator.terrainPosition, sceneCreator.terrainSize);
        //sceneCreator.CreateWindZone(sceneCreator.terrainPosition, sceneCreator.terrainSize);
        //TerrainGenerator terrainGenerator = new TerrainGenerator(terrainGo, landModel);
        //terrainGenerator.init();

        enemiesCreator = new EnemiesCreator(arena.scene);
        enemies = enemiesCreator.getEnemies(publicGameObjects.cannonInterfaceCanvas);

        new EnemiesHealthManager(publicGameObjects);

        spawnPlayer = new SpawnPlayer(arena.scene, enemiesCreator);
        playerPrefab = spawnPlayer.getPlayer(publicGameObjects.cannonInterfaceCanvas);
        activePlayer = playerPrefab;

        TargetOnScreenService targetOnScreenService = new TargetOnScreenService(aimTarget, playerConfigurationService);
        TargetPreviewUziService.init(playerConfigurationService, publicGameObjects.targetPreviewUzi);

        targetOnScreenService.onAwake();

        pendulumEffectController = new PendulumEffectController(playerConfigurationService);
        windController = new WindController(playerConfigurationService, publicGameObjects);
        timeManager = new TimeManager(timeRemaining, 30f, false, playerPrefab, publicGameObjects);
        cameraManager = new CameraManager(Camera.main.gameObject, playerPrefab, enemies, publicGameObjects);
        focusOnTargetManager = new FocusOnTargetManager(Camera.main.gameObject, cameraManager, playerPrefab, enemies, publicGameObjects);

        playerController = new PlayerController(activePlayer, enemies, playerConfigurationService, pendulumEffectController, publicGameObjects, cameraManager);
        enemyAIManager = new AIController(arena.metadata, playerController, publicGameObjects, playerPrefab, windController);
        touchManager = new TouchManager(cameraManager, scrollAndPinch, publicGameObjects, playerController, playerConfigurationService);

        fireController = new FireController(aimTarget, playerConfigurationService, playerController, pendulumEffectController);
        switchingController = new SwitchingController(cameraManager, playerController);

        gameRightPanelManager = new GameRightPanelManager(publicGameObjects, arena.scene, focusOnTargetManager, playerMetadataManager);
        endGameScreenManager = new EndGameScreenManager(publicGameObjects);
        deadScreenManager = new DeadScreenManager(publicGameObjects, arena.scene, playerPrefab);
        pauseMenuManager = new PauseMenuManager(publicGameObjects, arena.metadata.stats);
        levelController = new LevelController(publicGameObjects, endGameScreenManager, deadScreenManager);

        cameraManager.setCameraAimingAndShootingPositionAndRotation(activePlayer);
        playerController.changeTargetShieldView();
    }

    void Update()
    {
        //timeManager.OnUpdate();
        touchManager.OnUpdate(enableTouch, activePlayer);
        windController.onUpdate();
        pauseMenuManager.onUpdate();
        focusOnTargetManager.onUpdate();
        resolveStateChange();
        resolveState();
    }

    private void resolveStateChange()
    {
        if (previousStage != currentStage)
        {
            Debug.Log("previous stage: " + previousStage + " | current stage: " + currentStage);
            switch (currentStage)
            {
                case Stage.PLAYER_TURN_AIMING:
                    if (previousStage != Stage.PLAYER_TURN_SHOOTING)
                    {
                        playerController.changeTargetCursorActivity(true);
                        ballPrefab.GetComponent<CannonBall>().activePlayer = activePlayer;
                        timeManager.resetTimer();
                        timeManager.setDefaultLeftTime();
                    }
                    enableTouch = true;
                    break;

                case Stage.PLAYER_TURN_SHOOTING:
                    enableTouch = false;
                    if (cameraManager.isPositionSet)
                    {
                        fireController.OnStart();
                    }
                    break;

                case Stage.OPONNENT_TURN_AIMING:
                    if (previousStage != Stage.OPONNENT_TURN_SHOOTING)
                    {
                        playerController.changeTargetCursorActivity(true);
                        ballPrefab.GetComponent<CannonBall>().activePlayer = activePlayer;
                        timeManager.resetTimer();
                        enemyAIManager.aimStageChange(activePlayer);
                    }
                    break;

                case Stage.OPONNENT_TURN_SHOOTING:
                    enableTouch = false;
                    if (cameraManager.isPositionSet)
                    {
                        fireController.OnStart();
                    }
                    enemyAIManager.shootStageChange();
                    break;

                case Stage.AFTER_SHOOT:
                    timeManager.stopTime = true;
                    enableTouch = false;

                    enemiesCreator.fallOverHandler(enemies, activePlayer);
                    spawnPlayer.fallOverHandler(playerPrefab, activePlayer);

                    switchingController.switchActivePlayer(activePlayer, enemies, playerPrefab);
                    playerController.changeTargetShieldView();
                    break;

                case Stage.END_OF_TIME:
                    switchingController.switchActivePlayer(activePlayer, enemies, playerPrefab);
                    playerController.changeTargetShieldView();
                    break;

                case Stage.CAMERA_MOVEMENT:
                    cameraManager.setCameraAimingAndShootingPositionAndRotation(activePlayer);
                    break;

                case Stage.WIN_GAME:
                    levelController.winGame(arena);
                    break;

                case Stage.LOSE_GAME:
                    levelController.loseGame(playerPrefab);
                    break;
            }
        }
        previousStage = currentStage;
    }

    private void resolveState()
    {
        switch (currentStage)
        {
            case Stage.PLAYER_TURN_AIMING:
                timeManager.setTimeOfFlight();
                break;

            case Stage.PLAYER_TURN_SHOOTING:
                if (cameraManager.isPositionSet)
                {
                    fireController.OnUpdate(activePlayer);
                }
                break;

            case Stage.OPONNENT_TURN_AIMING:
                enemyAIManager.aimOnUpdate();
                timeManager.setTimeOfFlight();
                break;

            case Stage.OPONNENT_TURN_SHOOTING:
                if (cameraManager.isPositionSet)
                {
                    fireController.OnUpdate(activePlayer);
                }
                enemyAIManager.shootOnUpdate(fireController.horizontalLineGo, fireController.verticalLineGo);
                break;

            case Stage.AFTER_SHOOT:
                timeManager.countTimeOfFlight();
                break;

            case Stage.END_OF_TIME:
                timeManager.whoseTimeIsUp(activePlayer);
                break;

            case Stage.CAMERA_MOVEMENT:
                cameraManager.moveCamera(nextStage);
                break;
        }
    }

    public void changeState(Stage nextStageParam)
    {
        currentStage = Stage.CAMERA_MOVEMENT;
        nextStage = nextStageParam;
        cameraManager.changeCameraState(nextStageParam);
    }

    public PlayerController getPayerController()
    {
        return playerController;
    }

    public static GameManager getInstance
    {
        get
        {
            lock (padlock)
            {
                if (INSTANCE == null)
                {
                    INSTANCE = FindObjectOfType(typeof(GameManager)) as GameManager;
                }
                return INSTANCE;
            }
        }
    }

    public FireController getFireController()
    {
        return fireController;
    }

    public GameObject getActivePlayer()
    {
        return activePlayer;
    }

    public PlayerMetadata getActivePlayerMetadata()
    {
        return playerController.getPlayerMetadata(activePlayer.GetInstanceID());
    }

    public void interactWithListOfPlayers()
    {
        gameRightPanelManager.onClick();
    }

    public void loadMainMenu()
    {
        Loader.load((int)Loader.Scenes.MAIN_MENU);
    }

    private void OnDisable()
    {
        PlayerPrefs.Save();
    }
}