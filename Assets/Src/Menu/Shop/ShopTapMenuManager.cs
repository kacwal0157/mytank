using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShopTapMenuManager 
{
    private PublicGameObjects publicGameObjects;

    private List<GameObject> shopContent;
    private GameObject previousCategory;
    private GameObject focusGO;

    private Color32 focusColor = new Color32(255, 255, 255, 255);
    private Color32 defaultColor = new Color32(74, 172, 247, 255);

    private bool firstClick = true;

    public ShopTapMenuManager(PublicGameObjects publicGameObjects)
    {
        this.publicGameObjects = publicGameObjects;

        onInit();
    }

    private void onInit()
    {
        shopContent = publicGameObjects.shopOffert;
        focusGO = publicGameObjects.shopTapMenuCategories[0].transform.GetChild(0).gameObject; //defalut child of daily

        var tapDaily = publicGameObjects.shopTapMenuCategories[0];
        var tapChests = publicGameObjects.shopTapMenuCategories[1];
        var tapGoldPacks = publicGameObjects.shopTapMenuCategories[2];
        var tapGemsPack = publicGameObjects.shopTapMenuCategories[3];

        tapDaily.GetComponent<Button>().onClick.AddListener(delegate { setTapButtonProperties(tapDaily); });
        tapChests.GetComponent<Button>().onClick.AddListener(delegate { setTapButtonProperties(tapChests); });
        tapGoldPacks.GetComponent<Button>().onClick.AddListener(delegate { setTapButtonProperties(tapGoldPacks); });
        tapGemsPack.GetComponent<Button>().onClick.AddListener(delegate { setTapButtonProperties(tapGemsPack); });
    }

    public void setTapButtonProperties(GameObject tapCategory)
    {
        focusGO.SetActive(true);
        focusGO.transform.SetParent(tapCategory.transform);
        focusGO.transform.localPosition = new Vector3(0f, -51f, 0f);

        if(!firstClick && previousCategory == tapCategory)
        {
            foreach(GameObject go in shopContent)
            {
                go.SetActive(true);
                focusGO.SetActive(false);
                tapCategory.GetComponent<TextMeshProUGUI>().color = defaultColor;
                previousCategory.GetComponent<TextMeshProUGUI>().color = defaultColor;
            }

            firstClick = true;
            return;
        }

        firstClick = false;
        switch (tapCategory.name)
        {
            case "Daily":
                foreach(GameObject go in shopContent)
                {
                    if(go.name.Contains("Special") || go.name.Contains("Daily"))
                    {
                        go.SetActive(true);
                    }
                    else
                    {
                        go.SetActive(false);
                    }
                }
                break;
            case "Chests":
                foreach (GameObject go in shopContent)
                {
                    if (go.name.Contains("Chests"))
                    {
                        go.SetActive(true);
                    }
                    else
                    {
                        go.SetActive(false);
                    }
                }
                break;
            case "Gold Packs":
                foreach (GameObject go in shopContent)
                {
                    if (go.name.Contains("Gold"))
                    {
                        go.SetActive(true);
                    }
                    else
                    {
                        go.SetActive(false);
                    }
                }
                break;
            case "Gem Packs":
                foreach (GameObject go in shopContent)
                {
                    if (go.name.Contains("Gem"))
                    {
                        go.SetActive(true);
                    }
                    else
                    {
                        go.SetActive(false);
                    }
                }
                break;
        }

        tapCategory.GetComponent<TextMeshProUGUI>().color = focusColor;

        if (previousCategory == null || previousCategory == tapCategory)
        {
            previousCategory = tapCategory;
            return;
        }

        previousCategory.GetComponent<TextMeshProUGUI>().color = defaultColor;
        previousCategory = tapCategory;
    }
}
