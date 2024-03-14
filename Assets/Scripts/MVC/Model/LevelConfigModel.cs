using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Castle.CustomUtil;

[CreateAssetMenu(fileName = "LevelConfiguration", menuName = "ScriptableObject/LevelConfiguration")]
public class LevelConfigModel : ScriptableObject
{
    public LevelCharacterSpawn[] levelSpawns;

    [System.Serializable]
    public class EnemySpawn
    {
        public PoolName enemyType; 
        public int slotIndex;
        public int level;
    }

    [System.Serializable]
    public class LevelCharacterSpawn
    {
        public int levelNumber;
        public EnemySpawn[] enemiesToSpawn; 
        public int initialCoin; 
    }
}