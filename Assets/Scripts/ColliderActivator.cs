using UnityEngine;

public class ColliderActivator : MonoBehaviour
{
    [SerializeField] private Collider lightAttackColl;
    [SerializeField] private Collider heavyAttackColl;
    [SerializeField] private bool deactivateOnStart;

    private void Start()
    {
        if (deactivateOnStart)
        {
            DeactivateLightCollider();
            DeactivateHeavyCollider();
        }
    }

    [ContextMenu("Ignore")]
    private void ActivateLightCollider()
    {
        lightAttackColl.enabled = true;
    }

    [ContextMenu("Ignore")]
    private void DeactivateLightCollider()
    {
        lightAttackColl.enabled = false;
    }

    [ContextMenu("Ignore")]
    private void ActivateHeavyCollider()
    {
        heavyAttackColl.enabled = true;
    }

    [ContextMenu("Ignore")]
    private void DeactivateHeavyCollider()
    {
        heavyAttackColl.enabled = false;
    }
}
