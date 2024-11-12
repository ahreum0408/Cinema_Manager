using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentMovementComponent : AgentComponent
{
    [Header("Move")]
    [SerializeField] private float moveSpeed;
    [SerializeField] private float rotateSpeed;

    [Header("VFX")]
    [SerializeField] private ParticleSystem _footStepParticle;
    private float _currentTime = 0, _delayTime = 0.5f;

    private Rigidbody _rigidbody;
    private Vector3 moveVelocity;

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
            ParticleRoutine();
        }
    }

    public void SetVelocity(Vector3 velocity) 
    {
        if (velocity == Vector3.zero) 
        {
            _rigidbody.velocity = Vector3.zero;
        }

        moveVelocity = Quaternion.Euler(0, -45f, 0) * velocity;
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

    private void ParticleRoutine()
    {
        _currentTime += Time.fixedDeltaTime;
        if (_currentTime > _delayTime)
        {
            _currentTime = 0;
            _footStepParticle.Play();
        }
    }

    public void SetMoveSpeed(float speed) {
        moveSpeed = speed;
    }
}
