using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NaughtyAttributes;
using NTC.Global.Pool;
using Redcode.Extensions;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

namespace SorcererRush
{
    [RequireComponent(typeof(ComponentsCach))]
    public class Character : MonoBehaviour, ITakeDamage, IHighlightable, IPoolItem
    {
        private ComponentsCach prefab;

        private ComponentsCach componentsCach;
        [SerializeField] public UnitFraction fraction;
        [SerializeField] private float health = 100;
        [SerializeField] private Color damageColor = new Color(0.9f, 0.2f, 0.0f, 1f);
        [SerializeField] public ISpawner spawnedFrom;

        [SerializeField] protected UnitStats unitStats;
        private Renderer[] renderers;
        public static List<Character> all = new();
        public static List<Character> playerUnits => all.Where(x => x.fraction == UnitFraction.Player).ToList();

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

        public ComponentsCach GetPrefab() => prefab;

        public void SetPrefab() => prefab = ObjectBD.Instance.GetPrefab(this.GetPrefabName()).GetComponent<ComponentsCach>();

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
            if (!GetPrefab()) SetPrefab();
            all.Add(this);
            health = GetPrefab().character.health;
            if(componentsCach.baseAI) componentsCach.baseAI.OnSpawn();
        }

        public virtual void OnDespawn()
        {
            all.Remove(this);
            if(componentsCach.baseAI) componentsCach.baseAI.OnSpawn();
        }

        public virtual Stats GetStats()
        {
            return unitStats;
        }

        private void OnBecameVisible() => GetRenderers().ForEach(renderer => renderer.gameObject.SetActive(true));
        private void OnBecameInvisible() => GetRenderers().ForEach(renderer => renderer.gameObject.SetActive(false));
    }
}