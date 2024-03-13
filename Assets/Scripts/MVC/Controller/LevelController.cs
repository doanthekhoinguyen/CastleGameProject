using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MVC.Controller;

namespace MVC.Controller
{

    public class LevelController : MonoBehaviour
    {
        [SerializeField] private LevelConfigModel levelConfig; 
        [SerializeField] private BattleController battleController; 

        private int currentLevel = 1;

        public void SetupLevel(int levelNumber)
        {
            var levelSpawnConfig = FindLevelSpawnConfig(levelNumber);
            if (levelSpawnConfig != null)
            {
                StartCoroutine(SetupLevelRoutine(levelSpawnConfig));
            }
            else
            {
                Debug.LogError($"Level spawn config for level {levelNumber} not found.");
            }
        }

        
        private LevelConfigModel.LevelCharacterSpawn FindLevelSpawnConfig(int levelNumber)
        {
            foreach (var spawnConfig in levelConfig.levelSpawns)
            {
                if (spawnConfig.levelNumber == levelNumber)
                {
                    return spawnConfig;
                }
            }
            return null;
        }

        // Coroutine để setup level
        private IEnumerator SetupLevelRoutine(LevelConfigModel.LevelCharacterSpawn levelConfig)
        {
            
            yield return new WaitForSeconds(1f); 

            
            battleController.SetUpLevel(levelConfig);

            
        }
        public void NextLevel()
        {
            currentLevel++;
            SetupLevel(currentLevel);
        }
        public void ResetLevel()
        {
            currentLevel = 1; 
        }
        public int GetCurrentLevel()
        {
            return currentLevel;
        }
        public void SpawnEnemiesForCurrentLevel()
        {
            var levelSpawnConfig = FindLevelSpawnConfig(currentLevel);
            if (levelSpawnConfig != null)
            {
                battleController.SpawnEnemies(levelSpawnConfig.enemiesToSpawn);
            }
            else
            {
                Debug.LogError($"Cannot find spawn configuration for level {currentLevel}");
            }   
        }
        public int GetTotalEnemyInCurrentLevel() 
        {
            var levelSpawnConfig = FindLevelSpawnConfig(currentLevel);
            return levelSpawnConfig.enemiesToSpawn.Length;
        }

        // Thêm các phương thức khác như RestartLevel, LoadLevelData, v.v., tùy thuộc vào nhu cầu của game.
    }

}