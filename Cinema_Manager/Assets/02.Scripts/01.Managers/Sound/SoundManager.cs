using UnityEngine;
using static AyunDefine;

public class SoundManager : MonoSingleton<SoundManager>
{
    public void Play(AudioClip clip, float pitch = 1f, Transform parent = null, bool isLooping = false)
    {
        Vector3 spawnPos = parent == null ? Vector3.zero : parent.position;
        GameObject go = PoolManager.Instance.Pop
            (PoolableType.SoundObject.ToString(), spawnPos, Quaternion.identity);
        if (go.TryGetComponent(out SoundObject soundObj))
        {
            soundObj.PlayClip(clip, pitch, isLooping);
        }
    }
}
