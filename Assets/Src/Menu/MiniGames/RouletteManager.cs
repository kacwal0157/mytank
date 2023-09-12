using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Linq;
using TMPro;
using Newtonsoft.Json;

public class RouletteManager
{
    public readonly static int maxDailySpins = 2;

    private PlayerMetadataManager playerMetadataManager;
    private PublicGameObjects publicGameObjects;

    private List<GameObject> prize;
    private List<AnimationCurve> animationCurves;
    private GameObject spinGameObject;
    private Button spinButton;
    private Button dailySpinButton;
    private TextMeshProUGUI dailySpinAmountText;
    private TextMeshProUGUI goldAmountText;

    private bool spinning;
    private float anglePerItem;
    private int randomTime;
    private int itemNumber;
    private int goldAmount;
    private int procentageFactor;

    public RouletteManager(PublicGameObjects publicGameObjects, PlayerMetadataManager playerMetadataManager)
    {
        this.publicGameObjects = publicGameObjects;
        this.playerMetadataManager = playerMetadataManager;

        prize = publicGameObjects.slotGold;
        animationCurves = publicGameObjects.animationCurves;
        goldAmountText = publicGameObjects.goldAmountText;

        List<Transform> rouletteChilds = publicGameObjects.rouletteGameObject.GetComponentsInChildren<Transform>().ToList();
        foreach(Transform t in rouletteChilds)
        {
            switch(t.tag)
            {
                case "dailySpinButton":
                    dailySpinButton = t.GetComponent<Button>();
                    break;
                case "buttonSpin":
                    spinButton = t.GetComponent<Button>();
                    break;
                case "spinGameObject":
                    spinGameObject = t.gameObject;
                    break;
                case "dailySpinAmountText":
                    dailySpinAmountText = t.GetComponent<TextMeshProUGUI>();
                    break;
            }
        }

        onInit();
        
    }

    void onInit()
    {
        spinning = false;
        anglePerItem = 360 / prize.Count;

        dailySpinAmountText.text = playerMetadataManager.dailySpinGold.ToString() + "/" + maxDailySpins.ToString();

        spinButton.onClick.AddListener(spin);
        dailySpinButton.onClick.AddListener(popupAdd);
    }

    private void spin()
    {
        if(!spinning && playerMetadataManager.dailySpinGold > 0)
        {
            randomTime = Random.Range(3, 5);
            itemNumber = drawAnItem();
            float maxAngle = 360 * randomTime + (itemNumber * anglePerItem);

            playerMetadataManager.dailySpinGold--;
            dailySpinAmountText.text = playerMetadataManager.dailySpinGold.ToString() + "/" + maxDailySpins.ToString();

            MenuManager.getInstance.StartCoroutine(spinTheRoulette(randomTime, maxAngle));
        }
    }

    IEnumerator spinTheRoulette(float time, float maxAngle)
    {
        spinning = true;

        float timer = 0.0f;
        float startAngle = spinGameObject.transform.eulerAngles.z;
        maxAngle = maxAngle - startAngle;

        int animationCurveNumber = Random.Range(0, animationCurves.Count);

        while (timer < time)
        {
            //to calculate rotation
            float angle = maxAngle * animationCurves[animationCurveNumber].Evaluate(timer / time);
            spinGameObject.transform.eulerAngles = new Vector3(0.0f, 0.0f, angle + startAngle);
            timer += Time.deltaTime;
            yield return 0;
        }

        //adding gold
        string goldToAdd = prize[itemNumber].ToString().Substring(6, 4);
        goldAmount = playerMetadataManager.goldAmount;
        goldAmount += int.Parse(goldToAdd);
        goldAmountText.text = GoldManager.getGoldToDisplay(goldAmount);
        playerMetadataManager.goldAmount = goldAmount;
        StatusBarManager.updateGoldOnStatusBars(publicGameObjects, playerMetadataManager);

        spinGameObject.transform.eulerAngles = new Vector3(0.0f, 0.0f, maxAngle + startAngle);
        spinning = false;
        setLastSpinTimes();
    }

    private void popupAdd()
    {
        Debug.Log("ADD HERE TO ADD MORE CHANCES TO SPIN THE ROULETTE"); // < - - as it says
    }

    private int drawAnItem()
    {
        procentageFactor = Random.Range(1, 100);

        if (procentageFactor >= 1 && procentageFactor < 70)
        {
            return Random.Range(0, 3);
        }
        else if (procentageFactor >= 70 && procentageFactor < 85)
        {
            return Random.Range(4, 5);
        }
        else if (procentageFactor >= 85 && procentageFactor < 95)
        {
            return 6;
        }
        else
        {
            return 7;
        }
    }

    private void setLastSpinTimes()
    {
        System.DateTime timeOfSpin = System.DateTime.UtcNow.ToUniversalTime(); //.Subtract(new System.DateTime(1970, 1, 1, 0, 0, 0, System.DateTimeKind.Utc)).TotalMilliseconds;
        List<System.DateTime> spinTimesList = Utilities.convertStringToList(playerMetadataManager.spinTimes);

        if(spinTimesList.Count > maxDailySpins)
        {
            spinTimesList.Sort();
            spinTimesList[0] = timeOfSpin;
        } else
        {
            spinTimesList.Add(timeOfSpin);
        }

        playerMetadataManager.spinTimes = Utilities.convertListToString(spinTimesList);

    }

}
