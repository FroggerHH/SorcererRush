using System;

namespace SorcererRush
{
    public class MonsterAI : BaseAI
    {
        private bool inAttack = false;
        public Weapon currentWeapon { get; protected set; }

        public override void UpdateAI(float dt)
        {
            UpdateMovement(dt);
            UpdateAttack(dt);
        }

        private void UpdateAttack(float dt)
        {
            if (CanAttackTarget()) DoAttack();
        }

        private void DoAttack(bool selectNewWeapon = false)
        {
            if (currentWeapon == null || selectNewWeapon) SelectRandomWeapon();
            if(currentWeapon == null) return;
            
            currentWeapon.DoAttack(target as ITakeDamage);
        }

        private void SelectRandomWeapon()
        {
            currentWeapon = componentsCach.character.GetInventory().GetRandomWeapon();
        }

        private bool OnAttackDistance()
        {
            throw new System.NotImplementedException();
        }

        private bool CanAttackTarget() => OnAttackDistance() && !InAttack();

        private bool InAttack() => inAttack;


        private void UpdateMovement(float dt)
        {
            if (!HasTarget()) return;
        }

        public override void OnSpawn()
        {
            base.OnSpawn();
            inAttack = (GetPrefab().baseAI as MonsterAI).inAttack;
        }
    }
}