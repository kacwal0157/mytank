using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ColorDict
{
    public static Color32 getColor(string type)
    {
        Color32 color = new Color32(255, 255, 255, 255);

        switch(type)
        {
            case "Legendary":
                color = new Color32(241, 155, 2, 255);
                break;
            case "Epic":
                color = new Color32(205, 70, 235, 255);
                break;
            case "Rare":
                color = new Color32(252, 23, 2, 255);
                break;
            case "Unusual":
                color = new Color32(11, 145, 253, 255);
                break;
            case "Common":
                color = new Color32(87, 225, 107, 255);
                break;
            case "Normal":
                color = new Color32(153, 134, 109, 255);
                break;
        }

        return color;
    }
}
