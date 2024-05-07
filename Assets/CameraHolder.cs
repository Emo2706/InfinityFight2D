using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraHolder : MonoBehaviour
{
    [SerializeField] Transform _cam;

    // Update is called once per frame
    void Update()
    {
        transform.position = _cam.position;
    }
}
