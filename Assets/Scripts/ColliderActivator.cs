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

    private void ActivateLightCollider()
    {
        lightAttackColl.enabled = true;
    }

    private void DeactivateLightCollider()
    {
        lightAttackColl.enabled = false;
    }

    private void ActivateHeavyCollider()
    {
        heavyAttackColl.enabled = true;
    }

    private void DeactivateHeavyCollider()
    {
        heavyAttackColl.enabled = false;
    }
}