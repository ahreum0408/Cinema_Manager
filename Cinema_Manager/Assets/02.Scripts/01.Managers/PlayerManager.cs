using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoSingleton<PlayerManager>
{
    private PlayerController _playerController;
    public PlayerController PlayerController => _playerController;
    public Transform Transform => transform;
    public GameObject GameObject => gameObject;

    public void SetPlayer(PlayerController playerController)
    {
        _playerController = playerController;
    }
}
