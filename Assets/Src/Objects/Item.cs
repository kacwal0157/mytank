using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item 
{
    public string name { get; set; } //Name
    public string type { get; set; } //Legendary(yellow), Epic(purple), Rare(red), Unusual(Blue), Common(Green), Normal(Gray), Transparent(for empty item), Award(for gems, energy, gold awards)
    public string typeColor { get; set; } //color of itemType for exp. legendary - #F19A02
    public string info { get; set; } //Short description
    public int value { get; set; } //In gold
    public int upgradesCounter { get; set; } //how many times an item has been upgraded
    public string itemPropertyName { get; set; } //for exp. attack bonus
    public int itemPropertyValue { get; set; } //amount of bonus for exp. +50 attack

    public Item() { }
    public Item(string name, string type, string typeColor, string info, int value, int upgradesCounter, string itemPropertyName, int itemPropertyValue)
    {
        this.name = name;
        this.type = type;
        this.typeColor = typeColor;
        this.info = info;
        this.value = value;
        this.upgradesCounter = upgradesCounter;
        this.itemPropertyName = itemPropertyName;
        this.itemPropertyValue = itemPropertyValue;
    }
}
