using System.Collections.Generic;
using UnityEngine;

public class StaffManager : MonoSingleton<StaffManager>
{
    [SerializeField] private List<Transform> spawnPoints;

    private List<StaffController> staffList = new List<StaffController>();

    public void SetStaffStat(float speed, int stack, int staffCount)
    {
        int length = staffCount - staffList.Count;
        for (int i = 0; i < length; i++)
        {
            GameObject obj = PoolManager.Instance.Pop("Staff", 
                spawnPoints[staffList.Count].position, Quaternion.identity);
            staffList.Add(obj.GetComponent<StaffController>());
        }

        foreach (StaffController staff in staffList)
        {
            staff.Agent.speed = speed;
            staff.StackCompo.SetMaxStackCount(stack);
        }
    }
}
