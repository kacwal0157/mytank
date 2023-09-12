using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameRightPanelManager
{
    private Animator anim;
    private const string playerPrefabFromPrefs = "TANK_PREFAB";
    private List<Level.Enemy> enemies;
    private GameObject playerContent;
    private GameObject bulletContent;

    private GameObject listContentPrefab;
    private FocusOnTargetManager focusOnTargetManager;
    private PlayerMetadataManager playerMetadataManager;

    public GameRightPanelManager(PublicGameObjects publicGameObjects, Level.Scene scene, FocusOnTargetManager focusOnTargetManager, PlayerMetadataManager playerMetadataManager)
    {
        anim = publicGameObjects.listOpenerAnimator;
        enemies = scene.enemies;
        playerContent = publicGameObjects.listOfPlayersContent;
        bulletContent = publicGameObjects.listOfBulletsContent;
        listContentPrefab = publicGameObjects.listContentPrefab;
        this.focusOnTargetManager = focusOnTargetManager;
        this.playerMetadataManager = playerMetadataManager;

        onInit();
    }

    public void onClick()
    {
        if(anim != null)
        {
            bool isOpen = anim.GetBool("open");

            anim.SetBool("open", !isOpen);
        }
    }

    private void onInit()
    {
        Dictionary<string, int> playerNameToHealth = getPlayers();
        createPlayerContent(playerNameToHealth, playerContent, listContentPrefab);
        createBulletContent(bulletContent);
    }

    private Dictionary<string, int> getPlayers()
    {
        Dictionary<string, int> playerNameToHealth = new Dictionary<string, int>();
        var player = PlayerPrefs.GetString(playerPrefabFromPrefs) == string.Empty ? "Panzer2" : PlayerPrefs.GetString(playerPrefabFromPrefs);
        //TODO: change to the value loaded from prefs/storage/db
        playerNameToHealth.Add(player, 500);

        foreach(Level.Enemy enemy in enemies)
        {
            playerNameToHealth.Add(enemy.enemyName, enemy.healthPoints);
        }

        return playerNameToHealth;
    }

    private void createPlayerContent(Dictionary<string, int> playerNameToHealth, GameObject content, GameObject prefab)
    {
        foreach(string name in playerNameToHealth.Keys)
        {
            var go = GameObject.Instantiate(prefab, content.transform);
            go.name = name;
            go.transform.GetChild(3).transform.GetChild(0).GetComponent<Slider>().minValue = 0f;

            go.transform.GetChild(3).transform.GetChild(0).GetComponent<Slider>().maxValue = playerNameToHealth[name];
            go.transform.GetChild(3).transform.GetChild(0).GetComponent<Slider>().value = playerNameToHealth[name];

            var path = name.Contains("Panzer") ? "Textures/Icons/Tanks/" : "Textures/Icons/Enemies/";

            go.transform.GetChild(0).transform.GetChild(1).GetComponent<Image>().sprite = Resources.Load<Sprite>(path + name);
            go.GetComponent<Button>().onClick.AddListener(delegate { focusOnTargetManager.getTarget(name); });

        }
    }

    private void createBulletContent(GameObject content)
    {
        var bullets = Utilities.convertStringToListOfBullets(playerMetadataManager.getInventoryBullets());

        foreach (Bullet bullet in bullets)
        {
            
            if (!bullet.name.Equals("EMPTY"))
            {
                var itemToRender = instantiateItemToRender("Menu/Bullets/", bullet.name, content.transform);
                var textMeshProTransform = itemToRender.transform.Find("Item_Count");
                textMeshProTransform.gameObject.SetActive(true);
                textMeshProTransform.GetComponent<TextMeshProUGUI>().text = bullet.count.ToString();

            }
        }
    }

    private GameObject instantiateItemToRender(string prefabPrefix, string name, Transform parent)
    {
        // get item from prefabs and set good position in worldSpace
        var itemToRender = Utilities.getPrefabFromResources(prefabPrefix + name);
        itemToRender = GameObject.Instantiate(itemToRender, Vector3.zero, Quaternion.identity, parent);
        itemToRender.transform.localPosition = Vector3.zero;
        itemToRender.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);

        return itemToRender;
    }
}
