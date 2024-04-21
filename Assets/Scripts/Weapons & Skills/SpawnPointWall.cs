using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPointWall : MonoBehaviour
{
    [SerializeField] Transform _player;
    // Update is called once per frame
    void Update()
    {
        
        transform.rotation = Quaternion.Euler(_player.transform.right.x, 0, 0);
    }
}
