using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public class Card : Draggable
{
    public Color blindColor;

    private Color normalColor;
    
    private bool blind = false;

    public override void OnBeginDrag(PointerEventData eventData)
    {
        if (blind)
            FaceUpCard();

        base.OnBeginDrag(eventData);
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
