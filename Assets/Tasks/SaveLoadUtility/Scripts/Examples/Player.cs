using System;
using UnityEngine;

namespace EvgeniiMaklaev.SaveSystem
{

    public class Player : MonoBehaviour, ISaveable
    {
        [Serializable]
        public struct PlayerSaveData
        {
            public float health;
            public int score;
            public float px, py, pz;
        }

        [SerializeField] private float _health;
        [SerializeField] private int _score;
        public string SaveKey => "player";

        private void OnEnable() => SaveSystem.Register(this);
        private void OnDisable() => SaveSystem.Unregister(SaveKey);

        public object SaveHandle()
        {
            return new PlayerSaveData
            {
                health = _health,
                score = _score,
                px = transform.position.x,
                py = transform.position.y,
                pz = transform.position.z
            };
        }

        public void LoadHandle(object state)
        {
            if (state is PlayerSaveData data)
            {
                _health = data.health;
                _score = data.score;
                transform.position = new Vector3(data.px, data.py, data.pz);
                Debug.Log($"<color=green>[PLAYER] Loaded: Health: {_health}, Score: {_score}, Position: {transform.position}</color>");
            }
        }
    }
}
