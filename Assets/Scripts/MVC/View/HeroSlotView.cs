using Castle.CustomUtil;
using MVC.Model;
using UnityEngine;
using MVC.Controller;
using System;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using Castle.CustomUtil.EventManager;

namespace MVC.View
{
    public class HeroSlotView : MonoBehaviour   
    {
        [SerializeField] private GameObject flag;
        [SerializeField] private GameObject heroContainer;
        [Header("TextInForHero")]
        [SerializeField] private TextMeshProUGUI hpText;
        [SerializeField] private TextMeshProUGUI atkText;
        [Header("VFX")]
        [SerializeField] private GameObject vfxUpgrade;
        public ParticleSystem abilityVFXStart;
        public ParticleSystem abilityVFXHurt;
        public ParticleSystem abilityVFXFaint;
        public GameObject slotInfo;



        public HeroSlotModel Data;
        public HeroView heroView;
        [HideInInspector] public bool isFirstSlot = false;

        public bool IsHero
        {
            get; private set;
        }

        private GameObject heroGameObject;

        public bool IsEmpty
        {
            get { return Data.HeroModel == null; }
        }

        private void OnEnable()
        {
            ServiceLocator.Instance.GameEventManager.AddListener(GameEvent.CameraState, OnCameraChanged);
        }

        private void OnDisable()
        {
            ServiceLocator.Instance.GameEventManager.RemoveListener(GameEvent.CameraState, OnCameraChanged);
        }

        public void Init(HeroSlotModel heroSlotModel)
        {
            Data = heroSlotModel;
        }

        public void SetHero(HeroModel hero, bool isEnemy)
        {
            Data.HeroModel = hero;

            MakeHeroModel(isEnemy);
            IsHero = !isEnemy;
        }
        
        public void UpdateHeroStatsUI()
        {
            if (Data != null && Data.HeroModel != null)
            {
                if (hpText != null && atkText != null)
                {
                    hpText.text = $"{Data.HeroModel.hp}";
                    atkText.text = $"{Data.HeroModel.attack}";
                }
            }
            else
            {
                if (hpText != null && atkText != null)
                {
                    // Reset về 0 khi không có Hero
                    hpText.text = "HP: 0";
                    atkText.text = "ATK: 0";
                }
            }
        }




        private void MakeHeroModel(bool isEnemy)
        {
            heroGameObject = GamePool.Instance.GetObject(Data.HeroModel.id);
            if (heroGameObject == null)
            {
                Debug.LogError($"Hero GameObject with ID {Data.HeroModel.id} could not be obtained from the pool.");
                return;
            }

            heroGameObject.tag = isEnemy ? "Enemy" : "Hero";
            heroGameObject.transform.SetParent(heroContainer.transform);
            heroGameObject.transform.localPosition = Vector3.zero;
            heroGameObject.transform.localRotation = Quaternion.Euler(new Vector3(0, 90, -90));
            heroGameObject.SetActive(true);

            heroView = heroGameObject.GetComponent<HeroView>();
            if (heroView == null)
            {
                Debug.LogError($"HeroView component could not be found on the Hero GameObject with ID {Data.HeroModel.id}. Make sure your prefab has a HeroView attached.");
                // Optionally, you can add the HeroView component here if it's appropriate for your setup.
                // heroView = heroGameObject.AddComponent<HeroView>();
                return;
            }
            UpdateHeroStatsUI();
            heroView.Init(this);
        }

        public void SetEmpty()
        {
            if (heroGameObject != null)
            {
                GamePool.Instance.Release(heroGameObject);
                heroGameObject = null;
            }
            Data.HeroModel = null;
            heroView = null;
            UpdateHeroStatsUI();
        }

        public void ShowNoneSlot()
        {
            Data.HeroSlotState = HeroSlotState.None;
            flag.SetActive(true);
            heroContainer.SetActive(false);
        }
       

        public void ShowHero()
        {
            Data.HeroSlotState = HeroSlotState.Occupied;
            flag.SetActive(false);
            heroContainer.SetActive(true);
        }

        public void ShowUpgrade()
        {
            heroView.SetWeapon(Data.HeroModel.upgradeLevel);
            vfxUpgrade.SetActive(false);
            vfxUpgrade.SetActive(true);
        }
        public void ActivateAbilityVFXStart(HeroSlotView heroSlot)
        {
            
            if (heroSlot.abilityVFXStart != null)
            {
                heroSlot.abilityVFXStart.Play();
            }
        }
        public void ActivateAbilityVFXHurt(HeroSlotView heroSlot)
        {
            
            if (heroSlot.abilityVFXHurt != null)
            {
                heroSlot.abilityVFXHurt.Play();
            }
        }
        public void ActivateAbilityVFXFaint(HeroSlotView heroSlot)
        {
            
            if (heroSlot.abilityVFXFaint != null)
            {
                heroSlot.abilityVFXFaint.Play();
            }
        }

        public void ClearSlot()
        {
            if (heroGameObject != null)
            {
                GamePool.Instance.Release(heroGameObject);
                heroGameObject = null;
                Data.HeroModel = null;
                heroView = null;
            }
        }
        
        private void OnCameraChanged(Dictionary<string, object> eventData)
        {
            if (slotInfo == null) { return; }
            bool isMainCamera = (bool) eventData[GameConst.CameraEvent_State] ;
            slotInfo.SetActive(isMainCamera);
        }
    }
}