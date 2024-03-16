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
            splashStyleCounter = UnityEngine.Random.Range(0, 2);
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
            animator.SetFloat(SpeedParaName, 1);OnHpChanged += heroSlot.UpdateHeroStatsUI;
            boxCollider.enabled = true;
        }
        
        public HeroModel GetData() { return data; }

        public void GetDamage(int damage)
        {

            //Debug.Log(this.heroSlotView.Data.HeroModel.objectName + " GET DAMAGE " + damage.ToString());
            data.hp = Math.Max(0, data.hp - damage * (1 - data.defense / 100));
            
            var eventData = new Dictionary<string, object>();
            if (data.hp <= 0)
            {
                State = HeroState.Die;
                target = null;
                boxCollider.enabled = false;
                StartCoroutine(ShowDeadAnim());
                
            }

            OnHpChanged?.Invoke();
            //var eventData = new Dictionary<string, object>();
            eventData.Add(GameConst.AbilityEvent_HeroSlot, heroSlotView);
            eventData.Add(GameConst.AbilityEvent, data.abilities[0]);
            ServiceLocator.Instance.GameEventManager.Dispatch(GameEvent.Hurt, eventData);

        }

        private IEnumerator ShowDeadAnim()
        {
            ChangeAnim(State);
            yield return new WaitForSeconds(1.5f);

            // Kiểm tra và kích hoạt ability Faint
            foreach (var ability in data.abilities)
            {
                if (ability.Type == AbilityType.Faint)
                {
                    var abilityEventData = new Dictionary<string, object>
            {
                { GameConst.AbilityEvent, ability },
                { GameConst.AbilityEvent_HeroSlot, heroSlotView }
            };
                    ServiceLocator.Instance.GameEventManager.Dispatch(GameEvent.Faint, abilityEventData);
                }
            }

            // Dispatch sự kiện HeroDead sau khi đã xử lý ability Faint
            var eventData = new Dictionary<string, object>
    {
        { GameConst.HeroDeadEventName, heroSlotView }
    };
            ServiceLocator.Instance.GameEventManager.Dispatch(GameEvent.HeroDead, eventData);
        }




    }
}