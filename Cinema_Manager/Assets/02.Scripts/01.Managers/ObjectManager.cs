using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static AyunDefine;

public class ObjectManager : MonoSingleton<ObjectManager>
{
    public List<DisplayStand> displayStands;
    public List<Table> tables;
    public List<FoodContainer> foodContainers;

    public BoxContainer boxContainer;
    public Counter counter;
    public ParcelService parcelService;
    public TrashBin trashBin;

    protected override void Awake()
    {
        base.Awake();

        if (displayStands == null)
            displayStands = new List<DisplayStand>();

        if (tables == null)
            tables = new List<Table>();
    }

    // 원하는 음식이 있는 진열대 찾기
    public DisplayStand FindDisplayStand(PoolableType foodType)
    {
        foreach(var stand in displayStands)
        {
            if (stand.GetPoolObjType() == foodType && !stand.IsFullLine 
                && stand.gameObject.active)
                return stand;
        }
        return null;
    }

    public bool CanUseDisplayStand()
    {
        foreach(var stand in displayStands)
        {
            if (stand.CanStandPoint() != null)
                return true;
        }
        return false;
    }

    // 사용 가능한 테이블 체크
    public Table CanUseTable()
    {
        foreach (var table in tables)
        {
            if (table.CanSeatChair() && table.gameObject.active)
            {
                return table;
            }
        }
        return null;
    }
}
