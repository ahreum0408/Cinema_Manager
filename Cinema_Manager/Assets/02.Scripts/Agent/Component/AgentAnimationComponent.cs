using System.Collections.Generic;
using System.Collections;
using UnityEditor;
using UnityEngine;
using static AyunDefine;

public class AgentAnimationComponent : AgentComponent
{
    private Animator _animator;

    private int _isSeat = Animator.StringToHash("IsSeat");

    public override void Init(AgentController controller)
    {
        base.Init(controller);

        if (controller.Animator != null)
        {
            _animator = controller.Animator;
        }
    }

    public override void ControllerUpdate()
    {
    }

    public void SetMovementAnimation(Vector3 inputValue)
    {
        float moveValue = inputValue.magnitude;
        _animator.SetFloat(AnimatorPropertyHash.IsRun, moveValue);
    }

    public void UpperHoldingAnimation(bool isPlay)
    {
        float startValue = isPlay == true ? 0f : 1f;
        float endValue = isPlay == true ? 1f : 0f;

        StartCoroutine(HoldAnimationRoutine(startValue, endValue));
    }

    private IEnumerator HoldAnimationRoutine(float startValue, float endValue)
    {
        float currentTime = 0;
        float changeTime = 0.5f;
        while (currentTime < changeTime)
        {
            currentTime += Time.deltaTime;

            float newWeight = Mathf.Lerp(startValue, endValue, currentTime / changeTime);
            _animator.SetLayerWeight(1, newWeight);
            yield return null;
        }
    }

    public void SeatAnimation(float value)
    {
        _animator.SetFloat(_isSeat, value);
    }
}
