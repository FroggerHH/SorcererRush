using System;
using UnityEngine;

namespace SorcererRush
{
    public abstract class PlayerControl : MonoBehaviour
    {
        private const float maxSpeed = 1000;
        public static readonly bool inputEnabled = true;
        [SerializeField] protected float braking = 0.15f;
        [SerializeField] protected float speed = 5.2f;
        protected virtual Vector3 GetMoveDir() => Vector3.zero;
        public abstract void Move();

        public abstract void InitValues();

        public void SetSpeed(float value)
        {
            if (value < 0) throw new ArgumentException("Speed must be positive");
            if (value > maxSpeed) throw new ArgumentException($"Speed must be lower than {maxSpeed}");
            speed = value;
        }
    }
}