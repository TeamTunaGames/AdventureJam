using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DinoObsticle : MonoBehaviour
{
    public float speed;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position -= Vector3.right * (speed * Time.deltaTime);
        if(transform.position.y <= -5.0f) Destroy(gameObject);
    }

}
