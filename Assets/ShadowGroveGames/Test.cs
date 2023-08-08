using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using SorcererRush.Authentication;
using UnityEngine;

namespace ShadowGroveGames.LoginWithDiscord.Scripts
{
    public class Test : MonoBehaviour
    {
        [Button]
        private async void LoginFromSave()
        {
            GameAuthenticationManager.Init(AuthenticationType.Discord);
        }
    }
}
