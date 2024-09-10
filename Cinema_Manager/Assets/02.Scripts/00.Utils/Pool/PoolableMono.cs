using ObjectPool;
using UnityEngine;

namespace ObjectPooling {
    public abstract class PoolableMono : MonoBehaviour {
        public PoolObjectType type;
        public abstract void Reset();
    }
}
