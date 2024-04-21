using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Base : MonoBehaviour , IDamageable
{
   [SerializeField] int _hp;

    public void TakeDmg(int dmg)
    {
        _hp -= dmg;
    }

}
