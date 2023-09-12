using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelLoadingManager : MonoBehaviour
{
    private UserInfoManager userInfoManager;

    private void Start()
    {
        userInfoManager = MenuManager.getInstance.userInfo;
    }

    public void loadLevelInWorld_1(int levelToLoad)
    {
        MenuManager.levelToLoad = levelToLoad;

        var arena = JsonReader.loadWorldConfig(1, levelToLoad);
        var energyCost = arena.metadata.stats.energyCost;

        if (!userInfoManager.checkEnergyAmount(energyCost))
        {
            Application.Quit();
        }
        else
        {
            userInfoManager.takeEnergy(energyCost);
            Loader.load(getLevelIndexInWorld_1(levelToLoad));
        }
    }

    private int getLevelIndexInWorld_1(int level)
    {
        Loader.Scenes levelIndex = Loader.Scenes.MAIN_MENU; //default value in case of error

        switch(level)
        {
            case 1:
                levelIndex = Loader.Scenes.WORLD_1_LEVEL_1;
                break;
            case 2:
                levelIndex = Loader.Scenes.WORLD_1_LEVEL_2;
                break;
        }

        levelIndex = Loader.Scenes.WORLD_1_LEVEL;
        return (int)levelIndex;
    }
}
