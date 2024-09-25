using PlazmaGames.Core.Debugging;
using PlazmaGames.Core.MonoSystem;
using Steamworks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlazmaGames.Steam
{
    public interface ISteamMonoSystem : IMonoSystem
    {
        public bool IsSteamAvivable { get; }
        public void SetAchievementProgress(string achievementID, int progress);
        public void SetAchievementAsCompleted(string achievementID);
    }
}
