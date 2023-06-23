using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    private Vector3 _offset;
    [SerializeField] private Transform target;
    [SerializeField] private float smoothTime;
    private Vector3 _currentVelocity = Vector3.zero;


    // Object Visibility
    private ObjectVisibility _fader;
    private GameObject player;

    private void Awake()
    {
        _offset = transform.position - target.position;
        player = GameObject.FindGameObjectWithTag("Player");
    }


    private void Update()
    {

        if (player != null)
        {
            Vector3 _direction = player.transform.position - transform.position;
            Ray ray = new Ray(transform.position, _direction);
            RaycastHit hit;


            if (Physics.Raycast(ray, out hit))
            {
                Debug.LogWarning("1. if içerisine girildi...");
                if (hit.collider == null)
                {
                    Debug.LogWarning("2. if içerisine girildi...");
                    return;
                }
                if (hit.collider.gameObject == player)
                {
                    Debug.LogWarning("3. if içerisine girildi...");

                    if (_fader != null)
                    {
                        Debug.Log("Fade iþlemi yapýlamadý !");
                        _fader.doFade = false;
                    }
                    else
                    {
                        Debug.LogWarning("else içerisine girdi...");
                        _fader = hit.collider.gameObject.GetComponent<ObjectVisibility>();

                        if (_fader != null)
                        {
                            Debug.Log("Fade Ýþlemi Yapýldý !");
                            _fader.doFade = true;
                        }
                    }
                  
                }
            }
        }
    }

    private void LateUpdate()
    {
        Vector3 targetPosition = target.position + _offset;
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref _currentVelocity, smoothTime);
    }

}
