using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootInput : CommandInput
{
    Player_Attacks _attacks;

    public ShootInput(Player_Attacks attacks)
    {
        _attacks = attacks;   
    }

    public override void Execute()
    {
        _attacks.ShootBool();
    }

    
}
