using UnityEngine;

public interface IIneractionable
{
    GameObject GameObject { get; }

    public void EnterInteraction(AgentController agent);
    public void ExitInteraction(AgentController agent);
}
