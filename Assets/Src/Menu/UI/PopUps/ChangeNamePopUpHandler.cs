using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ChangeNamePopUpHandler
{
    private PublicGameObjects publicGameObjects;
    private PlayerMetadataManager playerMetadataManager;

    private GameObject popUp;
    private GameObject namePopUp;
    private GameObject changeNamePopUp;
    private GameObject lobby;
    private GameObject userInfoPopUp;
    private GameObject userInfo;

    private static int changeUsernameCost = 10;
    static string shortNameAlert_1 = "Your name is too short!";
    static string shortNameAlert_2 = "Minimum number of characters are 4, and maximum are 12.";
    static string gemsAlert_1 = "You don't have enough gems!";
    static string gemsAlert_2 = "You need at least 10 gems to change your username.";
    private static string playerTag;

    private TMP_InputField inputField;
    private Dictionary<string, string> usernames;

    public ChangeNamePopUpHandler(PublicGameObjects publicGameObjects, PlayerMetadataManager playerMetadataManager)
    {
        this.publicGameObjects = publicGameObjects;
        this.playerMetadataManager = playerMetadataManager;
        usernames = UsernamesDict.usernames;
        playerTag = PlayerPrefs.GetString("PLAYER_TAG");

        onInit();
    }

    private void onInit()
    {
        popUp = publicGameObjects.changeNamePopUp.transform.GetChild(1).gameObject;
        namePopUp = publicGameObjects.namePopUp;
        changeNamePopUp = publicGameObjects.changeNamePopUp;
        lobby = publicGameObjects.lobby;
        userInfoPopUp = publicGameObjects.userInfoPopUp;
        userInfo = publicGameObjects.userInfo;
        inputField = popUp.transform.GetChild(4).GetComponent<TMP_InputField>();
    }

    public void onClick()
    {
        string givenUsername = inputField.text;

        if(givenUsername.Length < 4)
        {
            namePopUp.transform.GetChild(1).GetComponentInChildren<TextMeshProUGUI>().text = shortNameAlert_1;
            namePopUp.transform.GetChild(2).transform.GetChild(5).GetComponent<TextMeshProUGUI>().text = shortNameAlert_2;

            namePopUp.transform.GetChild(2).transform.GetChild(4).GetComponent<TMP_InputField>().text = givenUsername;
            inputField.text = string.Empty;

            namePopUp.SetActive(true);
            userInfoPopUp.SetActive(true);
            changeNamePopUp.SetActive(false);
            lobby.SetActive(true);

            return;
        }

        foreach (KeyValuePair<string, string> entry in usernames)
        {
            if(entry.Value == givenUsername)
            {
                namePopUp.SetActive(true);
                changeNamePopUp.SetActive(false);
                lobby.SetActive(true);

                namePopUp.transform.GetChild(2).transform.GetChild(4).GetComponent<TMP_InputField>().text = givenUsername;
                inputField.text = string.Empty;
                return;
            }
        }

        changeNickname(givenUsername);
    }

    private void changeNickname(string username)
    {
        int gemsAmount = int.Parse(publicGameObjects.statusBarLobby[2].transform.GetChild(1).GetComponent<TextMeshProUGUI>().text);

        if (checkCurrency(gemsAmount))
        {
            gemsAmount -= changeUsernameCost;
            publicGameObjects.statusBarLobby[2].transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = gemsAmount.ToString();
            playerMetadataManager.gemAmount = gemsAmount;

            userInfoPopUp.transform.GetChild(1).transform.GetChild(0).transform.GetChild(1).GetComponentInChildren<TextMeshProUGUI>().text = username;
            userInfo.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = username;
            playerMetadataManager.username = username;

            usernames[playerTag] = username;
            UsernamesDict.usernames = usernames;

            lobby.GetComponentInParent<ScrollRect>().enabled = true;
            changeNamePopUp.SetActive(false);
            lobby.SetActive(true);
        }
        else
        {
            namePopUp.transform.GetChild(1).GetComponentInChildren<TextMeshProUGUI>().text = gemsAlert_1;
            namePopUp.transform.GetChild(2).transform.GetChild(5).GetComponent<TextMeshProUGUI>().text = gemsAlert_2;

            namePopUp.SetActive(true);
            userInfoPopUp.SetActive(true);
            changeNamePopUp.SetActive(false);
            lobby.SetActive(true);
        }

        inputField.text = string.Empty;
    }

    private bool checkCurrency(int gemsAmount)
    {
        bool changeAvaiable;
        changeAvaiable = gemsAmount >= changeUsernameCost ? true : false;

        return changeAvaiable;
    }
}
