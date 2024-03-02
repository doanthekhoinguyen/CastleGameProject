using Castle.CustomUtil;
using UnityEngine;

namespace MVC.Model
{
    public enum AttackType
    {
        Melee,
        Range
    }

    public enum CombatPhase
    {
        HeroAttackMonster,
        MonsterAttackHero
    }

    public class BaseBattleObjectModel : ScriptableObject
    {
        public PoolName id;
        public string objectName;
        public int hp;
        public int attack;
        public int defense;
        public float attackSpeed;
        public AttackType attackType;
    }
}