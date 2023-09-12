using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AwardsManager
{
    private List<WorldPrefabs> worldPrefabs;
    private PublicGameObjects publicGameObjects;

    public AwardsManager(PublicGameObjects publicGameObjects)
    {
        this.publicGameObjects = publicGameObjects;
        worldPrefabs = Utilities.convertWorldPrefabsStringToList(PlayerMetadataManager.levelInfo);
    }

    public void showAwards(int levelNumer)
    {
        var firstTimeAwards = publicGameObjects.firstTimeAwardsPopUps[levelNumer];
        var awardsGO = publicGameObjects.awardsPopUps[levelNumer];

        if (worldPrefabs[0].levelPrefabs[levelNumer].firstTimePlay.Equals(true))
        {
            firstTimeAwards.SetActive(true);
        }
        else
        {
            awardsGO.SetActive(true);
        }
    }
}
