using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStageTouchManager
{
    // targetCursor is parent of aimTarget. To calculate movement we use aimTarget (child) and later on the change (delta-position) we apply on targetCursor (parent)
    private GameObject targetCursor;
    private GameObject aimTarget;
    private GameObject targetYellowCircle;

    private PlayerController playerController;

    private Vector3 referencedVector = new Vector3(-999f, -999f, -999f);
    private Vector3 onTouchDownTargetPosition;

    private float transitionSpeed = 20f;

    private string GROUND_NAME = "Ground";
    private string TARGET_NAME = "Target";
    private string PRESS_DOWN_MOUSE_BUTTON_NAME = "Fire1";

    public GameStageTouchManager(PublicGameObjects publicGameObjects, PlayerController playerController, PlayerConfigurationService playerConfigurationService)
    {
        this.playerController = playerController;

        aimTarget = publicGameObjects.targetPreview;
        targetCursor = publicGameObjects.targetCursor;
        onTouchDownTargetPosition = referencedVector;
        targetYellowCircle = publicGameObjects.targetPreview.transform.GetChild(0).gameObject;
    }

    public void onUpdate(GameObject activeUser)
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, float.MaxValue, (1 << LayerMask.NameToLayer(GROUND_NAME)) | (1 << LayerMask.NameToLayer(TARGET_NAME))))
        {
            if (Input.GetButtonDown(PRESS_DOWN_MOUSE_BUTTON_NAME) && hit.collider.tag.Equals(TARGET_NAME))
            {
                var temp = aimTarget.transform.position - hit.point;
                onTouchDownTargetPosition = new Vector3(temp.x, targetCursor.transform.position.y, temp.z);

                TargetPreviewUziService.setAngle(activeUser.transform.position);
            }
        }

        if (Physics.Raycast(ray, out hit, float.MaxValue, (1 << LayerMask.NameToLayer(GROUND_NAME))))
        {
            if (Input.GetButton(PRESS_DOWN_MOUSE_BUTTON_NAME) && !onTouchDownTargetPosition.Equals(referencedVector))
            {
                //var temp = onTouchDownTargetPosition + hit.point;
                //Vector3 touchPoint = Vector3.Lerp(targetCursor.transform.position, new Vector3(temp.x, targetCursor.transform.position.y, temp.z), Time.deltaTime * transitionSpeed);
                var temp = hit.point;
                Vector3 touchPoint = new Vector3(temp.x, targetCursor.transform.position.y, temp.z);

                Debug.Log("Yellow point height: " + GameObject.FindGameObjectWithTag("Player").GetComponentsInChildren<ProjectileArc>()[1].endPoint.y);
                if (TargetInterface.shootingMode.Equals(TargetInterface.SHOOTING_MODE.CANON)) {
                    playerController.setTargetWithAngle(activeUser, touchPoint);
                } 
                else
                {
                    playerController.setTargetWithSpeed(activeUser, touchPoint, true);
                    TargetPreviewUziService.setAngle(activeUser.transform.position);
                }
                targetCursor.transform.position = Vector3.Lerp(targetCursor.transform.position, touchPoint, Time.deltaTime * 25f);
            }
        }

        if (Input.GetButtonUp(PRESS_DOWN_MOUSE_BUTTON_NAME))
        {
            onTouchDownTargetPosition = referencedVector;
        }
    }
}
