using UnityEngine;
using System.Collections.Generic;

public class SpawnPlayer
{
    private const string playerPrefsName = "TANK_PREFAB";

    private EnemiesCreator enemiesCreator;
    private Level.Scene scene;

    private int halfOfSizeX;
    private int halfOfSizeY;

    private GameObject playerContainer = new GameObject();
    private List<int> playerTankStats;
    private Vector3 playerStartPos;

    public SpawnPlayer(Level.Scene scene, EnemiesCreator enemiesCreator)
    {
        this.enemiesCreator = enemiesCreator;
        this.scene = scene;
    }

    public GameObject getPlayer(Transform canvas)
    {
        createContainerForPlayer();

        GameObject player = PlayerPrefs.GetString(playerPrefsName) != "" ? Utilities.getPrefabFromResources("Tanks/" + PlayerPrefs.GetString(playerPrefsName)) : Utilities.getPrefabFromResources("Tanks/Panzer2");
        Vector3 pos = setPlayerPosition(scene.player);

        GameObject newPlayer = GameObject.Instantiate(player, pos, player.transform.rotation);
        newPlayer.transform.parent = playerContainer.transform;
        newPlayer.name = PlayerPrefs.GetString(playerPrefsName) != "" ? PlayerPrefs.GetString(playerPrefsName) : "Panzer2";
        playerStartPos = pos;

        playerTankStats = getPlayerTankStats(newPlayer.name);
        enemiesCreator.loadHelathInfo(canvas, newPlayer, playerTankStats[0]);

        return newPlayer;
    }

    private void createContainerForPlayer()
    {
        //halfOfSizeX = scene.arena.x/ 2;
        //halfOfSizeY = scene.arena.y / 2;

        playerContainer.name = "Player";
        playerContainer.transform.position = Vector3.zero;

        playerContainer.transform.localScale = new Vector3(1f, 1f, 1f);
    }

    private Vector3 setPlayerPosition(Level.Player playerFromJson)
    {
        return new Vector3(playerFromJson.x, 1.5f, playerFromJson.y);
        //return new Vector3((playerFromJson.x * 2 - 2) - halfOfSizeX, 0.5f, (playerFromJson.y * 2 - 2) - halfOfSizeY);
    }

    private List<int> getPlayerTankStats(string tankName)
    {
        List<int> stats = new List<int>();
        var name = string.Empty;

        switch (tankName)
        {
            case "Panzer1":
                name = "PANZER1_STATS";
                break;
            case "Panzer2":
                name = "PANZER2_STATS";
                break;
            case "Panzer3":
                name = "PANZER3_STATS";
                break;
            case "Panzer4":
                name = "PANZER4_STATS";
                break;
            case "Panzer5":
                name = "PANZER5_STATS";
                break;
        }

        if(PlayerPrefs.GetString(name).Equals(""))
        {
            //Case when we did not visit menu (only for testing new features in game)
            stats.Add(560);
            stats.Add(15);
            stats.Add(10);
            stats.Add(1);
            stats.Add(1);
            stats.Add(1);
        }
        else
        {
            stats = Utilities.convertTankStringToList(PlayerPrefs.GetString(name));
        }

        return stats;
    }

    //TODO: REFACTOR CAUSE' STILL IT DOESN'T WORK PERFECT
    public void fallOverHandler(GameObject player, GameObject activePlayer)
    {
        if (player.Equals(null) || activePlayer.CompareTag("Player"))
        {
            return;
        }

        var rot = player.transform.GetChild(0).transform.rotation;
        var angle = player.transform.rotation;
        var pos = player.transform.position;

        if (!player.transform.position.Equals(playerStartPos))
        {
            player.transform.position = playerStartPos;
        }

        if (!rot.Equals(angle))
        {
            player.transform.GetChild(0).transform.localPosition = Vector3.zero;
            player.transform.GetChild(0).transform.rotation = angle;
        }

        if (!pos.Equals(playerStartPos))
        {
            player.transform.position = playerStartPos;
            player.transform.GetChild(0).transform.localPosition = Vector3.zero;
            player.transform.GetChild(0).transform.rotation = angle;
        }
    }
}
