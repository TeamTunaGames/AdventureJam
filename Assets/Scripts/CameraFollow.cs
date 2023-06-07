using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform cameraFocus;

    [Tooltip("How far back on the z-axis the camera will go before stopping"), SerializeField]
    private float backWall;
    [SerializeField] private float distanceFromFocus;
    [SerializeField] private float height;
    [SerializeField] private float smoothTime = .3f;

    private Vector3 velocity = Vector3.zero;
    private Vector3 offsetFromFocus;

    private void Start()
    {
        SetOffset(new(0, height, -distanceFromFocus));
        if (cameraFocus == null)
        {
            cameraFocus = GameObject.FindGameObjectWithTag("Player").transform;
            transform.position = cameraFocus.TransformPoint(offsetFromFocus);
        }
    }

    private void LateUpdate()
    {
        
        Vector3 target = cameraFocus.TransformPoint(new Vector3(0, height, -distanceFromFocus));

        target.z = Mathf.Max(-backWall, target.z);

        transform.position = Vector3.SmoothDamp(transform.position, target, ref velocity, smoothTime);
    }

    public void SetOffset(Vector3 offset)
    {
        offsetFromFocus = offset;
    }
}
