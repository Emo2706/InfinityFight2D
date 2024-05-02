using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraPointer : MonoBehaviour
{
    Camera _cam;
    public Transform player;
    [SerializeField] float threshhold;
    [SerializeField] float yLimitIncreaser;
    // Start is called before the first frame update
    void Start()
    {
        _cam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        if (!player) return;
        Vector3 mousePos = _cam.ScreenToWorldPoint(Input.mousePosition);
        Vector3 direction = (mousePos - player.position);
        if (direction.magnitude > threshhold)
        {
            direction = direction.normalized * threshhold;
        }
        transform.position = new Vector3(player.position.x + direction.x, (player.position.y + direction.y)* yLimitIncreaser);
    }
}
