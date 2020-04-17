using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public CanvasGroup[] layoutZones;
    public bool cardDrop = false;
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
        } else 
        {
            layoutZones[currentActiveLayoutIndex].blocksRaycasts = true;
        }
    }

    // Returns whether there is a any card left in the current layoutZone
    private bool IsThereAnyCardLeft()
    {
        Card[] cards = layoutZones[currentActiveLayoutIndex].GetComponentsInChildren<Card>();
        return (cards.Length <= 0) ? false : true;
    }

    public IEnumerator PlayRound()
    {
        cardDrop = false;
        SelectActiveLayout();
        while(!cardDrop)
        {
            yield return null;
        }

    }

    // Update is called once per frame
    void Update()
    {
    }


}
