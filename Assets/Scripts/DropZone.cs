using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DropZone : MonoBehaviour, IDropHandler
{

    //public Player player;

    public void Awake()
    {
        //player = GetComponentInParent<Player>();
    }

    public void OnDrop(PointerEventData eventData)
    {
        //Debug.Log(eventData.pointerDrag.name + " was dropped on " + gameObject.name);

        // Get the card object from the eventData (the object the player is dragging to the dropZone)
        Card card = eventData.pointerDrag.GetComponent<Card>();
        if (card != null)
        {
            // Set the parentToReturnto the "DragZone" (so it doesn't get back to the HandZone)
            // This is because the Draggable's OnEndDrag() method, by default, resets the parent 
            // to the original (the one it had when OnBeginDrag() was firts called)
            card.parentToReturnTo = transform;
            // Set the card object position the the dragZone so all the card object
            // will stack on it; on on the top of each other.
            card.transform.position = transform.position;

            GameManager.instance.GetCurrentTurnPlayer().cardDrop = true;
        }
    }
}
