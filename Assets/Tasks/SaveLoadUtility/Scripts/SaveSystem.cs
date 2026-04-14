using UnityEngine;
using System.IO;
using System.Collections.Generic;
using Newtonsoft.Json;
using System;
namespace EvgeniiMaklaev.SaveSystem
{
    public static class SaveSystem
    {
        private static readonly string SaveFileName = "SaveFile.json";
        private static string SaveFolder = Application.persistentDataPath;
        public static readonly string SavePath = Path.Combine(SaveFolder, SaveFileName);

        private static readonly JsonSerializerSettings _settings = new JsonSerializerSettings
        {
            // TypeNameHandling.Auto can be dangerous (injections), but for demonstration ill allow it
            TypeNameHandling = TypeNameHandling.Auto,
            Formatting = Formatting.Indented
        };

        private static readonly Dictionary<string, ISaveable> _registry = new();

        public static void Register(ISaveable saveable)
        {
            if (string.IsNullOrEmpty(saveable.SaveKey)) return;

            if (!_registry.TryAdd(saveable.SaveKey, saveable))
            {
                _registry[saveable.SaveKey] = saveable;
            }
        }
        public static void Unregister(string key) => _registry.Remove(key);



        public static async Awaitable<bool> Save()
        {
            var rootData = new Dictionary<string, object>();
            foreach (var pair in _registry)
            {
                rootData[pair.Key] = pair.Value.SaveHandle();
            }

            string json = JsonConvert.SerializeObject(rootData, _settings);

            string tempPath = SavePath + ".tmp";
            string backupPath = SavePath + ".bak";

            try
            {
                await File.WriteAllTextAsync(tempPath, json);

                if (File.Exists(SavePath))
                {
                    File.Replace(tempPath, SavePath, backupPath);
                }
                else
                {
                    File.Move(tempPath, SavePath);
                }

                Debug.Log($"<color=yellow>[SaveSystem] Successfully saved at {SavePath}</color>");
                return true;
            }
            catch (Exception e)
            {
                Debug.LogError($"[SaveSystem] Save Error: {e.Message}");
                if (File.Exists(tempPath))
                {
                    File.Delete(tempPath);
                }
                return false;
            }
        }
        public static async Awaitable<bool> Load()
        {
            if (await TryLoadFile(SavePath))
            {
                Debug.Log("<color=yellow>[SaveSystem] Game successfully loaded.</color>");
                return true;
            }

            string backupPath = SavePath + ".bak";
            if (File.Exists(backupPath))
            {
                Debug.LogWarning("[SaveSystem] Main file is missing or corrupted. Loading backup..");
                if (await TryLoadFile(backupPath))
                {
                    File.Copy(backupPath, SavePath, true);
                    Debug.Log("<color=yellow>[SaveSystem] Game successfully loaded from backup.</color>");
                    return true;
                }
            }

            Debug.LogError("[SaveSystem] Cant load game from main file or backup file.");
            return false;
        }
        private static async Awaitable<bool> TryLoadFile(string path)
        {
            if (!File.Exists(path)) return false;

            try
            {
                string json = await File.ReadAllTextAsync(path);
                var rootData = JsonConvert.DeserializeObject<Dictionary<string, object>>(json, _settings);

                if (rootData == null) return false;

                foreach (var pair in _registry)
                {
                    if (rootData.TryGetValue(pair.Key, out object data))
                    {
                        pair.Value.LoadHandle(data);
                    }
                }
                return true;
            }
            catch (Exception e)
            {
                Debug.LogError($"[SaveSystem] Read file error {path}: {e.Message}");
                return false;
            }
        }
    }
}
