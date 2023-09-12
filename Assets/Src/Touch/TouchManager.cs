using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TouchManager
{
    public CameraManager cameraManager;
    public ScrollAndPinch scrollAndPinch;

    private GameStageTouchManager gameStageTouchManager;

    private Button shootButton;
    //private GameObject aimTarget;

    public TouchManager(CameraManager cameraManager, ScrollAndPinch scrollAndPinch, PublicGameObjects publicGameObjects, PlayerController playerController, PlayerConfigurationService playerConfigurationService)
    {
        this.cameraManager = cameraManager;
        this.scrollAndPinch = scrollAndPinch;
        
        gameStageTouchManager = new GameStageTouchManager(publicGameObjects, playerController, playerConfigurationService);
        shootButton = publicGameObjects.shootButton;
        //aimTarget = publicGameObjects.aimTarget;
    }

    public void OnUpdate(bool enableTouch, GameObject activePlayer)
    {
        #if UNITY_IOS || UNITY_ANDROID
        {
            scrollAndPitch.gameObject.SetActive(true);
            if(enableTouch)
            {
                scrollAndPitch.ScrollAndPitchCamera(); // for phone
            }
        }
        #endif

        #if UNITY_STANDALONE_OSX || UNITY_STANDALONE_WIN || UNITY_STANDALONE_LINUX
        {
            scrollAndPinch.gameObject.SetActive(false);
            if (enableTouch)
            {
                handleDefaultStatement(false);

                if (Input.GetButton("Fire2"))
                    cameraManager.MoveAndRotateCameraByMouse();

                gameStageTouchManager.onUpdate(activePlayer);
            }
            else
            {
                handleDefaultStatement(true);
            }
        }
        #endif
    }

    private void handleDefaultStatement(bool isLocked)
    {
        if(isLocked)
        {
            shootButton.interactable = false;
            //Cursor.lockState = CursorLockMode.Locked;
            //Cursor.visible = false;
        }
        else
        {
            //shootButton.interactable = true;
            //Cursor.lockState = CursorLockMode.None;
            //Cursor.visible = true;
        }
    }
}
