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

    public void PlayClip(AudioClip clip, float pitch, bool isLooping = false)
    {
        StartCoroutine(PlayClipCor(clip, pitch, isLooping));
    }

    private IEnumerator PlayClipCor(AudioClip clip, float pitch, bool isLooping)
    {
        _audioSource.Stop();

        _audioSource.loop = isLooping;
        _audioSource.pitch = pitch;
        _audioSource.clip = clip;
        _audioSource.Play();
        if (_audioSource.loop == true)
        {
            yield break;
        }
        else
        {
            yield return new WaitForSeconds(clip.length);
            PoolManager.Instance.Push(gameObject.name, gameObject);
        }
    }
}
