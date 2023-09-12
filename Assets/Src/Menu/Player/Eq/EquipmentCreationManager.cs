using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EquipmentCreationManager
{
    public EquipmentCreationManager() { }

    public void initializeSlotsInEquipments(int slotsQuantity, Transform equipmentContent)
    {
        for (int i = 0; i < slotsQuantity; i++)
        {
            var slotPrefab = Utilities.getPrefabFromResources("Menu/Slot");
            slotPrefab = GameObject.Instantiate(slotPrefab, Vector3.zero, Quaternion.identity, equipmentContent);

            slotPrefab.name = "Slot";
            slotPrefab.transform.localPosition = Vector3.zero;
        }
    }

    public void createNewSlotsForEquipements(Transform content, List<Transform> slots)
    {
        for (int i = 0; i < 2; i++)
        {
            GameObject newSlotGO = GameObject.Instantiate(Utilities.getPrefabFromResources("Menu/Slot"), Vector3.zero, Quaternion.identity, content);
            newSlotGO.name = "Slot";
            newSlotGO.transform.localPosition = Vector3.zero;

            GameObject emptyItem = GameObject.Instantiate(Utilities.getPrefabFromResources("Menu/Items/Empty_Item"), Vector3.zero, Quaternion.identity, newSlotGO.transform);
            emptyItem.transform.localPosition = Vector3.zero;
            emptyItem.AddComponent<DragService>();
            
            slots.Add(newSlotGO.transform);
        }
    }
}
