using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class BillboardSprites : MonoBehaviour
{
    private Camera cam;

    private void OnEnable()
    {
        cam = Camera.main;
    }

    private void LateUpdate()
    {
        Vector3 cameraDir = cam.transform.forward;
        cameraDir.y = 0f;
        transform.rotation = Quaternion.LookRotation(cameraDir);
    }
}
