using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Fusion;


public class StartButton : NetworkObject
{
    [SerializeField]TMP_Text playersReadyText;
    
    [Networked, OnChangedRender(nameof(playersReadyChanged))]
    int playersReady { get; set; }

    void OnPlayersReadyChanged() => CheckPlayersReady();
    // Start is called before the first frame update
    void Start()
    {
        
    }

    void playersReadyChanged(int players) => playersReady = players;
    

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerStay2D(Collider2D collision)
    {



    }

    [Rpc(RpcSources.All, RpcTargets.All)]

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<Player>() == null) return;
        //CheckPlayersReadyRpc(true);
        //GameManager.instance.PlayerReady(true);

        playersReady++;
        playersReadyChanged(playersReady);
        playersReadyText.text = playersReady.ToString() + "/2";


    }


    [Rpc(RpcSources.All, RpcTargets.All)]

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<Player>() == null) return;
        //GameManager.instance.PlayerReady(false);
        //CheckPlayersReady(false);

        playersReadyText.text = playersReady.ToString() + "/2";

    }

    
    void CheckPlayersReadyRpc(bool trueornot)
    {
        if (trueornot) playersReady++;
        else playersReady--;

    }

    void CheckPlayersReady()
    {
        if (playersReady >= 2) EventManager.TriggerEvent(EventManager.EventsType.Event_StartGame);

    }
}
