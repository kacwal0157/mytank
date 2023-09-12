using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuTankActivator
{
    private static bool activateTank = true;

    public static void setTankActivation(bool active, PublicGameObjects publicGameObjects)
    {
        activateTank = checkWindowActivationPossibility(publicGameObjects);

        if(activateTank)
        {
            activateTankInLobby(PlayerPrefs.GetString("TANK_PREFAB"), publicGameObjects, active);
        }
        else
        {
            activateTankInLobby(PlayerPrefs.GetString("TANK_PREFAB"), publicGameObjects, false);
        }
    }

    //check if other windows expect panel are open
    private static bool checkWindowActivationPossibility(PublicGameObjects publicGameObjects)
    {
        foreach(Transform window in publicGameObjects.panel.transform)
        {
            if(window.gameObject.activeSelf && !window.name.Equals("Lobby"))
            {
                return false;
            }
        }

        return true;
    }

    private static void activateTankInLobby(string tankName, PublicGameObjects publicGameObjects, bool active)
    {
        foreach (GameObject tank in publicGameObjects.tanksInMenu)
        {
            if (tank.name.Equals(tankName))
            {
                tank.SetActive(active);
                break;
            }
        }
    }
}
