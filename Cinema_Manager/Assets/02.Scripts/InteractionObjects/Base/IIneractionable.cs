using UnityEngine;

public interface IIneractionable
{
    public void EnterInteraction(AgentController agent);
    public void ExitInteraction(AgentController agent);
}
