using System;
using System.Collections;
using TMPro;
using UnityEngine;

public abstract class CheckerArea : MonoBehaviour, IIneractionable {
    [HideInInspector] public GameObject GameObject => gameObject;

    [SerializeField] protected int _calculateWeight = 1; // 돈빠지는 속도
    [SerializeField] protected int _price;
    protected int _minusCoin;
    protected bool _isCalaulate;

    public abstract void EnterInteraction(AgentController agent);
    public abstract void ExitInteraction(AgentController agent);

    protected void CalculateWeght() {
        _minusCoin = 1 * _calculateWeight;
    }
}
