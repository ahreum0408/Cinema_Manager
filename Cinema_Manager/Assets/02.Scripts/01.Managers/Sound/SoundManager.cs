using System.Collections.Generic;
using UnityEngine;
using static AyunDefine;

public class SoundManager : MonoSingleton<SoundManager>
{
    [HideInInspector] public bool _isSoundOn = true;

    private List<GameObject> _soundObjList = new List<GameObject>();

    public GameObject Play(AudioClip clip, float pitch = 1f, Transform parent = null, bool isLooping = false)
    {
        if (false == _isSoundOn) return null;

        GameObject go = PoolManager.Instance.Pop(PoolableType.SoundObject.ToString(), parent);
        _soundObjList.Add(go);

        if (go.TryGetComponent(out SoundObject soundObj))
            soundObj.PlayClip(clip, pitch, isLooping);

        return go;
    }

    public GameObject Play(AudioClip clip, bool is3DSound = false, float pitch = 1f, Transform parent = null, bool isLooping = false)
    {
        if (false == _isSoundOn) return null;

        GameObject go = PoolManager.Instance.Pop(PoolableType.SoundObject.ToString(), parent);
        _soundObjList.Add(go);

        if (go.TryGetComponent(out SoundObject soundObj))
            soundObj.PlayClip(clip, pitch, isLooping, is3DSound);

        return go;
    }

    public void SoundSet(bool isSoundOn)
    {
        _isSoundOn = isSoundOn;

        foreach (GameObject go in _soundObjList)
        {
            if (go.TryGetComponent(out AudioSource audioSource))
            {
                audioSource.volume = isSoundOn ? 1f : 0f;
            }
        }
    }

    public void RemoveSoundObjList(GameObject go)
    {
        PoolManager.Instance.Push("SoundObject", go);
        _soundObjList.Remove(go);
    }
}
