using System.Collections.Generic;
using MVC.Model;
using UnityEngine;

namespace Castle.CustomUtil
{
    public class GameCollectionManager : MonoBehaviour
    {
        [SerializeField] private HeroModel[] heroModels;
        [SerializeField] private TavernModel tavern;

        // HashSet để theo dõi những hero đã được thêm
        private HashSet<PoolName> unlockedHeroes = new HashSet<PoolName>();


        public HeroModel GetHeroInfo(PoolName poolName)
        {
            for (int i = 0; i < heroModels.Length; i++)
            {
                if (heroModels[i].id == poolName) return ScriptableObject.Instantiate(heroModels[i]);
            }

            Debug.LogError("Not found hero info, fallback default");
            return ScriptableObject.Instantiate(heroModels[0]);
        }


        public List<HeroModel> GetTavernHeroes(int currentLevel)
        {
            var heroes = new List<HeroModel>();
            
            var tavernHeroes = tavern.Heroes;
            foreach (var hero in heroModels)    
            {
                if (currentLevel >= hero.unlockLevel && !unlockedHeroes.Contains(hero.id))
                {
                    unlockedHeroes.Add(hero.id);
                    heroes.Add(ScriptableObject.Instantiate(hero));
                }
            }
            return heroes;
        }
    }
}