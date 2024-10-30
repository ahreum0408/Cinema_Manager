using UnityEngine;

public interface IIneractionable
{
    public void EnterInteraction(Collider collider);
    public void ExitInteraction(Collider collider);
}
