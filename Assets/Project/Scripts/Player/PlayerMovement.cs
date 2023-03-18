using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float Movespeed;

    private Transform player;

    private Vector2 moveDir;

    private void Awake()
    {
        player = transform.parent;
    }

    private void Update()
    {
        moveDir = InputManager.Instance.PlayerMoveDir;

        if (moveDir == Vector2.zero) { return; }

        player.position += ((Vector3) moveDir) * Movespeed * Time.deltaTime;
    }

}
