using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentMovementComponent : AgentComponent
{
    private Rigidbody _rigidbody;

    private Vector3 moveVelocity;

    [SerializeField] private float moveSpeed;
    [SerializeField] private float rotateSpeed;

    public override void Init(AgentController controller)
    {
        base.Init(controller);

        if (controller.Rigidbody != null)
        {
            _rigidbody = controller.Rigidbody;
        }
    }

    public override void ControllerUpdate() {}

    public override void ControllerFixedUpdate()
    {
        if (moveVelocity != Vector3.zero)
        {
            Move();
            Rotate();
        }
    }

    public void SetVelocity(Vector3 velocity)
    {
        if (velocity == Vector3.zero)
        {
            _rigidbody.velocity = Vector3.zero;
        }
        moveVelocity = velocity;
    }

    private void Move()
    {
        moveVelocity = moveVelocity.normalized * moveSpeed;
        _rigidbody.velocity = moveVelocity;
    }

    private void Rotate()
    {
        Quaternion dirQuat = Quaternion.LookRotation(moveVelocity);
        Quaternion moveQuat = Quaternion.Slerp(_rigidbody.rotation, dirQuat, rotateSpeed * Time.fixedDeltaTime);
        _rigidbody.MoveRotation(moveQuat);
    }
}
