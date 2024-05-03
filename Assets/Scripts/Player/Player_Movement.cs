using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Player_Movement
{
    Player _player;
    int _speed;
    int _jumpForce;
    float _xAxis = 0f;
    Rigidbody2D _rb;
    public bool jump = true;

    public Player_Movement(Player player , Rigidbody2D rb)
    {
        _player = player;
        _speed = player.speed;
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
        _rb.velocity += Vector2.right * _xAxis * (_speed * 100) * _player.Runner.DeltaTime;
    }

    void LimitSpeed()
    {
        Vector2 vel = _rb.velocity;

        if(vel.x > _speed)
        {
            vel.x = _speed;
            _rb.velocity = vel;
        }

        else if (vel.x < -_speed)
        {
            vel.x = -_speed;
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
