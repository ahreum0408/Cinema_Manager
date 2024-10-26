using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using UnityEngine;
using static AyunDefine;

public class Table : MonoBehaviour, IIneractionable
{
    public List<Point> points;

    private bool _isEnterInteraction = false;

    private MoneyDummy _moneyDummy;
    private NotifyImageComponent _notifyImageComponent;

    private PlayerController _playerController;

    private void Awake()
    {
        _moneyDummy = transform.GetComponentInChildren<MoneyDummy>();
        _notifyImageComponent = GetComponentInChildren<NotifyImageComponent>();
        _playerController = FindObjectOfType<PlayerController>();

        points = GetComponentsInChildren<Point>().ToList();
    }

    public void EnterInteraction()
    {
        _isEnterInteraction = true;
        _notifyImageComponent.SetNotifySensorImage(1.1f);
        StartCoroutine(GetTrashRoutine());
    }

    public void ExitInteraction()
    {
        _isEnterInteraction = false;
        StopCoroutine(GetTrashRoutine());
        _notifyImageComponent.SetNotifySensorImage(1.0f);
    }

    private IEnumerator GetTrashRoutine()
    {
        while (_isEnterInteraction)
        {
            if (points[0].trash != null)
            {
                if (_playerController.CanTakeFood(PoolableType.Trash))
                {
                    _playerController.OnTakeTakeable?.Invoke
                        (points[0].trash.GetComponent<ITakeable>(), PoolableType.Trash, 0.15f, true);

                    PoolManager.Instance.Push(PoolableType.Trash.ToString(), points[0].trash);

                    points[0].ChangeDirtyState(false);

                    yield return new WaitForSeconds(0.15f);
                }
            }
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
}
