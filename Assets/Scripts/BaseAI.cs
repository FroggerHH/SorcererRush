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
    public abstract class BaseAI : MonoBehaviour
    {
        protected ComponentsCach componentsCach;
        public PlayerUnit target { get; protected set; }
        public bool HasTarget() => target;

        private void Reset()
        {
            if (Application.isEditor) GetComponent<ComponentsCach>().baseAI = this;
        }

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
        }

        public virtual void OnDespawn()
        {
        }

        protected ComponentsCach GetPrefab() => componentsCach.character.GetPrefab();
    }
}