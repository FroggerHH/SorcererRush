using NaughtyAttributes;
using NTC.Global.Pool;
using TMPro;
using UnityEngine;

namespace SorcererRush
{
    public class InGameUI : MonoBehaviour
    {
        [BoxGroup("PopUps"), SerializeField, Required()]
        private TMP_Text popUpPrefab;

        [BoxGroup("PopUps"), SerializeField] private float popUpLifeTime = 1.5f;

        public void CreatePopUp(string msg, Vector3 position)
        {
            NightPool.Spawn(popUpPrefab, position);
            NightPool.Despawn(popUpPrefab, popUpLifeTime);
        }
    }
}