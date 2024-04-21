using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallInput : CommandInput
{
    Player_Attacks _attacks;

    public WallInput(Player_Attacks attacks)
    {
        _attacks = attacks;
    }

    public override void Execute()
    {
        _attacks.WallBool();
    }

    
}
