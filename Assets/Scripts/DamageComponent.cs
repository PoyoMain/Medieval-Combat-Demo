using UnityEngine;

public class DamageComponent : MonoBehaviour
{
    [SerializeField] private int damage;
    public int Damage => damage;
}
