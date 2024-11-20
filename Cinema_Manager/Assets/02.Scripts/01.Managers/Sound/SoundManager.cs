using UnityEngine;
using UnityEngine.Audio;
using static AyunDefine;

public class SoundManager : MonoSingleton<SoundManager>
{
    [SerializeField] private AudioMixer _mainMixer;

    public GameObject Play(AudioClip clip, float pitch = 1f, Transform parent = null, bool isLooping = false)
    {
        GameObject go = PoolManager.Instance.Pop(PoolableType.SoundObject.ToString(), parent);

        if (go.TryGetComponent(out SoundObject soundObj))
            soundObj.PlayClip(clip, pitch, isLooping);

        return go;
    }

    public GameObject Play(AudioClip clip, bool is3DSound = false, float pitch = 1f, Transform parent = null, bool isLooping = false)
    {
        GameObject go = PoolManager.Instance.Pop(PoolableType.SoundObject.ToString(), parent);

        if (go.TryGetComponent(out SoundObject soundObj))
            soundObj.PlayClip(clip, pitch, isLooping, is3DSound);

        return go;
    }

    public void SoundSet(bool isSoundOn)
    {
        float volume = isSoundOn ? 0f : -80f;
        _mainMixer.SetFloat("Master", volume);
    }

    public void PushSoundObj(GameObject go)
    {
        PoolManager.Instance.Push("SoundObject", go);
    }
}
