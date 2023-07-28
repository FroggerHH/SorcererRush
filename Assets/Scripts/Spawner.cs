using System.Collections.Generic;
using UnityEngine;

namespace SorcererRush
{
    public interface ISpawner<T>
    {
        List<T> GetObjectsToSpawn();
        void Spawn(int index, int count);
        void Spawn(int index, RangeInt count);
        void SpawnRandom();
    }
}