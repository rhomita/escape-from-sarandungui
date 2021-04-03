using System;
using UnityEngine;

public class Team
{
    public int Number { get; }
    public Material Material { get; }

    public Team(int number, Material material)
    {
        Number = number;
        Material = material;
    }
}