using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Background : MonoBehaviour
{
    [SerializeField] float _lerp;
    // Update is called once per frame
    void Update()
    {
        transform.position =  Vector2.Lerp(transform.position, Camera.main.transform.position, _lerp);
    }
}
