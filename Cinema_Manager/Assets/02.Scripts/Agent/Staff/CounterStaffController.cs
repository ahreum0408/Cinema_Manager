using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CounterStaffController : AgentController
{
    // Component
    private AgentAnimationComponent _agentAnimation;

    protected override void Init()
    {
        Rigidbody = GetComponent<Rigidbody>();
        Animator = transform.Find("Visual").GetComponent<Animator>();

        // 계산 모션 넣어주기
    }
    protected override void SetAgentComponents()
    {
        base.SetAgentComponents();

        _agentAnimation = GetAgentComponent<AgentAnimationComponent>();
    }
}
