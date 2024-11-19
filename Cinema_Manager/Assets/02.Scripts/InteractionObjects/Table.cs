using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using UnityEngine;
using static AyunDefine;

public class Table : MonoBehaviour, IIneractionable, IOpenTarget
{
    [HideInInspector] public GameObject GameObject => gameObject;
    [SerializeField] private TargetType _targetType;
    public Transform staffPoint;

    public List<Point> points;

    private bool _isEnterInteraction = false;

    private MoneyDummy _moneyDummy;
    private NotifyImageComponent _notifyImageComponent;

    public bool IsWorking = false;

    private bool _isOpen = false;
    public bool IsOpen { get => _isOpen; set => _isOpen = value; }
    public TargetType Type { get => _targetType; set => _targetType = value; }

    [Header("Trash")]
    [SerializeField] private float _trashYSpacing;

    private void Awake()
    {
        _moneyDummy = transform.GetComponentInChildren<MoneyDummy>();
        _notifyImageComponent = GetComponentInChildren<NotifyImageComponent>();

        points = GetComponentsInChildren<Point>().ToList();
    }

    public void EnterInteraction(AgentController agent)
    {
        _isEnterInteraction = true;
        _notifyImageComponent.SetNotifySensorImage(1.1f);
        StartCoroutine(GetTrashRoutine(agent));
    }

    public void ExitInteraction(AgentController agent)
    {
        _isEnterInteraction = false;
        StopCoroutine(GetTrashRoutine(agent));
        _notifyImageComponent.SetNotifySensorImage(1.0f);
    }

    private IEnumerator GetTrashRoutine(AgentController agent)
    {
        while (_isEnterInteraction)
        {
            for (int i = 0; i < points.Count; i++)
            {
                if (points[i].trash != null)
                {
                    if (agent.CanTakeFood(PoolableType.Trash))
                    {
                        agent.OnTakeTakeable?.Invoke
                            (points[i].trash.GetComponent<ITakeable>(), PoolableType.Trash, _trashYSpacing, true);

                        points[i].ChangeDirtyState(false);
                        points[i].trash = null;

                        yield return new WaitForSeconds(0.15f);
                    }
                }
                else
                    continue;
            }
            yield return null;
        }
        yield return null;
    }


    public void AddMoney()
    {
        _moneyDummy.AddMoneyObject(1);
    }

    public Point CanSeatChair()
    {
        foreach (var chair in points)
        {
            if (!chair.IsUsing && !chair.IsDirty)
            {
                return chair;
            }
        }
        return null;
    }

    public Point FindDirtyChair()
    {
        foreach (var chair in points)
        {
            if (chair.IsDirty && !chair.IsUsing)
            {
                return chair;
            }
        }
        return null;
    }

    public void ActiveObj(bool active, bool on = false) {
        _isOpen = active;
        gameObject.SetActive(active);
        //LevelEvents.ChangeTableActiveEvent?.Invoke(this, active);
        ScaleSetting(active);
    }

    public void ScaleSetting(bool active)
    {
        float time = 0.5f;
        Vector3 originScale = transform.localScale;
        transform.localScale = Vector3.zero;
        transform.DOScale(originScale, time).SetEase(Ease.OutBack).OnComplete(()=> LevelEvents.ChangeTableActiveEvent?.Invoke(this, active));
    }
}
