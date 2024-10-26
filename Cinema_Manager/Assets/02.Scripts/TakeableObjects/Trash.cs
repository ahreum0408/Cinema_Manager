using UnityEngine;
using static AyunDefine;

public class Trash : TakeableBase
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Container"))
        {
            PoolManager.Instance.Push(PoolableType.Trash.ToString(), gameObject);
        }
    }
}
