using UnityEngine;

public class Damageable : MonoBehaviour
{
    [SerializeField] private int health;

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out DamageComponent dmgComponent))
        {
            health -= dmgComponent.Damage;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.TryGetComponent(out DamageComponent dmgComponent))
        {
            health -= dmgComponent.Damage;
        }
    }
}
