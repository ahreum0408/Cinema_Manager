using ObjectPooling;
using System;
using UnityEngine;

namespace ObjectPool {
    [CreateAssetMenu(menuName ="SO/Pool/Item")]
    public class PoolingItemSO : ScriptableObject{
        //public PoolObjectType objectType;
        public int prefabCount;
        public PoolableMono prefab;
    }
}