using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Collisions 
{
    Player_Movement _movement;

    public Player_Collisions(Player_Movement movement)
    {
        _movement = movement;
    }

    public void OnCollisionEnter(Collision2D collision)
    {
        if (collision.gameObject.layer == 6)
            _movement.jump = true;
    }

    public void OnCollisionExit(Collision2D collision)
    {
        if (collision.gameObject.layer == 6)
            _movement.jump = false;
    }
}
