using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;
using Random = UnityEngine.Random;

namespace SorcererRush
{
    public class MonsterAI : BaseAI
    {
        private bool inAttack = false;
        public Weapon currentWeapon { get; protected set; }

        [SerializeField, MinMaxSlider(0.5f, 10)]
        private Vector2 changeWeaponInternal = new(2, 5);


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
            if (currentWeapon == null) return;

            currentWeapon.DoAttack(target as ITakeDamage);
        }

        private void SelectRandomWeapon() => currentWeapon = componentsCach.character.GetInventory().GetRandomWeapon();

        private bool OnAttackDistance() => throw new System.NotImplementedException();

        private bool CanAttackTarget() => OnAttackDistance() && !InAttack();

        private bool InAttack() => inAttack;


        private void UpdateMovement(float dt)
        {
            if (!HasTarget()) return;
        }

        public override void OnSpawn()
        {
            base.OnSpawn();
            changeWeaponInternal = (GetPrefab().baseAI as MonsterAI).changeWeaponInternal;
            inAttack = (GetPrefab().baseAI as MonsterAI).inAttack;
            StartCoroutine(UpdateWeapon());
        }

        private IEnumerator UpdateWeapon()
        {
            SelectRandomWeapon();
            yield return new WaitForSeconds(Random.Range(changeWeaponInternal.x, changeWeaponInternal.y));
            StartCoroutine(UpdateWeapon());
        }

        public override void OnDespawn()
        {
            base.OnDespawn();
            StopCoroutine(UpdateWeapon());
        }
    }
}