using System;
using System.Collections;
using System.Collections.Generic;
using NTC.Global.Pool;
using UnityEngine;
using Random = UnityEngine.Random;

namespace SorcererRush
{
    [RequireComponent(typeof(ComponentsCach))]
    public class BaseAI : MonoBehaviour, IPoolItem
    {
        private ComponentsCach componentsCach;
        private Character character;
        private PlayerUnit target;

        protected virtual void Awake()
        {
            componentsCach = GetComponent<ComponentsCach>();
            character = GetComponent<Character>();
            componentsCach.character = character;
            componentsCach.baseAI = this;
        }

        public virtual void UpdateAI(float dt)
        {
            
        }

        public void OnSpawn( )
        {
            StartCoroutine(UpdateTarget());
        }

        public virtual void OnDespawn()
        {
            StopCoroutine(UpdateTarget());
        }

        private IEnumerator UpdateTarget()
        {
            target = Utils.Nearest(PlayerUnit.playerUnits, transform.position);
            yield return new WaitForSeconds(Random.Range(2f, 5f));
            StartCoroutine(UpdateTarget());
        }
    }
}