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
        public static string path => Path.GetDirectoryName(Application.dataPath) + "/ProjectSettings/PlazmaGamesSettings.asset";

        [SerializeField] private string _defaultGameManagerName;
        [SerializeField] private List<SceneSpecificGameManagerEntry> _sceneSpecificGameManagerEntries;

        private static void Initialize(PlazmaGamesSettings settings)
        {
            settings._defaultGameManagerName = "GameManager";
            settings._sceneSpecificGameManagerEntries = new List<SceneSpecificGameManagerEntry>();
            Directory.CreateDirectory(Path.GetDirectoryName(path));
            File.WriteAllText(path, JsonUtility.ToJson(settings));
        }

        public static PlazmaGamesSettings GetOrCreateSettings()
        {
            PlazmaGamesSettings settings = null;

            if (File.Exists(path))
            {
                settings = ScriptableObject.CreateInstance<PlazmaGamesSettings>();
                JsonUtility.FromJsonOverwrite(File.ReadAllText(path), settings);
            }

            if (settings == null)
            {
                settings = ScriptableObject.CreateInstance<PlazmaGamesSettings>();
                Initialize(settings);
            }

            return settings;
        }

        public static void SaveSettings(SerializedObject so)
        {
            PlazmaGamesSettings settings = so.targetObject as PlazmaGamesSettings;
            Directory.CreateDirectory(Path.GetDirectoryName(path));
            File.WriteAllText(path, JsonUtility.ToJson(settings));
        }

        public static SerializedObject GetSerializedSettings()
        {
            return new SerializedObject(GetOrCreateSettings());
        }

        public string GetDefaultGameManagerName() => _defaultGameManagerName;

        public string GetSceneGameManagerName(string sceneName) => _sceneSpecificGameManagerEntries.FirstOrDefault(e => e.sceneName.CompareTo(sceneName) == 0).gameManagerName;

        public string GetSceneGameManagerNameOrDefault(string sceneName) => _sceneSpecificGameManagerEntries.FirstOrDefault(e => e.sceneName.CompareTo(sceneName) == 0)?.gameManagerName ?? GetDefaultGameManagerName();
    }
}
