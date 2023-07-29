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

        [ReadOnly, ShowIf("HasInGameUIPrefab")]
        public InGameUI localInGameUI;

        [Required] public Player playerPrefab;
        [Required] public GameCamera cameraPrefab;
        [Required] public InGameUI inGameUIPrefab;

        private bool HasPlayerPrefab => playerPrefab && Application.isPlaying;
        private bool HasCameraPrefab => cameraPrefab && Application.isPlaying;
        private bool HasInGameUIPrefab => inGameUIPrefab && Application.isPlaying;

        private void Awake()
        {
            InitSingleton();
            SpawnPlayer();
            SpawnCamera();
            SpawnInGameUI();
        }

        private void SpawnInGameUI()
        {
            localInGameUI = NightPool.Spawn(inGameUIPrefab, parent: transform);
        }

        private void SpawnPlayer()
        {
            localPlayer = NightPool.Spawn(playerPrefab, parent: transform);
        }

        private void SpawnCamera()
        {
            localCamera = NightPool.Spawn(cameraPrefab, parent: transform);
            localCamera.transform.position = cameraPrefab.transform.position;
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