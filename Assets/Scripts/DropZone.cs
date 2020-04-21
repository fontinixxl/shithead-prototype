using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DropZone : MonoBehaviour, IDropHandler
{
    [HideInInspector] public Card lastCard;
    [HideInInspector] public List<Card> pile;
    public Transform hand;
    public Transform blindZone;

    public void Awake()
    {
        lastCard = null;
        pile = new List<Card>();
    }

    public void OnDrop(PointerEventData eventData)
    {

        // Get the card object from the eventData (the object the player is dragging to the dropZone)
        Card card = eventData.pointerDrag.GetComponent<Card>();
        if (card != null)
        {
            GameManager.instance.GetCurrentTurnPlayer().cardDrop = true;
            // Test rules: if the card's rank is less than the last placed card return
            if (lastCard != null &&  card.rank < lastCard.rank)
            {
                // TODO: Improve
                // If the card is comming from the Blind Zone, and it doesn't fit to the pile,
                // we have to fake the original parent so the card will go to the Hand zone instead
                // of going back to the blind zone (as it would be by default)
                if (card.parentToReturnTo == blindZone)
                {
                    card.parentToReturnTo = card.originalParent = hand;
                }

                return;
            }

            Debug.Log("Card " + card.GetRank().ToString() + " Dropped");
            lastCard = card;
            pile.Add(card);

            // Set the parentToReturnto the "DragZone" (so it doesn't get back to the HandZone)
            // This is because the Draggable's OnEndDrag() method, by default, resets the parent 
            // to the original (the one it had when OnBeginDrag() was firts called)
            card.parentToReturnTo = transform;
            // Set the card object position the the dragZone so all the card object
            // will stack on it; on on the top of each other.
            card.transform.position = transform.position;

            GameManager.instance.GetCurrentTurnPlayer().cardDrop = true;
            //Debug.Log(GameManager.instance.GetCurrentTurnPlayer().HasDroppedCard());
        }

    }

    // Reset the pile after the player has picked up all the pile.
    public void ResetPile()
    {
        gameObject.SetActive(false);
        gameObject.SetActive(true);
    }

    // Will be called when the gameObject is deactivated
    public void OnDisable()
    {
        lastCard = null;
        pile.Clear();
    }
}
