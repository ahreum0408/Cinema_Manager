using ObjectPool;
using ObjectPooling;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PoolManager : MonoSingleton<PoolManager> {
    [SerializeField] private List<PoolingItemSO> mapData;
    private Dictionary<PoolObjectType, Pool<PoolableMono>> _poolDictionary = new Dictionary<PoolObjectType, Pool<PoolableMono>>();

    protected override void Awake() {
        base.Awake();
        foreach (PoolingItemSO item in mapData) {
            CreatePool(item);
        }
    }

    private void CreatePool(PoolingItemSO item) {
        Pool<PoolableMono> pool = new Pool<PoolableMono>(item.prefab, item.prefab.type, transform, item.prefabCount);
        _poolDictionary.Add(item.prefab.type, pool);
    }
    public PoolableMono Pop(PoolObjectType type) {
        if (_poolDictionary.ContainsKey(type) == false) {
            Debug.LogError($"Prefab dose not exit on pool : {type.ToString()}");
            return null;
        }
        PoolableMono item = _poolDictionary[type].Pop();
        item.Reset();
        return item;
    }
    public void Push(PoolableMono obj, bool resetParent = false) {
        if (!resetParent) {
            if (!_poolDictionary.ContainsKey(obj.type)) {
                obj.transform.SetParent(transform);
                _poolDictionary[obj.type].Push(obj);
            }
            else {
                Debug.LogWarning("너 똥멍청이야? 똑바로 푸시하라고 저번이랑 같은 실수 하고 싶어?? 너의 소중한 3시간이 멍청함으로 날아갔다고");
            }
        }
    }
}
