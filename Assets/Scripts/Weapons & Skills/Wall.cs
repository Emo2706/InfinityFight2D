using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

public class Wall : NetworkBehaviour, IDamageable
{
    [SerializeField] int _hp;
    [SerializeField] int _lifeTime;
    float _lifeTimer;

    public void TakeDmg(int dmg)
    {
        _hp -= dmg;
    }

    private void Update()
    {
        _lifeTimer += Time.deltaTime;

        if (_lifeTimer >= _lifeTime)
            Runner.Despawn(Object);
    }


}
