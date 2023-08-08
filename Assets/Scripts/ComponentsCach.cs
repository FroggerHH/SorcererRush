using UnityEngine;

namespace SorcererRush
{
    public class ComponentsCach : MonoBehaviour
    {
        public Character character;
        public BaseAI baseAI;

        public Projectile projectile;
        //TODO: item

        private void Reset()
        {
            baseAI = GetComponent<BaseAI>();
            character = GetComponent<Character>();
            projectile = GetComponent<Projectile>();
        }
    }
}