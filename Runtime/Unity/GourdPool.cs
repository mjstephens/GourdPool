using System.Collections.Generic;
using UnityEngine;

namespace GourdPool
{
    /// <summary>
    /// Controls runtime instantiation of gameobjects/prefabs
    /// </summary>
    public static class GourdPool
    {
        #region Variables

        /// <summary>
        /// 
        /// </summary>
        private static readonly List<GameObjectPool> _pools = new List<GameObjectPool>();

        private const string CONST_PoolObjectNamePrefix = "Pool_";

        #endregion Variables
        
        
        #region Instantiation
        
        /// <summary>
        /// Instantiates and returns a GameObject instance.
        /// </summary>
        public static GameObject Instantiate(GameObject go)
        {
            return Object.Instantiate(go).gameObject;
        }

        #endregion


        #region Pooling

        public static GameObject Pooled(GameObject go, Vector3 p, Quaternion r)
        {
            GameObject g = Pooled(go);
            g.transform.SetPositionAndRotation(p, r);
            g.SetActive(true);
            
            return g;
        }

        public static GameObject Pooled(GameObject go, Transform t)
        {
            GameObject g = Pooled(go);
            g.transform.SetParent(t);
            g.SetActive(true);
            
            return g;
        }

        public static void SetObjectPoolCapacity(
            GameObject go,
            int capacityMin, 
            int capacityMax)
        {
            // Find the pool for the associated gameObject
            GameObjectPool targetPool = GetPoolForObject(go);
            targetPool.capacityMin = capacityMin;
            targetPool.capacityMax = capacityMax;
        }

        /// <summary>
        /// Returns the next available pooled gameObject.
        /// </summary>
        private static GameObject Pooled(GameObject go)
        {
            // Find the pool for the associated gameObject
            GameObjectPool targetPool = GetPoolForObject(go);

            // Return next pooled item
            GameObjectPooledComponent g = targetPool.GetNext() as GameObjectPooledComponent;
            g.OnAnonymousDisable();
            return g.gameObject;
        }

        /// <summary>
        /// Finds and returns the pool for the given object; if none exists, a pool is created
        /// </summary>
        private static GameObjectPool GetPoolForObject(GameObject go)
        {
            // Find the pool for the associated gameObject
            GameObjectPool targetPool = null;
            foreach (var pool in _pools)
            {
                if (pool.pooledGameObject == go)
                {
                    targetPool = pool;
                    break;
                }
            }
    
            // If the target is null, there doesn't exist a pool for this GameObject yet
            if (targetPool == null)
            {
                targetPool = new GameObjectPool
                {
                    pooledGameObject = go, 
                    poolLabel = CONST_PoolObjectNamePrefix + go.transform.name
                };
                _pools.Add(targetPool);
            }

            return targetPool;
        }

        #endregion Pooling
    }
}
