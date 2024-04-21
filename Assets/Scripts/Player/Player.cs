using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

public class Player : NetworkBehaviour , IDamageable
{
    Rigidbody2D _rb;
    [SerializeField] Pointer _pointer;

    [Header("Bools")]
    public bool hasToJump = false;
    public bool hasToShoot = false;
    public bool hasToGrenade = false;
    public bool hasToWall = false;

    [Header("Variables")]
    public int speed;
    [SerializeField] int _hp;
    public int jumpForce;
    public float shootDistance;
    public float throwForce;
    public float throwUpWardForce;
    public float throwCooldown;
    public float wallCooldown;


    
    Player_Movement _movement;
    Player_Inputs _inputs;
    Player_Collisions _collisions;
    Player_Attacks _attacks;

    [Header("Spawns")]
    [SerializeField] Grenade _grenadePrefab;
    [SerializeField] Wall _wallPrefab;
    public Transform spawnPointGrenade;


    // Start is called before the first frame update
    void Start()
    {
        _movement = new Player_Movement(this);
        _inputs = new Player_Inputs();
        _collisions = new Player_Collisions(_movement);
        _attacks = new Player_Attacks(this , _grenadePrefab , _wallPrefab);

        _rb = GetComponent<Rigidbody2D>();
        _movement.CompleteData(_rb);

        #region Inputs
        _inputs.BlindKeys(KeyCode.Space, new JumpInput(_movement));
        _inputs.BlindKeys(KeyCode.Mouse0, new ShootInput(_attacks));
        _inputs.BlindKeys(KeyCode.Mouse1, new GrenadeInput(_attacks));
        _inputs.BlindKeys(KeyCode.E, new WallInput(_attacks));
        #endregion 
    }

    public override void Spawned()
    {
       /* _rb = GetComponent<Rigidbody>();
        _movement.CompleteData(_rb);*/
    }

    // Update is called once per frame
    void Update()
    {
        CommandInput keypressed = _inputs.Inputs();
        if (keypressed != null)
        {
            keypressed.Execute();
        }

        Vector3 direction = new Vector3(_pointer.mousePos.x - transform.position.x, _pointer.mousePos.y - transform.position.y);

        transform.right = direction;

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

    public void TakeDmg(int dmg)
    {
        _hp -= dmg;
    }

    void CheckLife()
    {
        if (_hp <= 0)
            Runner.Despawn(Object);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
         _collisions.OnCollisionEnter(collision);
        
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        _collisions.OnCollisionExit(collision);
    }
}
