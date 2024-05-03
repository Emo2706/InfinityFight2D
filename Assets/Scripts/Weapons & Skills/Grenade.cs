using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

public class Grenade : NetworkBehaviour
{
    [SerializeField] int _timeToExplote;

    [SerializeField] float _radius;

    [SerializeField] LayerMask _layerMask;

    [SerializeField] int _dmg;

    public int idPlayer;

    Collider[] collidersGrenade = new Collider[5];

    public override void Spawned()
    {
        StartCoroutine(Explosion());
    }

    IEnumerator Explosion()
    {
        yield return new WaitForSeconds(_timeToExplote);

        AudioManager.instance.Play(AudioManager.Sounds.Grenade);

        RaycastHit hit;

        //Es get physics Scene2D decirle a maty de corregirlo 
        if (Runner.GetPhysicsScene().SphereCast(transform.position, _radius, Vector3.right, out hit))
        {
            IDamageable dmg = hit.transform.GetComponent<IDamageable>();

            if (dmg != null) dmg.TakeDmgRpc(_dmg, idPlayer);
        }

        Runner.Despawn(Object);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        Gizmos.DrawWireSphere(transform.position, _radius);
    }
}
