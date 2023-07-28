using System;
using NaughtyAttributes;
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
            localPlayer = Instantiate(playerPrefab, transform);
            localPlayer.name = $"{localPlayer.name} (Spawned)";
        }

        private void SpawnCamera()
        {
            localCamera = Instantiate(cameraPrefab, transform);
            localCamera.name = $"{localCamera.name} (Spawned)";
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