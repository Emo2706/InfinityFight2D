using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerView
{
    Player _player;
    Animator _anim;

    public PlayerView(Player player)
    {
        _player = player;
        _anim = player.GetComponentInChildren<Animator>();
    }

    public void Start()
    {
        HudManager.instance.ChangeGrenadesCount(_player.grenadesAmount);

        _anim.SetBool("Blue", _player.blue);
    }


    public void ChangeGrenades()
    {
        HudManager.instance.ChangeGrenadesCount(_player.grenadesAmount);
    }

    public void DrainFill()
    {
        _player.grenadeImg.fillAmount = 0;
    }

    public void RechargeFill()
    {
        _player.grenadeImg.fillAmount += Time.deltaTime / _player.throwCooldown;
    }

    public void DrainFillWall()
    {
        _player.wallImg.fillAmount = 0;
    }

    public void RechargeFillWall()
    {
        _player.wallImg.fillAmount += Time.deltaTime / _player.wallCooldown;
    }

    public void MovementAnimation(float axis , int playerID)
    {
        
        if (playerID == 0)
            _anim.SetBool("IsMovingRed", axis != 0);

        else if (playerID == 1)
            _anim.SetBool("IsMovingBlue", axis != 0);

        
    }
}
