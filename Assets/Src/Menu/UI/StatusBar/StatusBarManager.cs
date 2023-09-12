using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class StatusBarManager
{
    public StatusBarManager(PlayerMetadataManager playerMetadataManager, PublicGameObjects publicGameObjects)
    {
        handleStatusBars(playerMetadataManager, publicGameObjects);
    }

    private void handleStatusBars(PlayerMetadataManager playerMetadataManager, PublicGameObjects publicGameObjects)
    {
        foreach(Transform status in publicGameObjects.statusBars)
        {
            var energy = initializeStatusBarEnergy(status);
            var gold = initializeStatusBarGold(status);
            var gem = initializeStatusBarGem(status);

            energy.text = playerMetadataManager.energyAmount.ToString() + " / " + playerMetadataManager.maxEnergyAmount.ToString();
            gold.text = GoldManager.getGoldToDisplay(playerMetadataManager.goldAmount);
            gem.text = GemManager.getGemToDisplay(playerMetadataManager.gemAmount);
        }

    }

    public static void updateGemsOnStatusBars(PublicGameObjects publicGameObjects, PlayerMetadataManager playerMetadataManager)
    {
        foreach(Transform status in publicGameObjects.statusBars)
        {
            var gem = initializeStatusBarGem(status);
            gem.text = GemManager.getGemToDisplay(playerMetadataManager.gemAmount);
        }
    }

    public static void updateGoldOnStatusBars(PublicGameObjects publicGameObjects, PlayerMetadataManager playerMetadataManager)
    {
        foreach (Transform status in publicGameObjects.statusBars)
        {
            var gold = initializeStatusBarGold(status);
            gold.text = GoldManager.getGoldToDisplay(playerMetadataManager.goldAmount);
        }
    }

    public static void updateMaxEnergyOnStatusBars(PublicGameObjects publicGameObjects, PlayerMetadataManager playerMetadataManager)
    {
        foreach (Transform status in publicGameObjects.statusBars)
        {
            var energy = initializeStatusBarEnergy(status);
            energy.text = playerMetadataManager.energyAmount.ToString() + " / " + playerMetadataManager.maxEnergyAmount.ToString();
        }
    }

    private static TextMeshProUGUI initializeStatusBarEnergy(Transform statusBar)
    {
        TextMeshProUGUI energyText;

        foreach (Transform t in statusBar)
        {
            if (t.tag.Equals("energyAmountText"))
            {
                return energyText = t.GetComponentInChildren<TextMeshProUGUI>();
            }
        }

        return new TextMeshProUGUI();
    }

    private static TextMeshProUGUI initializeStatusBarGold(Transform statusBar)
    {
        TextMeshProUGUI goldText;

        foreach (Transform t in statusBar)
        {
            if (t.tag.Equals("goldAmountText"))
            {
                return goldText = t.GetComponentInChildren<TextMeshProUGUI>();
            }
        }

        return new TextMeshProUGUI();
    }

    private static TextMeshProUGUI initializeStatusBarGem(Transform statusBar)
    {
        TextMeshProUGUI gemText;

        foreach (Transform t in statusBar)
        {
            if (t.tag.Equals("gemAmountText"))
            {
                return gemText = t.GetComponentInChildren<TextMeshProUGUI>();
            }
        }

        return new TextMeshProUGUI();
    }
}
