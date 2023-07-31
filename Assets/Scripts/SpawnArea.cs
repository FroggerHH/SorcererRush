using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NTC.Global.Pool;
using UnityEngine;
using Random = UnityEngine.Random;

namespace SorcererRush
{
    public class SpawnArea : MonoBehaviour, ISpawner
    {
        [SerializeField] private List<SpawnData> spawnUnits = new();
        private List<ComponentsCach> spawnedObjects = new();
        [SerializeField] private float spawnRadius = 0.5f;
        [SerializeField] private float spawnDelay = 1.5f;
        [Header("Debug")] [SerializeField] private bool drawRandomPos = false;
        [SerializeField] private float spawnLimit = int.MaxValue;

        public List<SpawnData> GetObjectsToSpawn() => spawnUnits;

        public List<ComponentsCach> GetSpawnedObjects()
        {
            return spawnedObjects;
        }

        private void Start()
        {
            StartCoroutine(StartSpawning());
        }

        private IEnumerator StartSpawning()
        {
            SpawnRandom();
            yield return new WaitForSeconds(GetSpawnDelay());
            StartCoroutine(StartSpawning());
        }

        public void Spawn(int index)
        {
            if(!CheckSpawnLimit()) return;
            var pos = CalculateRandomSpawnPos();
            var spawn = NightPool.Spawn(spawnUnits[index].prefab, pos, Quaternion.identity);
            spawnedObjects.Add(spawn);
            if (spawn.character)
            {
                spawn.character.spawnedFrom = this;
            }
            
        }

        private Vector3 CalculateRandomSpawnPos()
        {
            var unitCircle = Random.insideUnitCircle * spawnRadius;
            Vector3 pos = new(unitCircle.x, 0, unitCircle.y);
            return transform.position + pos;
        }

        public void Spawn(SpawnData spawnData)
        {
            if(!CheckSpawnLimit()) return;
            for (int i = 0; i < Random.Range(spawnData.count.x, spawnData.count.y); i++)
            {
                var spawn = NightPool.Spawn(spawnData.prefab, CalculateRandomSpawnPos());
                spawnedObjects.Add(spawn);
                if (spawn.character)
                {
                    spawn.character.spawnedFrom = this;
                }
            }
        }

        private bool CheckSpawnLimit() => spawnedObjects.Count < GetSpawnLimit();

        public void SpawnRandom()
        {
            Spawn(GetObjectsToSpawn().Random());
        }

        public float GetSpawnDelay() => spawnDelay;

        public float GetSpawnLimit() => spawnLimit;

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.gray;
            Gizmos.DrawWireSphere(transform.position, spawnRadius);
            if (drawRandomPos) Gizmos.DrawSphere(CalculateRandomSpawnPos(), 0.2f);
        }
    }
}