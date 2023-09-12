using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using HWRWeaponSystem;

public class PlayerController
{
    public Dictionary<int, PlayerMetadata> playermetadataDict = new Dictionary<int, PlayerMetadata>();
    public static float currentTimeOfFlight;
    public static float lastShotTime;
    public static float lastShotTimeOfFlight;

    private const string FIRE_POINT_TAG = "firePoint";
    private const string TURRET_TAG = "turret";
    private const string CANNON_BASE_TAG = "cannonBase";
    private const float FIRE_COOLDOWN_IN_SECONDS = 1;
    //private const float UI_ELEMENTS_TO_GAME_WORLD_FACTOR = 0.02f;

    private PendulumEffectController pendulumEffectController;
    private CameraManager cameraManager;
    private PlayerConfigurationService playerConfigurationService;
    private PublicGameObjects publicGameObjects;

    private BulletController bulletController = new BulletController();

    //Shared game objects (shared between player and enemies)
    private ProjectileArc projectileArc;
    private GameObject cannonFireSmoke;
    private GameObject targetCursor;

    private Vector3 currentPointPosition;
    private float currentSpeed;

    private Vector3 mockNullVector3 = new Vector3(-999f, -999f, -999f);

    public PlayerController(GameObject player, List<GameObject> enemies, PlayerConfigurationService playerConfigurationService, PendulumEffectController pendulumEffectController, 
        PublicGameObjects publicGameObjects, CameraManager cameraManager)
    {
        this.cameraManager = cameraManager;
        this.cannonFireSmoke = publicGameObjects.cannonFireSmoke;
        this.projectileArc = publicGameObjects.projectileArc;
        this.targetCursor = publicGameObjects.targetCursor;
        this.pendulumEffectController = pendulumEffectController;
        this.playerConfigurationService = playerConfigurationService;
        this.publicGameObjects = publicGameObjects;

        new ProjectionManager(player, enemies);

        foreach (GameObject go in enemies)
        {
            var enemyChildren = go.GetComponentsInChildren<Transform>().ToList();
            var enemyFirePoints = enemyChildren.FindAll(e => e.tag.Equals(FIRE_POINT_TAG));
            var enemyTurret = enemyChildren.Find(e => e.tag.Equals(TURRET_TAG));
            var enemyCannonBase = enemyChildren.Find(e => e.tag.Equals(CANNON_BASE_TAG));
            var enemyLastAimTargetPosition = calculateMidPoint(go.transform, player.transform, targetCursor.transform);

            var goBullets = bulletController.initializePlayerBullets(go);
            //max speed and angle shoot be taken from conf
            var enemyMetadata = new PlayerMetadata(go, enemyCannonBase, enemyTurret, goBullets, enemyFirePoints, 15f, 100f, 45f, 0f, enemyLastAimTargetPosition, goBullets.selectedShootingMode);
            playermetadataDict.Add(go.GetInstanceID(), enemyMetadata);
        }

        var playerChildren = player.GetComponentsInChildren<Transform>().ToList();
        var playerFirePoints = playerChildren.FindAll(e => e.tag.Equals(FIRE_POINT_TAG));
        var playerTurret = playerChildren.Find(e => e.tag.Equals(TURRET_TAG));
        var playerCannonBase = playerChildren.Find(e => e.tag.Equals(CANNON_BASE_TAG));
        var playerLastAimTargetPosition = calculateMidPoint(player.transform, enemies[0].transform, targetCursor.transform);

        var playerBullets = bulletController.initializePlayerBullets(player);
        var playerMetadata = new PlayerMetadata(player, playerCannonBase, playerTurret, playerBullets, playerFirePoints, playerConfigurationService.maxSpeed, 
            playerConfigurationService.maxUziSpeed, playerConfigurationService.FireAngle, 0, playerLastAimTargetPosition, playerBullets.selectedShootingMode);
        playermetadataDict.Add(player.GetInstanceID(), playerMetadata);

        changeActiveUser(player);

    }

    public void changeTargetCursorActivity(bool active)
    {
        targetCursor.SetActive(active);
        projectileArc.gameObject.SetActive(active);
        publicGameObjects.shootButton.interactable = true;
    }

