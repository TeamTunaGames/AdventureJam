using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableFinder : MonoBehaviour
{
    private IInteractable interactable;
    public IInteractable Interactable => interactable;


    [SerializeField] private LayerMask layerMask;
    [SerializeField] private float interactableRadius = 2.0f;

    private Collider[] hit = new Collider[5];

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, interactableRadius);
    }
#endif

    private void Update()
    {
        interactable = null;
        int numHit = Physics.OverlapSphereNonAlloc(transform.position, interactableRadius, hit, layerMask);

        if (numHit > 0)
        {
            for(int i = 0; i < numHit; ++i)
            {
                if (hit[i].TryGetComponent(out IInteractable interactable))
                {
                    if (interactable.IsInteractable)
                    {
                        this.interactable = interactable;
                        break;
                    }
                }
            }
        }
    }
}
