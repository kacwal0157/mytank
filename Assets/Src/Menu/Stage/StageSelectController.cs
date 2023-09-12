using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class StageSelectController
{
    private const string playerLevel = "PLAYER_LEVEL";

    private List<Button> stagesBtn;
    private List<Transform> stagesTransform;
    private int worldUnlocked;

    public StageSelectController(PublicGameObjects publicGameObjects, PlayerMetadataManager playerMetadataManager)
    {
        stagesBtn = publicGameObjects.stagesBtn;
        stagesTransform = publicGameObjects.stages;
        worldUnlocked = playerMetadataManager.worldUnlocked;

        onInit();
    }

    private void onInit()
    {
        var worldsFromJson = JsonReader.loadWorldConfig();
        handleWorldUnlocking(worldsFromJson);
    }

    private void handleWorldUnlocking(Worlds world)
    {
        var playerLvl = PlayerPrefs.GetInt(playerLevel);
        var requiredLvlForNextStage = world.worldConfig[worldUnlocked - 1].requiredLevel;

        if(playerLvl < requiredLvlForNextStage)
        {
            Debug.Log("You don't have required level for next stage. You will unlock this stage later.");
        }
        else
        {
            for (int index = 0; index < worldUnlocked; index++)
            {
                stagesBtn[index].interactable = true;
                stagesTransform[index].GetChild(1).gameObject.SetActive(false);
                stagesTransform[index].GetChild(3).gameObject.SetActive(false);
            }
        }
    }
}
