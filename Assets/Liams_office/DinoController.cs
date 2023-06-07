using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DinoController : MonoBehaviour
{
    [SerializeField] private float jumpHeight = 7.5f;
    private PlayerInput.MainGameActions actions;
    private bool canJump = true;

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

    void OnCollisionEnter2D(Collision2D col)
    {
        canJump = true;
        //Debug.Log("123" + col.gameObject.ToString());
    }
}
