using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public CanvasGroup[] layoutZones;

    private int currentActiveLayoutIndex;

    // Start is called before the first frame update
    void Start()
    {
        // Register this player with our instance of GameManager
        // This allows the GameManager to issue commands.
        GameManager.instance.RegisterNewPlayer(this);

        currentActiveLayoutIndex = 0;
    }
    
    private void SetNextActiveLayout()
    {
        // Blocking/unblocking the Raycast element from the CanvasGroup component
        // allow to either drag the card or not.
        layoutZones[currentActiveLayoutIndex].blocksRaycasts = false;
        currentActiveLayoutIndex++;
        layoutZones[currentActiveLayoutIndex].blocksRaycasts = true;
    }

    public void PrepareLayoutToPlay()
    {
        Debug.Log("active Loyout!");

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
        return (cards.Length <= 0) ? true : false;
    }

    // Update is called once per frame
    void Update()
    {
        //foreach (CanvasGroup layout in layoutZones)
        //{
        //    Card[] cards = layout.GetComponentsInChildren<Card>();
        //    Debug.Log("Zone : " + layout.name + " has " + cards.Length + " cards");
        //}

        //if (layoutZones[currentActiveLayoutIndex].GetComponentsInChildren<Card>().Length <= 0)
        //{
        //    SetNextActiveLayout();
        //}
    }


}
