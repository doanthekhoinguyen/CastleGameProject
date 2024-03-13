using Castle.CustomUtil;
using MVC.Model;
using UnityEngine;
using MVC.Controller;
using System;

namespace MVC.View
{
    public class HeroSlotView : MonoBehaviour   
    {
        [SerializeField] private GameObject flag;
        [SerializeField] private GameObject heroContainer;
        [Header("VFX")]
        [SerializeField] private GameObject vfxUpgrade;
        public ParticleSystem abilityVFX; 


        public HeroSlotModel Data;
        public HeroView heroView;
        public bool isFirstSlot = false;

        public bool IsHero
        {
            get; private set;
        }

        private GameObject heroGameObject;

        public bool IsEmpty
        {
            get { return Data.HeroModel == null; }
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
        }

        public void ShowNoneSlot()
        {
            Data.HeroSlotState = HeroSlotState.None;
            flag.SetActive(true);
            heroContainer.SetActive(false);
        }
        public void ShowNoneSlotv2()
        {
            Data.HeroSlotState = HeroSlotState.None;
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
        public void ActivateAbilityVFX(HeroSlotView heroSlot)
        {
            // Ví dụ: Kích hoạt particle system đã được thêm vào trước đó trong Unity Editor
            if (heroSlot.abilityVFX != null)
            {
                heroSlot.abilityVFX.Play();
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
        

    }
}