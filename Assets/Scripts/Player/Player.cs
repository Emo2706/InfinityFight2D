using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
using UnityEngine.UI;
using System;

public class Player : NetworkBehaviour , IDamageable
{
    Rigidbody2D _rb;
    LineRenderer _Lr;
    [SerializeField] Pointer _pointer;
    [SerializeField] GameObject ShootSpawnpoint;

    [Header("Bools")]
    public bool hasToJump = false;
    public bool hasToShoot = false;
    public bool hasToGrenade = false;
    public bool hasToWall = false;

    [Header("Variables")]
    public int playerID;
    public int speed;
    [SerializeField] int _hp;
    [SerializeField] int _maxHp;
    public int jumpForce;
    public float shootDistance;
    public float throwForce;
    public float throwUpWardForce;
    public float throwCooldown;
    public float wallCooldown;

    public float delayToRespawn;

    [SerializeField] Laser _laser;

    
    Player_Movement _movement;
    Player_Inputs _inputs;
    Player_Collisions _collisions;
    Player_Attacks _attacks;

    [Header("Spawns")]
    [SerializeField] Grenade _grenadePrefab;
    [SerializeField] Wall _wallPrefab;
    public Transform spawnPointGrenade;


    [Header("UI")]
    public int grenadesAmount;
    public Image grenadeImg;

    public Action CurrentSpawnMethod;


    public override void Spawned()
    {
        _Lr = GetComponent<LineRenderer>();
        //addToPlayersList();
        EventManager.SubscribeToEvent(EventManager.EventsType.Event_StartGame, StartGameStateForPlayer);
        CurrentSpawnMethod = LobbySpawnMethod;

        if (!HasStateAuthority) return;
        HudManager.instance.IDPLAYER.text = Id.ToString();
        grenadeImg = HudManager.instance.grenadeImg;

        _hp = _maxHp;
        _rb = GetComponent<Rigidbody2D>();
        _movement = new Player_Movement(this, _rb);
        _inputs = new Player_Inputs();
        _collisions = new Player_Collisions(_movement);
        _attacks = new Player_Attacks(this, _grenadePrefab, _wallPrefab, ShootSpawnpoint , _laser, _Lr);
        Camera.main.GetComponent<CameraBehaviour>().SetParameters(this.gameObject.transform);
        HudManager.instance.playerRef = this;
        LocalPlayerDataManager.instance.SetLocalPlayer(this);
        EventManager.SubscribeToEvent(EventManager.EventsType.Event_PlayerDies, DespawnMethod);
        EventManager.SubscribeToEvent(EventManager.EventsType.Event_PlayerDies, SpawnTimer);

        _attacks.SetShootDmg(5);

        HudManager.instance.ChangeGrenadesCount(grenadesAmount);

        #region Inputs
        _inputs.BlindKeys(KeyCode.Space, new JumpInput(_movement));
        _inputs.BlindKeys(KeyCode.Mouse0, new ShootInput(_attacks));
        _inputs.BlindKeys(KeyCode.Mouse1, new GrenadeInput(_attacks));
        _inputs.BlindKeys(KeyCode.E, new WallInput(_attacks));
        #endregion 

    }

    /* No se puede con RPC, tira error
    void addToPlayersList()
    {
        GameManager.instance.playersList.Add(this);

    }*/

    void Update()
    {
        if (!HasStateAuthority) return;

        if (Object == null) return;

        CommandInput keypressed = _inputs.Inputs();
        if (keypressed != null)
        {
            keypressed.Execute();
        }

        Vector3 direction = new Vector3(_pointer.mousePos.x - transform.position.x, _pointer.mousePos.y - transform.position.y);

        transform.right = direction;

        if (grenadeImg.fillAmount < 1)
        {
            grenadeImg.fillAmount += Time.deltaTime / 10;
        }

    }
    
   

    public override void FixedUpdateNetwork()
    {
        if (!HasStateAuthority) return;

        if (Object == null) return;

        _movement.Update();

        if (hasToJump)
        {
            hasToJump = false;
            _movement.Jump();
        }

        if (hasToShoot)
        {
            hasToShoot = false;
            _attacks.Shoot();
        }

        if (hasToGrenade)
        {
            hasToGrenade = false;
            _attacks.Grenade();
        }

        if (hasToWall)
        {
            hasToWall = false;
            _attacks.Wall();
        }

    }

    [Rpc(RpcSources.All, RpcTargets.All)]
    public void TakeDmgRpc(int dmg , int ID)
    {
        if (!HasStateAuthority) return;
        
        if (ID == playerID) return;
       
        _hp -= dmg;
        AudioManager.instance.Play(AudioManager.Sounds.Dmg);
        CheckLife();
    }

    void CheckLife()
    {
        HudManager.instance.VidaTest.text = _hp.ToString();
        if (_hp <= 0)
            EventManager.TriggerEvent(EventManager.EventsType.Event_PlayerDies, playerID);
        
    }

    void DespawnMethod(params object[] parameters)
    {
        transform.position = new Vector3(-100, 100, transform.position.z);
        grenadesAmount = 2;
        HudManager.instance.ChangeGrenadesCount(grenadesAmount);
    }

    void SpawnTimer(params object[] parameters)
    {
        StartCoroutine(CorroutineSpawnTime());
    }
   

    IEnumerator CorroutineSpawnTime()
    {
        yield return new WaitForSeconds(delayToRespawn);
        CurrentSpawnMethod();
        _hp = _maxHp;

        Camera.main.GetComponent<CameraBehaviour>().SetParameters(this.gameObject.transform);
        EventManager.TriggerEvent(EventManager.EventsType.Event_PlayerSpawns, playerID);
    }



    public void InGameSpawnMethod()
    {
        transform.position = BaseManagers.instance.Bases[playerID].transform.position + BaseManagers.instance.Bases[playerID].offsetSpawnPJ;

    }
    void LobbySpawnMethod()
    {
        transform.position = GameManager.instance.LobbySpawnPoints[playerID].position;
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
         _collisions.OnCollisionEnter(collision);
        
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        _collisions.OnCollisionExit(collision);
    }

    [Rpc(RpcSources.All, RpcTargets.All)]
    public void LaserBeamRPC(float time, Vector2 startpos, Vector2 endPos)
    {
        //if (!HasStateAuthority) return;

        //Lo queriamos hacer en el constructor player.attacks pero al no haber RPCS lo tuvimos que hacer desde acá
        _Lr.SetPosition(0, startpos);
        _Lr.SetPosition(1, endPos);


        StartCoroutine(ActivateAndDesactiveLineRendererCorroutine(time, _Lr));
    }


    public void StartGameStateForPlayer(params object[] p)
    {
        Debug.Log("sexo");
        Debug.Log("sexo");
        Debug.Log("PAJERO ANDA");
        Debug.Log("sexo");
        Debug.Log("sexo");
        Debug.Log("sexo");
        InGameSpawnMethod();
        CurrentSpawnMethod = InGameSpawnMethod;
        _hp = _maxHp;
        grenadesAmount = 2;
        HudManager.instance.ChangeGrenadesCount(grenadesAmount);

       
        
    }

    

    IEnumerator ActivateAndDesactiveLineRendererCorroutine(float tim, LineRenderer _lr)
    {
        _lr.enabled = true;
        yield return new WaitForSeconds(tim);
        _lr.enabled = false;
    }


    /*[Rpc(RpcSources.All, RpcTargets.All)]
    public void TestRpc(Action VoidToExecuteRpc)
    {
        VoidToExecuteRpc();
    }*/
   

   



}
