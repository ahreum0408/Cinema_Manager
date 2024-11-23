using UnityEngine;
using static AyunDefine;

public class LodingView : MonoBehaviour
{
    private bool _isLoding = true;

    private float _currentTime = 0;
    private float _delayTime;

    private float _minDelayTime = 0.8f;
    private float _maxDelayTime = 1.2f;

    private void Awake()
    {
        _delayTime = Random.Range(_minDelayTime, _maxDelayTime);
        StartLoding();
    }

    private void Update()
    {
        if (_isLoding)
        {
            _currentTime += Time.deltaTime;
            if (_currentTime > _delayTime)
            {
                _currentTime = 0;
                EndLoding();
            }
        }
    }

    private void StartLoding()
    {
        _isLoding = true;
        SoundManager.Instance.LodingSet(_isLoding);
        UIManager.Instance.ActiveMainView(false);
    }

    private void EndLoding()
    {
        _isLoding = false;

        // Sound
        SoundManager.Instance.LodingSet(_isLoding);
        SoundManager.Instance.Play(AudioClips.BGM, 1, null, true);

        UIManager.Instance.ActiveMainView(true);
        gameObject.SetActive(false);

    }
}
