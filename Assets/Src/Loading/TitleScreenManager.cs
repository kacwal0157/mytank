using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleScreenManager : MonoBehaviour
{
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Tab))
        {
            Loader.load((int)Loader.Scenes.MAIN_MENU);
        }
    }
}
