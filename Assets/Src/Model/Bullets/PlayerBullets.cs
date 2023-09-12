using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBullets
{
    public enum BULLET_TYPE {UZI, CANON, MINI_ROCKET}
    public enum BULLET_SUBTYPE { NORMAL, ICE, GLUE, ELECTRIC, PLASMA}
    public List<TargetInterface.SHOOTING_MODE> availableShootingModes;
    public List<BULLET_TYPE> availableBulletTypes;

    public BULLET_TYPE selectedBulletType;
    public BULLET_SUBTYPE selectedBulletSubType;
    public TargetInterface.SHOOTING_MODE selectedShootingMode { get; set; }

}
