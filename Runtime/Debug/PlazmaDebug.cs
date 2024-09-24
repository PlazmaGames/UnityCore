using PlazmaGames.Core;
using System;
using UnityEngine;

namespace PlazmaGames.Debugging
{
    public static class PlazmaDebug
    {
        private static string ConstructLog(string msg, string quilfier = "", Color? color = null)
        {
            return (color == null) ? $"{quilfier}: {msg}" : $"<color=#{ColorUtility.ToHtmlStringRGBA(color ?? Color.white)}>{quilfier}:</color>: {msg}";
        }

        public static void Log(string msg, string quilfier = "", Color? color = null)
        {
#if UNITY_EDITOR
            if (GameManager.InDebugMode) Debug.Log(ConstructLog(msg, quilfier, color));
#endif
        }

        public static void LogWarning(string msg, string quilfier = "", Color? color = null)
        {
#if UNITY_EDITOR
            if (GameManager.InDebugMode) Debug.LogWarning(ConstructLog(msg, quilfier, color));
#endif
        }

        public static void LogExpection(Exception e, string quilfier = "", Color? color = null)
        {
#if UNITY_EDITOR
            if (GameManager.InDebugMode) Debug.LogError(ConstructLog(e.ToString(), quilfier, color));
#endif
        }

        public static void LogError(string msg, string quilfier = "", Color? color = null)
        {
#if UNITY_EDITOR
            if (GameManager.InDebugMode) Debug.LogError(ConstructLog(msg, quilfier, color));
#endif
        }
    }
}
