using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Doll : MonoBehaviour
{
    //[SerializeField] private float speed = 8.0f;
    [SerializeField] private Crane crane;
    [SerializeField] private List<GameObject> walls;
    private bool stopped = false;

    private void OnEnable()
    {
        crane.OnCraneGrab += OnCraneGrab;
    }

    private void OnDisable()
    {
        crane.OnCraneGrab -= OnCraneGrab;
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if(walls.Contains(col.gameObject))
        {
            float self_x = transform.position.x;
            float other_x = col.transform.position.x;
            Rigidbody2D self_rig = GetComponent<Rigidbody2D>();
            if(!self_rig)return;
            
            int direction = self_x >= other_x ? 1 : -1;
            float rand_Angle = UnityEngine.Random.Range(20.0f, 70.0f);
            Vector2 force = new Vector2(Mathf.Cos(rand_Angle) * direction,Mathf.Sin(rand_Angle));
            self_rig.AddForce(force, ForceMode2D.Impulse);
            
        }
        //Debug.Log("hit");

    }
    private void Update()
    {
        if (stopped)
            return;

        return;
        /*
        transform.position += Vector3.right * (speed * Time.deltaTime);
        if(transform.position.x >= 8.5f)
        {
            transform.position = new Vector3(-8.5f, transform.position.y, 0);

        }
        */
    }

    private void OnCraneGrab(Vector2 crane)
    {
        stopped = true;
    }
}
