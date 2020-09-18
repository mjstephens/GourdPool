using System.Collections.Generic;
using UnityEngine;

namespace GourdPool
{
    /// <summary>
    /// Controls runtime instantiation of gameobjects/prefabs
    /// </summary>
    public static class Pool
    {
        #region Variables

        /// <summary>
        /// List of our current pools.
        /// </summary>
        private static readonly List<GameObjectPool> _pools = new List<GameObjectPool>();

        #endregion Variables


        #region Accessors

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
        
        public static GameObject Pooled(
            GameObjectPoolConfigDataTemplate data, 
            Vector3 p, 
            Quaternion r,
            bool updatePoolProperties = true)
        {
            if (updatePoolProperties)
            {
                GetAndSetPoolData(data);
            }
            
            return Pooled(data.poolObject, p, r);
        }
        
        public static GameObject Pooled(
            GameObjectPoolConfigDataTemplate data, 
            Transform t,
            bool updatePoolProperties = true)
        {
            if (updatePoolProperties)
            {
                GetAndSetPoolData(data);
            }
            
            return Pooled(data.poolObject, t);
        }

        #endregion Accessors


        #region Instantiation

        public static GameObject Instantiate(GameObject go)
        {
            return Object.Instantiate(go).gameObject;
        }

        #endregion Instantiation


        #region Public Utility
        
        public static void SetObjectPoolCapacity(
            GameObject go,
            int capacityMin, 
            int capacityMax)
        {
            GameObjectPool targetPool = GetPoolForObject(go);
            targetPool.capacityMin = capacityMin;
            targetPool.capacityMax = capacityMax;
        }
        
        public static void SetObjectPoolCapacityMin(
            GameObject go,
            int capacityMin)
        {
            GameObjectPool targetPool = GetPoolForObject(go);
            targetPool.capacityMin = capacityMin;
        }
        
        public static void SetObjectPoolCapacityMax(
            GameObject go,
            int capacityMax)
        {
            GameObjectPool targetPool = GetPoolForObject(go);
            targetPool.capacityMax = capacityMax;
        }
        
        public static void SetObjectPoolSpilloverAllowance(
            GameObject go,
            int spilloverAllowance)
        {
            GameObjectPool targetPool = GetPoolForObject(go);
            targetPool.spilloverAllowance = spilloverAllowance;
        }
        
        public static void DeleteGameObjectPool(GameObject go)
        {
            GameObjectPool p = GetPoolForObject(go, false);
            if (p != null)
            {
                ((IPool) p).Clear();
                _pools.Remove(p);
            }
        }
        
        /// <summary>
        /// Finds and returns the pool for the given object; if none exists, a pool is created
        /// </summary>
        public static GameObjectPool GetPoolForObject(GameObject go, bool createIfNotFound = true)
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
            if (targetPool == null && createIfNotFound)
            {
                targetPool = new GameObjectPool
                {
                    pooledGameObject = go,
                    poolLabel = "monoPool_" + go.transform.name
                };
                    
                _pools.Add(targetPool);
            }

            return targetPool;
        }

        #endregion Public Utility
        
        
        #region Pooling
        
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

        private static void GetAndSetPoolData(GameObjectPoolConfigDataTemplate data)
        {
            GameObjectPool targetPool = GetPoolForObject(data.poolObject);
            targetPool.capacityMin = data.poolMinimumInstanceLimit;
            targetPool.capacityMax = data.poolMaximumInstanceLimit;
            targetPool.spilloverAllowance = data.spilloverAllowance;
        }

        #endregion Pooling
    }
}
