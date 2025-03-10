using UnityEngine;

public class ColliderActivator : MonoBehaviour
{
    [SerializeField] private Collider coll;
    [SerializeField] private bool deactivateOnStart;

    private void Start()
    {
        if (deactivateOnStart) DeactivateCollider();
    }

    [ContextMenu("Ignore")]
    private void ActivateCollider()
    {
        coll.enabled = true;
    }

    [ContextMenu("Ignore")]
    private void DeactivateCollider()
    {
        coll.enabled = false;
    }
}
