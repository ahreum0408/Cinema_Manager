using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SoundObject : MonoBehaviour
{
    private AudioSource _audioSource;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
        _audioSource.playOnAwake = false;
    }

    public void PlayClip(AudioClip clip, float pitch, bool isLooping = false, bool is3DSound = false)
    {
        StartCoroutine(PlayClipCor(clip, pitch, isLooping, is3DSound));
    }

    private IEnumerator PlayClipCor(AudioClip clip, float pitch, bool isLooping, bool is3DSound)
    {
        _audioSource.Stop();

        _audioSource.loop = isLooping;
        _audioSource.pitch = pitch;
        _audioSource.clip = clip;
        _audioSource.Play();

        if (is3DSound) _audioSource.spatialBlend = 1f; // 3D 사운드로 전환

        if (_audioSource.loop == true)
            yield break;
        else
        {
            yield return new WaitForSeconds(clip.length + 0.4f);

            // Reset
            if (is3DSound) _audioSource.spatialBlend = 0;
            PoolManager.Instance.Push(gameObject.name, gameObject);
            SoundManager.Instance.RemoveSoundObjList(gameObject);
        }
    }
}
