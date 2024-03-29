﻿using System.Collections.Generic;
using UnityEngine;

public class Deck: MonoBehaviour
{
    public Card cardPrefab;
    [HideInInspector]
    public List<Card> cards;
    public Transform spawnPoint;
    public Color[] suitColors;
    public string[] RankLables = new string[CardValue.MAX_RANKS + 1];

    private static readonly int MAX_RANKS = 13;
    private static readonly int SUIT_TYPES = 4;

    public void Awake()
    {
        spawnPoint = GetComponent<Transform>();
        cards = new List<Card>();

    }

    public void Initialize()
    {
        Generate();
        Shuffle();
        PlaceCardsOnTheDeckZone();
    }

    public void Generate()
    {
        for (int i = 0; i < SUIT_TYPES; i++)
        {
            for (int j = 1; j < MAX_RANKS + 1; j++)
            {
                Card card = Instantiate(cardPrefab, spawnPoint.position, Quaternion.identity);

                card.SetRankAndSpite((CardValue.Rank)j, (CardValue.Suit)i, suitColors[i], RankLables[j]);
                cards.Add(card);
            }

            // DEBUG
            if (GameManager.instance.debugMode)
                return;
        }
    }

    public void Shuffle()
    {
        System.Random random = new System.Random();

        for (int i = 0; i < cards.Count; i++)
        {
            int j = random.Next(i, cards.Count);
            Card temporary = cards[i];
            cards[i] = cards[j];
            cards[j] = temporary;
        }
    }

    public void PlaceCardsOnTheDeckZone()
    {
        for (int i = 0; i < cards.Count; i++)
        {
            cards[i].GetComponent<Transform>().SetParent(spawnPoint);
        }
    }

    public Card Draw()
    {
        int lastCard = cards.Count - 1;

        Card drew = cards[lastCard];
        cards.RemoveAt(lastCard);

        return drew;
    }

    // TODO: Consider keeping this method; maybe I can use DrawCards outside in a Loop.
    public Card[] DrawCards(int amount)
    {
        Card[] drew = new Card[amount];
        int lastCard = 0;

        for (int i = 0; i < amount; i++)
        {
            lastCard = cards.Count - 1;
            drew[i] = cards[lastCard];
            cards.RemoveAt(lastCard);
        }

        return drew;
    }

    public bool IsEmpty()
    {
        return cards.Count > 0 ? false : true;
    }
}
