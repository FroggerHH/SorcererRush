using UnityEngine;

namespace SorcererRush
{
    [AddComponentMenu("Game/Player")]
    [RequireComponent(typeof(PlayerControl))]
    public class Player : MonoBehaviour
    {
        private PlayerControl control;

    }
}