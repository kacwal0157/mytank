using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AIController
{
    private PlayerController playerController;
    private PlayerMetadata playerMetadata;
    private WindController windController;

    private Dictionary<int, Vector3> allEnemiesChangeStatus = new Dictionary<int, Vector3>();
    private Dictionary<int, bool> doesEnemyMakeHisFirstShoot = new Dictionary<int, bool>();
    private GameObject activeEnemy;
    private GameObject targetCursor;
    private GameObject player;
    private Button shootButton;
    private Button aimTarget;

    private Vector3 lastTargetCursorPosition = Vector3.zero;
    private Vector3 finalPos;
    private Vector3 playerPos;

    private int levelDifficulty;
    private bool allPositionVarablesAreSet = false;

    //clear it
    private float verticalLinePos;
    private float horizontalLinePos;
    private float horizontalLineTimeCounter = 0;
    private float verticalLineTimeCounter = 0;

    public AIController(Level.Metadata metadata, PlayerController playerController, PublicGameObjects publicGameObjects, GameObject playerPrefab, WindController windController)
    {
        this.playerController = playerController;
        this.windController = windController;

        targetCursor = publicGameObjects.targetCursor;
        player = playerPrefab;
        shootButton = publicGameObjects.shootButton;
        aimTarget = publicGameObjects.aimTarget.GetComponent<Button>();
        levelDifficulty = metadata.levelDifficulty;
        playerPos = player.transform.position;
    }

    public void aimStageChange(GameObject activeEnemy)
    {
        this.activeEnemy = activeEnemy;

        playerMetadata = playerController.getPlayerMetadata(activeEnemy.GetInstanceID());
        addEnemiesToDict(activeEnemy.GetInstanceID());

        allPositionVarablesAreSet = false;
    }

    public void aimOnUpdate()
    {
        GameManager.getInstance.StartCoroutine(moveAimTarget());
    }

    private IEnumerator moveAimTarget()
    {
        yield return new WaitForSeconds(1f);
        Vector3 pos = getPositionToAim(levelDifficulty, activeEnemy);
        playerController.setTargetWithAngle(activeEnemy, pos);
        targetCursor.transform.position = Vector3.Lerp(targetCursor.transform.position, pos, Time.deltaTime * 25f);
        yield return new WaitForSeconds(3f);
        shootButton.onClick.Invoke();
    }

    private Vector3 getPositionToAim(int enemyDifficultyLevel, GameObject activeEnemy)
    {
        if (!allPositionVarablesAreSet)
        {
            var newX = allEnemiesChangeStatus[activeEnemy.GetInstanceID()].x;
            var newZ = allEnemiesChangeStatus[activeEnemy.GetInstanceID()].z;

            var isItFirstShoot = doesEnemyMakeHisFirstShoot[activeEnemy.GetInstanceID()];

            newX = setStartAimTargetPosition(newX, isItFirstShoot, enemyDifficultyLevel);
            newZ = setStartAimTargetPosition(newZ, isItFirstShoot, enemyDifficultyLevel);

            newX = setNewAimTargetPosition(newX, playerPos.x, WindController.windDirection.x, enemyDifficultyLevel);
            newZ = setNewAimTargetPosition(newZ, playerPos.z, WindController.windDirection.y, enemyDifficultyLevel);

            isItFirstShoot = false;
            doesEnemyMakeHisFirstShoot[activeEnemy.GetInstanceID()] = isItFirstShoot;

            finalPos = new Vector3(newX, player.transform.position.y, newZ);
            lastTargetCursorPosition = finalPos;

            allEnemiesChangeStatus[activeEnemy.GetInstanceID()] = lastTargetCursorPosition;
            allPositionVarablesAreSet = true;
        }

        return Vector3.Lerp(playerMetadata.lastAimTargetPosition, finalPos, Time.deltaTime * 2f);
    }

    private float setStartAimTargetPosition(float newPosition, bool isItFirstShoot, int difficulty)
    {
        var range = 5 / difficulty;
        newPosition += isItFirstShoot ? (Random.Range(-range, range)) : 0;
        return newPosition;
    }

    private float setNewAimTargetPosition(float newPosition, float playerPos, float windDirection, int enemyDifficulty)
    {
        if (newPosition - playerPos <= 0.1f && newPosition - playerPos >= -0.1f || newPosition == playerPos)
        {
            return newPosition;
        }

        newPosition += newPosition > playerPos ? Random.Range(-enemyDifficulty, 0) : Random.Range(0, enemyDifficulty);
        newPosition = windController.handleDirectionOfWind(newPosition, windDirection, enemyDifficulty);

        return newPosition;
    }

    public void shootStageChange()
    {
        //CONCEPT
        switch (levelDifficulty)
        {
            case 3:
                horizontalLinePos = getAimTargetLinesPositions(horizontalLinePos, WindController.windDirection.y, 40f, 50f);
                verticalLinePos = getAimTargetLinesPositions(verticalLinePos, WindController.windDirection.x, 40f, 50f);
                break;

            case 4:
                horizontalLinePos = getAimTargetLinesPositions(horizontalLinePos, WindController.windDirection.y, 30f, 50f);
                verticalLinePos = getAimTargetLinesPositions(verticalLinePos, WindController.windDirection.x, 30f, 50f);
                break;

            case 5:
                horizontalLinePos = getAimTargetLinesPositions(horizontalLinePos, WindController.windDirection.y, 1f, 25f);
                verticalLinePos = getAimTargetLinesPositions(verticalLinePos, WindController.windDirection.x, 1f, 25f);
                break;

            default:
                GameManager.getInstance.StartCoroutine(getShootPositionFromAimTarget(Random.Range(2.5f, 4.5f), Random.Range(2.5f, 4.5f)));
                break;
        }
    }

    private float getAimTargetLinesPositions(float linePos, float windDirection, float minRange, float maxRange)
    {
        if (windDirection == 1)
        {
            linePos = Random.Range(-maxRange, -minRange);
        }

        if (windDirection == -1)
        {
            linePos = Random.Range(minRange, maxRange);
        }

        if (windDirection == 0)
        {
            linePos = Random.Range(-maxRange, maxRange);
        }

        return linePos;
    }

    public void shootOnUpdate(GameObject horizontalLine, GameObject verticalLine)
    {
        if(levelDifficulty >= 3)
        {
            if (horizontalLine.activeSelf && !verticalLine.activeSelf)
            {
                horizontalLineTimeCounter += Time.deltaTime;
            }

            if (verticalLine.activeSelf)
            {
                verticalLineTimeCounter += Time.deltaTime;
            }

            invokeShoot(horizontalLine, verticalLine, Random.Range(2.5f, 4.5f), Random.Range(2.5f, 4.5f));
        }
    }

    private void invokeShoot(GameObject horizontalLine, GameObject verticalLine, float stopTimeHorizontal, float stopTimeVertical)
    {
        if (horizontalLine.transform.localPosition.y < horizontalLinePos && horizontalLineTimeCounter > stopTimeHorizontal)
        {
            aimTarget.onClick.Invoke();
            horizontalLineTimeCounter = 0;
        }

        if (verticalLine.transform.localPosition.x < verticalLinePos && verticalLineTimeCounter > stopTimeVertical)
        {
            aimTarget.onClick.Invoke();
            verticalLineTimeCounter = 0;
        }
    }

    private IEnumerator getShootPositionFromAimTarget(float timeHorizontal, float timeVertical)
    {
        yield return new WaitForSeconds(timeHorizontal);
        aimTarget.onClick.Invoke();

        yield return new WaitForSeconds(timeVertical);
        aimTarget.onClick.Invoke();
    }

    private void addEnemiesToDict(int enemyID)
    {
        if (!allEnemiesChangeStatus.ContainsKey(enemyID))
        {
            allEnemiesChangeStatus.Add(enemyID, playerPos);
            doesEnemyMakeHisFirstShoot.Add(enemyID, true);
        }
    }
}
