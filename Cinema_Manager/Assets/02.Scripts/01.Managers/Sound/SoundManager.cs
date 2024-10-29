using System.Collections.Generic;
using UnityEngine;
using static AyunDefine;

public class SoundManager : MonoSingleton<SoundManager>
{
    public void Play(AudioClip clip, float pitch = 1f, Transform parent = null, bool isLooping = false)
    {
        GameObject go = PoolManager.Instance.Pop
            (PoolableType.SoundObject.ToString(), parent);
        if (go.TryGetComponent(out SoundObject soundObj))
            soundObj.PlayClip(clip, pitch, isLooping);
    }

    public void Play(AudioClip clip, float pitch = 1f, Transform parent = null, bool isLooping = false, bool isReverbZone = false)
    {
        GameObject go = PoolManager.Instance.Pop
            (PoolableType.SoundObject.ToString(), parent);
        if (go.TryGetComponent(out SoundObject soundObj))
        {
            if (isReverbZone)
            {
                AudioReverbZone zone = go.AddComponent<AudioReverbZone>();
                zone.minDistance = 5;
                zone.maxDistance = 10;
            }
            soundObj.PlayClip(clip, pitch, isLooping);
        }
    }
}
