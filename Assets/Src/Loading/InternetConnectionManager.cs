using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class InternetConnectionManager
{
    //future code for AWS here
    public static IEnumerator checkInternetConnection(GameObject networkErrorGO, Button retryConnection)
    {
        UnityWebRequest request = new UnityWebRequest("http://www.google.com");
        yield return request.SendWebRequest();

        if(request.error != null)
        {
            networkErrorGO.SetActive(true);
            retryConnection.onClick.AddListener(delegate { InternetConnectionManager.retryConnection(0, networkErrorGO); });
        }
        else
        {
            LoadingManager.instance.loadScene();
        }
    }

    public static void retryConnection(int basicIndex, GameObject networkErrorGO)
    {
        networkErrorGO.SetActive(false);
        SceneManager.LoadScene(basicIndex);
    }
}
