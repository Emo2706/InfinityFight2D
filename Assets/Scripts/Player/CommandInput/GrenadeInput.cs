using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrenadeInput : CommandInput
{
    Player_Attacks _attacks;

    public GrenadeInput(Player_Attacks attacks)
    {
        _attacks = attacks;
    }

    public override void Execute()
    {
        _attacks.GrenadeBool();
    }
}
