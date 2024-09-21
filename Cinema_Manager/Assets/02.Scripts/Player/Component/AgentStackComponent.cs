using System.Collections.Generic;
using UnityEngine;

public class AgentStackComponent : AgentComponent
{
    [SerializeField] private Transform holderTransform;
    [SerializeField] private float spacingY = 0.4f;
    [SerializeField] private int maxStackCount = 3;

    private Stack<ITakeable> takeObjectStack;

    // Counts
    public int CurrentStackCount => takeObjectStack.Count;
    public int RemainingStackCount => maxStackCount - CurrentStackCount;

    // Bool
    public bool IsStackMax => CurrentStackCount >= maxStackCount;
    public bool IsStacked => CurrentStackCount > 0;

    private readonly Vector3 stackObjectRotation = new Vector3(0, 90, 0);

    public override void Init(AgentController controller)
    {
        base.Init(controller);

        takeObjectStack = new Stack<ITakeable>();
    }

    private void OnEnable()
    {
        takeObjectStack ??= new Stack<ITakeable>();

        if (takeObjectStack.Count <= 0)
        {
            return;
        }

        foreach (var obj in takeObjectStack)
        {
            //PoolManager.Instance.Push(obj as PoolableMono);
        }

        takeObjectStack.Clear();
    }

    public void SetMaxStackCount(int maxStackCount)
    {
        if (false == IsStacked)
        {
            this.maxStackCount = maxStackCount;
        }
    }

    public void TakeObject(ITakeable takeableObject)
    {
        Vector3 objectPosition = Vector3.zero;
        objectPosition.y += spacingY * CurrentStackCount;

        takeableObject.Take(holderTransform, objectPosition, stackObjectRotation);
        takeObjectStack.Push(takeableObject);

        /*
        if (takeableObject is Bread)
        {
            SoundManager.Instance.Play(UsingAudioClips.TakeObject);
        }
        */
    }

    public ITakeable GetTopObject()
    {
        return takeObjectStack.Pop();
    }

    public override void ControllerUpdate()
    {
    }
}
