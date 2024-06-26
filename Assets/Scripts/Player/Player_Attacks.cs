using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Fusion;
using System;

public class Player_Attacks
{
    Player _player;
    Grenade _grenadePrefab;
    Wall _wall;
    GameObject _shootSpawn;
    float _throwForce;
    float _throwUpWardForce;
    bool _throw = true;
    int _shootDmg;
    bool _wallActivate = true;
    float _offsetX = 0.92f;
    float _laserDuration = 0.05f;
    Laser _laser;
    
    LineRenderer _lr;
    BoxCollider2D _bc;

    public event Action OnThrow = delegate { };
    public event Action OnResetThrow = delegate { };
    public event Action OnWall = delegate { };

    public Player_Attacks(Player player, Grenade grenadePrefab, Wall wall, GameObject shootSpawnpoint, Laser laser, LineRenderer Line, BoxCollider2D box)
    {
        _player = player;
        _grenadePrefab = grenadePrefab;
        _throwForce = player.throwForce;
        _throwUpWardForce = player.throwUpWardForce;
        _wall = wall;
        _shootSpawn = shootSpawnpoint;
        _laser = laser;
        _lr = Line;
        _bc = box;
    }
    public void SetShootDmg(int dmg)
    {
        _shootDmg = dmg;
    }
    public void ShootBool()
    {
        _player.hasToShoot = true;

        //_player.TestRpc(Shoot);
    }


    //[Rpc(RpcSources.All, RpcTargets.All)]
    public void Shoot()
    {
        AudioManager.instance.Play(AudioManager.Sounds.Shoot);

        Vector2 endpost;
       // _lr.SetPosition(0, _shootSpawn.gameObject.transform.position);
        var collider =_player.Runner.GetPhysicsScene2D().Raycast(_shootSpawn.gameObject.transform.position, _player.transform.right.normalized, _player.shootDistance);
        _player.StartCoroutine(LaserShoot(collider));
        if (collider == true)
        {
            IDamageable objectdmged = collider.collider.gameObject.GetComponent<IDamageable>();
            Debug.Log(collider.collider.gameObject.name.ToString() + " fue golpeado");
            endpost = collider.point;

            if (objectdmged != null)
            {
                objectdmged.TakeDmgRpc(_shootDmg, _player.playerID);
            }

            //_lr.SetPosition(1, collider.point);



        }
        else endpost = _shootSpawn.gameObject.transform.position + _shootSpawn.gameObject.transform.right * _player.shootDistance;
        //else _lr.SetPosition(1, _shootSpawn.gameObject.transform.position + _shootSpawn.gameObject.transform.right * _shootDistance);

        var startPos = new Vector2(_shootSpawn.gameObject.transform.position.x, _shootSpawn.gameObject.transform.position.y);
       
        _player.LaserBeamRPC(_laserDuration ,startPos, endpost);



    }


   
    IEnumerator LaserShoot(RaycastHit2D collider)
    {

        _lr.enabled = true;
        //yield return new WaitForSeconds(_laserDuration);
        yield return new WaitForSeconds(10);
        _lr.enabled = false;
        /*var laserShoot =  _player.Runner.Spawn(_laser, _shootSpawn.transform.position , _player.transform.rotation);


         yield return new WaitForSeconds(_laserDuration);

         _player.Runner.Despawn(laserShoot);*/
    }

    public void GrenadeBool()
    {
        _player.hasToGrenade = true;
    }


    
    public void Grenade()
    {
        if(_throw== true && _player.grenadesAmount>0)
        {
            _throw = false;

            _player.grenadesAmount--;

            OnThrow();

            HudManager.instance.ChangeGrenadesCount(_player.grenadesAmount);

            Grenade grenade = _player.Runner.Spawn(_grenadePrefab, _player.spawnPointGrenade.position);

            grenade.idPlayer = _player.playerID;

            Rigidbody2D grenadeRb = grenade.GetComponent<Rigidbody2D>();

            Vector2 force = _player.transform.right * _throwForce + _player.transform.up * _throwUpWardForce;

            grenadeRb.AddForce(force, ForceMode2D.Impulse);

            _player.StartCoroutine(ResetThrow());
        }
    }

    IEnumerator ResetThrow()
    {
       
        yield return new WaitForSeconds(_player.throwCooldown);

        _throw = true;
    }

    

    public void WallBool()
    {
        _player.hasToWall = true;
    }

    public void Wall()
    {
        if (_wallActivate)
        {
           _wallActivate = false;

            Wall wallInstance = _player.Runner.Spawn(_wall, _player.transform.position + new Vector3((_offsetX * _player.transform.right.x),0));
            wallInstance.IDPlayer = _player.playerID;
            OnWall();
           _player.StartCoroutine(ResetWall());
        }
    }

    
    IEnumerator ResetWall()
    {
        yield return new WaitForSeconds(_player.wallCooldown);

        _wallActivate = true;
    }
}
