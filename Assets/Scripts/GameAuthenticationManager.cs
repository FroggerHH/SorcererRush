using System;
using System.Linq;
using System.Threading.Tasks;
using DefaultNamespace;
using Newtonsoft.Json;
using ShadowGroveGames.LoginWithDiscord.Scripts;
using ShadowGroveGames.LoginWithDiscord.Scripts.Struct;
using Unity.Services.Authentication;
using Unity.Services.Core;
using UnityEngine;
using SorcererRush;

namespace SorcererRush.Authentication
{
    public static class GameAuthenticationManager
    {
        private static AuthenticationType authenticationType;

        public static void Init(AuthenticationType authenticationType)
        {
            GameAuthenticationManager.authenticationType = authenticationType;
            RemoteConfigManager.onFetchCompleted += RunAuthentication_void;
            RemoteConfigManager.UpdateConfig();
        }

        private async static void RunAuthentication_void() => await RunAuthentication();

        private static async Task RunAuthentication()
        {
            await TryAuthenticateAnonymously();
            switch (authenticationType)
            {
                // case AuthenticationType.Google:
                //     await TryAuthenticateAnonymously();
                //     break;
                case AuthenticationType.Discord:
                    await TryAuthenticateDiscord();
                    break;
            }

            Debug.Log("Initialization and signin complete.");
        }

        private static async Task TryAuthenticateDiscord()
        {
            try
            {
                var save = PlayerPrefs.GetString("DiscordAccount", String.Empty);
                if (string.IsNullOrEmpty(save) || string.IsNullOrWhiteSpace(save))
                {
                    LoginWithDiscordScript.Instance.OpenLoginPage();
                    GameStartPoint.Instance.SetTimeMode(TimeMode.Stopped);
                    LoginWithDiscordScript.Instance._loginWithDiscordServer.OnSuccess +=
                        (_) =>
                        {
                            CreateUserProfile();
                            GameStartPoint.Instance.SetTimeMode(TimeMode.Normal);
                        };
                    LoginWithDiscordScript.Instance._loginWithDiscordServer.OnFailure +=
                        () => GameStartPoint.Instance.SetTimeMode(TimeMode.Normal);
                }
                else
                {
                    var oAuthToken = JsonConvert.DeserializeObject<OAuthToken>(save);
                    LoginWithDiscordScript.Instance.LoginWithDiscordSuccess(oAuthToken);
                    GameStartPoint.Instance.SetTimeMode(TimeMode.Normal);
                    Debug.Log("LoginWithDiscord successfully");
                    CreateUserProfile();
                }
            }
            catch (Exception e)
            {
                Debug.LogException(e);
                GameStartPoint.Instance.SetTimeMode(TimeMode.Normal);
            }
        }

        private static void CreateUserProfile()
        {
            var allGuilds = LoginWithDiscordScript.Instance.GetUserGuilds();
            var discordGuidID = RemoteConfigManager.GetDiscordGuidID();
            var guild = allGuilds.Find(x => { return x.Id == discordGuidID; });
            if (guild.Id == 0)
            {
                Debug.Log("You are not in SorcererRush guild.");
                return;
            }

            if (guild.GetGuildMember().Roles.Contains(RemoteConfigManager.GetDiscordOwnerRoleID()))
                Debug.Log("User is owner");
            if (guild.GetGuildMember().Roles.Contains(RemoteConfigManager.GetDiscordModDevRoleID()))
                Debug.Log("User is mod dev");
        }

        private static async Task TryAuthenticateAnonymously()
        {
            try
            {
                await UnityServices.InitializeAsync();
                // if (this == null) return;

                if (!AuthenticationService.Instance.IsSignedIn)
                {
                    await AuthenticationService.Instance.SignInAnonymouslyAsync();
                    // if (this == null) return;
                }
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
        }
    }
}