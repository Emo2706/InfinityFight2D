using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutOfBoundsBox : MonoBehaviour
{
    [SerializeField] Transform respawnPos;
    // Start is called before the first frame update
    private void OnTriggerEnter2D(Collider2D collision)
    {
       
        if (collision.gameObject.TryGetComponent(out Player p))
        {
            p.gameObject.transform.position = respawnPos.position;
        }
    }
}
