using UnityEngine;

namespace GourdPool
{
    /// <summary>
    /// Base class for GameObject pools. Used automatically by Instantiator.cs
    /// </summary>
    public class GameObjectPool : PoolBase
    {
        public GameObject pooledGameObject;
        
        protected override IClientPoolable CreateNewPoolable()
        {
            GameObject newObj = GourdPool.Instantiate(pooledGameObject);
            return newObj.AddComponent<GameObjectPooledComponent>();
        }
    }
}