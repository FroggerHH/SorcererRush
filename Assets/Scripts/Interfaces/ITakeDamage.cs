using System;
using UnityEngine;

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
        Vector2 GetPosition();
        MonoBehaviour GetMonoBehaviour();
    }
}
