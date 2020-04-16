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

    private GameObject placeholder;

    public void Start()
    {
        // Create a placeholder for the card to keep the place on the layout in case
        // we bring the card back to the hand and therefor keeping the original position.
        placeholder = new GameObject{name = "GhostCard"};
        RectTransform rt = placeholder.AddComponent<RectTransform>();
        rt.sizeDelta = GetComponent<RectTransform>().sizeDelta;
        // TODO: if I parent the card to only the transform the placeholder is not keeping
        // the card's position (as soon as we drag the card get rearranged) but if we release the
        // card it is getting back to the original position. Consider to implement this approach instead.
        placeholder.transform.parent = transform.parent;
        // set active to false and activate just when required (when starting dragging the card)
        placeholder.SetActive(false);
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        //Debug.Log("OnBeginDrag");

        // Save the original parent
        parentToReturnTo = transform.parent;

        // Set the index position on the grid for the ghost to be the same as the card.
        // Meaning once we drag the card there is gonna be a empty space at the same exact
        // position the original card was before dragging.
        placeholder.transform.SetSiblingIndex(transform.GetSiblingIndex());
        placeholder.SetActive(true);

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
        // Set the parent back the parent to the corresponding one, depending on where we have
        // dropped the card.
        transform.SetParent(parentToReturnTo);


        // If at this point the parent of the Draggable is the Hand
        // meaning we haven't dropped the card to the DropZone, we set the rotation back to 0.
        // and we position the card on the layout at the same place it was before dragging.
        // TODO: Refactor-> look for a better way to dettect that the card is back to the Hand
        if (transform.parent.name == "Hand")
        {
            transform.rotation = Quaternion.identity;
            transform.SetSiblingIndex(placeholder.transform.GetSiblingIndex());
        }

        // Once the card is back to the hand we can deactivate the placeholder until further use.
        placeholder.SetActive(false);

        // Set back to the original value; to block Raycasts
        // TODO: Change that
        if (parentToReturnTo.name != "DropZone")
            GetComponent<CanvasGroup>().blocksRaycasts = true;
    }
}
