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
            if (other.gameObject.CompareTag("Untagged") && other.gameObject.layer == 7) { return; }
            if (other.gameObject.CompareTag("Enemy")) { return; }

            var meleeHeroView = other.GetComponent<MeleeHeroView>();

            var eventData = new Dictionary<string, object>();
            //Debug.Log($"Hero {this.data.name} clash Monster ${other.name} ${other.gameObject.layer}");

            //Debug.Log($"Hero {this.data.name} clash Monster ${meleeHeroView.data.name}");
            eventData.Add(GameConst.HeroEventName, this);
            eventData.Add(GameConst.MonsterEventName, meleeHeroView);
            eventData.Add(GameConst.CombatPhaseEventName, CombatPhase.HeroAttackMonster);
            ServiceLocator.Instance.GameEventManager.Dispatch(GameEvent.Combat, eventData);
        }

        protected override void ShowSkill()
        {
            DealDamageToTarget();
        }

        protected override void DealDamageToTarget()
        {
            //Debug.Log($"{this.data.objectName} DealDamageToTarget {data.attack}");
            var atkSpeed = data.attackSpeed * GameConst.BaseAttackSpeed;
            var atkStyleIndex = splashStyleCounter % 2;
            var clipDuration = atkAnimationClips[atkStyleIndex].length / atkSpeed;
            var finalAtk = Mathf.FloorToInt(data.attack);

            target.GetDamage(finalAtk);
        }
    }
}