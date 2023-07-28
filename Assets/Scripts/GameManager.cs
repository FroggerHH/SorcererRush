using System;
using NaughtyAttributes;
using NTC.Global.Pool;
using UnityEngine;

namespace SorcererRush
{
    [AddComponentMenu("Game/Game Manager")]
    public class GameManager : MonoBehaviour
    {
        [HideInInspector] public static GameManager Instance;
        [ReadOnly, ShowIf("HasCameraPrefab")] public GameCamera localCamera;
        [ReadOnly, ShowIf("HasPlayerPrefab")] public Player localPlayer;

        [Required] public Player playerPrefab;
        [Required] public GameCamera cameraPrefab;

        private bool HasPlayerPrefab => playerPrefab && Application.isPlaying;
        private bool HasCameraPrefab => cameraPrefab && Application.isPlaying;

        private void Awake()
        {
            InitSingleton();
            SpawnPlayer();
            SpawnCamera();
        }

        private void SpawnPlayer()
        {
            localPlayer = NightPool.Spawn(playerPrefab, parent: transform);
            localPlayer.name = $"{localPlayer.GetPrefabName()} (Spawned)";
        }

        private void SpawnCamera()
        {
            localCamera = NightPool.Spawn(cameraPrefab, parent: transform);
            localCamera.transform.position = cameraPrefab.transform.position;
            localCamera.name = $"{localCamera.GetPrefabName()} (Spawned)";
        }

        private void InitSingleton()
        {
            if (Instance)
            {
                Debug.LogError("Doble singleton.");
                Destroy(gameObject);
                return;
            }

            Instance = this;
        }
    }
}