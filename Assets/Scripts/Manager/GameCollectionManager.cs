using System.Collections.Generic;
using MVC.Model;
using UnityEngine;

namespace Castle.CustomUtil
{
    public class GameCollectionManager : MonoBehaviour
    {
        [SerializeField] private HeroModel[] heroModels;
        [SerializeField] private TavernModel tavern;

        public HeroModel GetHeroInfo(PoolName poolName)
        {
            for (int i = 0; i < heroModels.Length; i++)
            {
                if (heroModels[i].id == poolName) return ScriptableObject.Instantiate(heroModels[i]);
            }

            Debug.LogError("Not found hero info, fallback default");
            return ScriptableObject.Instantiate(heroModels[0]);
        }
        

        public List<HeroModel> GetTavernHeroes()
        {
            var heroes = new List<HeroModel>();
            var tavernHeroes = tavern.Heroes;
            for (int i = 0; i < tavernHeroes.Length; i++)
            {
                heroes.Add(GetHeroInfo(tavernHeroes[i]));
            }

            return heroes;
        }
    }
}