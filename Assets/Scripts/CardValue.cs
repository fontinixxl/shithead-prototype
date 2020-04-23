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

    //public enum Rank
    //{
    //    A = 1,
    //    Two = 2, Three = 3, Four = 4, Five = 5, Six = 6, Seven = 7, Eight = 8, Nine = 9, Ten = 10,
    //    J = 11,
    //    Q = 12,
    //    K = 13
    //};

    // Ordered in terms of rank for the specific game
    public enum Rank
    {
        Four = 1, Five = 2, Six = 3, Seven = 4, Eight = 5, Nine = 6,
        J = 7,
        Q = 8,
        K = 9,
        A = 10,
        Two = 11, Three = 12, Ten = 13
    };


    // Update if another Suit is added
    public static int SUIT_COUNT = 4;
}
