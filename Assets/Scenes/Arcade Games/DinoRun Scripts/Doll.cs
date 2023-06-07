using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Doll : MonoBehaviour
{
    [SerializeField] private float speed = 8.0f;
    [SerializeField] private Crane crane;

    private bool stopped = false;

    private void OnEnable()
    {
        crane.OnCraneGrab += OnCraneGrab;
    }

    private void OnDisable()
    {
        crane.OnCraneGrab -= OnCraneGrab;
    }

    private void Update()
    {
        if (stopped)
            return;

        transform.position += Vector3.right * (speed * Time.deltaTime);
        if(transform.position.x >= 8.5f)
        {
            transform.position = new Vector3(-8.5f, transform.position.y, 0);

        }
    }

    private void OnCraneGrab(Vector2 crane)
    {
        stopped = true;
    }
}
