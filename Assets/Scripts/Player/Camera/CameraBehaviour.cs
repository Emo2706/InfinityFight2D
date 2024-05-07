using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CameraBehaviour : MonoBehaviour
{
    Action CameraMovement;
    [SerializeField] float _cameraSpeed;
    [SerializeField] float _threshold;
    [SerializeField] CameraPointer _pointer;
    [HideInInspector]public Transform target;

    // Start is called before the first frame update
    void Start()
    {
        CameraMovement = Follow;
        
        EventManager.SubscribeToEvent(EventManager.EventsType.Event_PlayerDies, FollowBase);
    }

    public void SetParameters(Transform follow)
    {
        _pointer.player = follow;
        target = _pointer.transform;
    }

    public void FollowBase(params object[] parameters)
    {
        if ((int)parameters[0] == LocalPlayerDataManager.instance.LocalPlayer.playerID)
        {
            target = BaseManagers.instance.Bases[LocalPlayerDataManager.instance.LocalPlayer.playerID].transform;
        }
    }
    

    private void LateUpdate()
    {
        if (!_pointer.player) return;
        CameraMovement();
    }

    void Follow()
    {
        float ZOffset = -10;
        Vector3 targetOffset = new Vector3(target.position.x, target.position.y, ZOffset); 
        transform.position = Vector3.Lerp(transform.position, targetOffset, _cameraSpeed);
    }

    public void Shake(float duration , float magnitude)
    {
        StartCoroutine(ShakeCourotine(duration, magnitude));
    }

    public IEnumerator ShakeCourotine(float duration, float magnitude)
    {
        Vector3 originalPosition = transform.localPosition;

        float elapsed = 0.0f;

        while (elapsed < duration)
        {
            float x = UnityEngine.Random.Range(-1f, 1f) * magnitude;

            float y = UnityEngine.Random.Range(-1f, 1f) * magnitude;

            transform.localPosition = new Vector3(originalPosition.x, y, originalPosition.z);

            elapsed += Time.deltaTime;

            yield return null;

            Debug.Log("Shake");
        }

        transform.localPosition = originalPosition;
    }
}
