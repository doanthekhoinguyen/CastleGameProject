using Castle.CustomUtil;
using MVC.Model;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace MVC.View
{
    public class BuyHeroButton : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI lbCoin;
        [SerializeField] private Image avatar;
        [SerializeField] private CanvasGroup canvasGroup;
        public Button Button;

        private int price;
        public void SetData(HeroModel model)
        {
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