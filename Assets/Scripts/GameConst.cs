
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GameConst
{
   // Game tag
   public const string HeroSlotTag = "HeroSlot"; 
   public const string PlaneTag = "Plane"; 

   // Game config
   public const float BattleTme = 1;
   public const float BaseAttackSpeed = 1;
   public const int UpgradeCostCoefficient = 3;
   public const float SellCostCoefficient = 1;
   public const float BeHitMonsterDelayTime = 0.5f;
   public const float TotalTimeInPerLevel =  60 * 3; // in second
   
   
   public const float SpawnInitMinTime = 1;
   public const float SpawnInitMaxTime = 2;
   public const float SpawnMinTime = 15;
   public const float SpawnMaxTime = 30;
   public const float DelayTimeSpawnInSameLine = 15;
   
   public const string CombatPhaseEventName = "CombatPhaseEventName";

   // ========Hero bonus per upgrade======
   // we have a range bonus to diversity bonus value for hero
   public static readonly float[] HeroAtkBonusPerLevel = new []{ 1.5f, 2f };
   public static readonly float[] HeroDefBonusPerLevel = new []{ 1.5f, 2f };
   public static readonly float[] HeroHpBonusPerLevel = new []{ 1.5f, 2f };
   public static readonly float[] HeroAtkSpeedBonusPerLevel = new []{ 1.1f, 1.5f };
   
   // ======== Hero ======
   public const string HeroEventName = "Hero";
   public const string HeroDeadEventName = "HeroDead";
   public const string HeroPoolNamePrefix = "HeroType";
   public const float HeroBaseAttackGapDuration = 1f;
   public const float HeroBaseMinAtkFactor = 0.5f;
   public const float HeroBaseMaxAtkFactor = 1f;
   
   // ======== Monster ======
   public const string MonsterEventName = "Monster";
   public const string MonsterDeadEventName = "MonsterDead";
   public const string MonsterPoolNamePrefix = "MonsterType";
   public const float MonsterBaseAttackDuration = 1.75f;
   public const float MonsterRunBaseSpeed = 0.75f;
   public const float MonsterBaseMinAtkFactor = 0.5f;
   public const float MonsterBaseMaxAtkFactor = 1f;

    // ======== Abilities ======
    public const string AbilityEvent = "AbilityEvent";
    public const string AbilityEvent_HeroSlot = "AbilityEvent_HeroSlot";

}
