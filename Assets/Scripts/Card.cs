using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Card : Draggable
{
    public Color blindColor;
    public Color normalColor;

    // Update is called once per frame
    void Update()
    {

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
        GetComponent<Image>().color = blindColor;
        Text[] rankTexts = GetComponentsInChildren<Text>();
        for (int i = 0; i < rankTexts.Length; i++)
        {
            rankTexts[i].enabled = false;
        }
    }
}
