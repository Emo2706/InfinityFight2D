using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

public class GameManager : NetworkBehaviour
{
    public static GameManager instance;
    [HideInInspector]public int playersReady;
    public List<Transform> spawnPoints = new List<Transform>();
    public Transform[] LobbySpawnPoints;

    public List<Player> playersList = new List<Player>();
    // Start is called before the first frame update
    private void Awake()
    {
       if (!HasStateAuthority) return;
        if (instance == null)
        {
            
            instance = this;

        }
    }
    void Start()
    {
        EventManager.SubscribeToEvent(EventManager.EventsType.Event_StartGame, AllPlayersReady);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

   
    public void PlayerReady(bool TrueorNot)
    {
        if (TrueorNot)
        {
            playersReady++;
            CheckPlayersReady(playersReady);
        }
        else playersReady--;
    }

    ///[Rpc(RpcSources.All, RpcTargets.All)] No se puede con RPC, tira error, COMO HAGO?
    public void CheckPlayersReady(int playersID)
    {
       if (!HasStateAuthority) return;
        if (playersID >= 1) 
        {
            EventManager.TriggerEvent(EventManager.EventsType.Event_StartGame);
            foreach (var item in playersList)
            {
                item.InGameSpawnMethod();
            }
            LocalPlayerDataManager.instance.LocalPlayer.InGameSpawnMethod();
        }
        
    }
   
    public void AllPlayersReady(params object[] p)
    {
        foreach (var player in playersList)
        {
            player.StartGameStateForPlayer();
        }
    }



    
    

}
