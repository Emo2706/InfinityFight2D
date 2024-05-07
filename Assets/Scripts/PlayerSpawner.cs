using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
using System.Linq;

public class PlayerSpawner : SimulationBehaviour , IPlayerJoined
{
    [SerializeField] Player playerPref;
    public List<Transform> spawnPoints = new List<Transform>();

    public void PlayerJoined(PlayerRef player)
    {
        int currentPlayer = -1;

        if (player == Runner.LocalPlayer)
        {


            foreach (var item in Runner.ActivePlayers)
            {
                currentPlayer++;
            }
            Vector3 spawnPosition = spawnPoints.Count - 1 < currentPlayer ? Vector3.zero : spawnPoints[currentPlayer].position;

            Player playerSpawned = Runner.Spawn(playerPref, spawnPosition, Quaternion.identity);
     
            playerSpawned.playerID = currentPlayer;


            playerSpawned.blue = currentPlayer == 1 ? true : false;
            //playerSpawned.gameObject.transform.position = GameManager.instance.LobbySpawnPoints[playerSpawned.playerID].position;

            

            //HudManager.instance.PlayerIDAssign(currentPlayer);

        }
        RPC_StartGame(currentPlayer); 

    }
    //[Rpc(RpcSources.All, RpcTargets.All)] no te deja
    void RPC_StartGame(int currentplayer)
    {
        if (currentplayer >= 1)
        {
            EventManager.TriggerEvent(EventManager.EventsType.Event_StartGame);
        }
    }

   
}
