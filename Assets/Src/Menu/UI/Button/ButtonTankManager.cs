using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonTankManager : MonoBehaviour
{
    public PublicGameObjects publicGameObjects;
    private string defaultTank;

    void Start()
    {
        defaultTank = PlayerPrefs.GetString("TANK_PREFAB");

        foreach (GameObject tank in publicGameObjects.tanksInMenu)
        {
            if (tank.name.Equals(defaultTank))
            {
                tank.SetActive(true);
            }
            else
            {
                tank.SetActive(false);
            }
        }
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.transform.parent.name.Contains("Panzer") && hit.transform.parent.CompareTag("TankMenu"))
                {
                    publicGameObjects.tankListGO.SetActive(true);
                    this.gameObject.SetActive(false);
                }
            }
        }
    }
}