    public void changeActiveUser(GameObject activeUser)
    {
        PlayerMetadata playerMetadata;
        playermetadataDict.TryGetValue(activeUser.GetInstanceID(), out playerMetadata);

        projectileArc.transform.SetParent(playerMetadata.firePoints[0].transform);
        projectileArc.transform.localPosition = Vector3.zero;

        targetCursor.transform.position = playerMetadata.lastAimTargetPosition;
        setTargetWithAngle(activeUser, playerMetadata.lastAimTargetPosition);
    }

    public void setTargetWithAngle(GameObject activeUser, Vector3 point)
    {
        PlayerMetadata playerMetadata;
        playermetadataDict.TryGetValue(activeUser.GetInstanceID(), out playerMetadata);

        if (mockNullVector3.Equals(point))
        {
            point = playerMetadata.lastAimTargetPosition;
        }
        else
        {
            playerMetadata.lastAimTargetPosition = point;
        }

        setTurret(playerMetadata, point);

        var maxSpeed = getSpeedDependsOnMode(playerMetadata, TargetInterface.shootingMode);
        var angle = getAngleDependsOnMode(playerMetadata, TargetInterface.shootingMode);

        //TODO: for now first element. Please remember to fix when there is tank with multiple burrels
        Vector3 direction = point - playerMetadata.firePoints[0].position;
        float yOffset = -direction.y;
        direction = Math3d.ProjectVectorOnPlane(Vector3.up, direction);
        float distance = direction.magnitude;

        currentSpeed = ProjectileMath.LaunchSpeed(distance, yOffset, Physics.gravity.magnitude, angle * Mathf.Deg2Rad);

        if (currentSpeed > maxSpeed)
        {
            point = currentPointPosition;

            //TODO: for now first element. Please remember to fix when there is tank with multiple burrels
            direction = point - playerMetadata.firePoints[0].position;
            yOffset = -direction.y;
            direction = Math3d.ProjectVectorOnPlane(Vector3.up, direction);
            distance = direction.magnitude;

            currentSpeed = ProjectileMath.LaunchSpeed(distance, yOffset, Physics.gravity.magnitude, angle * Mathf.Deg2Rad);
        }

        //targetProjectorService.updateRingsSize(distance);

        projectileArc.UpdateArc(currentSpeed, distance, Physics.gravity.magnitude, angle * Mathf.Deg2Rad, direction, true);
        
        currentTimeOfFlight = ProjectileMath.TimeOfFlight(currentSpeed, angle * Mathf.Deg2Rad, yOffset, Physics.gravity.magnitude);

        currentPointPosition = point;
    }

    public void setTargetWithSpeed(GameObject activeUser, Vector3 point, bool useLowAngle)
    {
        PlayerMetadata playerMetadata;
        playermetadataDict.TryGetValue(activeUser.GetInstanceID(), out playerMetadata);

        if(mockNullVector3.Equals(point))
        {
            point = playerMetadata.lastAimTargetPosition;
        } else
        {
            playerMetadata.lastAimTargetPosition = point;
        }
        Vector3 pointWithYOffset = new Vector3(point.x, point.y + 0.5f, point.z);

        currentSpeed = playerMetadata.maxUziSpeed;
        var currentAngle = 0f;

        
        //TODO: for now first element. Please remember to fix when there is tank with multiple burrels
        Vector3 direction = pointWithYOffset - playerMetadata.firePoints[0].position;
        float yOffset = direction.y;
        direction = Math3d.ProjectVectorOnPlane(Vector3.up, direction);
        float distance = direction.magnitude;

        float angle0, angle1;
        bool targetInRange = ProjectileMath.LaunchAngle(currentSpeed, distance, yOffset, Physics.gravity.magnitude, out angle0, out angle1);

        if (targetInRange)
            currentAngle = useLowAngle ? angle1 : angle0;

        playerMetadata.uziAngle = currentAngle * Mathf.Rad2Deg;
        setTurret(playerMetadata, pointWithYOffset);

        targetCursor.transform.position = Vector3.Lerp(targetCursor.transform.position, point, Time.deltaTime * 25f);

        projectileArc.UpdateArc(currentSpeed, distance, Physics.gravity.magnitude, currentAngle, direction, targetInRange);

        currentTimeOfFlight = ProjectileMath.TimeOfFlight(currentSpeed, currentAngle, -yOffset, Physics.gravity.magnitude);

        currentPointPosition = point;
        //Debug.Log("Current Speed: " + currentSpeed + " | Current angle: " + currentAngle + " | Current point: " + currentPointPosition );

        // Debug.Log("Current Speed: " + currentSpeed + " | Current angle: " + currentAngle);
    }

