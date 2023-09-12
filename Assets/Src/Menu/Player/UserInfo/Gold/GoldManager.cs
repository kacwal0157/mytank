using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GoldManager
{
    public static string getGoldToDisplay(int goldAmount)
    {
        string goldToDisplay;

        if (goldAmount >= 1000 && goldAmount < 10000)
        {
            string firstPart = goldAmount.ToString().Substring(0, 1);
            string secondPart = goldAmount.ToString().Substring(1, 3);
            goldToDisplay = firstPart + "," + secondPart;
        }
        else if (goldAmount >= 10000 && goldAmount < 100000)
        {
            string firstPart = goldAmount.ToString().Substring(0, 2);
            string secondPart = goldAmount.ToString().Substring(2, 3);
            goldToDisplay = firstPart + "," + secondPart;
        }
        else if (goldAmount >= 100000 && goldAmount < 1000000)
        {
            string firstPart = goldAmount.ToString().Substring(0, 3);
            string secondPart = goldAmount.ToString().Substring(3, 3);
            goldToDisplay = firstPart + "," + secondPart;
        }
        else if (goldAmount >= 1000000)
        {
            string firstPart = goldAmount.ToString().Substring(0, 1);
            string secondPart = goldAmount.ToString().Substring(1, 3);
            string thirdPart = goldAmount.ToString().Substring(3, 3);
            goldToDisplay = firstPart + "," + secondPart + "," + thirdPart;
        }
        else
        {
            goldToDisplay = goldAmount.ToString();
        }

        return goldToDisplay;
    }

    public static int getGoldFromString(string goldAmount)
    {
        int gold;
        string[] goldArray = goldAmount.Split(char.Parse(","));

        if (goldArray.Length == 3)
        {
            gold = int.Parse(goldArray[0] + goldArray[1] + goldArray[2]);
        }
        else if (goldArray.Length == 2)
        {
            gold = int.Parse(goldArray[0] + goldArray[1]);
        }
        else
        {
            gold = int.Parse(goldArray[0]);
        }
        
        return gold;
    }
}
