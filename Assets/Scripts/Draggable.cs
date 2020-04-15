using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Draggable : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [HideInInspector] public Transform parentToReturnTo;

    public float minCardRotation = -15.0f;
    public float maxCardRotation = 15.0f;

    private HorizontalLayoutGroup layout;

    public void OnBeginDrag(PointerEventData eventData)
    {
        //Debug.Log("OnBeginDrag");

        // Save the original parent
        parentToReturnTo = transform.parent;
        // Once we begin dragging, change the current parent to the Canvas (parent->parent)
        // This will make the panel re-arrange the current items and deatach the object from it.
        transform.SetParent(transform.parent.parent);

        // Apply a random rotation on the Z axi as soon as we start draggin the card.
        transform.Rotate(0.0f, 0.0f, Random.Range(minCardRotation, maxCardRotation));

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

        // If at this point the parent of the Draggable is the Hand
        // meaning we haven't dropped the card to the DropZone, we set the rotation back to 0.
        // TODO: Refactor-> look for a better way to dettect that the card is back to the Hand
        if (transform.parent.name == "Hand")
        {
            transform.rotation = Quaternion.identity;
        }

        // Set back to the original value; to block Raycasts
        // TODO: Change that
        if (parentToReturnTo.name != "DropZone")
        GetComponent<CanvasGroup>().blocksRaycasts = true;
    }
}
