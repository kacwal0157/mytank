using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DragService : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler, IPointerDownHandler
{
    [HideInInspector] public CanvasGroup canvasGroup;
    [HideInInspector] public RectTransform dragRectTransform;
    [HideInInspector] public List<Transform> itemSlotList;
    [HideInInspector] public ScrollRect activeScrollRect;

    private PublicGameObjects publicGameObjects;
    private Canvas canvas;
    private Vector3 startPosition = new Vector3();

    public void Start()
    {
        publicGameObjects = GameObject.Find("InspectorGameObjects").GetComponent<PublicGameObjects>();
        canvas = publicGameObjects.canvas;

        dragRectTransform = gameObject.GetComponent<RectTransform>();
        canvasGroup = gameObject.GetComponent<CanvasGroup>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        startPosition = dragRectTransform.transform.position;
        canvasGroup.alpha = .6f;
        canvasGroup.blocksRaycasts = false;
        activeScrollRect.enabled = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        dragRectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
    }
    
    public void OnEndDrag(PointerEventData eventData)
    {
        int inSlotCounter = 0;
        foreach(Transform itemSlot in itemSlotList)
        {
            if (itemSlot.GetComponent<ItemSlotHandler>().inSlot)
            {
                inSlotCounter++;
            }
        }
        
        if(inSlotCounter == 0)
        {
            StartCoroutine(waitSomeTime(0, 2f));
        }

        canvasGroup.alpha = 1f;
        canvasGroup.blocksRaycasts = true;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        foreach(Transform itemSlot in itemSlotList)
        {
            itemSlot.GetComponent<ItemSlotHandler>().inSlot = false;
        }
    }

    private IEnumerator waitSomeTime(float elapsedTime, float waitTime)
    {
        while (elapsedTime < waitTime)
        {
            dragRectTransform.transform.position = Vector3.Lerp(dragRectTransform.transform.position, startPosition, (elapsedTime / waitTime));
            elapsedTime += Time.deltaTime;

            if(transform.position == startPosition)
            {
                activeScrollRect.enabled = true;
                yield break;
            }

            yield return null;
        }

        transform.position = startPosition;
        yield return null;
    }

    public void setParentAndPosition(RectTransform itemSlotChild)
    {
        itemSlotChild.SetParent(this.gameObject.transform.parent);
        itemSlotChild.anchoredPosition = Vector2.zero;
        activeScrollRect.enabled = true;
    }
}
