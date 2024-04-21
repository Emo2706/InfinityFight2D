using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Attacks
{
    Player _player;
    Grenade _grenadePrefab;
    Wall _wall;
    float _shootDistance;
    float _throwForce;
    float _throwUpWardForce;
    float _throwCooldown;
    float _wallCooldown;
    bool _throw = true;
    int _shootDmg;
    bool _wallActivate = true;
    float _offsetX = 0.92f;


    public Player_Attacks(Player player, Grenade grenadePrefab , Wall wall)
    {
        _player = player;
        _shootDistance = player.shootDistance;
        _grenadePrefab = grenadePrefab;
        _throwForce = player.throwForce;
        _throwUpWardForce = player.throwUpWardForce;
        _throwCooldown = player.throwCooldown;
        _wallCooldown = player.wallCooldown;
        _wall = wall;
    }

    public void ShootBool()
    {
        _player.hasToShoot = true;
    }

    public void Shoot()
    {
        RaycastHit hit;
        if (_player.Runner.GetPhysicsScene().Raycast(_player.transform.position, _player.transform.forward, out hit, _shootDistance))
        {
            IDamageable dmg = hit.transform.GetComponent<IDamageable>();

            if (dmg != null) dmg.TakeDmg(_shootDmg);
        }

        Debug.Log(hit);

    }

    public void GrenadeBool()
    {
        _player.hasToGrenade = true;
    }

    public void Grenade()
    {
        if(_throw== true)
        {
            _throw = false;

            Grenade grenade = _player.Runner.Spawn(_grenadePrefab, _player.spawnPointGrenade.position);

            Rigidbody2D grenadeRb = grenade.GetComponent<Rigidbody2D>();

            Vector2 force = _player.transform.right * _throwForce + _player.transform.up * _throwUpWardForce;

            grenadeRb.AddForce(force, ForceMode2D.Impulse);

            _player.StartCoroutine(ResetThrow());
        }
    }

    IEnumerator ResetThrow()
    {
        yield return new WaitForSeconds(_throwCooldown);

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

           _player.Runner.Spawn(_wall, _player.transform.position + new Vector3((_offsetX * _player.transform.right.x),0));

           _player.StartCoroutine(ResetWall());
        }
    }


    IEnumerator ResetWall()
    {
        yield return new WaitForSeconds(_wallCooldown);

        _wallActivate = true;
    }
}
