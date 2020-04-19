﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Card : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [HideInInspector] public Transform parentToReturnTo;

    public float minCardRotation = -15.0f;
    public float maxCardRotation = 15.0f;
    public Color blindColor;

    private Color normalColor;
    private GameObject placeholder;
    private Transform originalParent;
    private bool blind = false;
    private int rank;

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
        placeholder.transform.SetParent(transform.parent);
        // set active to false and activate just when required (when starting dragging the card)
        placeholder.SetActive(false);
    }

    public virtual void OnBeginDrag(PointerEventData eventData)
    {

        if (blind)
            FaceUpCard();

        // Save the original parent
        parentToReturnTo = originalParent = transform.parent;

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
        if (transform.parent == originalParent)
        {
            transform.rotation = Quaternion.identity;
            transform.SetSiblingIndex(placeholder.transform.GetSiblingIndex());
            // Allow raycast only if the card comes back to the hand again
            GetComponent<CanvasGroup>().blocksRaycasts = true;
        }

        // Once the card is back to the hand we can deactivate the placeholder until further use.
        placeholder.SetActive(false);
    }

    public void SetRankAndSpite(int rank, Color spite)
    {
        Text[] rankTexts = GetComponentsInChildren<Text>();
        for (int i = 0; i < rankTexts.Length; i++)
        {
            rankTexts[i].text = rank.ToString();
            rankTexts[i].color = spite;
        }
    }
    public void BlindCard()
    {
        blind = true;
        normalColor = GetComponent<Image>().color;
        GetComponent<Image>().color = blindColor;
        Text[] rankTexts = GetComponentsInChildren<Text>();
        for (int i = 0; i < rankTexts.Length; i++)
        {
            rankTexts[i].enabled = false;
        }
    }

    public void FaceUpCard()
    {
        GetComponent<Image>().color = normalColor;
        Text[] rankTexts = GetComponentsInChildren<Text>();
        for (int i = 0; i < rankTexts.Length; i++)
        {
            rankTexts[i].enabled = true;
        }
    }
}
