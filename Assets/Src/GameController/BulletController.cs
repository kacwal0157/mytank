using System;
using System.Collections.Generic;
using UnityEngine;

public class BulletController 
{
    Dictionary<int, PlayerBullets> playerToPlayerBullets = new Dictionary<int, PlayerBullets>();
    Dictionary<string, GameObject> nameToBullet = new Dictionary<string, GameObject>();

    public BulletController()
    {
        foreach(PlayerBullets.BULLET_TYPE bulletType in Enum.GetValues(typeof(PlayerBullets.BULLET_TYPE)))
        {
            foreach (PlayerBullets.BULLET_SUBTYPE bulletSubType in Enum.GetValues(typeof(PlayerBullets.BULLET_SUBTYPE)))
            {
                try
                {
                    nameToBullet.Add(bulletType + "_" + bulletSubType, Utilities.getPrefabFromResources("Bullets/" + bulletType + "/" + bulletSubType));
                }
                catch(Exception e)
                {
                    Debug.LogWarning("Cannot retrieve bullet for: " + bulletType + "_" + bulletSubType);
                }
                
            }
                
        }
    }

    public PlayerBullets initializePlayerBullets(GameObject player)
    {
        //TODO:
        //Load enemies based on JSON config
        //Load player based on PlayerPrefs/data storage/JSON config of tank

        PlayerBullets playerBullets = new PlayerBullets();
        playerBullets.availableBulletTypes = new List<PlayerBullets.BULLET_TYPE>(){PlayerBullets.BULLET_TYPE.UZI, PlayerBullets.BULLET_TYPE.CANON, PlayerBullets.BULLET_TYPE.MINI_ROCKET};
        playerBullets.availableShootingModes = new List<TargetInterface.SHOOTING_MODE>() { TargetInterface.SHOOTING_MODE.CANON, TargetInterface.SHOOTING_MODE.MINI_ROCKET, TargetInterface.SHOOTING_MODE.UZI };
        playerBullets.selectedBulletType = PlayerBullets.BULLET_TYPE.MINI_ROCKET;
        playerBullets.selectedBulletSubType = PlayerBullets.BULLET_SUBTYPE.NORMAL;
        playerBullets.selectedShootingMode = TargetInterface.SHOOTING_MODE.UZI;

        playerToPlayerBullets.Add(player.GetInstanceID(), playerBullets);

        return playerBullets;
    }

    public GameObject getBullet(GameObject player)
    {
        return nameToBullet[playerToPlayerBullets[player.GetInstanceID()].selectedBulletType + "_" + playerToPlayerBullets[player.GetInstanceID()].selectedBulletSubType];
    }
}
