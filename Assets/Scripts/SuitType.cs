using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuitType
{
    public string Name { get; private set; }
    public int Id { get; private set; }

    public static readonly SuitType SPITE = new SuitType(0, "Spite");
    public static readonly SuitType HEART = new SuitType(1, "Heart");
    public static readonly SuitType ClOVE = new SuitType(2, "Clove");
    public static readonly SuitType TILE = new SuitType(3, "Tile");

    public SuitType(int id, string name)
    {
        Id = id;
        Name = name;
    }


}
