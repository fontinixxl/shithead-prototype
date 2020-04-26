using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public GameObject[] layoutZones;
    public GameObject blindZone;
    public Transform handTransform;
    [HideInInspector] public bool movementDone = false;
    public DropZone dropZone;
    // Debug (private)
    public int currentActiveLayoutIndex;

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
        DeactivateCurrentLayout();
        currentActiveLayoutIndex++;
        // After the third layout, set back to 0 using module
        currentActiveLayoutIndex %= layoutZones.Length;
    }

    private void SelectActiveLayout()
    {
        // If there isn't a card to play in the currernt layout
        if (!IsThereAnyCardLeft())
        {
            // Recursive exit condition; stop when reaching the last loyout
            if (currentActiveLayoutIndex == (layoutZones.Length - 1))
                return;
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
        Debug.Log("Start PlayRound()");
        movementDone = false;
        SelectActiveLayout();

        if (!MovementAvailable())
        {
            PickUpPile();
            Debug.Log("Picking up Pile");
            yield break;
        }

        // Wait for the player to play a card
        while(!HasPlayedCards())
        {
            if (!IsThereAnyCardLeft())
                SelectActiveLayout();

            yield return null;
        }

        GameObject previousZone = GetActiveLayout();
        Debug.Log(" PlayRound: previousZone = " + previousZone.name);

        // Every time the player drop a card, the active layout will be the Hand
        ResetActiveLayout();

        // check again if I have to pick up cards
        if (previousZone == blindZone && IsThereAnyCardLeft())
        {
            PickUpPile();
            Debug.Log("Picking up pile");
        }

        Debug.Log("End PlayRound()");
    }


    private bool MovementAvailable()
    {
        // In the blind zone the rank it's not know so the movement will be allways allowed.
        if (GetActiveLayout() == blindZone)
            return true;

        Card lastCardDropped = dropZone.GetLastCardOnPile();

        // If there isn't any card on the pile (the player can put any card)
        if (lastCardDropped == null)
            return true;

        foreach (Card card in GetCardsFromActiveLayout())
        {
            if ((card.GetRank() >= lastCardDropped.GetRank())) 
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
        currentActiveLayoutIndex = 0;
        foreach (Card card in dropZone.GetPileOfCards())
        {
            Transform cardTransform = card.GetComponent<Transform>();
            cardTransform.SetParent(handTransform);
            // Make the card draggable
            card.GetComponent<CanvasGroup>().blocksRaycasts = true;
            // Rotate the card back to 0 degrees.
            cardTransform.rotation = Quaternion.identity;
            card.ModifyAlphaColor(1f);
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

    private GameObject GetActiveLayout()
    {
        return layoutZones[currentActiveLayoutIndex];
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

    public int GetTotalCardsOnHand()
    {
        return handTransform.GetComponentsInChildren<Card>().Length;
    }

}
