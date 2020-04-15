using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Draggable : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [HideInInspector] public Transform parentToReturnTo;

    public void OnBeginDrag(PointerEventData eventData)
    {
        //Debug.Log("OnBeginDrag");

        // Save the original parent
        parentToReturnTo = transform.parent;
        // Once we begin dragging, change the current parent to the Canvas (parent->parent)
        // This will make the panel re-arrange the current items and deatach the object from it.
        transform.SetParent(transform.parent.parent);

        // In order to dettect the "Drop Zone" we need to have the Raycast on (off by default)
        GetComponent<CanvasGroup>().blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        // Set the parent back to the original in case the object hasn't been dropped properly.
        transform.SetParent(parentToReturnTo);

        // Set back to the original value; to block Raycasts
        GetComponent<CanvasGroup>().blocksRaycasts = true;
    }
}
