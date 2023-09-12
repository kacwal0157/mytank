using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UserInfoPopUpHandler
{
    private PublicGameObjects publicGameObjects;

    private GameObject popUpGO;
    private TextMeshProUGUI username;
    private TextMeshProUGUI playerLvl;
    private Slider lvlSlider;
    private TextMeshProUGUI playerExp;

    public UserInfoPopUpHandler(PublicGameObjects publicGameObjects)
    {
        this.publicGameObjects = publicGameObjects;

        onInit();
    }

    private void onInit()
    {
        initializeVariables();
    }

    public void onClick()
    {
        getValuesFromUserInfo();
    }

    private void initializeVariables()
    {
        popUpGO = publicGameObjects.userInfoPopUp.transform.GetChild(1).gameObject;
        popUpGO.transform.GetChild(0).transform.GetChild(3).GetComponent<TextMeshProUGUI>().text = PlayerPrefs.GetString("PLAYER_TAG");

        username = popUpGO.transform.GetChild(0).transform.GetChild(1).GetComponentInChildren<TextMeshProUGUI>();
        playerLvl = popUpGO.transform.GetChild(1).transform.GetChild(2).transform.GetChild(1).GetComponentInChildren<TextMeshProUGUI>();
        playerExp = popUpGO.transform.GetChild(1).transform.GetChild(2).transform.GetChild(2).GetComponent<TextMeshProUGUI>();

        lvlSlider = popUpGO.transform.GetChild(1).transform.GetChild(2).GetComponent<Slider>();
    }

    private void getValuesFromUserInfo()
    {
        GameObject userInfoGO = publicGameObjects.lobby.transform.GetChild(3).gameObject;
        TextMeshProUGUI username = userInfoGO.transform.GetChild(2).GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI playerLvl = userInfoGO.transform.GetChild(1).GetComponentInChildren<TextMeshProUGUI>();
        TextMeshProUGUI playerExp = userInfoGO.transform.GetChild(0).transform.GetChild(0).GetComponentInChildren<TextMeshProUGUI>();
        Slider lvlSlider = userInfoGO.transform.GetChild(0).GetComponent<Slider>();

        this.username.text = username.text;
        this.playerLvl.text = playerLvl.text;
        this.playerExp.text = playerExp.text;

        this.lvlSlider.minValue = lvlSlider.minValue;
        this.lvlSlider.maxValue = lvlSlider.maxValue;
        this.lvlSlider.value = lvlSlider.value;

    }
}
