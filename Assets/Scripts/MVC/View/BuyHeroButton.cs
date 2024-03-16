using Castle.CustomUtil;
using MVC.Model;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace MVC.View
{
    public class BuyHeroButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private TextMeshProUGUI lbCoin;
        [SerializeField] private Image avatar;
        [SerializeField] private CanvasGroup canvasGroup;
        public Button Button;

        private int price;

        private HeroModel heroModel;

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (heroModel != null && heroModel.abilities.Count > 0)
            {
                ToolTipView.Instance.ShowTooltip(heroModel.objectName, heroModel.abilities[0].description, transform.position);
            }
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            // Ẩn tooltip khi chuột rời khỏi button
            ToolTipView.Instance.HideTooltip();
        }

        public void SetData(HeroModel model)
        {
            
            
            this.heroModel = model;
            price = model.coin;
            lbCoin.text = model.coin.ToString();
            var avatarSprite = ServiceLocator.Instance.GameAssetManager.GetHeroSprite(model.miniPortraitPath);
            avatar.sprite = avatarSprite;
            
            
        }

        public void SetStatusByCurrentCoin(int curCoin)
        {
            bool isOn = curCoin >= price;
            canvasGroup.alpha = isOn ? 1 : 0.3f;
            Button.interactable = isOn;
        }

        public void SetStatus(bool isOn)
        {
            canvasGroup.alpha = isOn ? 1 : 0.3f;
            Button.interactable = isOn;
        }
    }
}