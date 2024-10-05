using UnityEngine;

public class SoundManager : MonoSingleton<SoundManager>
{
    public void Play(AudioClip clip, float pitch = 1f, Transform parent = null, bool isLooping = false)
    {
        GameObject go = PoolManager.Instance.Pop("SoundObject", parent.position, Quaternion.identity);
        if (go.TryGetComponent(out SoundObject soundObj))
        {
            soundObj.PlayClip(clip, pitch, isLooping);
        }
    }
}
