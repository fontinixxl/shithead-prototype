using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CardValue
{
    public static int MAX_RANKS = 13;
    public enum Suit
    {
        Spades = 0,
        Hearts,
        Diamonds,
        Clubs
    }

    public enum Rank
    {
        A = 1,
        Two = 2, Three = 3, Four = 4, Five = 5, Six = 6, Seven = 7, Eight = 8, Nine = 9, Ten = 10,
        Jack = 11,
        Queen = 12,
        King = 13
    };

    // Update if another Suit is added
    public static int SUIT_COUNT = 4;
}
