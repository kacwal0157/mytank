using HWRWeaponSystem;
using MyTankWeaponSystem;
using UnityEngine;
using UnityEngine.UI;

public class FireController
{
    private PlayerController playerController;
    private PlayerConfigurationService playerConfigurationService; //linesSpeed, sizeMultiplier, timeToNextLaunch
    private PendulumEffectController pendulumEffectController;

    public float y = 0;
    public float x = 0;
    public static bool enemyReadyForShoot = false;

    private float horizontalLinePosition = 0f;
    private float verticalLinePosition = 0f;

    private float maxVerticalAndHorizontalLinePosition = 0.5f;
    private float resetAllLinePosition = 0f;

    private enum TARGET_SHIELD_STATE {RUNNING_HORIZONTAL, RUNNING_VERTICAL, DISABLED};
    private static TARGET_SHIELD_STATE targetShieldState = TARGET_SHIELD_STATE.DISABLED;

    private static GameObject targetShield;
    public GameObject horizontalLineGo;
    public GameObject verticalLineGo;

    public FireController(GameObject targetShield, PlayerConfigurationService playerConfigurationService, 
        PlayerController playerController, PendulumEffectController pendulumEffectController)
    {
        FireController.targetShield = targetShield;
        this.playerConfigurationService = playerConfigurationService;
        this.playerController = playerController;
        this.pendulumEffectController = pendulumEffectController;
        initializeVerticalAndHorizontalGo(targetShield.transform);
        maxVerticalAndHorizontalLinePosition *= playerConfigurationService.projectorSizeMultiplier;
    }


    public void OnStart()
    {
        SetDefaultTargetShieldState();
    }

    public void OnUpdate(GameObject activePlayer)
    {
        onUpdateTargetShieldStateChange(activePlayer);
        onUpdateTargetShieldLinePositions();
        onUpdateLineGlobalPositions();
    }

    private void onUpdateTargetShieldStateChange(GameObject activePlayer)
    {
        if (Input.GetMouseButtonDown(0) == true || enemyReadyForShoot)
        {
            switch (targetShieldState)
            {
                case TARGET_SHIELD_STATE.RUNNING_HORIZONTAL:
                    targetShieldState = TARGET_SHIELD_STATE.RUNNING_VERTICAL;
                    verticalLineGo.SetActive(true);
                    enemyReadyForShoot = false;
                    break;

                case TARGET_SHIELD_STATE.RUNNING_VERTICAL:
                    //TODO: remove comment
                    targetShieldState = TARGET_SHIELD_STATE.DISABLED;
                    targetShield.SetActive(false);
                    playerController.fire(activePlayer);

                    ResetOnScreenTarget();
                    enemyReadyForShoot = false;
                    break;
            }
        }
    }

    private void onUpdateTargetShieldLinePositions()
    {
        switch (targetShieldState)
        {
            case TARGET_SHIELD_STATE.DISABLED:
                if (targetShield.activeSelf)
                {
                    targetShield.SetActive(false);
                }
                break;
            case TARGET_SHIELD_STATE.RUNNING_HORIZONTAL:
                UpdateHorizontalLinePosition();
                break;
            case TARGET_SHIELD_STATE.RUNNING_VERTICAL:
                UpdateVerticalLinePosition();
                break;
        }
    }

    private void onUpdateLineGlobalPositions()
    {
        y = horizontalLineGo.transform.localPosition.y;
        x = verticalLineGo.transform.localPosition.x;
    }

    private void UpdateHorizontalLinePosition()
    {
        horizontalLinePosition = pendulumEffectController.getHorizontalLinePosition();

        Vector3 newPosition = new Vector3(0f, horizontalLinePosition, 0f);
        horizontalLineGo.transform.localPosition = newPosition;
    }

    private void UpdateVerticalLinePosition()
    {
        verticalLinePosition = pendulumEffectController.getVerticalLinePosition();

        Vector3 newPosition = new Vector3(verticalLinePosition, y, 0f);
        verticalLineGo.transform.localPosition = newPosition;
    }

    public void ResetOnScreenTarget()
    {
        horizontalLinePosition = resetAllLinePosition;
        verticalLinePosition = resetAllLinePosition;

        horizontalLineGo.transform.localPosition = Vector3.zero;
        verticalLineGo.transform.localPosition = Vector3.zero;
    }

    public void SetDefaultTargetShieldState()
    {
        targetShield.SetActive(true);
        horizontalLineGo.SetActive(true);
        verticalLineGo.SetActive(false);

        horizontalLinePosition = maxVerticalAndHorizontalLinePosition - 1f;
        targetShieldState = TARGET_SHIELD_STATE.RUNNING_HORIZONTAL;
    }

    private void initializeVerticalAndHorizontalGo(Transform transform)
    {
        foreach (Transform child in transform)
        {

            if (child.name.Contains("Horizontal"))
            {
                horizontalLineGo = child.gameObject;
            }

            if (child.name.Contains("Vertical"))
            {
                verticalLineGo = child.gameObject;
            }
        }
    }

    public static void turnOfTargetShield()
    {
        if(targetShield.activeSelf)
        {
            targetShield.SetActive(false);
            targetShieldState = TARGET_SHIELD_STATE.DISABLED;
        }
    }
}
