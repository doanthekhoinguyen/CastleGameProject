using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AbilityType 
{ 
    StartOfTurn,
    Hurt,
    Faint
}
public enum AbilityEffect 
{
    Buff,
    AddCoin 
}
public enum AbilityTarget
{ 
    Self,
    Behind,
    Enemies,
    Random
}
[Serializable]
public class AbilityModel
{ 
    public AbilityType Type;
    public AbilityTarget target;
    public AbilityEffect effect;
    public int hpChange;
    public int atkChange; 
    public string description;
}
