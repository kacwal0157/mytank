using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelController
{
    private const string energyAmount = "ENERGY_AMOUNT";
    private const string goldAmount = "GOLD_AMOUNT";
    private const string worldLvlUnlocked = "WORLD1_LEVEL_UNLOCKED";
    private const string highestTrophies = "HIGHEST_TROPHIES_STAT";
    private const string totalPlay = "TOTAL_PLAY_STAT";
    private const string mostWins = "MOST_WINS_STAT";
    private const string lastCompletedLevel = "LAST_COMPLETED_LEVEL";
    private const string worldUnlocked = "WORLD_UNLOCKED";

    private PublicGameObjects publicGameObjects;
    private EndGameScreenManager endGameScreenManager;
    private DeadScreenManager deadScreenManager;

    private List<WorldPrefabs> worldPrefabs;
    private int level;

    public LevelController(PublicGameObjects publicGameObjects, EndGameScreenManager endGameScreenManager, DeadScreenManager deadScreenManager)
    {
        this.publicGameObjects = publicGameObjects;
        this.endGameScreenManager = endGameScreenManager;
        this.deadScreenManager = deadScreenManager;

        onInit();
    }

    private void onInit()
    {
        try
        {
            worldPrefabs = Utilities.convertWorldPrefabsStringToList(PlayerMetadataManager.levelInfo);
            level = MenuManager.levelToLoad - 1;
        }
        catch
        {
            level = 0;

            LevelPrefabs basicLevelPrefabs = new LevelPrefabs();
            basicLevelPrefabs.isAvailable = true;
            basicLevelPrefabs.firstTimePlay = true;
            basicLevelPrefabs.minScore = 0;
            basicLevelPrefabs.maxScore = 0;
            basicLevelPrefabs.score = 0;
            basicLevelPrefabs.starNumber = 0;
            basicLevelPrefabs.earnedExp = 0;

            List<LevelPrefabs> levelPrefabs = new List<LevelPrefabs>();
            levelPrefabs.Add(basicLevelPrefabs);

            WorldPrefabs basicWorldPrefabs = new WorldPrefabs();
            basicWorldPrefabs.levelPrefabs = levelPrefabs;

            worldPrefabs = new List<WorldPrefabs>();
            worldPrefabs.Add(basicWorldPrefabs);
        }
    }

    public void winGame(Level.Arena arena)
    {
        Debug.Log("You killed all enemies");
        publicGameObjects.gameResultScreen.SetActive(true);

        if (worldPrefabs[0].levelPrefabs[level].firstTimePlay)
        {
            List<string> rewardsList = new List<string>();
            rewardsList.Add(arena.metadata.rewardsFirstTime.energy.ToString());
            rewardsList.Add(arena.metadata.rewardsFirstTime.gold.ToString());
            rewardsList.Add(arena.metadata.rewardsFirstTime.item1);
            rewardsList.Add(arena.metadata.rewardsFirstTime.item2);
            rewardsList.Add(arena.metadata.rewardsFirstTime.item3);

            //TODO: THINK ABOUT LOGIC THAT GIVES PLAYER DEFINED AMOUNT OF SCORE, EXP ETC.
            var energy = PlayerPrefs.GetInt(energyAmount) + arena.metadata.rewardsFirstTime.energy;
            var gold = PlayerPrefs.GetInt(goldAmount) + arena.metadata.rewardsFirstTime.gold;

            PlayerPrefs.SetInt(energyAmount, energy);
            PlayerPrefs.SetInt(goldAmount, gold);

            endGameScreenManager.getEndedLevelProperties(worldPrefabs[0].levelPrefabs[level], rewardsList);
        }
        else
        {
            List<string> rewardsList = new List<string>();
            rewardsList.Add(arena.metadata.rewards.energy.ToString());
            rewardsList.Add(arena.metadata.rewards.gold.ToString());

            var energy = PlayerPrefs.GetInt(energyAmount) + arena.metadata.rewards.energy;
            var gold = PlayerPrefs.GetInt(goldAmount) + arena.metadata.rewards.gold;

            PlayerPrefs.SetInt(energyAmount, energy);
            PlayerPrefs.SetInt(goldAmount, gold);

            endGameScreenManager.getEndedLevelProperties(worldPrefabs[0].levelPrefabs[level], rewardsList);
        }

        setWorldPrefabs(true);
    }

    public void loseGame(GameObject player)
    {
        if(player.activeSelf)
        {
            return;
        }
        else
        {
            deadScreenManager.initializeDeadScreen();
            setWorldPrefabs(false);
        }
    }

    private void setWorldPrefabs(bool win)
    {
        //TODO: MAKE LOGIC FOR STAR AND SCORE GIVING (FOR SURE IN DIFFERENT CLASS)
        int score = 100 * (level + 1);
        int starNumber = 3;
        float levelExp = (1 + (7 * Mathf.Sqrt(level + 1))) * 2;

        if (win)
        {
            var nextLevelInQueue = level + 1;
            if (PlayerMetadataManager.levelUnlocked < nextLevelInQueue)
            {
                var nextLevelJson = JsonReader.loadWorldConfig(1, nextLevelInQueue + 1);
                if (nextLevelJson != null)
                {
                    if (nextLevelJson.metadata.stats.unlockScore <= score * starNumber)
                    {
                        var unlockedLevel = level + 1;
                        PlayerPrefs.SetInt(worldLvlUnlocked, unlockedLevel);
                    }
                }
                else
                {
                    var stage = PlayerPrefs.GetInt(worldUnlocked);
                    stage++;
                    PlayerPrefs.SetInt(worldUnlocked, stage);
                }
            }

            var mostWinsStat = int.Parse(PlayerMetadataManager.mostWinsStat);
            mostWinsStat++;
            PlayerPrefs.SetString(mostWins, mostWinsStat.ToString());
            PlayerPrefs.SetInt(lastCompletedLevel, level + 1);
        }
        else
        {
            score = Mathf.RoundToInt(score * 0.7f);
            starNumber = 0;
            levelExp *= 0.5f;
            PlayerPrefs.SetInt(lastCompletedLevel, 0);
        }

        if (worldPrefabs[0].levelPrefabs[level].firstTimePlay)
        {
            worldPrefabs[0].levelPrefabs[level].firstTimePlay = false;
        }

        worldPrefabs[0].levelPrefabs[level].score = score * starNumber;
        worldPrefabs[0].levelPrefabs[level].starNumber = starNumber;
        //maybe rename this cause' its for tank?
        worldPrefabs[0].levelPrefabs[level].earnedExp = Mathf.RoundToInt(levelExp);
        PlayerMetadataManager.setLevelInfo(worldPrefabs);

        var highestTrophiesStat = int.Parse(PlayerMetadataManager.highestTrophiesStat);
        highestTrophiesStat += starNumber;
        PlayerPrefs.SetString(highestTrophies, highestTrophiesStat.ToString());

        var totalPlayStat = int.Parse(PlayerMetadataManager.totalPlayStat);
        totalPlayStat += score;
        PlayerPrefs.SetString(totalPlay, totalPlayStat.ToString());
    }
}
