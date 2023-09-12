using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopManager 
{
    private PublicGameObjects publicGameObjects;
    private ShopPopUp shopPopUp;

    private TextMeshProUGUI goldAmount;
    private TextMeshProUGUI gemAmount;
    private List<Transform> offertGold;

    public ShopManager(PublicGameObjects publicGameObjects, ShopPopUp shopPopUp)
    {
        this.publicGameObjects = publicGameObjects;
        this.shopPopUp = shopPopUp;
        goldAmount = publicGameObjects.goldAmountShop;
        gemAmount = publicGameObjects.gemAmountShop;

        onInit();
    }
    
    private void onInit()
    {
        offertGold = initializeGoldOffert();

        for (int i = 0; i < offertGold.Count; i++)
        {
            setButtonForGoldOffers(offertGold[i]);
        }
    }

    private List<Transform> initializeGoldOffert()
    {
        List<Transform> transforms = new List<Transform>();
        foreach (Transform t in publicGameObjects.shopOffert[3].transform.GetChild(1).transform)
        {
            transforms.Add(t);
        }

        return transforms;
    }

    private void setButtonForGoldOffers(Transform offert)
    {
        if(offert.GetComponent<Button>() != null)
        {
            offert.GetComponent<Button>().onClick.AddListener(delegate { shopPopUp.onClick(offert, goldAmount, gemAmount); });
            return;
        }

        foreach(Transform t in offert)
        {
            t.GetComponent<Button>().onClick.AddListener(delegate { shopPopUp.onClick(t, goldAmount, gemAmount); });
        }
    }
}
