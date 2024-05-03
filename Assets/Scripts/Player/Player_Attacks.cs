using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player_Attacks
{
    Player _player;
    Grenade _grenadePrefab;
    Wall _wall;
    GameObject _shootSpawn;
    float _shootDistance;
    float _throwForce;
    float _throwUpWardForce;
    float _throwCooldown;
    float _wallCooldown;
    bool _throw = true;
    int _shootDmg;
    bool _wallActivate = true;
    float _offsetX = 0.92f;
    float _laserDuration = 0.05f;
    Laser _laser;
    Image _grenadeImg;

    public Player_Attacks(Player player, Grenade grenadePrefab, Wall wall, GameObject shootSpawnpoint, Laser laser)
    {
        _player = player;
        _shootDistance = player.shootDistance;
        _grenadePrefab = grenadePrefab;
        _throwForce = player.throwForce;
        _throwUpWardForce = player.throwUpWardForce;
        _throwCooldown = player.throwCooldown;
        _wallCooldown = player.wallCooldown;
        _wall = wall;
        _shootSpawn = shootSpawnpoint;
        _laser = laser;
        _grenadeImg = player.grenadeImg;
    }
    public void SetShootDmg(int dmg)
    {
        _shootDmg = dmg;
    }
    public void ShootBool()
    {
        _player.hasToShoot = true;
    }

    public void Shoot()
    {
        AudioManager.instance.Play(AudioManager.Sounds.Shoot);

        _player.StartCoroutine(LaserShoot());

        var collider = _player.Runner.GetPhysicsScene2D().Raycast(_shootSpawn.gameObject.transform.position, _player.transform.right.normalized, _shootDistance);
        if (collider == true)
        {
            IDamageable objectdmged = collider.collider.gameObject.GetComponent<IDamageable>();
            Debug.Log(collider.collider.gameObject.name.ToString() + " fue golpeado");

            if (objectdmged != null) objectdmged.TakeDmgRpc(_shootDmg , _player.playerID);
        }


    }


    
    IEnumerator LaserShoot()
    {
       var laserShoot =  _player.Runner.Spawn(_laser, _shootSpawn.transform.position , _player.transform.rotation);

        yield return new WaitForSeconds(_laserDuration);

        _player.Runner.Despawn(laserShoot);
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

            _grenadeImg.fillAmount = 0;

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

            Wall wallInstance = _player.Runner.Spawn(_wall, _player.transform.position + new Vector3((_offsetX * _player.transform.right.x),0));
            wallInstance.IDPlayer = _player.playerID;
           _player.StartCoroutine(ResetWall());
        }
    }

    
    IEnumerator ResetWall()
    {
        yield return new WaitForSeconds(_wallCooldown);

        _wallActivate = true;
    }
}
