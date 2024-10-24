using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static AyunDefine;

public class Table : MonoBehaviour, IIneractionable
{
    public List<Point> points;

    private bool _isEnterInteraction = false;

    private MoneyDummy _moneyDummy;
    private NotifyImageComponent _notifyImageComponent;

    private void Awake()
    {
        _moneyDummy = transform.GetComponentInChildren<MoneyDummy>();
        _notifyImageComponent = GetComponentInChildren<NotifyImageComponent>();

        points = GetComponentsInChildren<Point>().ToList();
    }

    public void EnterInteraction()
    {
        _isEnterInteraction = true;
        _notifyImageComponent.SetNotifySensorImage(1.1f);
    }

    public void ExitInteraction()
    {
        _isEnterInteraction = false;
        _notifyImageComponent.SetNotifySensorImage(1.0f);
    }

    private void CleanTable()
    {
        if(_isEnterInteraction)
        {
            foreach(Point point in points)
            {
                point.ChangeDirtyState(false);
            }
        }
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
