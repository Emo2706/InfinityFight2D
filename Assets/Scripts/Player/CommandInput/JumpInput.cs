using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpInput : CommandInput
{
    Player_Movement _movement;

    public JumpInput(Player_Movement movement)
    {
        _movement = movement;
    }

    public override void Execute()
    {
         _movement.JumpBool();
    }
    
}