    private void setTurret(PlayerMetadata playerMetadata, Vector3 touchPoint)
    {
        var angle = getAngleDependsOnMode(playerMetadata, TargetInterface.shootingMode);

        Vector3 direction = touchPoint - playerMetadata.player.transform.position;
        direction = Math3d.ProjectVectorOnPlane(Vector3.up, direction);
        playerMetadata.cannonBase.rotation = Quaternion.LookRotation(direction) * Quaternion.Euler(0, 0, 0);
        playerMetadata.turret.localRotation = Quaternion.Euler(0, 0, 0) * Quaternion.AngleAxis(angle, new Vector3(-1, 0, 0));
    }


    private float getAngleDependsOnMode(PlayerMetadata playerMetadata, TargetInterface.SHOOTING_MODE mode)
    {
        if (TargetInterface.SHOOTING_MODE.UZI.Equals(mode))
        {
            return playerMetadata.uziAngle;
        }
        return playerMetadata.angle;
    }

    private float getSpeedDependsOnMode(PlayerMetadata playerMetadata, TargetInterface.SHOOTING_MODE mode)
    {
        if (TargetInterface.SHOOTING_MODE.UZI.Equals(mode))
        {
            return playerMetadata.maxUziSpeed;
        }
        return playerMetadata.maxSpeed;
    }

    public void fire(GameObject activeUser)
    {
        PlayerMetadata playerMetadata;
        playermetadataDict.TryGetValue(activeUser.GetInstanceID(), out playerMetadata);

        if (Time.time > lastShotTime + FIRE_COOLDOWN_IN_SECONDS)
        {
            //TODO: remove comment
            if (TargetInterface.SHOOTING_MODE.UZI.Equals(TargetInterface.shootingMode))
            {
                updateSpeedAndCurrentBaseOnAimingInUziMode(playerMetadata, currentPointPosition);
            } else
            {
                updateSpeedAndCurrentBaseOnAiming(playerMetadata, currentPointPosition);
            }

            GameManager.getInstance.StartCoroutine(spawnBullets(5, playerMetadata));
            //GameObject p = GameObject.Instantiate(playerMetadata.bullet, playerMetadata.firePoints[0].position, Quaternion.identity);
            //p.GetComponent<Rigidbody>().velocity = playerMetadata.turret.forward * currentSpeed;
            //cameraManager.setBulletToFollow(p.transform);

            //GameObject.Instantiate(cannonFireSmoke, playerMetadata.firePoints[0].position, Quaternion.LookRotation(playerMetadata.turret.forward));

            lastShotTime = Time.time;
            lastShotTimeOfFlight = currentTimeOfFlight;

            //anim.Rewind();
            //anim.Play();

            changeTargetCursorActivity(false);
        }
    }

    private IEnumerator spawnBullets(int numberOfBullets, PlayerMetadata playerMetadata)
    {
        for(int i=0; i< numberOfBullets; i++)
        {
            CameraEffects.Shake(playerMetadata.firePoints[0].position);
            CameraEffects.Shaker.setEnable(true);
            GameObject p = GameObject.Instantiate(bulletController.getBullet(playerMetadata.player), playerMetadata.firePoints[0].position, Quaternion.identity);
            p.transform.forward = playerMetadata.turret.forward;
            p.GetComponent<Rigidbody>().velocity = playerMetadata.turret.forward * currentSpeed;
            CannonBall cannonBall = p.GetComponent<CannonBall>();
            cannonBall.IgnoreSelf(playerMetadata.player);
            cannonBall.setTargetPoint(playerMetadata.lastAimTargetPosition);
            cameraManager.setBulletToFollow(p.transform);
            if(i == numberOfBullets -1)
            {
                GameManager.getInstance.changeState(GameManager.Stage.AFTER_SHOOT);
            }

            GameObject.Instantiate(cannonFireSmoke, playerMetadata.firePoints[0].position, Quaternion.LookRotation(playerMetadata.turret.forward));
            yield return new WaitForSeconds(0.5f);
        }

        yield return new WaitForSeconds(0f);
    }

