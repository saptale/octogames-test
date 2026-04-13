using System;
using UnityEngine;

namespace EvgeniiMaklaev.SaveSystem
{
    public class GameSettings : ISaveable
    {
        public string SaveKey => "game_settings";

        private int _quality = 2;
        private float _soundVolume = 0.3f;

        [Serializable]
        public struct SettingsSaveData
        {
            public int quality;
            public float soundVolume;
        }

        public GameSettings()
        {
            SaveSystem.Register(this);
        }

        public object SaveHandle()
        {
            return new SettingsSaveData
            {
                quality = _quality,
                soundVolume = _soundVolume
            };
        }

        public void LoadHandle(object state)
        {
            if (state is SettingsSaveData data)
            {
                _quality = data.quality;
                _soundVolume = data.soundVolume;
                Debug.Log($"[GAME SETTINGS] Загружено: Quality: {_quality}, Volume: {_soundVolume}");
            }
        }
    }
}