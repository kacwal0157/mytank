using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateTankInMenuManager : MonoBehaviour
{
    private Vector3 mPrevPos = Vector3.zero;
    private Vector3 mPosDelta = Vector3.zero;
    private Quaternion defaultRot;

    void Start()
    {
        defaultRot = GetComponent<RectTransform>().transform.localRotation;
    }

    void Update()
    {
        if(Input.GetMouseButton(0))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if(Physics.Raycast(ray, out hit))
            {
                if(hit.transform.parent.name.Contains("Panzer") && !hit.transform.parent.CompareTag("TankMenu"))
                {
                    mPosDelta = Input.mousePosition - mPrevPos;
                    transform.Rotate(transform.up, -Vector3.Dot(mPosDelta, Camera.main.transform.right), Space.World);
                }
            }

            //If we want to rotate up and down someday
            /*if (Vector3.Dot(transform.up, Vector3.up) >= 0)
            {
                transform.Rotate(transform.up, -Vector3.Dot(mPosDelta, Camera.main.transform.right), Space.World);
            }
            else
            {
                transform.Rotate(transform.up, Vector3.Dot(mPosDelta, Camera.main.transform.right), Space.World);
            }

            transform.Rotate(Camera.main.transform.right, Vector3.Dot(mPosDelta, Camera.main.transform.up), Space.World);*/
        }

        mPrevPos = Input.mousePosition;
    }

    public void returnTankDefaultRot()
    {
        GetComponent<RectTransform>().transform.localRotation = defaultRot;
    }
}
