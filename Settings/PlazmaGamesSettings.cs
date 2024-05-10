using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace PlazmaGames.Settings
{
    public class PlazmaGamesSettings : ScriptableObject
    {
        public static string path => "Assets/Resources/Settings/PlazmaGamesSettings.asset";

        [SerializeField] private string _defaultGameManagerName;
        [SerializeField] private List<SceneSpecificGameManagerEntry> _sceneSpecificGameManagerEntries;

        private static void Initialize(PlazmaGamesSettings settings)
        {
            settings._defaultGameManagerName = "GameManager";
            settings._sceneSpecificGameManagerEntries = new List<SceneSpecificGameManagerEntry>();
            Directory.CreateDirectory(Path.GetDirectoryName(path));
            AssetDatabase.CreateAsset(settings, path);
            AssetDatabase.SaveAssets();
        }

        public static PlazmaGamesSettings GetOrCreateSettings()
        {
            PlazmaGamesSettings settings = AssetDatabase.LoadAssetAtPath<PlazmaGamesSettings>(path);

            if (settings == null)
            {
                settings = ScriptableObject.CreateInstance<PlazmaGamesSettings>();
                Initialize(settings);
            }

            return settings;
        }

#if UNITY_EDITOR
        public static SerializedObject GetSerializedSettings()
        {
            return new SerializedObject(GetOrCreateSettings());
        }
#endif

        public string GetDefaultGameManagerName() => _defaultGameManagerName;

        public string GetSceneGameManagerName(string sceneName) => _sceneSpecificGameManagerEntries.FirstOrDefault(e => e.sceneName.CompareTo(sceneName) == 0).gameManagerName;

        public string GetSceneGameManagerNameOrDefault(string sceneName) => _sceneSpecificGameManagerEntries.FirstOrDefault(e => e.sceneName.CompareTo(sceneName) == 0)?.gameManagerName ?? GetDefaultGameManagerName();
    }
}
