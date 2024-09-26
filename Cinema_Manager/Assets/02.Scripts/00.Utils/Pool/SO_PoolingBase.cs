using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct PoolingPair
{
    public GameObject prefab;
    public string prefabTypeName;
    public int poolCount;
}

[CreateAssetMenu (menuName = "SO/PoolingBase")]
public class SO_PoolingBase : ScriptableObject
{
    public List<PoolingPair> pairs;
}
