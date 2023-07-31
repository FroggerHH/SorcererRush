using Redcode.Extensions;
using UnityEngine;

namespace SorcererRush
{
    [AddComponentMenu("Game/TopDownMovement")]
    [RequireComponent(typeof(Rigidbody))]
    public class TopDownMovement : PlayerControl
    {
        private Rigidbody rb;
        private Vector3 oldDir;

        public override void Move()
        {
            base.Move();
            transform.Translate(GetMoveDir() * speed / 100f);
        }

        public override void InitValues()
        {
            rb = GetComponent<Rigidbody>();
        }

        protected override Vector3 GetMoveDir()
        {
            float moveX = Input.GetAxisRaw("Horizontal");
            float moveY = Input.GetAxisRaw("Vertical");
            var result = Vector3.Lerp(oldDir, new Vector3(moveX, 0, moveY).normalized, Time.deltaTime / braking);
            oldDir = new Vector3(moveX, 0, moveY).normalized;
            return result;
        }
    }
}