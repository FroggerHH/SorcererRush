using System;

namespace SorcererRush
{
    public interface ITakeDamage
    {
        static Action<int> onDeath;
        static Action<int, int> onTakeDamage;
        float GetHealth();
        ITakeDamage TakeDamage(Damage damage);
        void OnDamaged();
        void OnDeath();
    }
}
