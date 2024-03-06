using System.Collections.Generic;
using Castle.CustomUtil;
using Castle.CustomUtil.EventManager;
using MVC.Controller;
using MVC.Model;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace MVC.View
{
    public class BattleHUD : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI lbCoin;
        [SerializeField] private Animator summonPanelAnimator;
        [SerializeField] private Animator heroPanelAnimator;

        [Header("Heart")]
        [SerializeField] private GameObject heartGo;
        [SerializeField] private Transform heartContainer;

        [Header("Pause")]
        [SerializeField] private GameObject pauseLayer;

        [Header("Game Win")]
        [SerializeField] private GameObject gameWin;

        [Header("Game Lose")]
        [SerializeField] private GameObject gameLose;

        [Header("Tavern Panel")]
        [SerializeField] private GameObject buyHeroButtonPrefab;
        [SerializeField] private Transform heroesContainer;

        [Header("Hero Info Panel")]
        [SerializeField] private TextMeshProUGUI lbName;
        [SerializeField] private TextMeshProUGUI lbHp;
        [SerializeField] private TextMeshProUGUI lbAtk;
        [SerializeField] private TextMeshProUGUI lbDef;
        [SerializeField] private TextMeshProUGUI lbUpgradeCoin;
        [SerializeField] private TextMeshProUGUI lbSellCoin;
        [SerializeField] private Image heroPortrait;
        [SerializeField] private Button btnUpgrade;
        [SerializeField] private Button btnEndTurn;
        [SerializeField] private CanvasGroup upgradeCanvasGroup;

        [SerializeField] private CameraController cameraController;


        private BattleController battleController;
        private GameAssetManager gameAsset;
        private List<BuyHeroButton> purchasableHeroButtons = new List<BuyHeroButton>();
        private List<GameObject> hearts = new List<GameObject>();
        private bool hasPaused;

        private static readonly int AnimShow = Animator.StringToHash("show");
        private static readonly int AnimHide = Animator.StringToHash("hide");
        private static readonly int AnimHideImmediate = Animator.StringToHash("default");


        public void Init(BattleController battleCtrl, List<HeroModel> purchasableHeroes)
        {
            battleController = battleCtrl;

            gameAsset = ServiceLocator.Instance.GameAssetManager;

            for (int i = 0; i < purchasableHeroes.Count; i++)
            {
                GameObject go = Instantiate(buyHeroButtonPrefab, heroesContainer);
                var ctrl = go.GetComponent<BuyHeroButton>();
                ctrl.SetData(purchasableHeroes[i]);
                var i1 = i;
                ctrl.Button.onClick.AddListener(() => OnSummonHero(purchasableHeroes[i1].id));
                purchasableHeroButtons.Add(ctrl);
            }
        }

        public void OnViewBattleButtonPressed()
        {
            Debug.Log("OnViewBattleButtonPressed called.");
            if (cameraController != null)
            {
                cameraController.MoveCameraToPosition();
            }
            else
            {
                Debug.LogError("CameraController is null.");
            }
        }



        public void SetCoin(int value)
        {
            lbCoin.text = $"{value}";
        }

        public void SetBeginLayout(int coin)
        {
            SetCoin(coin);
        }

        public void ShowSummonPanel()
        {
            UpdatePurchasableHeroes();
            heroPanelAnimator.SetTrigger(AnimHideImmediate);
            summonPanelAnimator.SetTrigger(AnimShow);
        }

        public void SetUpHeart(int count)
        {
            for (int i = 0; i < count; i++)
            {
                GameObject go = Instantiate(heartGo, heartContainer);
                hearts.Add(go);
            }
        }

        public void UpdateHeart(int heart)
        {
            for (int i = 0; i < hearts.Count; i++)
            {
                hearts[i].SetActive(i < heart);
            }
        }

        public void UpdatePurchasableHeroes()
        {
            for (int i = 0; i < purchasableHeroButtons.Count; i++)
            {
                purchasableHeroButtons[i].SetStatusByCurrentCoin(battleController.Coin);
            }
        }

        public void UpdateHeroInfo(HeroModel hero)
        {
            lbName.text = hero.objectName;
            lbAtk.text = hero.attack.ToString();
            lbDef.text = hero.defense.ToString();
            lbHp.text = hero.hp.ToString();

            var upgradeCoin = battleController.GetUpgradeCoin();
            lbUpgradeCoin.text = upgradeCoin.ToString();
            lbSellCoin.text = battleController.GetSellCoin().ToString();

            bool isMaxUpgrade = hero.upgradeLevel >= hero.maxUpgradeLevel;
            bool isEnoughCoinToUpgrade = upgradeCoin <= battleController.Coin;

            btnUpgrade.gameObject.SetActive(!isMaxUpgrade);
            btnUpgrade.interactable = isEnoughCoinToUpgrade;
            upgradeCanvasGroup.alpha = isEnoughCoinToUpgrade ? 1 : 0.3f;
            heroPortrait.sprite = gameAsset.GetHeroSprite(hero.fullPortraitPath);
        }

        public void HideAllPanel()
        {
            heroPanelAnimator.SetTrigger(AnimHideImmediate);
            summonPanelAnimator.SetTrigger(AnimHideImmediate);
        }

        public void ShowHeroInfoPanel(HeroModel hero)
        {
            UpdateHeroInfo(hero);
            heroPanelAnimator.SetTrigger(AnimShow);
            summonPanelAnimator.SetTrigger(AnimHideImmediate);
        }

        public void ShowGameWin()
        {
            gameWin.SetActive(true);
        }

        public void ShowGameLose()
        {
            gameLose.SetActive(true);
        }

        private void OnSummonHero(PoolName poolName)
        {
            summonPanelAnimator.SetTrigger(AnimHide);
            battleController.SummonHero(poolName);
        }


        #region Button event

        public void SellHero()
        {
            heroPanelAnimator.SetTrigger(AnimHideImmediate);
            ServiceLocator.Instance.GameEventManager.Dispatch(GameEvent.SellHero);
        }

        public void UpgradeHero()
        {
            ServiceLocator.Instance.GameEventManager.Dispatch(GameEvent.UpgradeHero);
        }

        public void TryAgain()
        {
            SceneManager.LoadScene("Gameplay");
        }

        public void PauseGame()
        {
            hasPaused = !hasPaused;
            Time.timeScale = hasPaused ? 0 : 1;
            pauseLayer.SetActive(hasPaused);
        }
        void Start()
        {
            btnEndTurn.onClick.AddListener(EndTurn); // Đăng ký sự kiện khi nút được nhấn
        }
        public void EndTurn()
        {
            battleController.OnEndTurnButtonClicked(); // Gọi phương thức trong BattleController
        }
        #endregion
    }
}