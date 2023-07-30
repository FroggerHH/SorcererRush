using System.Collections.Generic;
using UnityEngine;

namespace SorcererRush
{
    public interface ISpawner
    {
        List<SpawnData> GetObjectsToSpawn();
        List<GameObject> GetSpawnedObjects();
        void Spawn(int index);
        void Spawn(SpawnData obj);
        void SpawnRandom();
        float GetSpawnDelay();
        float GetSpawnLimit();
    }
}