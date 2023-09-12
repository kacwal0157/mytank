using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ItemSlotHandler : MonoBehaviour, IDropHandler
{
    [HideInInspector] public bool inSlot;

    public void OnDrop(PointerEventData eventData)
    {
        if(eventData.pointerDrag != null && !eventData.pointerDrag.name.Contains("ScrollRect") && !eventData.pointerDrag.name.Contains("Panel"))
        {
            var droppedObj = eventData.pointerDrag;
            var droppedDragService = droppedObj.GetComponent<DragService>();

            droppedDragService.setParentAndPosition(this.gameObject.transform.GetChild(0).GetComponent<RectTransform>());
            droppedObj.GetComponent<RectTransform>().anchoredPosition = GetComponent<RectTransform>().anchoredPosition;

            droppedDragService.dragRectTransform.SetParent(this.gameObject.transform);
            droppedDragService.dragRectTransform.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;

            inSlot = true;
        }
    }
}
