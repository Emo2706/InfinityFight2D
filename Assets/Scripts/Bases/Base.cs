using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;


public class Base : NetworkBehaviour , IDamageable
{
   [SerializeField] int _hp;
    [SerializeField] int ID;
    public Vector3 offsetSpawnPJ;
    public FieldForce forceField;
    

    private void Start()
    {
        EventManager.SubscribeToEvent(EventManager.EventsType.Event_PlayerSpawns, TurnFieldOn);
    }

    void TurnFieldOn(params object[] parameters)
    {
        if ((int)parameters[0] == ID)
            forceField.gameObject.SetActive(true);
            forceField.TurnFieldOn();
    }

    
    private void Update()
    {
      
    }

    [Rpc(RpcSources.All, RpcTargets.All)]
    public void TakeDmgRpc(int dmg, int IDShooter)
    {
        if (IDShooter == ID) return;
        _hp -= dmg;
        HudManager.instance.BaseReceivesDMG(_hp, ID);
    }

   

    
}
