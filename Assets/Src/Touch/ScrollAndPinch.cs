using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollAndPinch : MonoBehaviour
{
    [SerializeField]
    Camera cameraMain;

    [SerializeField]
    bool rotate;

    protected Plane plane; //used for calculations

    public float DecreaseCameraPanSpeed = 1f; //Default speed is 1
    public float CameraUpperHeightBound = 1f; //Zoom out
    public float CameraLowerHeightBound = 1f; //Zoom in

    private Vector3 cameraStartPosition;
    private Vector3 minimumPosition = new Vector3(-50f, -15f, -50f);
    private Vector3 maximumPosition = new Vector3(50f, -15f, 50f);

    private void Awake()
    {
        if (cameraMain == null)
            cameraMain = Camera.main;

        cameraStartPosition = cameraMain.transform.position;
    }

    public void ScrollAndPitchCamera()
    {
        //Update Plane
        if (Input.touchCount >= 1)
            plane.SetNormalAndPosition(transform.up, transform.position);

        var Delta1 = Vector3.zero;
        var Delta2 = Vector3.zero;

        //Scroll (Pan function)
        if (Input.touchCount >= 1)
        {
            //Get distance camera should travel
            Delta1 = PlanePositionDelta(Input.GetTouch(0)) / DecreaseCameraPanSpeed;
            float y = cameraMain.transform.position.y;
            if (Input.GetTouch(0).phase == TouchPhase.Moved)
                cameraMain.transform.Translate(Delta1, Space.World);

            Vector3 pos = cameraMain.transform.position;
            pos.x = Mathf.Clamp(pos.x, minimumPosition.x, maximumPosition.x);
            pos.y = y;
            pos.z = Mathf.Clamp(pos.z, minimumPosition.z, maximumPosition.z);

            cameraMain.transform.position = pos;
        }

        //Pinch
        if (Input.touchCount >= 2)
        {
            var pos1 = PlanePosition(Input.GetTouch(0).position);
            var pos2 = PlanePosition(Input.GetTouch(1).position);
            var pos1b = PlanePosition(Input.GetTouch(0).position - Input.GetTouch(0).deltaPosition);
            var pos2b = PlanePosition(Input.GetTouch(1).position - Input.GetTouch(1).deltaPosition);

            //calc zoom
            var zoom = Vector3.Distance(pos1, pos2) / Vector3.Distance(pos1b, pos2b);

            //edge case
            if (zoom == 0 || zoom > 10)
                return;

            //Move cam amount the mid ray
            Vector3 camPositionBeforeAdjustment = cameraMain.transform.position;
            cameraMain.transform.position = Vector3.LerpUnclamped(pos1, cameraMain.transform.position, 1 / zoom);

            //Restricts zoom height 

            //Upper (ZoomOut)
            if (cameraMain.transform.position.y > (cameraStartPosition.y + CameraUpperHeightBound))
            {
                cameraMain.transform.position = camPositionBeforeAdjustment;
                Debug.Log("OUT:" + cameraMain.transform.position.y);
            }
            //Lower (Zoom in)
            if (cameraMain.transform.position.y < (cameraStartPosition.y - CameraLowerHeightBound) || cameraMain.transform.position.y <= 1)
            {
                cameraMain.transform.position = camPositionBeforeAdjustment;
                Debug.Log("IN: " + cameraMain.transform.position.y);
            }

            //Rotation Function
            if (rotate && pos2b != pos2)
                cameraMain.transform.RotateAround(pos1, plane.normal, Vector3.SignedAngle(pos2 - pos1, pos2b - pos1b, plane.normal));
        }
    }

    protected Vector3 PlanePosition(Vector2 screenPos)
    {
        //position
        var rayNow = cameraMain.ScreenPointToRay(screenPos);
        if (plane.Raycast(rayNow, out var enterNow))
            return rayNow.GetPoint(enterNow);

        return Vector3.zero;
    }

    protected Vector3 PlanePositionDelta(Touch touch)
    {
        //not moved
        if (touch.phase != TouchPhase.Moved) return Vector3.zero;

        //delta
        var rayBefore = cameraMain.ScreenPointToRay(touch.position - touch.deltaPosition);
        var rayNow = cameraMain.ScreenPointToRay(touch.position);
        if (plane.Raycast(rayBefore, out var enterBefore) && plane.Raycast(rayNow, out var enterNow))
            return rayBefore.GetPoint(enterBefore) - rayNow.GetPoint(enterNow);

        //not on plane
        return Vector3.zero;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(transform.position, transform.position + transform.up);
    }
}
