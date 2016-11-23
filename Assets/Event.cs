﻿using System.Collections.Generic;
using Casanova.Prelude;
using UnityEngine;

public class Event
{
    public bool IsPlayerControlled;
    public float Radius;
    public int Id;
    public int InterestLevel;
    public int MaxAmountOfParticipants;
    public string AnimationName;
    public string ModelName;
    public string Name;
    public string Sound;
    public Vector3 Position;

    public List<int> AssociatedActions = new List<int>();
    public List<Tuple<int, int>> PersonalityMinimums = new List<Tuple<int, int>>();
}
                                                                                          