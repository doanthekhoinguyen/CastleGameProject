using System;
using System.Collections;
using System.Collections.Generic;
using Castle.CustomUtil;
using Castle.CustomUtil.EventManager;
using MVC.Model;
using MVC.View;
using UnityEngine;
using Random = UnityEngine.Random;

namespace MVC.Controller
{
    public enum BattleState
    {
        None,
        Prepare,
        Start,
        BeOngoing,
        Pause,
        End,
        ChangingState
    }

    public class BattleController : MonoBehaviour
    {
        [SerializeField] private BattleHUD battleHUD;
        [SerializeField] private Camera battleCamera;
        [SerializeField] private HeroSlotView[] heroSlotViews;
        //[SerializeField] private MonsterSpawner monsterSpawner;

        public int Coin { get; private set; }

        private GameCollectionManager gameCollectionManager;

        public BattleState BattleState
        {
            get => battleState;
            private set
            {
                if (battleState == value) return;
                battleState = value;
                Debug.Log($"Battle change state {battleState}");
            }
        }

        private BattleState battleState;
        private BattleModel data;
        private HeroSlotView curHeroSlotView;
        private float battleTimer;
        private int heart;

        public void Init(BattleModel battleData)
        {
            data = battleData;
            BattleState = BattleState.Prepare;
            gameCollectionManager = ServiceLocator.Instance.GameCollectionManager;
            battleTimer = GameConst.TotalTimeInPerLevel;
            heart = battleData.Heart;
            battleHUD.SetUpHeart(heart);
        }

        #region Script Life Cycle

        private void OnEnable()
        {
            RegisterEvent();
        }

        private void OnDisable()
        {
            RemoveEvent();
        }

        private void Update()
        {
            ProcessGameLoop();
            //UpdateLevelProgress();
        }
        #endregion


        #region Battle Life Cycle

        private void ProcessGameLoop()
        {
            switch (BattleState)
            {
                case BattleState.Prepare:
                    StartCoroutine(PrepareBattle());
                    break;
                case BattleState.Start:
                    StartBattle();
                    break;
                case BattleState.BeOngoing:
                    ProcessBattle();
                    break;
                case BattleState.Pause:
                    PauseBattle();
                    break;
                case BattleState.End:
                    EndBattle();
                    break;
                case BattleState.None:
                    break;
                case BattleState.ChangingState:
                    break;
            }
        }
        private IEnumerator PrepareBattle()
        {
            BattleState = BattleState.ChangingState;
            ServiceLocator.Instance.InputManager.SetCamera(battleCamera);
            ServiceLocator.Instance.InputManager.HasBlockInput = true;

            var heroes = gameCollectionManager.GetTavernHeroes();
            battleHUD.Init(this, heroes);
            battleHUD.SetBeginLayout(data.InitCoin);

            // fill data
            Coin = data.InitCoin;
            SetUpHeroSlot();

            // animate
            yield return ShowBattleSlotAnim();
            yield return new WaitForSeconds(1);
            BattleState = BattleState.Start;
        }

        private void StartBattle()
        {
            ServiceLocator.Instance.InputManager.HasBlockInput = false;
            //monsterSpawner.StartTimer();
            BattleState = BattleState.BeOngoing;
            ServiceLocator.Instance.AudioManager.PlayBgm();
        }

        private void PauseBattle()
        {

        }

        private void ProcessBattle()
        {
            //monsterSpawner.Update();
            //if (battleTimer <= 0 && monsterSpawner.HasNoMonsterOnMap()) BattleState = BattleState.End;
        }

        private void EndBattle()
        {
            ServiceLocator.Instance.AudioManager.PlayBgm(false);

            if (heart == 0)
            {
                battleHUD.ShowGameLose();
                ServiceLocator.Instance.AudioManager.PlaySfx(SFX.GameLose);
            }
            else
            {
                battleHUD.ShowGameWin();
                ServiceLocator.Instance.AudioManager.PlaySfx(SFX.GameWin);
            }

            BattleState = BattleState.ChangingState;
        }

        #endregion

        #region Battle Logic

        //private void UpdateLevelProgress()
        //{
        //    if (BattleState != BattleState.BeOngoing) return;

        //    battleTimer -= Time.deltaTime;

        //    if (battleTimer <= 0)
        //    {
        //        battleTimer = 0;
        //        monsterSpawner.Stop();
        //        return;
        //    }
        //    monsterSpawner.SetLevelProgress(1 - battleTimer / GameConst.TotalTimeInPerLevel);
        //}

        //private void OnMonsterEnterCastle(Dictionary<string, object> obj)
        //{
        //    if (BattleState != BattleState.BeOngoing) return;

        //    heart = Mathf.Max(heart - 1, 0);
        //    battleHUD.UpdateHeart(heart);
        //    monsterSpawner.ReduceMonsterCounter();
        //    if (heart == 0)
        //    {
        //        BattleState = BattleState.End;
        //        return;
        //    }

