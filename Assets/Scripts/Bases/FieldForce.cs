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
        if (!HasStateAuthority) return;

        _SRforceField = gameObject.GetComponent<SpriteRenderer>();
        _fieldColor = _SRforceField.material.color;
        RPCShieldGameObject(false);


    }
    void Start()
    {
        
    }

   [Rpc(RpcSources.All, RpcTargets.StateAuthority)]

   public void RPCTurnFieldOn()
    {
        if (!HasStateAuthority) return;

        StartCoroutine(TurnFieldOnCorroutine());
    }

   

    
    IEnumerator TurnFieldOnCorroutine()
    {
        RPCShieldGameObject(true);

        WaitForSeconds waitBlink = new WaitForSeconds(_durationBetweenBlinks);

        yield return new WaitForSeconds (liveTime);
        for (int i = 0; i < _blinkTimes; i++)
        {
            yield return waitBlink;
            _SRforceField.material.color = _fieldColorOpaque;
            yield return waitBlink;
            _SRforceField.material.color = _fieldColor;


        }
        RPCShieldGameObject(false);


    }
  
    [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
    void RPCShieldGameObject(bool onof)
    {
        if (!HasStateAuthority) return;

        gameObject.SetActive(onof);
    }
    

    private void OnTriggerStay2D(Collider2D collision)
    {
        //if (!HasStateAuthority) return;

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
