using System;
using Unity.RemoteConfig;
using UnityEngine;

namespace DefaultNamespace
{
    public static class RemoteConfigManager
    {
        static RemoteConfigManager()
        {
            onFetchCompleted += OnFetchCompleted;
        }

        public static event Action onFetchCompleted;

        public static void UpdateConfig()
        {
            ConfigManager.FetchCompleted += response => onFetchCompleted?.Invoke();
            ConfigManager.FetchConfigs(new userAttributes(), new userAttributes());
        }

        private static void OnFetchCompleted()
        {
            Debug.Log("Config fetched successfully");
        }

        private struct userAttributes
        {
        }

        private struct appAttributes
        {
        }

        public static ulong GetDiscordOwnerRoleID()
        {
            return (ulong)(ConfigManager.appConfig.GetLong("DiscordOwnerRoleID"));
        }
        public static ulong GetDiscordModDevRoleID()
        {
            return (ulong)(ConfigManager.appConfig.GetLong("DiscordModDevRoleID"));
        }

        public static ulong GetDiscordAuthenticationID()
        {
            return (ulong)(ConfigManager.appConfig.GetLong("DiscordAuthenticationID"));
        }

        public static ulong GetDiscordGuidID()
        {
            return (ulong)(ConfigManager.appConfig.GetLong("DiscordGuidID"));
        }
    }
}