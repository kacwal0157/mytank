using Newtonsoft.Json;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public static class Utilities
{
    public static GameObject getPrefabFromResources(string prefabName)
    {
        return Resources.Load("Prefabs/" + prefabName) as GameObject;
    }

    public static string convertListToString(List<System.DateTime> spinTimes)
    {
        return JsonConvert.SerializeObject(spinTimes);
    }

    public static List<System.DateTime> convertStringToList(string spinTimes)
    {
        return JsonConvert.DeserializeObject<List<System.DateTime>>(spinTimes);
    }

    public static string convertListToString(List<WorldPrefabs> worldPrefabs)
    {
        return JsonConvert.SerializeObject(worldPrefabs);
    }

    public static List<WorldPrefabs> convertWorldPrefabsStringToList(string worldPrefabs)
    {
        return JsonConvert.DeserializeObject<List<WorldPrefabs>>(worldPrefabs);
    }

    public static string convertListToString(List<int> tankStats)
    {
        return JsonConvert.SerializeObject(tankStats);
    }

    public static List<int> convertTankStringToList(string tankStats)
    {
        return JsonConvert.DeserializeObject<List<int>>(tankStats);
    }

    public static Quaternion lookAtPlayerDirection(Vector3 enemy, Vector3 player)
    {
        Vector3 direction = player - enemy;
        Quaternion finalRotation = Quaternion.LookRotation(direction);

        return finalRotation;
    }

    public static string convertListOfItemToString(List<Item> itemList)
    {
        return JsonConvert.SerializeObject(itemList);
    }

    public static string convertListOfBulletItemToString(List<Bullet> itemList)
    {
        return JsonConvert.SerializeObject(itemList);
    }

    public static List<Item> convertStringToListOfItem(string items)
    {
        return JsonConvert.DeserializeObject<List<Item>>(items);
    }

    public static List<Bullet> convertStringToListOfBullets(string items)
    {
        return JsonConvert.DeserializeObject<List<Bullet>>(items);
    }

    public static string getRandomTag(int length)
    {
        const string chars = "0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";

        StringBuilder sb = new StringBuilder();
        sb.Append("#");

        for (int i = 0; i < length; i++)
        {
            int index = Random.Range(0, chars.Length);
            sb.Append(chars[index]);
        }

        return sb.ToString();
    }

    public static Dictionary<string, string> convertStringToDictionary(string usernames)
    {
        return JsonConvert.DeserializeObject<Dictionary<string, string>>(usernames);
    }

    public static string convertDictionaryToString(Dictionary<string, string> dict)
    {
        return JsonConvert.SerializeObject(dict);
    }
}
