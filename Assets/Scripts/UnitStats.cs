using System;

namespace SorcererRush
{
    [Serializable]
    public class UnitStats : Stats
    {
        public float speed = 0.0f;
        public float damage = 0.0f;
        public float armor = 0.0f;
        public float health = 0.0f;

        public static UnitStats operator +(UnitStats s1, UnitStats s2)
        {
            s1.speed += s2.speed;
            s1.damage += s2.damage;
            s1.armor += s2.armor;
            s1.health += s2.health;

            return s1;
        }

        public static UnitStats operator *(UnitStats s1, UnitStats s2)
        {
            s1.speed *= s2.speed;
            s1.damage *= s2.damage;
            s1.armor *= s2.armor;
            s1.health *= s2.health;

            return s1;
        }
    }
}