using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FocusOnTargetManager
{
    public enum FOCUS { ON_PLAYER, ON_ENEMY};
    private FOCUS focusOn = FOCUS.ON_PLAYER;

    private CameraManager cameraManager;

    private List<GameObject> enemies;
    private GameObject target;
    private GameObject player;
    private GameObject camera;
    private Button shootBtn;

    private string targetName = string.Empty;
    private float movementSpeed = 2f;
    private bool focusOnSpecyficTarget = false;

    public FocusOnTargetManager(GameObject camera, CameraManager cameraManager, GameObject player, List<GameObject> enemies, PublicGameObjects publicGameObjects)
    {
        this.cameraManager = cameraManager;
        this.player = player;
        this.enemies = enemies;
        this.camera = camera;
        shootBtn = publicGameObjects.shootButton;
    }

    public void getTarget(string targetName)
    {
        this.targetName = targetName;
        shootBtn.interactable = false;
        focusOnSpecyficTarget = true;

        if (targetName.Contains("Panzer"))
        {
            focusOn = FOCUS.ON_PLAYER;
        }
        else
        {
            focusOn = FOCUS.ON_ENEMY;
        }
    }

    public void onUpdate()
    {
        if(focusOnSpecyficTarget)
        {
            var metadata = handleTarget();
            focus(metadata);
        }
    }

    private CameraTargetMetadata handleTarget()
    {
        switch(focusOn)
        {
            case FOCUS.ON_PLAYER:
                target = player;
                break;
            case FOCUS.ON_ENEMY:
                foreach(GameObject enemy in enemies)
                {
                    if(targetName.Equals(enemy.name))
                    {
                        target = enemy;
                    }
                }
                break;
        }

        return cameraManager.gameObjectsDictionary[target.GetInstanceID()];
    }

    private void focus(CameraTargetMetadata metadata)
    {
        var positionToLerp = metadata.startCameraPosition;
        var rotationToLerp = metadata.startCameraRotation;

        camera.transform.position = Vector3.Lerp(camera.transform.position, positionToLerp, Time.deltaTime * movementSpeed);
        camera.transform.rotation = Quaternion.Lerp(camera.transform.rotation, rotationToLerp, Time.deltaTime * movementSpeed);

        if (Vector3.Distance(camera.transform.position, positionToLerp) <= 0.01f && Vector3.Distance(camera.transform.rotation.eulerAngles, rotationToLerp.eulerAngles) <= 0.01f)
        {
            shootBtn.interactable = true;
            focusOnSpecyficTarget = false;
        }
    }
}
