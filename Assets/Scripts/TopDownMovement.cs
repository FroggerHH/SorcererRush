using NaughtyAttributes;
using UnityEngine;

namespace SorcererRush
{
    [AddComponentMenu("Game/TopDownMovement")]
    [RequireComponent(typeof(Rigidbody2D))]
    public class TopDownMovement : PlayerControl
    {
        private Rigidbody2D rb;
        [ReadOnly] private Vector3 oldDir;

        public override void Move()
        {
            rb.velocity = Vector2.Lerp(rb.velocity, GetMoveDir() * speed, Time.deltaTime / braking);
        }

        public override void InitValues()
        {
            rb = GetComponent<Rigidbody2D>();
        }

        protected override Vector3 GetMoveDir()
        {
            float moveX = Input.GetAxisRaw("Horizontal");
            float moveY = Input.GetAxisRaw("Vertical");
            var result = Vector2.Lerp(oldDir, new Vector3(moveX, moveY, 0).normalized, Time.deltaTime / braking);
            oldDir = new Vector3(moveX, moveY, 0).normalized;
            return result;
        }
    }
}