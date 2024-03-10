using System;
using System.Collections;
using System.Collections.Generic;
using Castle.CustomUtil;
using Castle.CustomUtil.EventManager;
using MVC.Model;
using UnityEngine;

namespace MVC.View
{
    public enum HeroState
    {
        Idle,
        Attack,
        Die
    }

    public class HeroView : MonoBehaviour
    {   
        [SerializeField] private HeroState state;
        [SerializeField] private GameObject[] weapons;
        [SerializeField] private Animator animator;
        [SerializeField] protected AnimationClip[] atkAnimationClips;
        [SerializeField] private BoxCollider boxCollider;
        [SerializeField] private AudioClip[] splashSfx;

        public Action OnHpChanged;

        protected HeroView target;

        private float nextAttackTime;
        protected HeroModel data;
        private HeroSlotView heroSlotView;
        protected int splashStyleCounter = 0;
        //private Queue<MonsterView> hittingMonsters = new Queue<MonsterView>();

        private static readonly int IdleStateName = Animator.StringToHash("idle");
        private static readonly int DieStateName = Animator.StringToHash("die");
        private static readonly int SpeedParaName = Animator.StringToHash("speed");

        public HeroState State
        {
            get => state;
            protected set => state = value;
        }

        public void AttackTarget(HeroView targetView)
        {
            //if (target != null)
            //{
            //    // No need attack when hero already was in clash with other monster
            //    // But we add keep the monster in queue to fight later
            //    hittingMonsters.Enqueue(monster);
            //    return;
            //}

            nextAttackTime = 0;
            target = targetView;
            State = HeroState.Attack;
        }

        public void SetWeapon(int level)
        {
            for (int i = 0; i < weapons.Length; i++)
            {
                weapons[i].SetActive(i == level - 1);
            }
        }

        private void Update()
        {
            ProcessClash();
        }

        private void ProcessClash()
        {
            if (State != HeroState.Attack) return;

            if (target.State == HeroState.Die)
            {
                return;
            }

            //if (IsRangeHero && HasBehindMonster(target))
            //{
            //    TryFightNextMonster();
            //    return;
            //}

            nextAttackTime -= Time.deltaTime;
            if (nextAttackTime <= 0)
            {
                var atkSpeed = data.attackSpeed * GameConst.BaseAttackSpeed;
                var atkStyleIndex = splashStyleCounter % splashSfx.Length;
                Debug.Log($"{this.heroSlotView.Data.HeroModel.objectName} atkIndex {atkStyleIndex}");
                animator.SetFloat(SpeedParaName, atkSpeed);
                animator.SetTrigger($"attack{atkStyleIndex}");

                var clipDuration = atkAnimationClips[atkStyleIndex].length / atkSpeed;
                nextAttackTime = GameConst.HeroBaseAttackGapDuration / atkSpeed + clipDuration;
                ServiceLocator.Instance.AudioManager.PlaySfx(splashSfx[atkStyleIndex]);
                splashStyleCounter += 1;
                ShowSkill();
            }
        }

        protected virtual void ShowSkill() { }

        protected virtual void DealDamageToTarget() { }

        //private void TryFightNextMonster()
        //{
        //    target = null;
        //    State = HeroState.Idle;
        //    nextAttackTime = 0;
        //    ChangeAnim(State);

        //    if (hittingMonsters.Count == 0) return;
        //    MonsterView monster;
        //    bool hasMonster;
        //    if (IsRangeHero)
        //    {
        //        hasMonster = hittingMonsters.TryDequeue(out monster);

        //        if (!hasMonster) return;

        //        while (hasMonster && HasBehindMonster(monster))
        //        {
        //            hasMonster = hittingMonsters.TryDequeue(out monster);
        //        }

        //    }
        //    else
        //    {
        //        hasMonster = hittingMonsters.TryDequeue(out monster);
        //    }

        //    if (!hasMonster) return;

        //    AttackTarget(monster);
        //}

        //private bool HasBehindMonster(MonsterView monster)
        //{
        //    var offset = 0.65f;
        //    return monster == null || monster.transform.position.x < transform.position.x - offset;
        //}

        private void ChangeAnim(HeroState heroState)
        {
            switch (heroState)
            {
                case HeroState.Idle:
                    animator.SetTrigger(IdleStateName);
                    break;
                case HeroState.Die:
                    animator.SetTrigger(DieStateName);
                    break;
            }
        }

        public bool IsRangeHero => data.attackType == AttackType.Range;

        public void Init(HeroSlotView heroSlot)
        {
            State = HeroState.Idle;
            heroSlotView = heroSlot;
            data = heroSlotView.Data.HeroModel;
            SetWeapon(data.upgradeLevel);
            animator.SetFloat(SpeedParaName, 1);
            boxCollider.enabled = true;
        }

        public void GetDamage(int damage)
        {
            Debug.Log(this.heroSlotView.Data.HeroModel.objectName + " GET DAMAGE " + damage.ToString());
            data.hp -= damage * (1 - data.defense / 100);

            if (data.hp <= 0)
            {
                State = HeroState.Die;
                target = null;
                boxCollider.enabled = false;
                StartCoroutine(ShowDeadAnim());
            }

            OnHpChanged?.Invoke();
        }

        private IEnumerator ShowDeadAnim()
        {
            ChangeAnim(State);
            yield return new WaitForSeconds(2f);
            var eventData = new Dictionary<string, object>();
            eventData.Add(GameConst.HeroDeadEventName, heroSlotView);
            ServiceLocator.Instance.GameEventManager.Dispatch(GameEvent.HeroDead, eventData);
        }

        
    }
}