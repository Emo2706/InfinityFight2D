using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Player_Movement
{
    Player _player;
    int _jumpForce;
    float _xAxis = 0f;
    Rigidbody2D _rb;
    public bool jump = true;
    public event Action<float , int> OnMovement = delegate { };

    public Player_Movement(Player player , Rigidbody2D rb)
    {
        _player = player;
        _jumpForce = player.jumpForce;
        _rb = rb;
    }

   
 
    public void Update()
    {
        _xAxis = Input.GetAxisRaw("Horizontal");

        Move();

        LimitSpeed();

        if (_xAxis == 0) _rb.velocity *= Vector2.up;
    }

    void Move()
    {
        _rb.velocity += Vector2.right * _xAxis * (_player.speed * 100) * _player.Runner.DeltaTime;



        if (Input.GetKey(KeyCode.A))
            _player.transform.eulerAngles = new Vector3(0, 180, 0);


        if (Input.GetKey(KeyCode.D))
            _player.transform.eulerAngles = new Vector3(0, 0, 0);


        OnMovement(_xAxis, _player.playerID);
    }

    void LimitSpeed()
    {
        Vector2 vel = _rb.velocity;

        if(vel.x > _player.speed)
        {
            vel.x = _player.speed;
            _rb.velocity = vel;
        }

        else if (vel.x < -_player.speed)
        {
            vel.x = -_player.speed;
            _rb.velocity = vel;
        }
    }

    public void JumpBool()
    {
        if(jump)
        _player.hasToJump = true;
    }

    public void Jump()
    {
        _rb.velocity += Vector2.up * _jumpForce;
        jump = false;
    }
}
