using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

public class Wall : NetworkBehaviour, IDamageable
{
    [SerializeField] int _hp;
    [SerializeField] int _lifeTime;
    float _lifeTimer;
    public int IDPlayer;


    public void TakeDmgRpc(int dmg , int ID)
    {
        if (IDPlayer == ID) return;
        _hp -= dmg;
    }

    private void Update()
    {
        _lifeTimer += Time.deltaTime;

        if (_lifeTimer >= _lifeTime)
            Runner.Despawn(Object);
    }


}
