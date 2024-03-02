using UnityEngine;

namespace MVC.Model
{
    [CreateAssetMenu(fileName = "HeroData", menuName = "ScriptableObject/HeroData")]
    public class HeroModel : BaseBattleObjectModel
    {
        public string fullPortraitPath;
        public string miniPortraitPath;
        public int coin;
        public int upgradeLevel = 1;
        public int maxUpgradeLevel = 3;
    }
}