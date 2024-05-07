using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;


public class Base : NetworkBehaviour, IDamageable
{
    [SerializeField] int _hp;
    [SerializeField] int ID;
    public Vector3 offsetSpawnPJ;
    public FieldForce forceField;
    bool _IsProtected = true;


    private void Start()
    {
        _IsProtected = true;
        EventManager.SubscribeToEvent(EventManager.EventsType.Event_PlayerDies, MyPlayerDies);

        EventManager.SubscribeToEvent(EventManager.EventsType.Event_PlayerSpawns, TurnFieldOn);
        EventManager.SubscribeToEvent(EventManager.EventsType.Event_PlayerSpawns, MyPlayerRespawns);

        HudManager.instance.MyBaseLifeSlider.maxValue = _hp;
        HudManager.instance.MyBaseLifeSlider.value = _hp;
        HudManager.instance.EnemyBaseLifeSlider.value = 0;
        HudManager.instance.EnemyBaseLifeSlider.maxValue = _hp;

    }

    void TurnFieldOn(params object[] parameters)
    {
        if ((int)parameters[0] == ID)
        {
            //forceField.gameObject.SetActive(true);
            //forceField.RPCTurnFieldOn();
        }

    }

    [Rpc(RpcSources.All, RpcTargets.All)]
    public void TakeDmgRpc(int dmg, int IDShooter)
    {
        if (IDShooter == ID) return;
        if (_IsProtected) return;
        _hp -= dmg;
        HudManager.instance.BaseReceivesDMG(_hp, ID);
        AudioManager.instance.Play(AudioManager.Sounds.BaseDmg);
        CheckBaseLife();
    }

    void CheckBaseLife()
    {
        if (_hp <= 0)
        {
            EventManager.TriggerEvent(EventManager.EventsType.Event_GameOver, ID);
        }
    }

    void MyPlayerDies(params object[] parameters)
    {
        if ((int)parameters[0] == ID)
            RPCProtectBase(false);

    }

    void MyPlayerRespawns(params object[] parameters)
    {
        if ((int)parameters[0] == ID)
            RPCProtectBase(true);
    }

    [Rpc(RpcSources.All, RpcTargets.All)]
    void RPCProtectBase(bool trueorfalse)
    {
        _IsProtected = trueorfalse;

    }





}
