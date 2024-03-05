using System;
using System.Collections.Generic;
using UnityEngine;

namespace Castle.CustomUtil
{
    [System.Serializable]
    public class PoolObjectConfig
    {
        public PoolName poolName;
        public GameObject prefab;
        [Tooltip("How many object is init at the first time")]
        public int initPoolNumber = 3;
    }
    
    public enum PoolName
    {
        HeroType1,
        HeroType2,
        HeroType3,
        HeroType4,
        HeroType5,
        HeroType6,
        HeroType7,
    }
    
    public enum PoolState
    {
        In,
        Out
    }
    
    public class GamePool : MonoBehaviour
    {
        [SerializeField] private List<PoolObjectConfig> configs;
        
        private Dictionary<PoolName, List<GameObject>> poolDic = new Dictionary<PoolName, List<GameObject>>();
        private Dictionary<PoolName, Transform> containers = new Dictionary<PoolName, Transform>(); // where we fill pool object

        public static GamePool Instance { get; private set; }
        
        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(this);
                return;
            }
            
            Instance = this;
            PreparePool();
        }

        private void PreparePool()
        {
            // create containers
            var containerGo = new GameObject("container");
            containerGo.transform.position = new Vector3(99999, 9999); // The farthest of position is from away camera view to avoid rendering unnecessary and get better performance
            containerGo.transform.SetParent(gameObject.transform);
            for (int i = 0; i < configs.Count; i++)
            {
                var config = configs[i];
                var poolContainer = new GameObject($"{config.poolName}");
                poolContainer.transform.SetParent(containerGo.transform);
                containers.Add(config.poolName, poolContainer.transform);
            }
            
            // create object pool and in put them into container properly
            for (int i = 0; i < configs.Count; i++)
            {
                var config = configs[i];
                List<GameObject> objects = new List<GameObject>(config.initPoolNumber);
                for (int j = 0; j < config.initPoolNumber; j++)
                {
                    var go = Instantiate(config.prefab, containers[config.poolName]);
                    go.SetActive(false);
                    var poolCtrl = go.AddComponent<PoolObject>();
                    poolCtrl.PoolName = config.poolName;
                    poolCtrl.PoolState = PoolState.In;
                    objects.Add(go);
                }
                poolDic.Add(config.poolName, objects);
            }
        }
        
        public GameObject GetObject(PoolName poolName)
        {
            poolDic.TryGetValue(poolName, out List<GameObject> pool);
            if (pool == null)
            {
                Debug.LogError($"Not found {poolName} in pool");
                return null;
            }

            for (int i = 0; i < pool.Count; i++)
            {
                var poolObjectCtrl = pool[i].GetComponent<PoolObject>();
                if (poolObjectCtrl.PoolState == PoolState.Out) continue;
                poolObjectCtrl.PoolState = PoolState.Out;
                return pool[i];
            }
                
            // if all object in pool were occupied, we create new
            var config = GetObjectPoolConfig(poolName);
            var go = MakePoolObjectByConfig(config);
            go.GetComponent<PoolObject>().PoolState = PoolState.Out;

            // add new one into pool
            pool.Add(go);
                
            return go;
        }

        public void Release(GameObject go)
        {
            var poolObject = go.GetComponent<PoolObject>();
            if (poolObject == null)
            {
                Debug.LogError($"Not found poolObject in {go.name}");
                return;
            }
            
            if (poolObject.PoolState == PoolState.In) return;
            
            go.SetActive(false);
            go.transform.SetParent(containers[poolObject.PoolName]);
            go.transform.localPosition = Vector3.zero;
            poolObject.PoolState = PoolState.In;
        }

        private GameObject MakePoolObjectByConfig(PoolObjectConfig config)
        {
            var go = Instantiate(config.prefab);
            var poolCtrl = go.AddComponent<PoolObject>();
            poolCtrl.PoolName = config.poolName;
            poolCtrl.PoolState = PoolState.In;

            return go;
        }

        private PoolObjectConfig GetObjectPoolConfig(PoolName poolName)
        {
            for (int i = 0; i < configs.Count; i++)
            {
                if (configs[i].poolName == poolName) return configs[i];
            }
            
            Debug.LogError($"Not found {poolName} config");
            return null;
        }
    }

    public class PoolObject : MonoBehaviour
    {
        public PoolName PoolName;
        public PoolState PoolState;
    }
}