        //    ServiceLocator.Instance.AudioManager.PlayPenaltySfx();
        //}

        #endregion

        #region Hero Event

        private void OnHeroUpgrade(Dictionary<string, object> eventData)
        {
            var minusCoin = GetUpgradeCoin();
            AddCoin(minusCoin * -1);
            curHeroSlotView.Data.HeroModel.upgradeLevel = Mathf.Clamp(curHeroSlotView.Data.HeroModel.upgradeLevel + 1,
                1, curHeroSlotView.Data.HeroModel.maxUpgradeLevel);

            var originHeroInfo = gameCollectionManager.GetHeroInfo(curHeroSlotView.Data.HeroModel.id);
            var atkBonus = Random.Range(GameConst.HeroAtkBonusPerLevel[0], GameConst.HeroAtkBonusPerLevel[1]);
            var defBonus = Random.Range(GameConst.HeroDefBonusPerLevel[0], GameConst.HeroDefBonusPerLevel[1]);
            var hpBonus = Random.Range(GameConst.HeroHpBonusPerLevel[0], GameConst.HeroHpBonusPerLevel[1]);
            var atkSpeedBonus = Random.Range(GameConst.HeroAtkSpeedBonusPerLevel[0], GameConst.HeroAtkSpeedBonusPerLevel[1]);

            curHeroSlotView.Data.HeroModel.attack = Mathf.FloorToInt(curHeroSlotView.Data.HeroModel.attack * atkBonus);
            curHeroSlotView.Data.HeroModel.defense = Mathf.FloorToInt(curHeroSlotView.Data.HeroModel.defense * defBonus);
            curHeroSlotView.Data.HeroModel.hp = Mathf.FloorToInt(originHeroInfo.hp * hpBonus * curHeroSlotView.Data.HeroModel.upgradeLevel);
            curHeroSlotView.Data.HeroModel.attackSpeed = curHeroSlotView.Data.HeroModel.attackSpeed * atkSpeedBonus;
            curHeroSlotView.ShowUpgrade();
            battleHUD.ShowHeroInfoPanel(curHeroSlotView.Data.HeroModel);
        }


        private void OnHeroDie(Dictionary<string, object> eventData)
        {
            var slotView = eventData[GameConst.HeroDeadEventName] as HeroSlotView;
            if (slotView == null)
            {
                Debug.LogError("Not found slotView in HeroDead Event");
                return;
            }

            slotView.SetEmpty();
            slotView.ShowNoneSlot();
        }

        private void OnSlotSellHero(Dictionary<string, object> eventData)
        {
            var addCoin = GetSellCoin();
            AddCoin(addCoin);
            curHeroSlotView.SetEmpty();
            curHeroSlotView.ShowNoneSlot();
        }
        #endregion

        #region Monster Event


        //private void OnMonsterDie(Dictionary<string, object> eventData)
        //{
        //    var monster = eventData[GameConst.MonsterDeadEventName] as MonsterModel;
        //    if (monster == null)
        //    {
        //        Debug.LogError("Not found monster in MonsterDead Event");
        //        return;
        //    }
        //    ServiceLocator.Instance.AudioManager.PlayCoinSfx();
        //    AddCoin(monster.CoinReward);
        //    monsterSpawner.ReduceMonsterCounter();
        //}
        #endregion

        #region Input event

        private void RegisterEvent()
        {
            ServiceLocator.Instance.GameEventManager.AddListener(GameEvent.SellHero, OnSlotSellHero);
            ServiceLocator.Instance.GameEventManager.AddListener(GameEvent.UpgradeHero, OnHeroUpgrade);
            ServiceLocator.Instance.GameEventManager.AddListener(GameEvent.Combat, OnCombat);
            ServiceLocator.Instance.GameEventManager.AddListener(GameEvent.HeroDead, OnHeroDie);
            //ServiceLocator.Instance.GameEventManager.AddListener(GameEvent.MonsterDead, OnMonsterDie);
            //ServiceLocator.Instance.GameEventManager.AddListener(GameEvent.MonsterEnterCastle, OnMonsterEnterCastle);

            ServiceLocator.Instance.InputManager.OnHeroSlotClicked += OnHeroSlotClicked;
            ServiceLocator.Instance.InputManager.OnPlaneClicked += OnPlaneClicked;
        }

