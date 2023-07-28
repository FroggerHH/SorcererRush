using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NTC.Global.Pool;
using UnityEngine;
using Random = UnityEngine.Random;

namespace SorcererRush
{
    public class SpawnArea : MonoBehaviour, ISpawner<Unit>
    {
        [SerializeField] private List<Unit> spawnUnits = new();
        [SerializeField] private float spawnRadius = 0.5f;
        [SerializeField] private float spawnDelay = 1.5f;
        [Header("Debug")] [SerializeField] private bool drawRandomPos = false;

        public List<Unit> GetObjectsToSpawn() => spawnUnits;

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

        public void Spawn(int index, Vector2Int count)
        {
            var pos = CalculateRandomSpawnPos();
            var spawn = NightPool.Spawn(spawnUnits[index], pos, Quaternion.identity);
        }

        private Vector3 CalculateRandomSpawnPos()
        {
            var unitCircle = Random.insideUnitCircle * spawnRadius;
            Vector3 pos = new(unitCircle.x, unitCircle.y, 0);
            return transform.position + pos;
        }

        public void Spawn(Unit obj, Vector2Int count)
        {
            for (int i = 0; i < Random.Range(count.x, count.y); i++)
            {
                NightPool.Spawn(obj, CalculateRandomSpawnPos());
            }
        }


        public void SpawnRandom(Vector2Int count)
        {
            throw new System.NotImplementedException();
        }


        public void SpawnRandom()
        {
            Spawn(GetObjectsToSpawn().Random(), new(1, 5));
        }

        public float GetSpawnDelay() => spawnDelay;

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.gray;
            Gizmos.DrawWireSphere(transform.position, spawnRadius);
            if (drawRandomPos) Gizmos.DrawSphere(CalculateRandomSpawnPos(), 0.2f);
        }
    }
}