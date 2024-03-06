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
    Random }
[Serializable]
public struct AbilityModel
{ 
    public AbilityType Type;
    public AbilityTarget target;
    public AbilityEffect effect;
    public int magnitude;  // Độ lớn của hiệu ứng (ví dụ: số lượng HP hoặc Damage tăng/giảm)
    public string description; // Mô tả chi tiết về ability này
}
