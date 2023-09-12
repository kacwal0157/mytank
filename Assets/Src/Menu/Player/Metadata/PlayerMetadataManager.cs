using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerMetadataManager
{
    public static string levelInfo;
    public static string highestTrophiesStat;
    public static string highestStageStat;
    public static string mostWinsStat;
    //public static string soloVictoriesStat;
    //public static string duoVictoriesStat;
    //public static string tripleVictoriesStat;
    //public static string bossVictioriesStat;
    public static string totalPlayStat;
    //public static string distoryStat;
    public static int levelUnlocked;

    public bool firstPlay;
    public string usernamesDict;
    public string lastLogIn;
    public string username;
    public string language;
    public string spinTimes;
    public string tankPrefabName;
    public string panzer1;
    public string panzer2;
    public string panzer3;
    public string panzer4;
    public string panzer5;
    public string lvlPanzer1;
    public string lvlPanzer2;
    public string lvlPanzer3;
    public string lvlPanzer4;
    public string lvlPanzer5;
    public int logInDayStrike;
    public int claimedDailyRewards;
    public int currentExp;
    public int worldUnlocked;
    public int lastCompletedLevel;
    public int previousExpNeeded;
    public int expToLevelUp;
    public int playerLevel;
    public int energyAmount;
    public int goldAmount;
    public int gemAmount;
    public int dailySpinGold;
    public int maxEnergyAmount = 60;
    public List<WorldPrefabs> worldPrefabs;
    public int inventorySlotsQuantity;

    private const string FIRST_LAUNCH = "FIRST_LAUNCH";
    private const string PLAYER_TAG = "PLAYER_TAG";
    private const string LAST_LOG_IN = "LAST_LOG_IN";
    private const string LOG_IN_STRIKE = "LOG_IN_STRIKE";
    private const string CLAIMED_DAILY_REWARDS = "CLAIMED_DAILY_REWARDS";
    private const string HIGHEST_TROPHIES_STAT = "HIGHEST_TROPHIES_STAT";
    private const string HIGHEST_STAGE_STAT = "HIGHEST_STAGE_STAT";
    private const string MOST_WINS_STAT = "MOST_WINS_STAT";
    //private const string SOLO_VICTORIES_STAT = "SOLO_VICTORIES_STAT";
    //private const string DUO_VICTORIES_STAT = "DUO_VICTORIES_STAT";
    //private const string TRIPLE_VICTORIES_STAT = "TRIPLE_VICTORIES_STAT";
    //private const string BOSS_VICTORIES_STAT = "BOSS_VICTORIES_STAT";
    private const string TOTAL_PLAY_STAT = "TOTAL_PLAY_STAT";
    //private const string DISTORY_STAT = "DISTORY_STAT";
    private const string USERNAMES_DICTIONARY = "USERNAMES_DICTIONARY";
    private const string LEVEL_INFO = "LEVEL_INFO";
    private const string CURRENT_EXP = "CURRENT_EXP";
    private const string PREVIOUS_LEVEL_EXP = "PREVIOUS_LEVEL_EXP";
    private const string NEXT_LEVEL_EXP = "NEXT_LEVEL_EXP";
    private const string PLAYER_LEVEL = "PLAYER_LEVEL";
    private const string USERNAME = "USERNAME";
    private const string LANGUAGE = "LANGUAGE";
    private const string WORLD_UNLOCKED = "WORLD_UNLOCKED";
    private const string WORLD1_LEVEL_UNLOCKED = "WORLD1_LEVEL_UNLOCKED";
    private const string LAST_COMPLETED_LEVEL = "LAST_COMPLETED_LEVEL";
    private const string ENERGY_AMOUNT = "ENERGY_AMOUNT";
    private const string MAX_ENERGY_AMOUNT = "MAX_ENERGY_AMOUNT";
    private const string GOLD_AMOUNT = "GOLD_AMOUNT";
    private const string GEM_AMOUNT = "GEM_AMOUNT";
    private const string DAILY_SPIN_GOLD = "DAILY_SPIN_GOLD";
    private const string SPIN_TIMES = "SPIN_TIMES";
    private const string TANK_PREFAB = "TANK_PREFAB";
    private const string PANZER1_STATS = "PANZER1_STATS";
    private const string PANZER2_STATS = "PANZER2_STATS";
    private const string PANZER3_STATS = "PANZER3_STATS";
    private const string PANZER4_STATS = "PANZER4_STATS";
    private const string PANZER5_STATS = "PANZER5_STATS";
    private const string PANZER1_LEVEL = "PANZER1_LEVEL";
    private const string PANZER2_LEVEL = "PANZER2_LEVEL";
    private const string PANZER3_LEVEL = "PANZER3_LEVEL";
    private const string PANZER4_LEVEL = "PANZER4_LEVEL";
    private const string PANZER5_LEVEL = "PANZER5_LEVEL";
    private const string PLAYER_INVENTORY = "PLAYER_INVENTORY";
    private const string PLAYER_INVENTORY_BULLETS = "PLAYER_INVENTORY_BULLETS";
    private const string PLAYER_INVENTORY_SLOTS_QUANTITY = "PLAYER_INVENTORY_SLOTS_QUANTITY";

    public PlayerMetadataManager()
    {
        setLevelInfo();
        onInit();

        firstTimeHere();
        setLastLogInTime();
        setLogInDayStrike();
        setUsernamesDict();
        setPlayerStats();
        setCurrentExp();
        setExpNeedForPreviousLvl();
        setExpNeedToLevelUp();
        setPlayerLevel();
        setUsername();
        setLanguage();
        setUnlockedStage();
        setUnlockedLevel();
        setLastCompletedLevel();
        setEnergyAmount();
        setCoinAmount();
        setGemAmount();
        setDailySpinGold();
        setSpinTimes();
        setTankPrefab();
        setTanksStats();
        setTankLevelAndStars();
        setInventorySlotsQuantity();
    }

    public void saveAllPrefs()
    {
        PlayerPrefs.SetString(LAST_LOG_IN, lastLogIn);
        PlayerPrefs.SetInt(LOG_IN_STRIKE, logInDayStrike);
        PlayerPrefs.SetInt(CLAIMED_DAILY_REWARDS, claimedDailyRewards);
        PlayerPrefs.SetString(USERNAMES_DICTIONARY, usernamesDict);
        PlayerPrefs.SetString(HIGHEST_TROPHIES_STAT, highestTrophiesStat);
        PlayerPrefs.SetString(HIGHEST_STAGE_STAT, highestStageStat);
        PlayerPrefs.SetString(MOST_WINS_STAT, mostWinsStat);
        //PlayerPrefs.SetString(SOLO_VICTORIES_STAT, soloVictoriesStat);
        //PlayerPrefs.SetString(DUO_VICTORIES_STAT, duoVictoriesStat);
        //PlayerPrefs.SetString(TRIPLE_VICTORIES_STAT, tripleVictoriesStat);
        //PlayerPrefs.SetString(BOSS_VICTORIES_STAT, bossVictioriesStat);
        PlayerPrefs.SetString(TOTAL_PLAY_STAT, totalPlayStat);
        //PlayerPrefs.SetString(DISTORY_STAT, distoryStat);
        PlayerPrefs.SetInt(CURRENT_EXP, currentExp);
        PlayerPrefs.SetInt(PREVIOUS_LEVEL_EXP, previousExpNeeded);
        PlayerPrefs.SetInt(NEXT_LEVEL_EXP, expToLevelUp);
        PlayerPrefs.SetInt(WORLD_UNLOCKED, worldUnlocked);
        PlayerPrefs.SetInt(WORLD1_LEVEL_UNLOCKED, levelUnlocked);
        PlayerPrefs.SetInt(LAST_COMPLETED_LEVEL, lastCompletedLevel);
        PlayerPrefs.SetString(LEVEL_INFO, levelInfo);
        PlayerPrefs.SetInt(PLAYER_LEVEL, playerLevel);
        PlayerPrefs.SetString(USERNAME, username);
        PlayerPrefs.SetString(LANGUAGE, language);
        PlayerPrefs.SetInt(ENERGY_AMOUNT, energyAmount);
        PlayerPrefs.SetInt(MAX_ENERGY_AMOUNT, maxEnergyAmount);
        PlayerPrefs.SetInt(GOLD_AMOUNT, goldAmount);
        PlayerPrefs.SetInt(GEM_AMOUNT, gemAmount);
        PlayerPrefs.SetInt(DAILY_SPIN_GOLD, dailySpinGold);
        PlayerPrefs.SetString(SPIN_TIMES, spinTimes);
        PlayerPrefs.SetString(TANK_PREFAB, tankPrefabName);
        PlayerPrefs.SetString(PANZER1_STATS, panzer1);
        PlayerPrefs.SetString(PANZER2_STATS, panzer2);
        PlayerPrefs.SetString(PANZER3_STATS, panzer3);
        PlayerPrefs.SetString(PANZER4_STATS, panzer4);
        PlayerPrefs.SetString(PANZER5_STATS, panzer5);
        PlayerPrefs.SetString(PANZER1_LEVEL, lvlPanzer1);
        PlayerPrefs.SetString(PANZER2_LEVEL, lvlPanzer2);
        PlayerPrefs.SetString(PANZER3_LEVEL, lvlPanzer3);
        PlayerPrefs.SetString(PANZER4_LEVEL, lvlPanzer4);
        PlayerPrefs.SetString(PANZER5_LEVEL, lvlPanzer5);
        PlayerPrefs.SetInt(PLAYER_INVENTORY_SLOTS_QUANTITY, inventorySlotsQuantity);
        PlayerPrefs.Save();
    }

    private void firstTimeHere()
    {
        if (PlayerPrefs.GetInt(FIRST_LAUNCH, 0) == 0)
        {
            PlayerPrefs.DeleteAll();
            firstPlay = true;

            PlayerPrefs.SetInt(FIRST_LAUNCH, 1);
            PlayerPrefs.SetString(PLAYER_TAG, Utilities.getRandomTag(8));
        }
        else
        {
            firstPlay = false;
        }
    }

    private void setLastLogInTime()
    {
        setPrefs(LAST_LOG_IN, ref lastLogIn, DateTime.Now.ToString());
    }

    private void setLogInDayStrike()
    {
        setPrefs(LOG_IN_STRIKE, ref logInDayStrike, 0);
        setPrefs(CLAIMED_DAILY_REWARDS, ref claimedDailyRewards, 0);
    }

    //in cloud server in the future
    private void setUsernamesDict()
    {
        setPrefs(USERNAMES_DICTIONARY, ref usernamesDict, string.Empty);
    }

    private void setLevelInfo()
    {
        setPrefs(LEVEL_INFO, ref levelInfo, "");
    }

    private void setPlayerStats()
    {
        setPrefs(HIGHEST_TROPHIES_STAT, ref highestTrophiesStat, "0");
        setPrefs(HIGHEST_STAGE_STAT, ref highestStageStat, "0");
        setPrefs(MOST_WINS_STAT, ref mostWinsStat, "0");
        //setPrefs(SOLO_VICTORIES_STAT, ref soloVictoriesStat, "0");
        //setPrefs(DUO_VICTORIES_STAT, ref duoVictoriesStat, "0");
        //setPrefs(tripleVictoriesStat, ref tripleVictoriesStat, "0");
        //setPrefs(BOSS_VICTORIES_STAT, ref bossVictioriesStat, "0");
        setPrefs(TOTAL_PLAY_STAT, ref totalPlayStat, "0");
        //setPrefs(DISTORY_STAT, ref distoryStat, "0");
    }

    private void setCurrentExp()
    {
        setPrefs(CURRENT_EXP, ref currentExp, 0);
    }

    private void setExpNeedForPreviousLvl()
    {
        setPrefs(PREVIOUS_LEVEL_EXP, ref previousExpNeeded, 0);
    }

    private void setExpNeedToLevelUp()
    {
        setPrefs(NEXT_LEVEL_EXP, ref expToLevelUp, 0);
    }

    private void setPlayerLevel()
    {
        setPrefs(PLAYER_LEVEL, ref playerLevel, 1);
    }

    private void setUsername()
    {
        setPrefs(USERNAME, ref username, "Player");
    }

    private void setLanguage()
    {
        setPrefs(LANGUAGE, ref language, "ENG");
    }

    private void setUnlockedStage()
    {
        setPrefs(WORLD_UNLOCKED, ref worldUnlocked, 1);
    }

    private void setLastCompletedLevel()
    {
        setPrefs(LAST_COMPLETED_LEVEL, ref lastCompletedLevel, 0);
    }

    private void setInventorySlotsQuantity()
    {
        setPrefs(PLAYER_INVENTORY_SLOTS_QUANTITY, ref inventorySlotsQuantity, 20);
    }

    private void setTankPrefab()
    {
        setPrefs(TANK_PREFAB, ref tankPrefabName, "Panzer2");
    }

    private void setTanksStats()
    {
        List<int> p1 = new List<int>();
        List<int> p2 = new List<int>();
        List<int> p3 = new List<int>();
        List<int> p4 = new List<int>();
        List<int> p5 = new List<int>();

        initializeTankStatsList(p1, 50, 10, 20, 1, 1, 1);
        setPrefs(PANZER1_STATS, ref panzer1, Utilities.convertListToString(p1));

        initializeTankStatsList(p2, 560, 15, 10, 1, 1, 1);
        setPrefs(PANZER2_STATS, ref panzer2, Utilities.convertListToString(p2));

        initializeTankStatsList(p3, 45, 10, 25, 1, 1, 1);
        setPrefs(PANZER3_STATS, ref panzer3, Utilities.convertListToString(p3));

        initializeTankStatsList(p4, 45, 15, 20, 1, 1, 1);
        setPrefs(PANZER4_STATS, ref panzer4, Utilities.convertListToString(p4));

        initializeTankStatsList(p5, 55, 10, 15, 1, 1, 1);
        setPrefs(PANZER5_STATS, ref panzer5, Utilities.convertListToString(p5));
    }

    private void setTankLevelAndStars()
    {
        //TODO: improve stars mechanic
        List<int> p1 = new List<int>();
        List<int> p2 = new List<int>();
        List<int> p3 = new List<int>();
        List<int> p4 = new List<int>();
        List<int> p5 = new List<int>();

        initializeTankLevelAndStars(p1, 19, 60, 50, 0);
        setPrefs(PANZER1_LEVEL, ref lvlPanzer1, Utilities.convertListToString(p1));

        initializeTankLevelAndStars(p2, 1, 60, 50, 0);
        setPrefs(PANZER2_LEVEL, ref lvlPanzer2, Utilities.convertListToString(p2));

        initializeTankLevelAndStars(p3, 1, 0, 50, 0);
        setPrefs(PANZER3_LEVEL, ref lvlPanzer3, Utilities.convertListToString(p3));

        initializeTankLevelAndStars(p4, 1, 0, 50, 0);
        setPrefs(PANZER4_LEVEL, ref lvlPanzer4, Utilities.convertListToString(p4));

        initializeTankLevelAndStars(p5, 1, 0, 50, 0);
        setPrefs(PANZER5_LEVEL, ref lvlPanzer5, Utilities.convertListToString(p5));
    }

    private void setUnlockedLevel()
    {
        setPrefs(WORLD1_LEVEL_UNLOCKED, ref levelUnlocked, 0);
    }

    private void setEnergyAmount()
    {
        setPrefs(ENERGY_AMOUNT, ref energyAmount, 60);
        setPrefs(MAX_ENERGY_AMOUNT, ref maxEnergyAmount, 60);
    }

    private void setCoinAmount()
    {
        setPrefs(GOLD_AMOUNT, ref goldAmount, 10000);
    }

    private void setGemAmount()
    {
        setPrefs(GEM_AMOUNT, ref gemAmount, 100);
    }

    private void setDailySpinGold()
    {
        setPrefs(DAILY_SPIN_GOLD, ref dailySpinGold, 2);
    }

    //TODO: IS IT WORKING?
    private void setSpinTimes()
    {
        var defaultList = new List<System.DateTime>() { new System.DateTime(1970, 1, 1, 0, 0, 0, System.DateTimeKind.Utc), new System.DateTime(1970, 1, 1, 0, 0, 0, System.DateTimeKind.Utc) };
        setPrefs(SPIN_TIMES, ref spinTimes, Utilities.convertListToString(defaultList));
    }

    private void setPrefs(string name, ref int value, int defaultValue)
    {
        if (PlayerPrefs.HasKey(name))
            value = PlayerPrefs.GetInt(name);
        else
        {
            PlayerPrefs.SetInt(name, defaultValue);
            value = defaultValue;
        }
    }

    private void setPrefs(string name, ref string value, string defaultValue)
    {
        if (PlayerPrefs.HasKey(name))
            value = PlayerPrefs.GetString(name);
        else
        {
            PlayerPrefs.SetString(name, defaultValue);
            value = defaultValue;
        }
    }

    private void onInit()
    {
        if (levelInfo.Equals(""))
        {
            worldPrefabs = initializedLevelPrefs();
            setLevelInfo(worldPrefabs);
        }
        else
        {
            worldPrefabs = Utilities.convertWorldPrefabsStringToList(levelInfo);
        }
    }

    public static void setLevelInfo(List<WorldPrefabs> worldPrefabs)
    {
        string worldPrefabString = Utilities.convertListToString(worldPrefabs);
        PlayerPrefs.SetString("LEVEL_INFO", worldPrefabString);
        levelInfo = worldPrefabString;
    }

    private List<WorldPrefabs> initializedLevelPrefs()
    {
        List<WorldPrefabs> result = new List<WorldPrefabs>();
        WorldPrefabs worldPrefabs = new WorldPrefabs();

        worldPrefabs.levelPrefabs = new List<LevelPrefabs>();

        for (int i = 1; i <= 7; i++)
        {
            LevelPrefabs levelPrefabs = new LevelPrefabs();
            if (i == 1)
            {
                levelPrefabs.isAvailable = true;
            }
            else
            {
                levelPrefabs.isAvailable = false;
            }

            levelPrefabs.firstTimePlay = true;

            worldPrefabs.levelPrefabs.Add(levelPrefabs);
        }

        result.Add(worldPrefabs);

        return result;
    }

    private void initializeTankStatsList(List<int> tank, int health, int attack, int defence, int healthLVL, int attackLVL, int defenceLVL)
    {
        tank.Add(health);
        tank.Add(attack);
        tank.Add(defence);
        tank.Add(healthLVL);
        tank.Add(attackLVL);
        tank.Add(defenceLVL);
    }

    private void initializeTankLevelAndStars(List<int> tank, int level, int value, int maxValue, int minValue)
    {
        tank.Add(level);
        tank.Add(value);
        tank.Add(maxValue);
        tank.Add(minValue);
    }

    public string getInventory()
    {
        return PlayerPrefs.GetString(PLAYER_INVENTORY, string.Empty);
    }

    public void setInventory(string items)
    {
        PlayerPrefs.SetString(PLAYER_INVENTORY, items);
    }

    public string getInventoryBullets()
    {
        return PlayerPrefs.GetString(PLAYER_INVENTORY_BULLETS, string.Empty);
    }

    public void setInventoryBullets(string items)
    {
        PlayerPrefs.SetString(PLAYER_INVENTORY_BULLETS, items);
    }
}