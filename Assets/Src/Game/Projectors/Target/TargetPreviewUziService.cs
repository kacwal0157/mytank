using System.Collections.Generic;
using UnityEngine;

public static class TargetPreviewUziService
{
    private static GameObject targetPreviewUzi;

    public static void init(PlayerConfigurationService playerConfigurationService, GameObject targetPreviewUzi)
    {
        TargetPreviewUziService.targetPreviewUzi = targetPreviewUzi;
        targetPreviewUzi.transform.localScale = getTragetPreviewSize(playerConfigurationService.targetSizeFactor, playerConfigurationService.whiteProjectorSize);
    }

    private static Vector3 getTragetPreviewSize(float factor, float shieldSize)
    {
        return new Vector3(0.2f, shieldSize * factor, shieldSize * factor);
    }

    public static void setAngle(Vector3 playerPosition)
    {
        if (TargetInterface.shootingMode.Equals(TargetInterface.SHOOTING_MODE.UZI))
        {
            Vector3 direction = targetPreviewUzi.transform.position - playerPosition;
            direction = Math3d.ProjectVectorOnPlane(Vector3.up, direction);
            Quaternion temp = Quaternion.LookRotation(direction) * Quaternion.Euler(0, 0, 0);
            targetPreviewUzi.transform.rotation = Quaternion.Euler(temp.eulerAngles.x, temp.eulerAngles.y + 90, temp.eulerAngles.z);
        }
        
    }
}
