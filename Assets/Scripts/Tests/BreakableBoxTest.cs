using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
public class BreakableBoxTest : NetworkBehaviour, IDamageable
{
    [Rpc(RpcSources.All, RpcTargets.All)]

    public void TakeDmgRpc(int dmg, int lootro)
    {
        Runner.Despawn(Object);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
