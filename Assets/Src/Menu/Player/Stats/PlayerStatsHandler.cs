using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerStatsHandler
{
    private PlayerMetadataManager playerMetadataManager;
    private PublicGameObjects publicGameObjects;

    private TextMeshProUGUI highestTrophies;
    private TextMeshProUGUI highestStage;
    private TextMeshProUGUI mostWins;
    private TextMeshProUGUI soloVictiories;
    private TextMeshProUGUI duoVictiories;
    private TextMeshProUGUI tripleVictiories;
    private TextMeshProUGUI bossVictiories;
    private TextMeshProUGUI totalPlay;
    private TextMeshProUGUI distory;


    public PlayerStatsHandler(PlayerMetadataManager playerMetadataManager, PublicGameObjects publicGameObjects)
    {
        this.playerMetadataManager = playerMetadataManager;
        this.publicGameObjects = publicGameObjects;

        onInit();
    }

    private void onInit()
    {
        initializeBasicVariables(publicGameObjects.userInfoPopUp.transform.GetChild(1).transform.GetChild(2).transform.GetChild(1).gameObject);
        getValuesFromPrefs();
    }

    private void initializeBasicVariables(GameObject playerStats)
    {
        //TODO: think about other stats (mostWins is done)
        highestTrophies = playerStats.transform.GetChild(0).transform.GetChild(0).transform.GetChild(2).GetComponent<TextMeshProUGUI>();
        highestStage = playerStats.transform.GetChild(0).transform.GetChild(1).transform.GetChild(2).GetComponent<TextMeshProUGUI>();
        mostWins = playerStats.transform.GetChild(0).transform.GetChild(2).transform.GetChild(2).GetComponent<TextMeshProUGUI>();

        soloVictiories = playerStats.transform.GetChild(1).transform.GetChild(0).transform.GetChild(2).GetComponent<TextMeshProUGUI>();
        duoVictiories = playerStats.transform.GetChild(1).transform.GetChild(1).transform.GetChild(2).GetComponent<TextMeshProUGUI>();
        tripleVictiories = playerStats.transform.GetChild(1).transform.GetChild(2).transform.GetChild(2).GetComponent<TextMeshProUGUI>();

        bossVictiories = playerStats.transform.GetChild(2).transform.GetChild(0).transform.GetChild(2).GetComponent<TextMeshProUGUI>();
        totalPlay = playerStats.transform.GetChild(2).transform.GetChild(1).transform.GetChild(2).GetComponent<TextMeshProUGUI>();
        distory = playerStats.transform.GetChild(2).transform.GetChild(2).transform.GetChild(2).GetComponent<TextMeshProUGUI>();
    }

    private void getValuesFromPrefs()
    {
        highestTrophies.text = PlayerMetadataManager.highestTrophiesStat;
        highestStage.text = PlayerMetadataManager.highestStageStat;
        mostWins.text = PlayerMetadataManager.mostWinsStat;

        //soloVictiories = PlayerMetadataManager.soloVictoriesStat;
        //duoVictiories = PlayerMetadataManager.duoVictoriesStat;
        //tripleVictiories = PlayerMetadataManager.tripleVictoriesStat;

        //bossVictiories = PlayerMetadataManager.bossVictioriesStat;
        totalPlay.text = PlayerMetadataManager.totalPlayStat;
        //distory = PlayerMetadataManager.distoryStat;
    }
}
