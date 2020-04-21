using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public CanvasGroup[] layoutZones;
    public bool cardDrop = false;
    public int currentActiveLayoutIndex;
    public DropZone dropZone;
    private bool emptyHand = false;
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
        Debug.Log("ResetActiveLayout");
        layoutZones[currentActiveLayoutIndex].blocksRaycasts = false;
        currentActiveLayoutIndex = 0;
        layoutZones[currentActiveLayoutIndex].blocksRaycasts = true;
    }
    
    private void SetNextActiveLayout()
    {
        // Blocking/unblocking the Raycast element from the CanvasGroup component
        // allow to either drag the card or not.
        layoutZones[currentActiveLayoutIndex].blocksRaycasts = false;
        currentActiveLayoutIndex++;
        // After the third layout, set back to 0 using module
        currentActiveLayoutIndex %= layoutZones.Length;
        layoutZones[currentActiveLayoutIndex].blocksRaycasts = true;
    }

    private void SelectActiveLayout()
    {

        // If there are no card to play in the curernt layout
        if (!IsThereAnyCardLeft())
        {
            // Set the next layout (in order) as active and deactivate the current one.
            SetNextActiveLayout();
            SelectActiveLayout();
        } else 
        {
            layoutZones[currentActiveLayoutIndex].blocksRaycasts = true;
        }
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
        cardDrop = false;
        SelectActiveLayout();
        Debug.Log("active layout index = " + currentActiveLayoutIndex);
        //Debug.Log("Movement allowed: " + MovementAvailable());
        if (!MovementAvailable())
        {
            PickUpPile();
            cardDrop = true;

        }
        while(!cardDrop)
        {
            yield return null;
        }

        ResetActiveLayout();
        // Once the player has put some cards, we have to draw from the deck (if some left)
    }


    private bool MovementAvailable()
    {
        Card lastCardDropped = dropZone.lastCard;

        // TODO: refactor
        // If there isn't any card on the pile (the player can put any card)
        // or current layout is the blind, movement always allowed ; movement available ->true
        if (lastCardDropped == null)
        {
            Debug.Log("No card in the pile: movement allowed");
            return true;

        }

        Card[] cards = GetCardsFromActiveLayout();
        if (cards == null)
        {
            Debug.Log("No card in the active Layout; movement available = false");
            return false;
        }


        foreach (Card card in cards)
        {
            if (card.IsBlind() || (card.GetRank() >= lastCardDropped.GetRank())) 
            {
                Debug.Log("Card Blind or card with a higher rank: movement allowed");
                return true;
            }
        }

        return false;
    }

    // Refactor this method; it is not optimized
    private void PickUpPile()
    {
        // Disable Dragging from the current layout, as after picking up cards the only
        // active layout must be the Hand
        layoutZones[currentActiveLayoutIndex].blocksRaycasts = false;
        // Reset current active layout to be the hand
        // TODO: Look for a better way than hardcode the index of the public value...
        currentActiveLayoutIndex = 0;
        foreach (Card card in dropZone.pile)
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

    public bool HasNoCardsLeft()
    {
        for (int i = 0; i < layoutZones.Length; i++)
        {
            if (layoutZones[i].GetComponentsInChildren<Card>().Length > 0)
                return false;
        }

        return true;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public bool HasDroppedCard()
    {
        return cardDrop;
    }

}
