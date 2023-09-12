using HWRWeaponSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraManager
{
    public Dictionary<int, CameraTargetMetadata> gameObjectsDictionary = new Dictionary<int, CameraTargetMetadata>();
    public enum CAMERA_STAGE { ON_AIMING, ON_SHOOTING, ON_BULLET_FLYING};
    private CAMERA_STAGE cameraStage = CAMERA_STAGE.ON_AIMING;

    private const float CAMERA_OFFSET = 12f;
    private const float CAMERA_OFFSET_Y_SHOOTING = 5f;
    private const float CAMERA_ROTATION_X = 10f;


    //MOVE CAMERA
    private const float CAMERA_MOVEMENT_SPEED = 30f;
    private const float CAMERA_ZOOM_SPEED = 2f;
    private const float MIN_AXIS_X = 15.0f;
    private const float MAX_AXIS_X = 45.0f;
    private const float SENSIVITY_X = 100.0f;
    private const float SENSIVITY_Y = 100.0f;

    //AIMING
    private const float CAMERA_HEIGHT = 13f;

    //ROTATION FOR MOUSE AND FINGER METHODS
    private float ROTATION_X = 40.0f;
    private float ROTATION_Y = 0.0f;
    
    private Vector3 minimumPosition = new Vector3(-50f, -15f, -50f);
    private Vector3 maximumPosition = new Vector3(50f, -15f, 50f);

    public bool isPositionSet = false;

    private CameraTargetMetadata currentCameraMetadata;

    private GameObject camera;
    private Transform targetPreview;

    private Vector3 cameraViewOnPlayerInAimingMode;
    private Quaternion cameraViewOnPlayerRotationInAimingMode;

    private Vector3 cameraViewOnPlayerInShootingMode;
    private Quaternion cameraViewOnPlayerRotationInShootingMode;

    private Transform bulletToFollow;
    private Vector3 deltaBetweenBulletAndCamera = Vector3.zero;

    private float movementSpeed = 1f;

    public CameraManager(GameObject camera, GameObject player, List<GameObject> enemies, PublicGameObjects publicGameObjects)
    {
        this.camera = camera;

        targetPreview = publicGameObjects.targetPreview.transform;

        for (int i = 0; i < enemies.Count; i++)
        {
            enemies[i].transform.rotation = Utilities.lookAtPlayerDirection(enemies[i].transform.position, player.transform.position);

            var positionInShootingMode = getCameraOnViewPosition(enemies[i].transform.position, player.transform.position);
            var rotationInShootingMode = getCameraOnViewRotation(enemies[i].transform.position, player.transform.position);
            
            var lastCamPos = new Vector3(positionInShootingMode.x, CAMERA_HEIGHT, positionInShootingMode.z);
            var lastCamRot = Quaternion.Euler(ROTATION_X, rotationInShootingMode.eulerAngles.y, rotationInShootingMode.eulerAngles.z);

            var cameraTargetMetaData = new CameraTargetMetadata(enemies[i], lastCamPos, lastCamRot);
            gameObjectsDictionary.Add(enemies[i].GetInstanceID(), cameraTargetMetaData);
        }

        var playerPositionInShootingMode = getCameraOnViewPosition(player.transform.position, enemies[0].transform.position);
        var playerRotationInShootingMode = getCameraOnViewRotation(player.transform.position, enemies[0].transform.position);

        var lastPlayerCamPos = new Vector3(playerPositionInShootingMode.x, CAMERA_HEIGHT, playerPositionInShootingMode.z);
        var lastPlayerCamRot = Quaternion.Euler(ROTATION_X, playerRotationInShootingMode.eulerAngles.y, playerRotationInShootingMode.eulerAngles.z);

        var cameraTargetMetaDataPlayer = new CameraTargetMetadata(player, lastPlayerCamPos, lastPlayerCamRot);
        gameObjectsDictionary.Add(player.GetInstanceID(), cameraTargetMetaDataPlayer);
    }

    public void moveCamera(GameManager.Stage nextStage)
    {
        Vector3 destinationPosition = Vector3.zero;
        Quaternion destinationRotation = Quaternion.identity;

        switch(cameraStage)
        {
            case CAMERA_STAGE.ON_AIMING:
                destinationPosition = cameraViewOnPlayerInAimingMode;
                destinationRotation = cameraViewOnPlayerRotationInAimingMode;
                movementSpeed = movementSpeed + 0.2f;
                break;

            case CAMERA_STAGE.ON_SHOOTING:
                destinationPosition = cameraViewOnPlayerInShootingMode;
                destinationRotation = cameraViewOnPlayerRotationInShootingMode;
                movementSpeed = movementSpeed + 0.2f;
                break;

            case CAMERA_STAGE.ON_BULLET_FLYING:
                if (TargetInterface.shootingMode.Equals(TargetInterface.SHOOTING_MODE.CANON))
                {
                    if (bulletToFollow != null)
                    {
                        destinationPosition = bulletToFollow.position + deltaBetweenBulletAndCamera;
                        destinationRotation = cameraViewOnPlayerRotationInShootingMode;
                        movementSpeed -= 3f;
                        if(movementSpeed < 2f)
                        {
                            movementSpeed = 2f;
                        }

                        camera.transform.position = Vector3.Lerp(camera.transform.position, destinationPosition, Time.deltaTime * movementSpeed) ;
                        camera.transform.rotation = Quaternion.Lerp(camera.transform.rotation, destinationRotation, Time.deltaTime * movementSpeed);
                        return;
                    }
                    else
                    {
                        setNextStageSpeedAndPosition(nextStage);
                        return;
                    }
                } else
                {
                    if (bulletToFollow == null)
                    {
                        setNextStageSpeedAndPosition(nextStage);
                        return;
                    }
                }
                return;
                
        }

        if (Vector3.Distance(camera.transform.position, destinationPosition) > 0.01f || Vector3.Distance(camera.transform.rotation.eulerAngles, destinationRotation.eulerAngles) > 0.01f)
        {
            camera.transform.position = Vector3.Lerp(camera.transform.position, destinationPosition, Time.deltaTime * movementSpeed);
            camera.transform.rotation = Quaternion.Lerp(camera.transform.rotation, destinationRotation, Time.deltaTime * movementSpeed);
        }
        else
        {
            setNextStageSpeedAndPosition(nextStage);
        }
    }

    private void setNextStageSpeedAndPosition(GameManager.Stage nextStage)
    {
        GameManager.currentStage = nextStage;
        movementSpeed = 1f;
        isPositionSet = true;
    }

    private Vector3 getCameraOnViewPosition(Vector3 playerPosition, Vector3 targetPreviewPostion)
    {
        if (playerPosition.x == targetPreviewPostion.x || playerPosition.z == targetPreviewPostion.z)
        {
            return getCameraOnViewPositionEdgeCases(playerPosition, targetPreviewPostion);
        }

        float a1 = (playerPosition.z - targetPreviewPostion.z) / (playerPosition.x - targetPreviewPostion.x);
        float b1 = playerPosition.z - a1 * playerPosition.x;

        float a = Mathf.Pow(a1, 2) + 1f;
        float b = 2 * a1 * b1 - 2 * playerPosition.x - 2 * a1 * playerPosition.z;
        float c = Mathf.Pow(b1, 2) + Mathf.Pow(playerPosition.x, 2) + Mathf.Pow(playerPosition.z, 2) - 2 * b1 * playerPosition.z - Mathf.Pow(CAMERA_OFFSET, 2);

        float delta = Mathf.Pow(b, 2) - 4f * a * c;

        float x1 = (-b - Mathf.Sqrt(delta)) / (2f * a);
        float y1 = a1 * x1 + b1;

        float x2 = (-b + Mathf.Sqrt(delta)) / (2f * a);
        float y2 = a1 * x2 + b1;

        Vector3 pos1 = new Vector3(x1, CAMERA_OFFSET_Y_SHOOTING, y1);
        Vector3 pos2 = new Vector3(x2, CAMERA_OFFSET_Y_SHOOTING, y2);

        if (Vector3.Distance(targetPreviewPostion, pos1) > Vector3.Distance(targetPreviewPostion, pos2))
        {
            return pos1;
        }

        return pos2;
    }

    private Vector3 getCameraOnViewPositionEdgeCases(Vector3 playerPosition, Vector3 targetPreviewPostion)
    {
        // x --> x, z --> y
        if (playerPosition.x == targetPreviewPostion.x)
        {
            if (playerPosition.z > targetPreviewPostion.z)
            {
                return new Vector3(playerPosition.x, CAMERA_OFFSET_Y_SHOOTING, playerPosition.z + CAMERA_OFFSET);
            }
            else if (playerPosition.z < targetPreviewPostion.z)
            {
                return new Vector3(playerPosition.x, CAMERA_OFFSET_Y_SHOOTING, playerPosition.z - CAMERA_OFFSET);
            }
        }

        if (playerPosition.z == targetPreviewPostion.z)
        {
            if (playerPosition.x > targetPreviewPostion.x)
            {
                return new Vector3(playerPosition.x + CAMERA_OFFSET, CAMERA_OFFSET_Y_SHOOTING, playerPosition.z);
            }
            else if (playerPosition.x < targetPreviewPostion.x)
            {
                return new Vector3(playerPosition.x - CAMERA_OFFSET, CAMERA_OFFSET_Y_SHOOTING, playerPosition.z);
            }
        }

        return new Vector3(playerPosition.x, CAMERA_OFFSET_Y_SHOOTING, playerPosition.z);
    }

    private Quaternion getCameraOnViewRotation(Vector3 playerPosition, Vector3 targetPreviewPostion)
    {
        Vector3 direction = targetPreviewPostion - playerPosition;
        Quaternion rotation = Quaternion.LookRotation(direction);

        return Quaternion.Euler(CAMERA_ROTATION_X, rotation.eulerAngles.y, rotation.eulerAngles.z);
    }

    public void setCameraAimingAndShootingPositionAndRotation(GameObject activePlayer)
    {
        CameraTargetMetadata cameraMetadata = gameObjectsDictionary[activePlayer.GetInstanceID()];
        currentCameraMetadata = cameraMetadata;

        cameraViewOnPlayerInShootingMode = getCameraOnViewPosition(activePlayer.transform.position, targetPreview.position);
        cameraViewOnPlayerRotationInShootingMode = getCameraOnViewRotation(activePlayer.transform.position, targetPreview.position);

        cameraViewOnPlayerInAimingMode = new Vector3(cameraViewOnPlayerInShootingMode.x, CAMERA_HEIGHT, cameraViewOnPlayerInShootingMode.z);
        cameraViewOnPlayerRotationInAimingMode = Quaternion.Euler(ROTATION_X, cameraViewOnPlayerRotationInShootingMode.eulerAngles.y, cameraViewOnPlayerRotationInShootingMode.eulerAngles.z);

        if (cameraMetadata.lastCameraPosition != Vector3.zero)
        {
            cameraViewOnPlayerInAimingMode = cameraMetadata.lastCameraPosition;
        }

        if (cameraMetadata.lastCameraRotation.eulerAngles != Vector3.zero)
        {
            cameraViewOnPlayerRotationInAimingMode = cameraMetadata.lastCameraRotation;
        }
    }

    public void setBulletToFollow(Transform bulletToFollow)
    {
        this.bulletToFollow = bulletToFollow;
        deltaBetweenBulletAndCamera = camera.transform.position - bulletToFollow.position;
    }

    public void changeCameraState(GameManager.Stage nextStage)
    {
        if (nextStage == GameManager.Stage.PLAYER_TURN_AIMING || nextStage == GameManager.Stage.OPONNENT_TURN_AIMING)
        {
            movementSpeed = 1f;
            cameraStage = CAMERA_STAGE.ON_AIMING;
        }

        if (nextStage == GameManager.Stage.PLAYER_TURN_SHOOTING || nextStage == GameManager.Stage.OPONNENT_TURN_SHOOTING)
        {
            movementSpeed = 1f;
            cameraStage = CAMERA_STAGE.ON_SHOOTING;
        }

        if (nextStage == GameManager.Stage.AFTER_SHOOT)
        {
            movementSpeed = 200f;
            cameraStage = CAMERA_STAGE.ON_BULLET_FLYING;
        } 
    }

    public void MoveAndRotateCameraByMouse()
    {
        //MOVE
        if (Input.GetAxis("Mouse X") != 0)
        {
            float y = camera.transform.position.y;
            camera.transform.Translate(new Vector3(-Input.GetAxis("Mouse X") * Time.deltaTime * CAMERA_MOVEMENT_SPEED, 0.0f, -Input.GetAxis("Mouse Y") * Time.deltaTime * CAMERA_MOVEMENT_SPEED));
            
            Vector3 pos = camera.transform.position;
            pos.x = Mathf.Clamp(pos.x, minimumPosition.x, maximumPosition.x);
            pos.y = y;
            pos.z = Mathf.Clamp(pos.z, minimumPosition.z, maximumPosition.z);
            
            camera.transform.position = pos; //reassign clamped Vector to position
        }

        //SCROLL
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (camera.transform.localPosition.y >= -20f && camera.transform.localPosition.y <= -13f)
        {
            camera.transform.Translate(0, scroll * CAMERA_ZOOM_SPEED, scroll * CAMERA_ZOOM_SPEED, Space.World);

            if (camera.transform.localPosition.y < -20f)
                camera.transform.localPosition = new Vector3(camera.transform.localPosition.x, -20f, camera.transform.localPosition.z);

            if (camera.transform.localPosition.y > -13f)
                camera.transform.localPosition = new Vector3(camera.transform.localPosition.x, -13f, camera.transform.localPosition.z);
        }
            

        //ROTATE
        if (Input.GetMouseButton(0))
        {
            ROTATION_Y = camera.transform.localEulerAngles.y + Input.GetAxis("Mouse X") * SENSIVITY_X * Time.deltaTime;
            ROTATION_X += Input.GetAxis("Mouse Y") * SENSIVITY_Y * Time.deltaTime;
            ROTATION_X = Mathf.Clamp(ROTATION_X, MIN_AXIS_X, MAX_AXIS_X);
            camera.transform.localEulerAngles = new Vector3(ROTATION_X, ROTATION_Y, 0);
        }

        currentCameraMetadata.lastCameraPosition = camera.transform.position;
        currentCameraMetadata.lastCameraRotation = camera.transform.rotation;
    }
}

