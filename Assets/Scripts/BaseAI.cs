using System;
using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using NTC.Global.Pool;
using UnityEngine;
using Random = UnityEngine.Random;

namespace SorcererRush
{
    [RequireComponent(typeof(ComponentsCach))]
    public abstract class BaseAI : MonoBehaviour
    {
        protected ComponentsCach componentsCach;
        public PlayerUnit target { get; protected set; }
        public bool HasTarget() => target;

        [SerializeField, MinMaxSlider(0.5f, 10)]
        private Vector2 updateTargetInternal = new(2, 5);

        protected virtual void Awake()
        {
            componentsCach = GetComponent<ComponentsCach>();
            componentsCach.character = GetComponent<Character>();
            componentsCach.baseAI = this;
        }

        public abstract void UpdateAI(float dt);

        public virtual void OnSpawn()
        {
            target = null;
            updateTargetInternal = GetPrefab().baseAI.updateTargetInternal;
            StartCoroutine(UpdateTarget());
        }

        public virtual void OnDespawn()
        {
            StopCoroutine(UpdateTarget());
        }

        protected virtual IEnumerator UpdateTarget()
        {
            target = Utils.Nearest(PlayerUnit.playerUnits, transform.position) as PlayerUnit;
            yield return new WaitForSeconds(Random.Range(updateTargetInternal.x, updateTargetInternal.y));
            StartCoroutine(UpdateTarget());
        }

        protected ComponentsCach GetPrefab() => componentsCach.character.GetPrefab();
    }
}