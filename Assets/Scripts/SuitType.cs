using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class DeckHelper
{
    public static int MAX_RANKS = 13;
    public enum Spites { SPITE = 0, HEART, ClOVE, TILE } // 4 types.
    public enum Ranks { J = 11, Q = 12, K = 13 };

    // Update if another Suit is added
    public static int SUIT_COUNT = 4;
}
