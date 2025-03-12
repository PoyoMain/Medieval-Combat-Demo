public interface ICombatant
{
    public bool IsBlocking { get; }
    public void StartAttack();
    public void EndAttack();
    public void StartBlock();
    public void EndBlock();

    public void Hit();
    public void ShieldHit();

    public void StartHitStun();
    public void EndHitStun();
}