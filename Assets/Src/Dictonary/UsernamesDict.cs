using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UsernamesDict
{
    public static Dictionary<string, string> usernames;

    private PlayerMetadataManager playerMetadataManager;
    private string usernamesPrefs;

    public UsernamesDict(PlayerMetadataManager playerMetadataManager)
    {
        this.playerMetadataManager = playerMetadataManager;

        onInit();
    }

    private void onInit()
    {
        usernamesPrefs = playerMetadataManager.usernamesDict;
        usernames = Utilities.convertStringToDictionary(usernamesPrefs);

        if (usernames == null || usernames.Count == 0)
        {
            usernames = getBasicUsernames(usernames);
        }
    }

    private Dictionary<string, string> getBasicUsernames(Dictionary<string, string> usernamesDict)
    {
        usernamesDict = new Dictionary<string, string>();
        usernamesDict.Add(PlayerPrefs.GetString("PLAYER_TAG"), PlayerPrefs.GetString("USERNAME"));

        //TODO: temporary
        usernamesDict.Add(Utilities.getRandomTag(8), "Gamer123");
        usernamesDict.Add(Utilities.getRandomTag(8), "Daniel");
        usernamesDict.Add(Utilities.getRandomTag(8), "Kacper");

        return usernamesDict;
    }

    public void saveUsernames()
    {
        usernamesPrefs = Utilities.convertDictionaryToString(usernames);
        playerMetadataManager.usernamesDict = usernamesPrefs;
    }
}
