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
            int currentPlayer = -1;

            foreach (var item in Runner.ActivePlayers)
            {
                currentPlayer++;
            }
            

            Vector3 spawnPosition = spawnPoints.Count - 1 < currentPlayer ? Vector3.zero : spawnPoints[currentPlayer].position;

            Player playerSpawned = Runner.Spawn(playerPref, spawnPosition, Quaternion.identity);
            playerSpawned.playerID = currentPlayer;
            //HudManager.instance.PlayerIDAssign(currentPlayer);

            Debug.Log(currentPlayer);
        }
        
    }

   
}
