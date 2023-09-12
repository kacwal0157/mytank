using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMetadataLimitRefresherService
{
    private const int TIME_TO_REFRESH_SPIN_IN_MILISECONDS = 10000;

    private PlayerMetadataManager playerMetadataManager;

    public PlayerMetadataLimitRefresherService(PlayerMetadataManager playerMetadataManager)
    {
        this.playerMetadataManager = playerMetadataManager;
        MenuManager.getInstance.StartCoroutine(refreshUpdateTimes());
    }

    IEnumerator refreshUpdateTimes()
    {
        for (; ; )
        {
            checkMoneySpinAvailability();
            yield return new WaitForSeconds(2f);
        }
    }

    private void checkMoneySpinAvailability()
    {
        List<System.DateTime> spinTimesList = Utilities.convertStringToList(playerMetadataManager.spinTimes);
        List<System.DateTime> toRemove = new List<System.DateTime>();

        foreach (System.DateTime date in spinTimesList)
        {
            long miliseconds = (long)System.DateTime.UtcNow.ToUniversalTime().Subtract(date).TotalMilliseconds;
            if(miliseconds > TIME_TO_REFRESH_SPIN_IN_MILISECONDS)
            {
                toRemove.Add(date);
            }
        }

        spinTimesList.RemoveAll(it => toRemove.Contains(it));

        playerMetadataManager.spinTimes = Utilities.convertListToString(spinTimesList);
        playerMetadataManager.dailySpinGold = RouletteManager.maxDailySpins - spinTimesList.Count;

    }
}
