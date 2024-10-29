using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static AyunDefine;

public class AgentStackComponent : AgentComponent
{
    [SerializeField] private Transform _holderTransform;
    [SerializeField] private float _spacingY = 0.4f;
    [SerializeField] private int _maxStackCount = 3;

    private Stack<ITakeable> _takeObjectStack;

    private PoolableType _currentHoldType = PoolableType.None;
    public PoolableType CurrentHoldType => _currentHoldType;

    private Vector3 _topObjPos = Vector3.zero;
    public Vector3 TopObjPos => _topObjPos;

    // Counts
    public int CurrentStackCount => _takeObjectStack.Count;
    public int RemainingStackCount => _maxStackCount - CurrentStackCount;

    // Bool
    public bool IsStackMax => CurrentStackCount >= _maxStackCount;
    public bool IsStacked => CurrentStackCount > 0;

    // 오브젝트가 Jump를 해서 스택에 쌓였는지
    public bool IsObJumped = false;

    public override void Init(AgentController controller)
    {
        base.Init(controller);

        _takeObjectStack = new Stack<ITakeable>();
    }

    private void OnEnable()
    {
        _takeObjectStack ??= new Stack<ITakeable>();

        if (_takeObjectStack.Count <= 0)
        {
            return;
        }

        _takeObjectStack.Clear();
    }

    public void SetMaxStackCount(int maxStackCount)
    {
        if (false == IsStacked)
        {
            _maxStackCount = maxStackCount;
        }
    }

    public void TakeObject(ITakeable takeableObject, PoolableType type, float spacingY, bool isFood)
    {
        if (_currentHoldType == PoolableType.None)
            _currentHoldType = type;

        Vector3 objectPosition = Vector3.zero;
        objectPosition.y += spacingY * CurrentStackCount;
        Vector3 rotation = isFood == true ? new Vector3(-90, 0, 0) : Vector3.zero;
        takeableObject.Take(_holderTransform, objectPosition, rotation);

        // Stack에 쌓이는 소리
        SoundManager.Instance.Play(AudioClips.Stack, 1 * CurrentStackCount, null, false);

        IsObJumped = false;
        _takeObjectStack.Push(takeableObject);

        if (IsStackMax)
            StartCoroutine(JumpWaitRoutine(spacingY));
    }

    private IEnumerator JumpWaitRoutine(float spacingY)
    {
        yield return new WaitForSeconds(0.5f); // Jump 끝나서 스택 위치로 갈 때 까지 기다리기
        Vector3 pos = _holderTransform.GetChild(_holderTransform.childCount - 1).GetComponent<Transform>().position;
        _topObjPos = pos;
        IsObJumped = true;
    }

    public ITakeable GetTopObject()
    {
        if (false == IsStacked) return null;

        ITakeable takeable = _takeObjectStack.Pop();

        // Stack에서 나가는 소리
        SoundManager.Instance.Play(AudioClips.Stack, 1 * CurrentStackCount, null, false);

        if (_takeObjectStack.Count <= 0) _currentHoldType = PoolableType.None;
        return takeable;
    }

    public void ChangeHoldType(PoolableType holdType)
    {
        _currentHoldType = holdType;
    }

    public override void ControllerUpdate() {}
}
