using System.Collections.Generic;
using UnityEngine;

namespace MVC.Model
{

    [CreateAssetMenu(fileName = "HeroData", menuName = "ScriptableObject/HeroData")]
    public class HeroModel : BaseBattleObjectModel
    {
        //đánh dấu rằng ability đã được kích hoạt trong lượt đó và sau đó reset lại ở cuối mỗi lượt
        public bool abilityTriggeredThisTurn = false;
        public List<AbilityModel> abilities;
        public string fullPortraitPath;
        public string miniPortraitPath;
        public int coin;
        public int upgradeLevel = 1;
        public int maxUpgradeLevel = 3;
    }
}