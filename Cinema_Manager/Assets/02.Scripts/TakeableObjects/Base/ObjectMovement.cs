using DG.Tweening;
using UnityEngine;

public class ObjectMovement : MonoBehaviour
{
    [Header("Jump")]
    [SerializeField] private float _jumpPower = 2f;

    public void JumpToPosition(Vector3 position, float palyTime, Space space = Space.Self)
    {
        if (space == Space.Self)
        {
            transform.DOLocalJump(position, _jumpPower, 1, palyTime).SetEase(Ease.OutQuad);
        }
        else
        {
            transform.DOJump(position, _jumpPower, 1, palyTime).SetEase(Ease.OutQuad);
        }
    }
}
