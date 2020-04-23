using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DropZone : MonoBehaviour, IDropHandler
{
    public Transform hand;
    public Transform blindZone;
    private List<Card> pile;
    // TODO: temporal just to check the value
    public Card lastCard;

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
            // Test rules: if the card's rank is less than the last placed card return
            // TODO: Replace with compareTo() (not implemented yet).
            if (lastCard != null &&  card.GetRank() < lastCard.GetRank())
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

            pile.Add(card);

            // Set the parentToReturnto the "DragZone" (so it doesn't get back to the HandZone)
            // This is because the Draggable's OnEndDrag() method, by default, resets the parent 
            // to the original (the one it had when OnBeginDrag() was firts called)

            card.parentToReturnTo = transform;
            // Set the card object position the the dragZone so all the card object
            // will stack on it; on on the top of each other.

            card.transform.position = transform.position;

            card.transform.SetParent(transform);

            bool movementDone = true;
            // Test: Special cards
            switch (card.GetRank())
            {
                case CardValue.Rank.Two:
                    lastCard = null;
                    break;
                case CardValue.Rank.Three:
                    // If it's a three the previous one will stay as last card to count for the next player
                    // Make it transparent so the player can see the previous card.
                    card.ModifyAlphaColor(0.4f);
                    break;
                case CardValue.Rank.Ten:
                    // Reset pile
                    Invoke("BurnPile", 1.0f);
                    // If it's a 10, the player takes another turn
                    movementDone = false;
                    break;
                default:
                    lastCard = card;
                    break;
            }

            GameManager.instance.GetCurrentTurnPlayer().movementDone = movementDone;
        }

    }

    private void BurnPile()
    {
        gameObject.SetActive(false);
        foreach (Transform child in transform)
        {
            if (child.GetComponent<Card>() == null)
                continue;

            child.gameObject.SetActive(false);
        }

        gameObject.SetActive(true);

    }

    // Reset the pile after the player has picked up all the pile.
    public void ResetPile()
    {
        gameObject.SetActive(false);
        gameObject.SetActive(true);
    }

    public List<Card> GetPileOfCards()
    {
        return pile;
    }

    public Card GetLastCardOnPile()
    {
        return lastCard;
    }

    // Will be called when the gameObject is deactivated
    public void OnDisable()
    {
        lastCard = null;
        pile.Clear();
    }
}
