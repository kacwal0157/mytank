using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ItemDict
{
    public static string BOLT_ITEM = "Bolt";
    public static string BOMB_ITEM = "Bomb";
    public static string BOOK_ITEM = "Book";
    public static string BOXINGGLOVES_ITEM = "Boxing Gloves";
    public static string CHEST_ITEM = "Chest";
    public static string CLOVER_ITEM = "Clover";
    public static string DOGGUM_ITEM = "Dog Gum";
    public static string EMERGENCYBAG_ITEM = "Emergency Bag";
    public static string EMPTY_ITEM = "Empty_Item";
    public static string LIGHTNING_ITEM = "Lightning";
    public static string FLIPPERS_ITEM = "Flippers";
    public static string FOODCAN_ITEM = "Food Can";
    public static string FOODSHELL_ITEM = "Food Shell";
    public static string GPS_ITEM = "Gps";
    public static string HAMMER_ITEM = "Hammer";
    public static string HORSESHOES_ITEM = "Horse Shoes";
    public static string KEY_ITEM = "Key";
    public static string LANDMINE_ITEM = "Land Mine";
    public static string MAGNET_ITEM = "Magnet";
    public static string MISSLE_ITEM = "Missle";
    public static string NUT_ITEM = "Nut";
    public static string OIL_ITEM = "Oil";
    public static string POTION01_ITEM = "Potion01";
    public static string POTION02_ITEM = "Potion02";
    public static string RAINBOWEGG_ITEM = "Rainbow Egg";
    public static string SHIELD_ITEM = "Shield";
    public static string SHOVEL_ITEM = "Shovel";
    public static string STAR_ITEM = "Star";
    public static string SWORD_ITEM = "Sword";
    public static string TOOTH_ITEM = "Tooth";
    //STATUS BAR ITEMS
    public static string ENERGY_ITEM = "Energy";
    public static string GOLD_ITEM = "Gold";
    public static string GEM_ITEM = "Gem";

    //BULLETS ITEMS
    public static string CANON_NORMAL = "canon_normal";
    public static string CANON_ELECTRIC = "canon_electric";
    public static string CANON_GLUE = "canon_glue";
    public static string CANON_ICE = "canon_ice";
    public static string CANON_PLASMA = "canon_plasma";

    public static string UZI_NORMAL = "uzi_normal";
    public static string UZI_ELECTRIC = "uzi_electric";
    public static string UZI_GLUE = "uzi_glue";
    public static string UZI_ICE = "uzi_ice";
    public static string UZI_PLASMA = "uzi_plasma";

    public static string MINI_ROCKET_NORMAL = "mini_rocket_normal";
    public static string MINI_ROCKET_ELECTRIC = "mini_rocket_electric";
    public static string MINI_ROCKET_GLUE = "mini_rocket_glue";
    public static string MINI_ROCKET_ICE = "mini_rocket_ice";
    public static string MINI_ROCKET_PLASMA = "mini_rocket_plasma";

    public static string LEGENDARY_COLOR = ColorUtility.ToHtmlStringRGBA(new Color32(241, 154, 2, 255));
    public static string EPIC_COLOR = ColorUtility.ToHtmlStringRGBA(new Color32(158, 57, 210, 255));
    public static string RARE_COLOR = ColorUtility.ToHtmlStringRGBA(new Color32(233, 15, 1, 255));
    public static string UNUSUAL_COLOR = ColorUtility.ToHtmlStringRGBA(new Color32(15, 104, 244, 255));
    public static string COMMON_COLOR = ColorUtility.ToHtmlStringRGBA(new Color32(82, 170, 35, 255));
    public static string NORMAL_COLOR = ColorUtility.ToHtmlStringRGBA(new Color32(136, 118, 93, 255));


    //TODO: add a property for every item
    public static Item getItem(string itemName)
    {
        var item = new Item();

        switch (itemName)
        {
            case "Bolt":
                item.name = BOLT_ITEM;
                item.type = "Common";
                item.typeColor = COMMON_COLOR;
                item.info = "Description";
                item.value = 150;
                break;
            case "Bomb":
                item.name = BOMB_ITEM;
                item.type = "Unusual";
                item.typeColor = UNUSUAL_COLOR;
                item.info = "Description";
                item.value = 250;
                break;
            case "Book":
                item.name = BOOK_ITEM;
                item.type = "Epic";
                item.typeColor = EPIC_COLOR;
                item.info = "Description";
                item.value = 750;
                break;
            case "Boxing Gloves":
                item.name = BOXINGGLOVES_ITEM;
                item.type = "Legendary";
                item.typeColor = LEGENDARY_COLOR;
                item.info = "Description";
                item.value = 1000;
                break;
            case "Chest":
                item.name = CHEST_ITEM;
                item.type = "Normal";
                item.typeColor = NORMAL_COLOR;
                item.info = "Description";
                item.value = 50;
                break;
            case "Clover":
                item.name = CLOVER_ITEM;
                item.type = "Common";
                item.typeColor = COMMON_COLOR;
                item.info = "Description";
                item.value = 150;
                break;
            case "Dog Gum":
                item.name = DOGGUM_ITEM;
                item.type = "Unusual";
                item.typeColor = UNUSUAL_COLOR;
                item.info = "Description";
                item.value = 250;
                break;
            case "Emergency Bag":
                item.name = EMERGENCYBAG_ITEM;
                item.type = "Unusual";
                item.typeColor = UNUSUAL_COLOR;
                item.info = "Description";
                item.value = 250;
                break;
            case "Empty_Item":
                item.name = EMPTY_ITEM;
                item.type = "Transparent";
                item.typeColor = ColorUtility.ToHtmlStringRGBA(Color.white);
                item.info = "Description";
                item.value = 0;
                break;
            case "Flippers":
                item.name = FLIPPERS_ITEM;
                item.type = "Unusual";
                item.typeColor = UNUSUAL_COLOR;
                item.info = "Description";
                item.value = 250;
                break;
            case "Food Can":
                item.name = FOODCAN_ITEM;
                item.type = "Rare";
                item.typeColor = RARE_COLOR;
                item.info = "Description";
                item.value = 500;
                break;
            case "Food Shell":
                item.name = FOODSHELL_ITEM;
                item.type = "Epic";
                item.typeColor = EPIC_COLOR;
                item.info = "Description";
                item.value = 750;
                break;
            case "Gps":
                item.name = GPS_ITEM;
                item.type = "Rare";
                item.typeColor = RARE_COLOR;
                item.info = "Description";
                item.value = 500;
                break;
            case "Hammer":
                item.name = HAMMER_ITEM;
                item.type = "Legendary";
                item.typeColor = LEGENDARY_COLOR;
                item.info = "Description";
                item.value = 1000;
                break;
            case "Horse Shoes":
                item.name = HORSESHOES_ITEM;
                item.type = "Normal";
                item.typeColor = NORMAL_COLOR;
                item.info = "Description";
                item.value = 50;
                break;
            case "Key":
                item.name = KEY_ITEM;
                item.type = "Normal";
                item.typeColor = NORMAL_COLOR;
                item.info = "Description";
                item.value = 50;
                break;
            case "Land Mine":
                item.name = LANDMINE_ITEM;
                item.type = "Common";
                item.typeColor = COMMON_COLOR;
                item.info = "Description";
                item.value = 150;
                break;
            case "Lightning":
                item.name = LIGHTNING_ITEM;
                item.type = "Legendary";
                item.typeColor = LEGENDARY_COLOR;
                item.info = "Description";
                item.value = 1000;
                break;
            case "Magnet":
                item.name = MAGNET_ITEM;
                item.type = "Rare";
                item.typeColor = RARE_COLOR;
                item.info = "Description";
                item.value = 500;
                break;
            case "Missle":
                item.name = MISSLE_ITEM;
                item.type = "Normal";
                item.typeColor = NORMAL_COLOR;
                item.info = "Description";
                item.value = 50;
                break;
            case "Nut":
                item.name = NUT_ITEM;
                item.type = "Normal";
                item.typeColor = NORMAL_COLOR;
                item.info = "Description";
                item.value = 50;
                break;
            case "Oil":
                item.name = OIL_ITEM;
                item.type = "Rare";
                item.typeColor = RARE_COLOR;
                item.info = "Description";
                item.value = 500;
                break;
            case "Potion01":
                item.name = POTION01_ITEM;
                item.type = "Normal";
                item.typeColor = NORMAL_COLOR;
                item.info = "Description";
                item.value = 50;
                break;
            case "Potion02":
                item.name = POTION02_ITEM;
                item.type = "Epic";
                item.typeColor = EPIC_COLOR;
                item.info = "Description";
                item.value = 750;
                break;
            case "Rainbow Egg":
                item.name = RAINBOWEGG_ITEM;
                item.type = "Legendary";
                item.typeColor = LEGENDARY_COLOR;
                item.info = "Description";
                item.value = 1000;
                break;
            case "Shield":
                item.name = SHIELD_ITEM;
                item.type = "Common";
                item.typeColor = COMMON_COLOR;
                item.info = "Description";
                item.value = 150;
                break;
            case "Shovel":
                item.name = SHOVEL_ITEM;
                item.type = "Unusual";
                item.typeColor = UNUSUAL_COLOR;
                item.info = "Description";
                item.value = 250;
                break;
            case "Star":
                item.name = STAR_ITEM;
                item.type = "Normal";
                item.typeColor = NORMAL_COLOR;
                item.info = "Description";
                item.value = 50;
                break;
            case "Sword":
                item.name = SWORD_ITEM;
                item.type = "Normal";
                item.typeColor = NORMAL_COLOR;
                item.info = "Description";
                item.value = 50;
                break;
            case "Tooth":
                item.name = TOOTH_ITEM;
                item.type = "Legendary";
                item.typeColor = LEGENDARY_COLOR;
                item.info = "Description";
                item.value = 1000;
                break;
            case "Energy":
                item.name = ENERGY_ITEM;
                item.type = "Award";
                item.typeColor = ColorUtility.ToHtmlStringRGBA(Color.white);
                item.info = "Energy";
                item.value = 0;
                break;
            case "Gold":
                item.name = GOLD_ITEM;
                item.type = "Award";
                item.typeColor = ColorUtility.ToHtmlStringRGBA(Color.white);
                item.info = "Gold";
                item.value = 0;
                break;
            case "Gem":
                item.name = GEM_ITEM;
                item.type = "Award";
                item.typeColor = ColorUtility.ToHtmlStringRGBA(Color.white);
                item.info = "Gem";
                item.value = 0;
                break;
        }

        return item;
    }

    public static List<Item> getStartItems()
    {
        List<Item> defitems = new List<Item>();

        Item defItem_1 = new Item(RAINBOWEGG_ITEM, "Legendary", LEGENDARY_COLOR, "Description", 1000, 0, "AttackDamage", 100);
        Item defItem_2 = new Item(BOOK_ITEM, "Epic", EPIC_COLOR, "Description", 750, 0, "Health", 300);

        defitems.Add(defItem_1);
        defitems.Add(defItem_2);

        for (int index = defitems.Count; index < PlayerPrefs.GetInt("PLAYER_INVENTORY_SLOTS_QUANTITY"); index++)
        {
            Item itemEmpty = new Item(EMPTY_ITEM, "Transparent", ColorUtility.ToHtmlStringRGBA(Color.white), "Description", 0, 0, string.Empty, 0);
            defitems.Add(itemEmpty);
        }

        return defitems;
    }

    public static List<Bullet> getStartBullets()
    {
        List<Bullet> defitems = new List<Bullet>();

        Bullet defItem_1 = new Bullet(PlayerBullets.BULLET_TYPE.CANON, PlayerBullets.BULLET_SUBTYPE.NORMAL, 100);
        Bullet defItem_2 = new Bullet(PlayerBullets.BULLET_TYPE.MINI_ROCKET, PlayerBullets.BULLET_SUBTYPE.ICE, 10);
        Bullet defItem_3 = new Bullet(PlayerBullets.BULLET_TYPE.UZI, PlayerBullets.BULLET_SUBTYPE.GLUE, 30);

        defitems.Add(defItem_1);
        defitems.Add(defItem_2);
        defitems.Add(defItem_3);

        for (int index = defitems.Count; index < PlayerPrefs.GetInt("PLAYER_INVENTORY_SLOTS_QUANTITY"); index++)
        {
            Bullet itemEmpty = new Bullet();
            defitems.Add(itemEmpty);
        }

        return defitems;
    }
}