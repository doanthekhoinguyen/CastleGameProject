using Castle.CustomUtil;
using UnityEngine;

namespace MVC.Model
{
    [CreateAssetMenu(fileName = "TavernData", menuName = "ScriptableObject/TavernData")]
    public class TavernModel : ScriptableObject
    {
        public PoolName[] Heroes;
    }
}