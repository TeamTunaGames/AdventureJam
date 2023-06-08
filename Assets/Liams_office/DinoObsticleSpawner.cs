using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DinoObsticleSpawner : MonoBehaviour
{

    [SerializeField] private GameObject PlayerObject;
    public GameObject ObsticleObject;

    private HashSet<GameObject> spawnedActors = new HashSet<GameObject>();
    public Vector2 spawnTimer;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    void Awake()
    {
        DinoController dinoPlayerController = PlayerObject.GetComponent<DinoController>();
        dinoPlayerController.NotifyWin += OnEndPlay;
        dinoPlayerController.NotifyLoss += OnEndPlay;
        StartCoroutine(SpawnTimer(UnityEngine.Random.Range(spawnTimer.x, spawnTimer.y)));
    }
    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator SpawnTimer(float Timer)
    {
        yield return new WaitForSeconds(Timer);
        GameObject actor = Instantiate(ObsticleObject, transform.position, Quaternion.identity);
        spawnedActors.Add(actor);
        StartCoroutine(SpawnTimer(UnityEngine.Random.Range(spawnTimer.x, spawnTimer.y)));
    }

    void OnEndPlay()
    {
        StopAllCoroutines();
        spawnedActors = new HashSet<GameObject>(spawnedActors.Where(p => p != null));
        foreach(GameObject actor in spawnedActors)
        {
            if(!actor) return;
            DinoObsticle obsticle = actor.GetComponent<DinoObsticle>();
            if(obsticle) obsticle.EndPlay();
        }
    }
}
