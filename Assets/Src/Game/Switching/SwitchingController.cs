using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchingController
{
    private CameraManager cameraManager;
    private PlayerController playerController;

    private int enemyIndex = 0;

    public SwitchingController(CameraManager cameraManager, PlayerController playerController)
    {
        this.cameraManager = cameraManager;
        this.playerController = playerController;
    }


    public void switchActivePlayer(GameObject activePlayer, List<GameObject> enemies, GameObject playerPrefab)
    {
        GameManager.activePlayer = changeActivePlayer(activePlayer, enemies, playerPrefab);

        playerController.changeActiveUser(GameManager.activePlayer);
        cameraManager.setCameraAimingAndShootingPositionAndRotation(GameManager.activePlayer);
    }
    
    private GameObject changeActivePlayer(GameObject activePlayer, List<GameObject> enemies, GameObject playerPrefab)
    {
        switch (activePlayer.tag)
        {
            case "Player":
                checkStatusOfEnemies(enemies);
                checkIfEnemiesAreAlive(enemies, playerPrefab);

                activePlayer = enemies[enemyIndex];
                enemyIndex++;
                if(enemyIndex == enemies.Count)
                {
                    enemyIndex = 0;
                }

                GameManager.getInstance.changeState(GameManager.Stage.OPONNENT_TURN_AIMING);
                break;

            case "Enemy":
                checkIfPlayerIsAlive(playerPrefab, enemies);

                activePlayer = playerPrefab;
                GameManager.getInstance.changeState(GameManager.Stage.PLAYER_TURN_AIMING);
                break;
        }

        return activePlayer;
    }

    private void checkStatusOfEnemies(List<GameObject> enemies)
    {
        foreach(GameObject go in enemies)
        {
            if(go.Equals(null))
            {
                enemies.Remove(go);
            }
        }

        for (int i = 0; i < enemies.Count; i++)
        { 
            if (!enemies[i].activeSelf)
            {
                playerController.playermetadataDict.Remove(enemies[i].GetInstanceID());
                cameraManager.gameObjectsDictionary.Remove(enemies[i].GetInstanceID());

                enemies.Remove(enemies[i]);
                GameManager.enemies = enemies;

                enemyIndex = 0;
                GameManager.getInstance.changeState(GameManager.Stage.OPONNENT_TURN_AIMING);
                break;
            }
        }
    }

    private void checkIfEnemiesAreAlive(List<GameObject> enemies, GameObject playerPref)
    {
        if (enemies.Count == 0)
        {
            playerPref.GetComponent<HealthBar>().HealthbarPrefab.gameObject.SetActive(false);
            GameManager.getInstance.changeState(GameManager.Stage.WIN_GAME);
            return;
        }
    }

    private void checkIfPlayerIsAlive(GameObject playerPref, List<GameObject> enemies)
    {
        if (!playerPref.activeSelf)
        {
            foreach(GameObject enemy in enemies)
            {
                enemy.GetComponent<HealthBar>().HealthbarPrefab.gameObject.SetActive(false);
            }

            playerController.playermetadataDict.Remove(playerPref.GetInstanceID());
            cameraManager.gameObjectsDictionary.Remove(playerPref.GetInstanceID());

            GameManager.getInstance.changeState(GameManager.Stage.LOSE_GAME);
            return;
        }
    }
}
