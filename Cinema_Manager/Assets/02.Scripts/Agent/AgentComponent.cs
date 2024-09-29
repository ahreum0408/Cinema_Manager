using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AgentComponent : MonoBehaviour
{
    protected AgentController _controller;

    // 재정의한 곳에서는 Base 함수 실행 필수
    public virtual void Init(AgentController controller)
    {
        this._controller = controller;

        _controller.OnEnableEvent += ControllerEnable;
        _controller.OnUpdateEvent += ControllerUpdate;
        _controller.OnFixedUpdateEvent += ControllerFixedUpdate;
        _controller.OnDisableEvent += ControllerDisable;
    }

    public abstract void ControllerUpdate();
    public virtual void ControllerEnable() { }
    public virtual void ControllerFixedUpdate() { }
    public virtual void ControllerDisable() { }
}
