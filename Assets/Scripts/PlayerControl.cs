using UnityEngine;

namespace SorcererRush
{
    public abstract class PlayerControl : MonoBehaviour
    {
        public static readonly bool inputEnabled = true;
        [SerializeField] protected float braking = 0.15f;
        [SerializeField] protected float speed = 5.2f;
        protected virtual Vector3 GetMoveDir() => Vector3.zero;
        public abstract void Move();

        public abstract void InitValues();
    }
}