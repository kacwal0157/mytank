using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LastLoginDateManager
{
    public bool enableDailyRewardPopUp = false;
    private PlayerMetadataManager playerMetadataManager;

    public LastLoginDateManager(PlayerMetadataManager playerMetadataManager)
    {
        this.playerMetadataManager = playerMetadataManager;
        onInit();
    }

    private void onInit()
    {
        DateTime currentDateTime = DateTime.Now;
        if (playerMetadataManager.firstPlay)
        {
            playerMetadataManager.logInDayStrike = 1;
            enableDailyRewardPopUp = true;
        }
        else
        {
            handleLastLogInTime(currentDateTime);
            playerMetadataManager.lastLogIn = currentDateTime.ToString();
        }
    }

    private void handleLastLogInTime(DateTime currentDateTime)
    {
        //DateTime lastLogInDateTime = DateTime.ParseExact(playerMetadataManager.lastLogIn, "dd.MM.yyyy HH:mm:ss", null);
        DateTime lastLogInDateTime = DateTime.Parse(playerMetadataManager.lastLogIn);
        DateTime timeStamp = new DateTime(lastLogInDateTime.Year, lastLogInDateTime.Month, lastLogInDateTime.Day, 23, 59, 59);

        double elapsedHours = (currentDateTime - lastLogInDateTime).TotalHours;
        double hoursToMidnight = (timeStamp - currentDateTime).TotalHours;

        if(elapsedHours < 24 && hoursToMidnight >= 0) 
        {
            enableDailyRewardPopUp = false;
        }
        else if(elapsedHours < 24 && hoursToMidnight < 0)
        {
            enableDailyRewardPopUp = true;

            if(playerMetadataManager.claimedDailyRewards != playerMetadataManager.logInDayStrike)
            {
                setDefaultTimePrefsValue();
                Debug.Log("previous item not collected");
            }
            else
            {
                playerMetadataManager.logInDayStrike = playerMetadataManager.logInDayStrike + 1;
            }
        }
        else if(elapsedHours >= 24)
        {
            enableDailyRewardPopUp = true;
            setDefaultTimePrefsValue();
            Debug.Log("time or day strike broken");
        }
    }

    private void setDefaultTimePrefsValue()
    {
        playerMetadataManager.logInDayStrike = 1;
        playerMetadataManager.claimedDailyRewards = 0;
    }
}
