using System;
using NaughtyAttributes;
using UnityEngine;

namespace SorcererRush
{
    [Serializable]
    public class SpawnData
    {
        private const int minSpawnCount = 1;
        private const int maxSpawnCount = 20;
        
        public ComponentsCach prefab;

        [MinMaxSlider(minSpawnCount, maxSpawnCount)]
        public Vector2Int count = new Vector2Int(1, 3);
    }
}
