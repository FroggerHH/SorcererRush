using System;
using UnityEngine;

namespace SorcererRush
{
    [AddComponentMenu("Game/Player")]
    [RequireComponent(typeof(PlayerControl))]
    public class Player : MonoBehaviour
    {
        private PlayerControl control;

        private void Awake()
        {
            control = GetComponent<PlayerControl>();
            control.InitValues();
        }

        private void FixedUpdate()
        {
            control.Move();
        }
    }
}