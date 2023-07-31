using System;
using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using NTC.Global.Pool;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

namespace SorcererRush
{
    [RequireComponent(typeof(ComponentsCach))]
    public class Character : MonoBehaviour, ITakeDamage, IHighlightable, IPoolItem
    {
        private Character prefab;
        private ComponentsCach componentsCach;
        [SerializeField] public UnitFraction fraction;
        [SerializeField] private float health = 100;
        [SerializeField] private Color damageColor = new Color(0.9f, 0.2f, 0.0f, 1f);
        [SerializeField] public ISpawner spawnedFrom;

        [SerializeField] protected UnitStats unitStats;
        private Renderer[] renderers;
        public static List<Character> Instances = new();
        
        [SerializeField] private Inventory m_inventory;
        public Inventory GetInventory() => m_inventory;

        [SerializeField] private List<string> startItems;

        private void Awake()
        {
            SetPrefab();
            componentsCach = GetComponent<ComponentsCach>();
            componentsCach.character = this;

            foreach (var item in startItems)
            {
                GetInventory().AddItem(ObjectBD.Instance.GetItem(item));
            }
        }

        private void SetPrefab()
        {
            prefab = ObjectBD.Instance.GetPrefab(this.GetPrefabName()).GetComponent<Character>();
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
            Utils.Heightlight(this, damageColor);
            // TODO: damage vfx
        }

        public virtual void Die()
        {
            if (health > 0)
            {
                Debug.Log("Still alive, killing");
                health = 0;
            }

            OnDeath();
        }

        public virtual void OnDeath()
        {
            NightPool.Despawn(gameObject);
            spawnedFrom.GetSpawnedObjects().Remove(componentsCach);
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
            if (!Application.isPlaying) return;
            TakeDamage(new Damage(Random.Range(1f, 15f), DamageType.DevDamage));
        }

        public virtual void OnSpawn()
        {
            if (!prefab) SetPrefab();
            Instances.Add(this);
            health = prefab.health;
        }

        public virtual void OnDespawn()
        {
            Instances.Remove(this);
        }

        public virtual Stats GetStats()
        {
            return unitStats;
        }
    }
}