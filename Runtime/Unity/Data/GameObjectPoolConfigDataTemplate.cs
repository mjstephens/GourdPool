using UnityEngine;

namespace GourdPool
{
    [CreateAssetMenu(
        fileName = "New GameObject Pool Data",
        menuName = "Config Data/Pooling/GameObject Pool Data")]
    public class GameObjectPoolConfigDataTemplate : ScriptableObject
    {
        [Header("Prefab")] 
        [Tooltip("The object being pooled.")]
        public GameObject poolObject;

        [Header("Values")] 
        public int poolMinimumInstanceLimit = 0;
        public int poolMaximumInstanceLimit = -1;
        public int spilloverAllowance = 0;
    }
}