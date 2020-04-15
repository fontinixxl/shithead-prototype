using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DropZone : MonoBehaviour, IDropHandler
{
    public int spaceingFactor = 9;

    private HorizontalLayoutGroup layout;

    public void Start()
    {
        layout =GetComponent<HorizontalLayoutGroup>();
    }
    public void OnDrop(PointerEventData eventData)
    {
        //Debug.Log(eventData.pointerDrag.name + " was dropped on " + gameObject.name);

        // Get the draggable object from the eventData (the object the player is draggin to the dropZone)
        Draggable draggable = eventData.pointerDrag.GetComponent<Draggable>();
        if (draggable != null)
        {
            // Set the parentToReturnto the "DragZone" (so it doesn't get back to the HandZone)
            // This is because the Draggable's OnEndDrag() method, by default, resets the parent 
            // to the original (the one it had when OnBeginDrag() was firts called)
            draggable.parentToReturnTo = transform;
            // Set the draggable object position the the dragZone so all the draggable object
            // will stack on it; on on the top of each other.
            draggable.transform.position = transform.position;
        }
        
        // If the dropZone we has a layout
        if (layout != null)
        {

            layout.spacing -= spaceingFactor;
        }
    }
}
