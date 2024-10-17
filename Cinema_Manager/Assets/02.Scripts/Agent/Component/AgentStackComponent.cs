using System.Collections.Generic;
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

    // Counts
    public int CurrentStackCount => _takeObjectStack.Count;
    public int RemainingStackCount => _maxStackCount - CurrentStackCount;

    // Bool
    public bool IsStackMax => CurrentStackCount >= _maxStackCount;
    public bool IsStacked => CurrentStackCount > 0;

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
        Debug.Log(spacingY);

        if (_currentHoldType == PoolableType.None)
            _currentHoldType = type;

        Vector3 objectPosition = Vector3.zero;
        objectPosition.y += spacingY * CurrentStackCount;
        Vector3 rotation = isFood == true ? new Vector3(-90, 0, 0) : Vector3.zero;
        takeableObject.Take(_holderTransform, objectPosition, rotation);

        _takeObjectStack.Push(takeableObject);
    }

    public ITakeable GetTopObject()
    {
        if (false == IsStacked) return null;

        ITakeable takeable = _takeObjectStack.Pop();
        if (_takeObjectStack.Count <= 0) _currentHoldType = PoolableType.None;
        return takeable;
    }

    public void ChangeHoldType(PoolableType holdType)
    {
        _currentHoldType = holdType;
    }

    public override void ControllerUpdate()
    {
    }
}
