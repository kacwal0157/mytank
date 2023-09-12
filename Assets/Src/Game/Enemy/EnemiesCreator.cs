using UnityEngine;
using System.IO;
using Newtonsoft.Json;
using System.Collections.Generic;

public class EnemiesCreator
{
    public Level.Scene enemiesFromJson;

    private Dictionary<int, Vector3> startingPosDict = new Dictionary<int, Vector3>();
    private GameObject enemiesContainer = new GameObject();

    private Vector3 parentScale;
    private int halfOfSizeX;
    private int halfOfSizeY;
    
    public EnemiesCreator(Level.Scene scene)
    {
        enemiesFromJson = scene;
    }

    public List<GameObject> getEnemies(Transform canvas)
    {
        List<GameObject> enemies = new List<GameObject>();

        createContainerForEnemies();

        for (int i = 0; i < enemiesFromJson.enemies.Count; i++)
        {
            Level.Enemy enemyJson = enemiesFromJson.enemies[i];
            GameObject enemy = Utilities.getPrefabFromResources("Enemies/" + enemyJson.enemyName);
            Vector3 position = getEnemyPosition(enemyJson);

            GameObject newEnemy = GameObject.Instantiate(enemy, position, enemy.transform.rotation);
            newEnemy.transform.parent = enemiesContainer.transform;
            newEnemy.name = newEnemy.name.Substring(0, 8);

            loadHelathInfo(canvas, newEnemy, enemyJson.healthPoints);

            enemies.Add(newEnemy);
            startingPosDict.Add(newEnemy.GetInstanceID(), position);
        }

        return enemies;
    }

    private void createContainerForEnemies()
    {
        //halfOfSizeX = enemiesFromJson.arena.x / 2;
        //halfOfSizeY = enemiesFromJson.arena.y / 2;

        enemiesContainer.name = "Enemies";
        enemiesContainer.transform.position = Vector3.zero;

        parentScale = new Vector3(1f, 1f, 1f);
        enemiesContainer.transform.localScale = parentScale;
    }

    public void loadHelathInfo(Transform canvas, GameObject newEnemy, int healthPoints)
    {
        GameObject healthBarPrefab = Utilities.getPrefabFromResources("Health/HealthBar");
        GameObject newhealthBar = GameObject.Instantiate(healthBarPrefab, Vector3.zero, Quaternion.identity);
        //TODO: look carefully after merge
        //newhealthBar.transform.SetParent(canvas); //newhealthBar.transform.parent = newEnemy;

        HealthBar healthBar = newEnemy.AddComponent<HealthBar>();
        EnemyHealth enemyHealth = newEnemy.AddComponent<EnemyHealth>();
        enemyHealth.health = healthPoints;

        healthBar.healthLink.targetScript = enemyHealth;
        healthBar.healthLink.fieldName = "health";
        healthBar.HealthbarPrefab = newhealthBar.GetComponent<RectTransform>();

        EnemiesHealthManager.enemiesHealthDict.Add(newEnemy.GetInstanceID(), healthPoints);
        EnemiesHealthManager.enemyHealthScriptDict.Add(newEnemy.GetInstanceID(), enemyHealth);

    }

    private Vector3 getEnemyPosition(Level.Enemy enemy)
    {
        return new Vector3(enemy.x, 1.5f, enemy.y);
       //return new Vector3(((enemy.x * 2) - 2) - halfOfSizeX, 0.5f, ((enemy.y * 2) - 2) - halfOfSizeY);
    }

    public void fallOverHandler(List<GameObject> enemies, GameObject activePlayer)
    {
        foreach (GameObject go in enemies)
        {
            if (go.Equals(null) || activePlayer.CompareTag("Enemy"))
            {
                return;
            }

            var rot = go.transform.GetChild(0).transform.rotation;
            var angle = go.transform.rotation;

            var pos = go.transform.position;
            var rightPos = startingPosDict[go.GetInstanceID()];

            if (!rot.Equals(angle))
            {
                go.transform.GetChild(0).transform.localPosition = Vector3.zero;
                go.transform.GetChild(0).transform.rotation = angle;
            }

            if (!pos.Equals(rightPos))
            {
                go.transform.position = startingPosDict[go.GetInstanceID()];
                go.transform.GetChild(0).transform.localPosition = Vector3.zero;
                go.transform.GetChild(0).transform.rotation = angle;
            }
        }
    }
}