using System;
using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using NTC.Global.Pool;
using UnityEngine;
using Random = UnityEngine.Random;

namespace SorcererRush
{
    public abstract class Unit : MonoBehaviour, ITakeDamage, IHighlightable, IPoolItem
    {
        private Unit prefab;
        [SerializeField] public UnitFraction fraction;
        [SerializeField] private float health = 100;
        private Renderer[] renderers;
        public static List<Unit> Instances = new();

        private void Awake()
        {
            SetPrefab();
        }

        private void SetPrefab()
        {
            prefab = ObjectBD.Instance.GetPrefab(this.GetPrefabName()).GetComponent<Unit>();
        }

        protected virtual void Reset()
        {
            SetRenderers();
        }

        private void SetRenderers()
        {
            renderers = GetComponentsInChildren<Renderer>(true);
        }

        public virtual float GetHealth()
        {
            return Mathf.Max(0, health);
        }

        public ITakeDamage TakeDamage(Damage damage)
        {
            health -= damage.damage;
            GameManager.Instance.localInGameUI.CreatePopUp(damage.damage.ToString(), transform.position);

            OnDamaged();
            return this;
        }

        public virtual void OnDamaged()
        {
            if (health <= 0) Die();
            // TODO: damage vfx
        }

        public virtual void Die()
        {
            OnDeath();
        }

        public virtual void OnDeath()
        {
            NightPool.Despawn(gameObject);
            //TODO: death vfx
        }

        public Renderer[] GetRenderers()
        {
            if (renderers == null) SetRenderers();
            return renderers;
        }

        [Button]
        private void DebugDamage()
        {
            TakeDamage(new Damage(Random.Range(1f, 15f), DamageType.DevDamage));
        }

        public void OnSpawn()
        {
            if (!prefab) SetPrefab();
            Instances.Add(this);
            health = prefab.health;
        }

        public void OnDespawn()
        {
            Instances.Remove(this);
        }
    }

    public enum UnitFraction
    {
        Player = 0,
        Monster = 1
    }
}