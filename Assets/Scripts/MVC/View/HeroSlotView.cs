using Castle.CustomUtil;
using MVC.Model;
using UnityEngine;

namespace MVC.View
{
    public class HeroSlotView : MonoBehaviour
    {
        [SerializeField] private GameObject flag;
        [SerializeField] private GameObject heroContainer;
        [Header("VFX")]
        [SerializeField] private GameObject vfxUpgrade;

        public HeroSlotModel Data;
        public HeroView heroView;

        private GameObject heroGameObject;

        public void Init(HeroSlotModel heroSlotModel)
        {
            Data = heroSlotModel;
        }

        public void SetHero(HeroModel hero)
        {
            Data.HeroModel = hero;
            MakeHeroModel();
        }

        private void MakeHeroModel()
        {
            heroGameObject = GamePool.Instance.GetObject(Data.HeroModel.id);
         
            heroGameObject.transform.SetParent(heroContainer.transform);
            heroGameObject.transform.localPosition = Vector3.zero;
            heroGameObject.transform.localRotation = Quaternion.Euler(new Vector3(0, 90, -90));
            heroGameObject.SetActive(true);

            heroView = heroGameObject.GetComponent<HeroView>();
            if (heroView == null)
            {
                Debug.LogError("HeroView component is missing on the Hero GameObject");
                return;
            }

            heroView.Init(this);
        }

        public void SetEmpty()
        {
            GamePool.Instance.Release(heroGameObject);
            heroGameObject = null;
            Data.HeroModel = null;
            heroView = null;
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
    }
}