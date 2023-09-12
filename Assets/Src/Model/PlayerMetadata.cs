using System.Collections.Generic;
using UnityEngine;

public class PlayerMetadata
{
    public GameObject player { get; set; }
    public Transform cannonBase { get; set; }
    public Transform turret { get; set; }
    public PlayerBullets playerBullets { get; set; }
    public List<Transform> firePoints { get; set; }
    public float maxSpeed { get; set; }

    public float maxUziSpeed { get; set; }
    public float angle { get; set; }

    public float uziAngle { get; set; }
    public Vector3 lastAimTargetPosition { get; set; }

    public TargetInterface.SHOOTING_MODE lastShootingMode { get; set; }

    public PlayerMetadata(GameObject player, Transform cannonBase, Transform turret, PlayerBullets playerBullets, List<Transform> firePoints, 
        float maxSpeed, float maxUziSpeed, float angle, float uziAngle, Vector3 lastAimTargetPosition, TargetInterface.SHOOTING_MODE lastShootingMode)
    {
        this.player = player;
        this.cannonBase = cannonBase;
        this.turret = turret;
        this.playerBullets = playerBullets;
        this.firePoints = firePoints;
        this.maxSpeed = maxSpeed;
        this.maxUziSpeed = maxUziSpeed;
        this.angle = angle;
        this.uziAngle = uziAngle;
        this.lastAimTargetPosition = lastAimTargetPosition;

        this.lastShootingMode = lastShootingMode;
    }
}
