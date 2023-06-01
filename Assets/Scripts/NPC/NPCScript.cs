using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCScript : MonoBehaviour
{
    [SerializeField] private bool interactable = true;
    [SerializeField] private bool facesLeftByDefault = true;
    [SerializeField] private bool acknowledgesPlayer = true;

    [SerializeField] private float acknowledgePlayerDistance = 5.0f;

    [SerializeField] private SpriteRenderer rend;

    private float APDSquared;

    private Transform playerTransform;

    private void Awake()
    {
        APDSquared = acknowledgePlayerDistance * acknowledgePlayerDistance;
    }

    private void Start()
    {
        playerTransform = GameMaster.Instance.PlayerInstance.transform;
    }

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, acknowledgePlayerDistance);
    }

    private void OnValidate()
    {
        APDSquared = acknowledgePlayerDistance * acknowledgePlayerDistance;
    }
#endif

    private void Update()
    {
        Vector3 difference = transform.position - playerTransform.position;
        if(difference.sqrMagnitude < APDSquared)
        {
            float dotProduct = Vector3.Dot(difference.normalized, Vector3.left);

            rend.flipX = (Mathf.Sign(dotProduct) == 1);
        }
        else
        {
            rend.flipX = !facesLeftByDefault;
        }
    }
}