    private void updateSpeedAndCurrentBaseOnAiming(PlayerMetadata playerMetadata, Vector3 point)
    {
        point += getShootPositionDelta(playerMetadata);

        setTurret(playerMetadata, point);

        Vector3 direction = point - playerMetadata.firePoints[0].position;
        float yOffset = -direction.y;
        direction = Math3d.ProjectVectorOnPlane(Vector3.up, direction);
        float distance = direction.magnitude;

        var angle = getAngleDependsOnMode(playerMetadata, TargetInterface.shootingMode);

        //Current speed should not be changed for uzi mode
        currentSpeed = ProjectileMath.LaunchSpeed(distance, yOffset, Physics.gravity.magnitude, angle * Mathf.Deg2Rad);
        currentTimeOfFlight = ProjectileMath.TimeOfFlight(currentSpeed, angle * Mathf.Deg2Rad, yOffset, Physics.gravity.magnitude);
    }

    private void updateSpeedAndCurrentBaseOnAimingInUziMode(PlayerMetadata playerMetadata, Vector3 point)
    {
        if (mockNullVector3.Equals(point))
        {
            point = playerMetadata.lastAimTargetPosition;
        }
        else
        {
            playerMetadata.lastAimTargetPosition = point;
        }
        point += getShootPositionDelta(playerMetadata);
        Vector3 pointWithYOffset = new Vector3(point.x, point.y + 0.5f, point.z);

        currentSpeed = playerMetadata.maxUziSpeed;
        var currentAngle = 0f;

        //TODO: for now first element. Please remember to fix when there is tank with multiple burrels
        Vector3 direction = point - playerMetadata.firePoints[0].position;
        float yOffset = direction.y;
        direction = Math3d.ProjectVectorOnPlane(Vector3.up, direction);
        float distance = direction.magnitude;

        float angle0, angle1;
        bool targetInRange = ProjectileMath.LaunchAngle(currentSpeed, distance, yOffset, Physics.gravity.magnitude, out angle0, out angle1);

        if (targetInRange)
            currentAngle = angle1;

        playerMetadata.uziAngle = currentAngle * Mathf.Rad2Deg;
        setTurret(playerMetadata, pointWithYOffset);

        currentTimeOfFlight = ProjectileMath.TimeOfFlight(currentSpeed, currentAngle, -yOffset, Physics.gravity.magnitude);
    }

    private Vector3 getShootPositionDelta(PlayerMetadata playerMetadata)
    {
        var z = (pendulumEffectController.getHorizontalLinePositionInPercentage() * playerConfigurationService.whiteProjectorSize) * playerConfigurationService.targetSizeFactor / 2f;
        var x = (pendulumEffectController.getVerticalLinePositionInPercentage() * playerConfigurationService.whiteProjectorSize) * playerConfigurationService.targetSizeFactor / 2f;

        Vector3 deltaToAdd = new Vector3(x, 0, z);
        if (TargetInterface.SHOOTING_MODE.UZI.Equals(TargetInterface.shootingMode))
        {
            z = Mathf.Abs(z);
            deltaToAdd = new Vector3(x, z, 0);
            Debug.Log("DELTA: " + deltaToAdd);
        }

        return Quaternion.Euler(0, playerMetadata.cannonBase.rotation.eulerAngles.y, 0) * deltaToAdd;
    }

    private Vector3 calculateMidPoint(Transform from, Transform to, Transform targetCursor)
    {
        return new Vector3((from.position.x + to.position.x) / 2f, targetCursor.position.y, (from.position.z + to.position.z) / 2f);
    }

    public void changeTargetShieldView()
    {
        TargetInterface.SHOOTING_MODE shootingMode = playermetadataDict[GameManager.getInstance.getActivePlayer().GetInstanceID()].lastShootingMode;
        switch (shootingMode)
        {
            case TargetInterface.SHOOTING_MODE.CANON:
                GameManager.getInstance.buttonController.TurnOnCanonMode();
                break;
            //TODO: change if needed
            case TargetInterface.SHOOTING_MODE.UZI:
            case TargetInterface.SHOOTING_MODE.MINI_ROCKET:
                GameManager.getInstance.buttonController.TurnOnUziMode();
                break;
        }
    }

    public PlayerMetadata getPlayerMetadata(int gameObjectID)
    {
        return playermetadataDict[gameObjectID];
    }
}
