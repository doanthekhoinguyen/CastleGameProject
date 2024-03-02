using System;
using System.Collections.Generic;
using Castle.CustomUtil;
using Castle.CustomUtil.EventManager;
using MVC.Model;
using UnityEngine;

namespace MVC.View
{
    public class MeleeHeroView : HeroView
    {
        private void OnTriggerEnter(Collider other)
        {
            //var monsterView = other.GetComponent<MonsterView>();
            
            var eventData = new Dictionary<string, object>();
            eventData.Add(GameConst.HeroEventName, this);
            //eventData.Add(GameConst.MonsterEventName, monsterView);
            eventData.Add(GameConst.CombatPhaseEventName, CombatPhase.HeroAttackMonster);
            ServiceLocator.Instance.GameEventManager.Dispatch(GameEvent.Combat, eventData);
        }

        protected override void ShowSkill()
        {
            DealDamageToTarget();
        }

        protected override void DealDamageToTarget()
        {
            var atkSpeed = data.attackSpeed * GameConst.BaseAttackSpeed;
            var atkStyleIndex = splashStyleCounter % 2;
            var clipDuration = atkAnimationClips[atkStyleIndex].length / atkSpeed;
            var finalAtk = Mathf.FloorToInt(RandomUtils.Range(GameConst.HeroBaseMinAtkFactor * data.attack, GameConst.HeroBaseMaxAtkFactor * data.attack));

            //target.GetDamage(finalAtk, GameConst.BeHitMonsterDelayTime * clipDuration);
        }
    }
}