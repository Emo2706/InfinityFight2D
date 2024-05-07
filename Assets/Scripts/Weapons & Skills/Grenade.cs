using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

public class Grenade : NetworkBehaviour
{
    [SerializeField] int _timeToExplote;

    [SerializeField] float _radius;
    
    [SerializeField] float _distance;
    
    [SerializeField] float _shakeMagnitude;
    
    [SerializeField] float _shakeDuration;

    [SerializeField] LayerMask _layerMask = 1 << 7;

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

        Camera.main.GetComponent<CameraBehaviour>().Shake(_shakeDuration, _shakeMagnitude);

        AudioManager.instance.Play(AudioManager.Sounds.Grenade);

        

        //Es get physics Scene2D decirle a maty de corregirlo 
        /*if (Runner.GetPhysicsScene2D().CircleCast(transform.position, _radius, Vector2.right , _distance))
        {
            IDamageable dmg = hit.transform.GetComponent<IDamageable>();

            if (dmg != null) dmg.TakeDmgRpc(_dmg, idPlayer);
        }*/

       var collider = Runner.GetPhysicsScene2D().OverlapCircle(transform.position, _radius , _layerMask).GetComponent<IDamageable>();

        if (collider!= null)
        {
            collider.TakeDmgRpc(_dmg, idPlayer);
            Debug.Log("Le exploto la granada");
            Debug.Log(idPlayer);
        }

        Runner.Despawn(Object);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        Gizmos.DrawWireSphere(transform.position, _radius);
    }
}
