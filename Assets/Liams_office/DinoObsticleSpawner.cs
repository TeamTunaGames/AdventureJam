using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DinoObsticleSpawner : MonoBehaviour
{

    public GameObject ObsticleObject;
    public Vector2 spawnTimer;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    void Awake()
    {
        StartCoroutine(SpawnTimer(Random.Range(spawnTimer.x, spawnTimer.y)));
    }
    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator SpawnTimer(float Timer)
    {
        yield return new WaitForSeconds(Timer);
        Instantiate(ObsticleObject, transform.position, Quaternion.identity);
        StartCoroutine(SpawnTimer(Random.Range(spawnTimer.x, spawnTimer.y)));
    }


}
