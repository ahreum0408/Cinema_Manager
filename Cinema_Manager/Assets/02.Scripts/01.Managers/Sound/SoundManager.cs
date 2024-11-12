using UnityEngine;
using static AyunDefine;

public class SoundManager : MonoSingleton<SoundManager>
{
    [HideInInspector] public bool _isSoundOn = true;

    public void Play(AudioClip clip, float pitch = 1f, Transform parent = null, bool isLooping = false)
    {
        if (false == _isSoundOn) return;

        GameObject go = PoolManager.Instance.Pop(PoolableType.SoundObject.ToString(), parent);

        if (go.TryGetComponent(out SoundObject soundObj))
            soundObj.PlayClip(clip, pitch, isLooping);
    }

    public void Play(AudioClip clip, bool is3DSound = false, float pitch = 1f, Transform parent = null, bool isLooping = false)
    {
        if (false == _isSoundOn) return;

        GameObject go = PoolManager.Instance.Pop(PoolableType.SoundObject.ToString(), parent);

        if (go.TryGetComponent(out SoundObject soundObj))
            soundObj.PlayClip(clip, pitch, isLooping, is3DSound);
    }

    public void SoundSet(bool isSoundOn)
    {
        _isSoundOn = isSoundOn;
    }
}
