using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SorcererRush
{
    public abstract class Unit : MonoBehaviour, ITakeDamage, IHighlightable
    {
        private float health = 0;
        private Renderer[] renderers;
        private static List<Unit> Instances = new();

        protected virtual void Reset()
        {
            SetRenderers();
        }

        private void SetRenderers()
        {
            renderers = GetComponentsInChildren<Renderer>(true);
        }

        protected virtual void OnEnable()
        {
        }

        protected virtual void OnDisable()
        {
        }

        public virtual float GetHealth()
        {
            return Mathf.Max(0, health);
        }

        public ITakeDamage TakeDamage(Damage damage)
        {
            health -= damage.damage;

            OnDamaged();
            return this;
        }

        public abstract void OnDamaged();

        public virtual void OnDeath()
        {
            Instances.Remove(this);
        }

        public Renderer[] GetRenderers()
        {
            if (renderers == null) SetRenderers();
            return renderers;
        }
    }
}