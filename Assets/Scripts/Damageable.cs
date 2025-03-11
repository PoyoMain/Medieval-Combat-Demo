using UnityEngine;

[RequireComponent(typeof(ICombatant), typeof(Animator))]
public class Damageable : MonoBehaviour
{
    [SerializeField] private int health;

    private ICombatant combatant;
    private Animator anim;

    private void Awake()
    {
        TryGetComponent(out anim);
        TryGetComponent(out combatant);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out DamageComponent dmgComponent))
        {
            if (combatant.IsBlocking)
            {
                ICombatant otherCombatant = other.GetComponentInParent<ICombatant>();
                otherCombatant?.Hit();
            }
            else
            {
                combatant.Hit();
                health -= dmgComponent.Damage;
            }
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
