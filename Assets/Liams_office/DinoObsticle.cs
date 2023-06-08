using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DinoObsticle : MonoBehaviour
{
    public float speed;
    private bool isEnable;
    // Start is called before the first frame update
    void Start()
    {
        isEnable = true;
    }

    void Awake()
    {
        isEnable = true;
    }

    void FixedUpdate()
    {
        if(!isEnable) return;
        transform.position -= Vector3.right * (speed * Time.deltaTime);
        if(transform.position.y <= -5.0f) Destroy(gameObject);
    }

    public void EndPlay()
    {
        isEnable = false;
    }
}
