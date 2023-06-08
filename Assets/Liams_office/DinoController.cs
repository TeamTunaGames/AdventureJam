using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DinoController : MonoBehaviour
{
    [SerializeField] private float jumpHeight = 7.5f;
    private PlayerInput.MainGameActions actions;
    private bool canJump = true;

    public int winScoreCondition = 10;
    private int score;

    private HashSet<GameObject> ignoreObjects = new HashSet<GameObject>();

    public Action<int> NotifyScore;

    public Action NotifyWin;
    public Action NotifyLoss;

    private bool hasEnd = false;
    private void Awake()
    {
        actions = ControllerManager.Instance.Actions;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(actions.Interact.triggered & canJump)
        {
            canJump = false;
            gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(0.0f , jumpHeight), ForceMode2D.Impulse);
        }
    }

    void FixedUpdate()
    {
        if(hasEnd) return;
        Vector2 start = new Vector2(transform.position.x, -10.0f);
        Vector2 size = GetComponent<BoxCollider2D>().size;
        RaycastHit2D[] hits = Physics2D.BoxCastAll(start, size, 0.0f, Vector2.up);
        foreach(RaycastHit2D hit in hits)
        {
            if(hit.collider.GetComponent<DinoObsticle>())
            {
                if(ignoreObjects.Contains(hit.collider.gameObject)) return;
                ignoreObjects.Add(hit.collider.gameObject);
                score++;
                NotifyScore?.Invoke(score);

                if(score >= winScoreCondition) 
                {
                    hasEnd = true;
                    NotifyWin?.Invoke();
                }

                
            }
        }
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if(hasEnd) return;
        if(col.gameObject.GetComponent<DinoObsticle>()) 
        {
            hasEnd = true;
            NotifyLoss?.Invoke();
        }
        
        canJump = true;
        //Debug.Log("123" + col.gameObject.ToString());
    }
}
