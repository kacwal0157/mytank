using UnityEngine;

public class CameraTargetMetadata
{
    public GameObject target { get; set; }
    public Vector3 lastCameraPosition { get; set; }
    public Quaternion lastCameraRotation { get; set; }

    public Vector3 startCameraPosition;
    public Quaternion startCameraRotation;

    public CameraTargetMetadata(GameObject target, Vector3 lastCameraPosition, Quaternion lastCameraRotation)
    {
        this.target = target;
        this.lastCameraPosition = lastCameraPosition;
        this.lastCameraRotation = lastCameraRotation;

        startCameraPosition = lastCameraPosition;
        startCameraRotation = lastCameraRotation;
    }
}
