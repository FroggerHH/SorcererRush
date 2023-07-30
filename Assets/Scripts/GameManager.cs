using System;
using NaughtyAttributes;
using NTC.Global.Pool;
using UnityEngine;
using UnityEngine.Serialization;

namespace SorcererRush
{
    [AddComponentMenu("Game/Game Manager")]
    public class GameManager : MonoBehaviour
    {
        [HideInInspector] public static GameManager Instance;
        [ReadOnly, ShowIf("HasCameraPrefab")] public GameCamera localCamera;
        [FormerlySerializedAs("localPlayer")] [ReadOnly, ShowIf("HasPlayerPrefab")] public PlayerUnit localPlayerUnit;

        [ReadOnly, ShowIf("HasInGameUIPrefab")]
        public InGameUI localInGameUI;

        [FormerlySerializedAs("playerPrefab")] [Required] public PlayerUnit playerUnitPrefab;
        [Required] public GameCamera cameraPrefab;
        [Required] public InGameUI inGameUIPrefab;

        private bool HasPlayerPrefab => playerUnitPrefab && Application.isPlaying;
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
            localInGameUI = NightPool.Spawn(inGameUIPrefab);
        }

        private void SpawnPlayer()
        {
            localPlayerUnit = NightPool.Spawn(playerUnitPrefab);
        }

        private void SpawnCamera()
        {
            localCamera = NightPool.Spawn(cameraPrefab);
            localCamera.transform.position = cameraPrefab.transform.position;
        }

        private void InitSingleton()
        {
            if (Instance)
            {
                Debug.LogError("Double singleton.");
                Destroy(gameObject);
                return;
            }

            Instance = this;
        }
    }
}