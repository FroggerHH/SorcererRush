namespace SorcererRush
{
    public interface IDamageSource
    {
        HitInfo GetHitInfo();
        void ApplyDamageTo(ITakeDamage target);
    }
}