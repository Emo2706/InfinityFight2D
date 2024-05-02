using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
public class FieldForce : NetworkBehaviour
{
    // Start is called before the first frame update

    public float pushForce;
    [SerializeField] int ID;
    public float liveTime;
    SpriteRenderer _SRforceField;
    [SerializeField] int _blinkTimes;
    [SerializeField] float _durationBetweenBlinks;
    Color _fieldColor;
    [SerializeField] Color _fieldColorOpaque;
    

    public override void Spawned()
    {
        _SRforceField = gameObject.GetComponent<SpriteRenderer>();
        _fieldColor = _SRforceField.material.color;
        gameObject.SetActive(false);
    }
    void Start()
    {
        
    }

   // [Rpc(RpcSources.All, RpcTargets.StateAuthority)]

   public void TurnFieldOn()
    {
       
        StartCoroutine(TurnFieldOnCorroutine());
    }

    //NO ME DEJA PONER RPC EN NINGUNO DE LOS 2
    //[Rpc(RpcSources.All, RpcTargets.StateAuthority)]

    
    IEnumerator TurnFieldOnCorroutine()
    {
        gameObject.SetActive(true);
        WaitForSeconds waitBlink = new WaitForSeconds(_durationBetweenBlinks);

        yield return new WaitForSeconds (liveTime);
        for (int i = 0; i < _blinkTimes; i++)
        {
            yield return waitBlink;
            _SRforceField.material.color = _fieldColorOpaque;
            yield return waitBlink;
            _SRforceField.material.color = _fieldColor;


        }
        gameObject.SetActive(false);


    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (!HasStateAuthority) return;

        if (collision.TryGetComponent(out Player player))
        {
            if (player.playerID != ID)
            {
                var dir = (player.transform.position - transform.position + Vector3.up * 4).normalized;
                player.GetComponent<Rigidbody2D>().AddForce(dir * pushForce, ForceMode2D.Impulse);
                //Empujar llamando un método del player y pasandolé como parametro la posición del force field
            }
            

            
            //Empujar al jugador solo si comparte id
        }
    }
    // Update is called once per frame
   
}
