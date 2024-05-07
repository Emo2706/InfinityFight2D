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
    public bool blue = false;

    [Header("Variables")]
    public int playerID;
    public int speed;
    [SerializeField] int _hp;
    [SerializeField] int _maxHp;
    [SerializeField] int _shootDmg;
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
    PlayerView _view;

    [Header("Spawns")]
    [SerializeField] Grenade _grenadePrefab;
    [SerializeField] Wall _wallPrefab;
    public Transform spawnPointGrenade;


    [Header("UI")]
    public int grenadesAmount;
    public Image grenadeImg;
    public Image wallImg;

    public Action CurrentSpawnMethod;

    public event Action OnSpeedUp = delegate { };
    public event Action OnDmgUp = delegate { };
    public event Action OnThrowCooldownDown = delegate { };
    public event Action OnShootDistanceUp = delegate { };
    public event Action OnWallCooldownDown = delegate { };

    List<Action> _buffs = new List<Action>();

    public override void Spawned()
    {
        _Lr = GetComponent<LineRenderer>();
        //addToPlayersList();
        EventManager.SubscribeToEvent(EventManager.EventsType.Event_StartGame, StartGameStateForPlayer);
        CurrentSpawnMethod = LobbySpawnMethod;

        if (!HasStateAuthority) return;
        HudManager.instance.IDPLAYER.text = Id.ToString();
        grenadeImg = HudManager.instance.grenadeImg;
        wallImg = HudManager.instance.wallImg;

        _hp = _maxHp;
        _rb = GetComponent<Rigidbody2D>();
        _movement = new Player_Movement(this, _rb);
        _inputs = new Player_Inputs();
        _collisions = new Player_Collisions(_movement);
        _attacks = new Player_Attacks(this, _grenadePrefab, _wallPrefab, ShootSpawnpoint , _laser, _Lr);
        _view = new PlayerView(this);
        Camera.main.GetComponent<CameraBehaviour>().SetParameters(this.gameObject.transform);
        HudManager.instance.playerRef = this;
        LocalPlayerDataManager.instance.SetLocalPlayer(this);
        EventManager.SubscribeToEvent(EventManager.EventsType.Event_PlayerDies, DespawnMethod);
        EventManager.SubscribeToEvent(EventManager.EventsType.Event_PlayerDies, SpawnTimer);

        _attacks.SetShootDmg(_shootDmg);

        _attacks.OnThrow += _view.ChangeGrenades;
        _attacks.OnThrow += _view.DrainFill;
        _attacks.OnWall += _view.DrainFillWall;
        _movement.OnMovement += _view.MovementAnimation;
        

        #region Inputs
        _inputs.BlindKeys(KeyCode.Space, new JumpInput(_movement));
        _inputs.BlindKeys(KeyCode.Mouse0, new ShootInput(_attacks));
        _inputs.BlindKeys(KeyCode.Mouse1, new GrenadeInput(_attacks));
        _inputs.BlindKeys(KeyCode.E, new WallInput(_attacks));
        #endregion 

        _view.Start();
        Debug.Log(playerID);

        #region Buffs

        OnSpeedUp += SpeedUp;
        OnSpeedUp += _view.SpeedUpEffect;

        OnDmgUp += DmgUp;
        OnDmgUp += _view.DmgUpEffect;

        OnShootDistanceUp += ShootDistanceUp;
        OnShootDistanceUp += _view.ShootDistanceUpEffect;

        OnThrowCooldownDown += LessThrowCooldown;
        OnThrowCooldownDown += _view.ThrowCooldownDownEffect;

        OnWallCooldownDown += LessWallCooldown;
        OnWallCooldownDown += _view.WallCooldownDownEffect;

        _buffs.Add(OnSpeedUp);
        _buffs.Add(OnDmgUp);
        _buffs.Add(OnShootDistanceUp);
        _buffs.Add(OnThrowCooldownDown);
        _buffs.Add(OnWallCooldownDown);
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

       /* Vector3 direction = new Vector3(_pointer.mousePos.x - transform.position.x, _pointer.mousePos.y - transform.position.y);

        transform.right = direction;*/

        if (grenadeImg.fillAmount < 1)
        {
            _view.RechargeFill();
        }

        if (wallImg.fillAmount < 1)
        {
            _view.RechargeFillWall();
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
        grenadesAmount = 3;
        _view.ChangeGrenades();
        Buff();

    }

    void Buff()
    {
        var chance = UnityEngine.Random.Range(0, _buffs.Count + 1);

        _buffs[chance]();
    }

    void SpawnTimer(params object[] parameters)
    {
        StartCoroutine(CorroutineSpawnTime());
    }
   

    void SpeedUp()
    {
        speed += 2;

        Debug.Log("Speed up");
    }

    void ShootDistanceUp()
    {
        shootDistance += 1;

        Debug.Log("Distance up");
    }

    void LessThrowCooldown()
    {
        throwCooldown -= 2;

        Debug.Log("GrenadeCooldown down");
    }

    void DmgUp()
    {
        _attacks.SetShootDmg(_shootDmg+=1);

        Debug.Log("Damage up");
    }

    void LessWallCooldown()
    {
        wallCooldown -= 1;

        Debug.Log("WallCooldown down");
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
        grenadesAmount = 3;
        _view.SetColor(blue);

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
