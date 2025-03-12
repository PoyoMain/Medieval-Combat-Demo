using UnityEngine;

[RequireComponent(typeof(ICombatant), typeof(Animator))]
public class Damageable : MonoBehaviour
{
    [SerializeField] private int health;

    private ICombatant combatant;

    private void Awake()
    {
        TryGetComponent(out combatant);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out DamageComponent dmgComponent))
        {
            if (combatant.IsBlocking)
            {
                if (dmgComponent.Damage == 2)
                {
                    combatant.Hit();
                }
                else
                {
                    ICombatant otherCombatant = other.GetComponentInParent<ICombatant>();
                    otherCombatant?.Hit();
                    combatant.ShieldHit();
                }
                
            }
            else
            {
                combatant.Hit();
                health -= dmgComponent.Damage;

                if (health <= 0)
                {
                    Die();
                }
            }
        }
    }

    private void Die()
    {
        combatant.Die();
    }

}