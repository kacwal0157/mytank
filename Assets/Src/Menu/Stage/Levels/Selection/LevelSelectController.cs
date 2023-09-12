using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSelectController
{
    private PublicGameObjects publicGameObjects;
    private PlayerMetadataManager playerMetadataManager;

    private int activeLevelNumber;
    private List<Transform> levels;

    public LevelSelectController(PublicGameObjects publicGameObjects, PlayerMetadataManager playerMetadataManager)
    {
        this.publicGameObjects = publicGameObjects;
        this.playerMetadataManager = playerMetadataManager;

        onInit();
    }

    private void onInit()
    {
        activeLevelNumber = PlayerMetadataManager.levelUnlocked;
        levels = publicGameObjects.levelsList;

        setUnlockedLevelActiveSelf();
    }

    private void setUnlockedLevelActiveSelf()
    {
        if (levels[activeLevelNumber].GetChild(1).transform.GetChild(3).gameObject.activeSelf)
        {
            levels[activeLevelNumber].GetChild(1).transform.GetChild(3).gameObject.SetActive(false);
            levels[activeLevelNumber].GetChild(1).transform.GetChild(2).gameObject.SetActive(true);

            levels[activeLevelNumber - 1].GetChild(1).transform.GetChild(2).gameObject.SetActive(false);
            levels[activeLevelNumber - 1].GetChild(1).transform.GetChild(1).gameObject.SetActive(true);

            checkLevelsPath(activeLevelNumber - 1);
        }

        publicGameObjects.levelsList = levels;
    }


    private void checkLevelsPath(int previousLevel)
    {
        foreach(Transform t in levels[previousLevel].GetChild(1).transform)
        {
            if(t.name.Contains("_Top") && t.gameObject.activeSelf)
            {
                t.GetChild(0).gameObject.SetActive(true);
                t.GetChild(1).gameObject.SetActive(false);
            }

            if (t.name.Contains("_Bottom") && t.gameObject.activeSelf)
            {
                t.GetChild(0).gameObject.SetActive(true);
                t.GetChild(1).gameObject.SetActive(false);
            }
        }
    }
}
