using System.Collections.Generic;
using UnityEngine;

namespace SorcererRush
{
    public interface ISpawner<T>
    {
        List<T> GetObjectsToSpawn();
        void Spawn(int index, Vector2Int count);
        void Spawn(T obj, Vector2Int count);
        void SpawnRandom(Vector2Int count);
        void SpawnRandom();
        float GetSpawnDelay();
    }
}