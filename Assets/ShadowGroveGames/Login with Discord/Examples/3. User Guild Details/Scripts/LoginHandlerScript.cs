using ShadowGroveGames.LoginWithDiscord.Scripts;
using UnityEngine;

namespace ShadowGroveGames.LoginWithDiscord.Examples.UserGuildDetails
{
    public class LoginHandlerScript : MonoBehaviour
    {
        private const string URL_REGISTER = "https://discord.com/register";

        public void OpenRegisterPage()
        {
            Application.OpenURL(URL_REGISTER);
        }

        public void OpenLoginPage() => LoginWithDiscordScript.Instance.OpenLoginPage();

        public void OnLoginSuccess()
        {
            gameObject.SetActive(false);
        }

        public void OnLoginFailure()
        {
            gameObject.SetActive(true);
        }
    }
}
