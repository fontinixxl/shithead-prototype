using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public GameObject[] layoutZones;
    public Transform handTransform;
    public bool movementDone = false;
    public DropZone dropZone;
    private int currentActiveLayoutIndex;

    private void Awake()
    {
        // Register this player with our instance of GameManager
        // This allows the GameManager to issue commands.
        GameManager.instance.RegisterNewPlayer(this);
    }

    // Start is called before the first frame update
    void Start()
    {
        currentActiveLayoutIndex = 0;
    }

    // Turn off the Raycast of the currentActive Layout and set it back to the hand
    private void ResetActiveLayout()
    {
        DeactivateCurrentLayout();
        currentActiveLayoutIndex = 0;
        ActivateCurrentLayout();
    }
    
    private void SetNextActiveLayout()
    {
        currentActiveLayoutIndex++;
        // After the third layout, set back to 0 using module
        currentActiveLayoutIndex %= layoutZones.Length;
    }

    private void SelectActiveLayout()
    {

        // If there isn't a card to play in the currernt layout
        if (!IsThereAnyCardLeft())
        {
            // Increment the current active layout index
            SetNextActiveLayout();
            // Check again (recursive) for the current active layout if there are cards left
            SelectActiveLayout();
        }

        // Activate the correct layout
        ActivateCurrentLayout();
    }

    // Returns whether there is any card left in the current layoutZone
    private bool IsThereAnyCardLeft()
    {
        Card[] cards = layoutZones[currentActiveLayoutIndex].GetComponentsInChildren<Card>();
        return (cards.Length <= 0) ? false : true;
    }
    public Card[] GetCardsFromActiveLayout()
    {
        return layoutZones[currentActiveLayoutIndex].GetComponentsInChildren<Card>();
    }

    public IEnumerator PlayRound()
    {
        movementDone = false;
        SelectActiveLayout();

        if (!MovementAvailable())
        {
            PickUpPile();
            yield break;
        }

        // Wait for the player to play a card
        while(!HasPlayedCards())
        {
            yield return null;
        }

        // Every time the player drop a card, the active layout will be the Hand
        ResetActiveLayout();

        // TODO: Once the player has put some cards, we have to draw from the deck (if some left)
    }


    private bool MovementAvailable()
    {
        Card lastCardDropped = dropZone.GetLastCardOnPile();

        // If there isn't any card on the pile (the player can put any card)
        // or current layout is the blind, movement always allowed ; movement available ->true
        if (lastCardDropped == null)
            return true;

        Card[] cards = GetCardsFromActiveLayout();
        if (cards == null)
            return false;

        foreach (Card card in cards)
        {
            if (card.IsBlind() || (card.GetRank() >= lastCardDropped.GetRank())) 
                return true;
        }

        // If none of the previous conditions is met, no movement available
        return false;
    }

    // Refactor this method; it is not optimized
    private void PickUpPile()
    {
        // Disable Dragging from the current layout, as after picking up cards the only
        // active layout must be the Hand
        DeactivateCurrentLayout();
        // Reset current active layout to be the hand
        // TODO: Look for a better way than hardcode the index of the public value...
        currentActiveLayoutIndex = 0;
        foreach (Card card in dropZone.GetPileOfCards())
        {
            // TODO: change how I get the Hand Zone
            Transform handZone = layoutZones[currentActiveLayoutIndex].GetComponent<Transform>();
            Transform cardTransform = card.GetComponent<Transform>();
            cardTransform.SetParent(handZone);
            // Make the card draggable
            card.GetComponent<CanvasGroup>().blocksRaycasts = true;
            // Rotate the card back to 0 degrees.
            cardTransform.rotation = Quaternion.identity;
        }

        // Reset the Pile
        dropZone.ResetPile();
    }

    // Loop through all the layouts and return whether there is any card left in them.
    public bool HasNoCardsLeft()
    {
        for (int i = 0; i < layoutZones.Length; i++)
        {
            if (layoutZones[i].GetComponentsInChildren<Card>().Length > 0)
                return false;
        }

        return true;
    }

    // Turn on current layout raycast so the player can drag cards on it.
    private void ActivateCurrentLayout()
    {
        layoutZones[currentActiveLayoutIndex].GetComponent<CanvasGroup>().blocksRaycasts = true;
    }

    // Turn off current layout raycast so the player can't drag cards on it.
    private void DeactivateCurrentLayout()
    {
        layoutZones[currentActiveLayoutIndex].GetComponent<CanvasGroup>().blocksRaycasts = false;
    }

    public bool HasPlayedCards()
    {
        return movementDone;
    }

}
