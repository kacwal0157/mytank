using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Loader
{
    public enum Scenes
    {
        TITLE_SCREEN = 0,
        LOADING_SCREEN = 1,
        MAIN_MENU = 2,
        WORLD_1_LEVEL_1 = 3,
        WORLD_1_LEVEL_2 = 4,
        WORLD_1_LEVEL = 5
    }

    public static void load(int levelNum)
    {
        LoadingManager.sceneIndex = levelNum;
        SceneManager.LoadScene((int)Scenes.LOADING_SCREEN);
    }
}
