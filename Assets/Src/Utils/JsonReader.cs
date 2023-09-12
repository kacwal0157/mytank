using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Newtonsoft.Json;

public static class JsonReader
{
    public static Level.Arena loadWorldConfig(int worldNum, int levelToLoad)
    {
        if (!File.Exists("Assets/Config/Worlds/World_" + worldNum.ToString() + "/level_" + levelToLoad.ToString() + ".json"))
        {
            return null;
        }

        var _jsonFromFile = File.ReadAllText("Assets/Config/Worlds/World_" + worldNum.ToString() + "/level_" + levelToLoad.ToString() + ".json");

        Level.Arena arena = JsonConvert.DeserializeObject<Level.Arena>(_jsonFromFile);
        return arena;
    }

    public static LevelUpRewards loadLevelUpRewards()
    {
        var _jsonFromFile = File.ReadAllText("Assets/Config/Rewards/Level/LevelUpRewards.json");

        LevelUpRewards levelUpRews = JsonConvert.DeserializeObject<LevelUpRewards>(_jsonFromFile);
        return levelUpRews;
    }

    public static DailyRewards loadDailyRewards()
    {
        var _jsonFromFile = File.ReadAllText("Assets/Config/Rewards/Daily/DailyRewards.json");

        DailyRewards daily = JsonConvert.DeserializeObject<DailyRewards>(_jsonFromFile);
        return daily;
    }

    public static Worlds loadWorldConfig()
    {
        var _jsonFromFile = File.ReadAllText("Assets/Config/Worlds/worlds.json");

        Worlds worlds = JsonConvert.DeserializeObject<Worlds>(_jsonFromFile);
        return worlds;
    }
}
