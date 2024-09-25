#if !(UNITY_STANDALONE_WIN || UNITY_STANDALONE_LINUX || UNITY_STANDALONE_OSX || STEAMWORKS_WIN || STEAMWORKS_LIN_OSX)
#define DISABLE_STEAMWORKS
#endif

using UnityEngine;
using PlazmaGames.Core.Debugging;

#if !DISABLE_STEAMWORKS
using Steamworks;
#endif

namespace PlazmaGames.Steam
{
    public class SteamMonoSystem : MonoBehaviour, ISteamMonoSystem
    {
        [Header("Steam App Infomation")]
        [SerializeField] private bool _enableSteam = true;
        [SerializeField] private AppId_t _appID = AppId_t.Invalid;

        public bool IsSteamAvivable { get { return IsInitalized && _enableSteam; } }
        private bool IsInitalized { get; set; }

#if !DISABLE_STEAMWORKS

        [Header("Steam App Infomation")]
        private SteamAPIWarningMessageHook_t _steamAPIWarningMessageHook;

        private bool IsAchievementVaild(string achievementID)
        {
            if (!SteamUserStats.RequestCurrentStats())
            {
                PlazmaDebug.LogWarning($"Failed to fetch steam achevement {achievementID}. No user is logged in.", "Steamworks");
                return false;
            }

            bool doesAchievementExist = SteamUserStats.GetAchievement(achievementID, out bool hasCompleted);

            if (hasCompleted) return false;

            if (!doesAchievementExist)
            {
                PlazmaDebug.LogWarning($"Failed to fetch steam achevement {achievementID}.", "Steamworks");
                return false;
            }

            return !hasCompleted;
        }

        public void SetAchievementProgress(string achievementID, int progress)
        {
            if (!IsSteamAvivable || !IsAchievementVaild(achievementID)) return;

            bool successful = SteamUserStats.SetStat(achievementID, progress);
            successful &= SteamUserStats.StoreStats();

            if (successful) PlazmaDebug.Log($"Suscessfully updated {achievementID} stats.", "Steamworks", Color.green);
            else PlazmaDebug.LogWarning($"Failed to update {achievementID} stats.", "Steamworks");
        }

        public void SetAchievementAsCompleted(string achievementID)
        {
            if (!IsSteamAvivable || !IsAchievementVaild(achievementID)) return;

            bool successful = SteamUserStats.SetAchievement(achievementID);
            successful &= SteamUserStats.StoreStats();

            if (successful) PlazmaDebug.Log($"Suscessfully marked {achievementID} as compelte.", "Steamworks", Color.green);
            else PlazmaDebug.LogWarning($"Failed to marked {achievementID} as compelte.", "Steamworks");
        }

        [AOT.MonoPInvokeCallback(typeof(SteamAPIWarningMessageHook_t))]
        protected static void SteamAPIDebugTextHook(int nSeverity, System.Text.StringBuilder pchDebugText)
        {
            PlazmaDebug.LogWarning(pchDebugText.ToString(), "Steamworks", 1);
        }

        private void InitializeSteam()
        {
            IsInitalized = SteamAPI.Init();
            if (!IsInitalized)
            {
                PlazmaDebug.LogError("SteamAPI_Init() failed. Refer to Valve's documentation or the comment above this line for more information.", "Steamworks");
            }

            if (_steamAPIWarningMessageHook == null)
            {
                _steamAPIWarningMessageHook = new SteamAPIWarningMessageHook_t(SteamAPIDebugTextHook);
                SteamClient.SetWarningMessageHook(_steamAPIWarningMessageHook);
            }
        }

        private void Awake()
        {
            IsInitalized = false;

            if (!_enableSteam) return;

            if (!Packsize.Test())
            {
                PlazmaDebug.LogError("Packsize Test returned false, the wrong version of Steamworks.NET is being run in this platform.", "Steamworks");
            }

            if (!DllCheck.Test())
            {
                PlazmaDebug.LogError("DllCheck Test returned false, One or more of the Steamworks binaries seems to be the wrong version.", "Steamworks");
            }

            try
            {
                if (SteamAPI.RestartAppIfNecessary(_appID)) return; 
            }
            catch (System.DllNotFoundException e)
            {
                PlazmaDebug.LogError("Could not load [lib]steam_api.dll/so/dylib. It's likely not in the correct location.\n" + e, "Steamworks");
                return;
            }

            InitializeSteam();
        }

        private void Update()
        {
            if (!IsSteamAvivable) return;
            SteamAPI.RunCallbacks();
        }

        private void OnDestroy()
        {
            if (!IsInitalized) return;
            SteamAPI.Shutdown();
        }
#endif
    }
}
