using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

public class PlayerSpawner : SimulationBehaviour , IPlayerJoined
{
    [SerializeField] Player playerPref;
    public List<Transform> spawnPoints = new List<Transform>();

    public void PlayerJoined(PlayerRef player)
    {
        if (player == Runner.LocalPlayer)
        {
            int currentPlayer = 0;

            foreach (var item in Runner.ActivePlayers)
            {
                if (item == player) break;
                currentPlayer++;
            }

            Vector3 spawnPosition = spawnPoints.Count - 1 <= currentPlayer ? Vector3.zero : spawnPoints[currentPlayer].position;

            Runner.Spawn(playerPref, spawnPosition, Quaternion.identity);
        }
        
    }

   
}
