using UnityEngine;
using UnityEngine.UI;

public class ButtonController : MonoBehaviour
{
    public GameObject targetShield;
    public GameObject targetPreviewCanon;
    public GameObject targetPreviewUzi;
    public PublicGameObjects publicGameObjects;
    public Button uziButton;
    public Button canonButton;

    private Vector3 mockNullVector3 = new Vector3(-999f, -999f, -999f);

    public void TurnOnShootMode()
    {
        if (GameManager.currentStage.Equals(GameManager.Stage.PLAYER_TURN_AIMING))
        {
            GameManager.getInstance.changeState(GameManager.Stage.PLAYER_TURN_SHOOTING);
            targetShield.SetActive(true);
        }
        else if (GameManager.currentStage.Equals(GameManager.Stage.OPONNENT_TURN_AIMING))
        {
            GameManager.getInstance.changeState(GameManager.Stage.OPONNENT_TURN_SHOOTING);
        }
    }

    public void TurnOffShootMode()
    {
        if (GameManager.currentStage.Equals(GameManager.Stage.PLAYER_TURN_SHOOTING))
        {
            GameManager.getInstance.changeState(GameManager.Stage.PLAYER_TURN_AIMING);
        }
        else if (GameManager.currentStage.Equals(GameManager.Stage.OPONNENT_TURN_SHOOTING))
        {
            GameManager.getInstance.changeState(GameManager.Stage.OPONNENT_TURN_AIMING);
        }
        targetShield.SetActive(false);
    }

    public void TurnOnCanonMode()
    {
        TargetInterface.shootingMode = TargetInterface.SHOOTING_MODE.CANON;
        targetPreviewCanon.SetActive(true);
        targetPreviewUzi.SetActive(false);

        GameManager.getInstance.getPayerController().setTargetWithAngle(GameManager.getInstance.getActivePlayer(), mockNullVector3);
        GameManager.getInstance.getPayerController().setTargetWithAngle(GameManager.getInstance.getActivePlayer(), mockNullVector3);

        GameManager.getInstance.getActivePlayerMetadata().lastShootingMode = TargetInterface.SHOOTING_MODE.CANON;

        uziButton.enabled = true;
        canonButton.enabled = false;

        GameManager.getInstance.getActivePlayerMetadata().playerBullets.selectedShootingMode = TargetInterface.SHOOTING_MODE.CANON;
    }

    public void TurnOnUziMode()
    {
        TargetInterface.shootingMode = TargetInterface.SHOOTING_MODE.UZI;
        targetPreviewCanon.SetActive(false);
        targetPreviewUzi.SetActive(true);

        TargetPreviewUziService.setAngle(GameManager.getInstance.getActivePlayer().transform.position);
        //The script interpolates to the point, that's why we have 2 invokations of same script
        GameManager.getInstance.getPayerController().setTargetWithSpeed(GameManager.getInstance.getActivePlayer(), mockNullVector3, true);
        GameManager.getInstance.getPayerController().setTargetWithSpeed(GameManager.getInstance.getActivePlayer(), mockNullVector3, true);

        GameManager.getInstance.getActivePlayerMetadata().lastShootingMode = TargetInterface.SHOOTING_MODE.UZI;

        uziButton.enabled = false;
        canonButton.enabled = true;

        GameManager.getInstance.getActivePlayerMetadata().playerBullets.selectedShootingMode = TargetInterface.SHOOTING_MODE.UZI;


    }

    public void changeFireController()
    {
        FireController.enemyReadyForShoot = true;
    }
}

