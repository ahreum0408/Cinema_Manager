using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaffManager : MonoSingleton<StaffManager>
{
    private List<StaffController> staffList = new List<StaffController>();

    private int staffSum;
    private float moveSpeed = 3.5f;
    private int maxSum = 3;

    public void AddStaff(StaffController staff)
    {
        PoolManager.Instance.Pop("Staff", transform.position, Quaternion.identity);
        staffList.Add(staff);
        staffSum++;
    }

    public void SetMoveSpeed(float speed)
    {
        moveSpeed = speed;
        foreach(StaffController staff in staffList)
        {
            staff.Agent.speed = moveSpeed;
        }
    }

    public void SetMaxSum(int sum)
    {
        maxSum = sum;
        foreach(StaffController staff in staffList)
        {
            staff.StackCompo.SetMaxStackCount(maxSum);
        }
    }
}
