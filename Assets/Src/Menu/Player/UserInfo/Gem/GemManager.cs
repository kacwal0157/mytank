using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GemManager
{
    public static string getGemToDisplay(int gemAmount)
    {
        string gemToDisplay;

        if (gemAmount >= 1000 && gemAmount < 10000)
        {
            string firstPart = gemAmount.ToString().Substring(0, 1);
            string secondPart = gemAmount.ToString().Substring(1, 3);
            gemToDisplay = firstPart + "," + secondPart;
        }
        else if (gemAmount >= 10000 && gemAmount < 100000)
        {
            string firstPart = gemAmount.ToString().Substring(0, 2);
            string secondPart = gemAmount.ToString().Substring(2, 3);
            gemToDisplay = firstPart + "," + secondPart;
        }
        else if (gemAmount >= 100000)
        {
            string firstPart = gemAmount.ToString().Substring(0, 3);
            string secondPart = gemAmount.ToString().Substring(3, 3);
            gemToDisplay = firstPart + "," + secondPart;
        }
        else
        {
            gemToDisplay = gemAmount.ToString();
        }

        return gemToDisplay;
    }

    public static int getGemFromString(string gemAmount)
    {
        int gems;
        string[] gemsArray = gemAmount.Split(char.Parse(","));

        if (gemsArray.Length == 3)
        {
            gems = int.Parse(gemsArray[0] + gemsArray[1] + gemsArray[2]);
        }
        else if (gemsArray.Length == 2)
        {
            gems = int.Parse(gemsArray[0] + gemsArray[1]);
        }
        else
        {
            gems = int.Parse(gemsArray[0]);
        }

        return gems;
    }
}
