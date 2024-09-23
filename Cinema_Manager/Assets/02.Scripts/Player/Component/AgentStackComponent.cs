using System.Collections.Generic;
using UnityEngine;

public class AgentStackComponent : AgentComponent
{
    [SerializeField] private Transform _holderTransform;
    [SerializeField] private float _spacingY = 0.4f;
    [SerializeField] private int _maxStackCount = 3;

    private Stack<ITakeable> _takeObjectStack;

    // Counts
    public int CurrentStackCount => _takeObjectStack.Count;
    public int RemainingStackCount => _maxStackCount - CurrentStackCount;

    // Bool
    public bool IsStackMax => CurrentStackCount >= _maxStackCount;
    public bool IsStacked => CurrentStackCount > 0;

    private readonly Vector3 _stackObjectRotation = new Vector3(0, 90, 0);

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

        foreach (var obj in _takeObjectStack)
        {
            //PoolManager.Instance.Push(obj as PoolableMono);
        }

        _takeObjectStack.Clear();
    }

    public void SetMaxStackCount(int maxStackCount)
    {
        if (false == IsStacked)
        {
            this._maxStackCount = maxStackCount;
        }
    }

    public void TakeObject(ITakeable takeableObject)
    {
        Vector3 objectPosition = Vector3.zero;
        objectPosition.y += _spacingY * CurrentStackCount;

        takeableObject.Take(_holderTransform, objectPosition, _stackObjectRotation);
        _takeObjectStack.Push(takeableObject);

        /*
        if (takeableObject is Bread)
        {
            SoundManager.Instance.Play(UsingAudioClips.TakeObject);
        }
        */
    }

    public ITakeable GetTopObject()
    {
        return _takeObjectStack.Pop();
    }

    public override void ControllerUpdate()
    {
    }
}
