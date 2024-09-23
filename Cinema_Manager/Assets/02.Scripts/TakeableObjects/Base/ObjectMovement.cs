using UnityEngine;
using DG.Tweening;

public class ObjectMovement : MonoBehaviour
{
    [Header("Jump")]
    [SerializeField] private float _jumpPower = 2f;
    [SerializeField] private float _animationDuration = 0.5f;
    public float AnimationDuration => _animationDuration;

    public void JumpToPosition(Vector3 position, Space space = Space.Self)
    {
        if (space == Space.Self)
        {
            transform.DOLocalJump(position, _jumpPower, 1, _animationDuration).SetEase(Ease.OutQuad);
        }
        else
        {
            transform.DOJump(position, _jumpPower, 1, _animationDuration).SetEase(Ease.OutQuad);
        }
    }
}
