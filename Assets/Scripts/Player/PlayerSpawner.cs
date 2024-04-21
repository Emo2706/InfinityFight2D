using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

public class PlayerSpawner : SimulationBehaviour , IPlayerJoined
{
    [SerializeField] Player playerPref;

    public void PlayerJoined(PlayerRef player)
    {
        if (player == Runner.LocalPlayer)
        {
            Runner.Spawn(playerPref, new Vector3(0, 1, 0), Quaternion.identity);
        }
        
    }

   
}
