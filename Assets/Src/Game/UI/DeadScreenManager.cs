using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class DeadScreenManager
{
    private PublicGameObjects publicGameObjects;

    private GameObject deadScreenGO;
    private GameObject continueGemBtn;
    private GameObject continueAdBtn;
    private GameObject returnBtn;
    private GameObject playerGO;
    private TextMeshProUGUI textContinue;

    private int gemAmount;
    private bool firstDead;

    public DeadScreenManager(PublicGameObjects publicGameObjects, Level.Scene scene, GameObject player)
    {
        this.publicGameObjects = publicGameObjects;
        playerGO = player;

        onInit();
    }

    private void onInit()
    {
        deadScreenGO = publicGameObjects.deadScreen;
        gemAmount = PlayerPrefs.GetInt("GEM_AMOUNT");
        firstDead = true;
    }

    public void initializeDeadScreen()
    {
        deadScreenGO.SetActive(true);

        continueGemBtn = deadScreenGO.transform.GetChild(5).gameObject;
        continueAdBtn = deadScreenGO.transform.GetChild(6).gameObject;
        returnBtn = deadScreenGO.transform.GetChild(7).gameObject;

        textContinue = deadScreenGO.transform.GetChild(4).GetComponent<TextMeshProUGUI>();

        if (!firstDead)
        {
            continueGemBtn.SetActive(false);
            continueAdBtn.SetActive(false);

            textContinue.text = "Good luck next time!";
            returnBtn.SetActive(true);

            return;
        }

        textContinue.text = "Continue?";
        continueGemBtn.GetComponent<Button>().onClick.AddListener(continuteGameForGems);
    }

    private void continuteGameForGems()
    {
        var cost = int.Parse(continueGemBtn.GetComponent<Button>().GetComponentInChildren<TextMeshProUGUI>().text);

        if(cost <= gemAmount)
        {
            gemAmount -= cost;
            PlayerPrefs.SetInt("GEM_AMOUNT", gemAmount);

            deadScreenGO.SetActive(false);
            playerGO.SetActive(true);
            playerGO.GetComponent<HealthBar>().HealthbarPrefab.gameObject.SetActive(true);
            playerGO.GetComponent<EnemyHealth>().health = 560;
            GameManager.currentStage = GameManager.Stage.PLAYER_TURN_AIMING;
        }

        firstDead = false;
    }
}
