using UnityEngine;

public class PlayerConfigurationService : MonoBehaviour
{
    public float maxSpeed;
    public float maxUziSpeed;
    public float whiteProjectorSize;
    public float blackProjectorSize;
    public float blueProjectorSize;
    public float redProjectorSize;
    public float yellowProjectorSize;
    public float projectorSizeMultiplier;
    public float targetSizeFactor;
    public float triggerBallSize;
    public float increaseSizeWithDistanceFactor;
    public float speedOfAiming;
    public float horizontalAndVerticalLineSpeed;
    public float returnCameraToPlayerTime;
    public float cameraSpeed;
    public float FireSpeed;
    public float FireAngle;
    public int windStrengh;
    [Range(-1, 1)] public int directionOfWindX;
    [Range(-1, 1)] public int directionOfWindZ;

    void Start()
    {
        //Load variables from tank configuration/user-prefabs
    }

}