        private void RemoveEvent()
        {
            ServiceLocator.Instance.GameEventManager.RemoveListener(GameEvent.SellHero, OnSlotSellHero);
            ServiceLocator.Instance.GameEventManager.RemoveListener(GameEvent.UpgradeHero, OnHeroUpgrade);
            ServiceLocator.Instance.GameEventManager.RemoveListener(GameEvent.Combat, OnCombat);
            ServiceLocator.Instance.GameEventManager.RemoveListener(GameEvent.HeroDead, OnHeroDie);
            //ServiceLocator.Instance.GameEventManager.RemoveListener(GameEvent.MonsterDead, OnMonsterDie);
            //ServiceLocator.Instance.GameEventManager.RemoveListener(GameEvent.MonsterEnterCastle, OnMonsterEnterCastle);

            ServiceLocator.Instance.InputManager.OnHeroSlotClicked -= OnHeroSlotClicked;
            ServiceLocator.Instance.InputManager.OnPlaneClicked -= OnPlaneClicked;
        }

        private void OnPlaneClicked()
        {
            battleHUD.HideAllPanel();
        }

        private void OnHeroSlotClicked(HeroSlotView heroSlotView)
        {
            curHeroSlotView = heroSlotView;
            switch (heroSlotView.Data.HeroSlotState)
            {
                case HeroSlotState.None:
                    battleHUD.ShowSummonPanel();
                    break;
                case HeroSlotState.Occupied:
                    battleHUD.ShowHeroInfoPanel(curHeroSlotView.Data.HeroModel);
                    curHeroSlotView.heroView.OnHpChanged -= OnUpdateHpHeroHUD;
                    curHeroSlotView.heroView.OnHpChanged += OnUpdateHpHeroHUD;
                    break;
            }
        }

        private void OnUpdateHpHeroHUD()
        {
            if (curHeroSlotView.Data == null || curHeroSlotView.Data.HeroModel == null) return;
            battleHUD.UpdateHeroInfo(curHeroSlotView.Data.HeroModel);
        }

        #endregion

        #region Set up Data

        private void SetUpHeroSlot()
        {
            for (int i = 0; i < heroSlotViews.Length; i++)
            {
                heroSlotViews[i].Init(new HeroSlotModel()
                {
                    SlotIndex = i
                });
            }
        }

        #endregion

        #region Simulate View

        private IEnumerator ShowBattleSlotAnim()
        {
            for (int i = 0; i < heroSlotViews.Length; i++)
            {
                heroSlotViews[i].ShowNoneSlot();
                yield return new WaitForSeconds(0.2f);
            }
        }

        public void SummonHero(PoolName poolName)
        {
            var hero = gameCollectionManager.GetHeroInfo(poolName);
            AddCoin(hero.coin * -1);
            curHeroSlotView.SetHero(hero);
            curHeroSlotView.ShowHero();
            battleHUD.UpdateHeroInfo(hero);
        }
        #endregion

        #region Player logic

        public int GetUpgradeCoin() => GetUpgradeCoin(curHeroSlotView.Data.HeroModel);
        public int GetSellCoin() => GetSellCoin(curHeroSlotView.Data.HeroModel);

        private int GetUpgradeCoin(HeroModel hero)
        {
            var heroLevel = hero.upgradeLevel;
            return Mathf.FloorToInt(hero.coin * heroLevel * GameConst.UpgradeCostCoefficient);
        }

        private int GetSellCoin(HeroModel hero)
        {
            var heroLevel = hero.upgradeLevel;
            return Mathf.FloorToInt(hero.coin * heroLevel * GameConst.SellCostCoefficient);
        }

        private void AddCoin(int value)
        {
            Coin = Mathf.Max(Coin + value, 0);
            battleHUD.SetCoin(Coin);
        }
        #endregion

        #region Clash logic

        private void OnCombat(Dictionary<string, object> eventData)
        {
            if (!ValidateCombatEventData(eventData)) return;

            var hero = eventData[GameConst.HeroEventName] as HeroView;
            //var monster = eventData[GameConst.MonsterEventName] as MonsterView;
            var combatPhase = (CombatPhase)eventData[GameConst.CombatPhaseEventName];

            switch (combatPhase)
            {
                case CombatPhase.HeroAttackMonster:
                    //hero.AttackTarget(monster);
                    break;
                case CombatPhase.MonsterAttackHero:
                    //monster.AttackTarget(hero);
                    break;
            }
        }

        private bool ValidateCombatEventData(Dictionary<string, object> eventData)
        {
            var hero = eventData[GameConst.HeroEventName] as HeroView;
            if (hero == null)
            {
                Debug.LogError("Not found hero in OnMeleeCombat event");
                return false;
            }

            //var monster = eventData[GameConst.MonsterEventName] as MonsterView;
            //if (monster == null)
            //{
            //    Debug.LogError("Not found monster in OnMeleeCombat event");
            //    return false;
            //}

            var hasCombatPhase = eventData.ContainsKey(GameConst.CombatPhaseEventName);
            if (!hasCombatPhase)
            {
                Debug.LogError("Not found combat phase in OnMeleeCombat event");
                return false;
            }

            return true;
        }
        #endregion
    }
}