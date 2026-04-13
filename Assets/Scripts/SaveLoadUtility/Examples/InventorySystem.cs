using System;
using System.Collections.Generic;
using UnityEngine;

namespace EvgeniiMaklaev.SaveSystem
{
    public class InventorySystem : MonoBehaviour, ISaveable
    {
        [SerializeField] private List<InventoryItemData> _items = new();
        [SerializeField] bool _fillTestData = true;

        private void OnEnable() => SaveSystem.Register(this);
        private void OnDisable() => SaveSystem.Unregister(SaveKey);

        public string SaveKey => "inventory";

        void Awake()
        {
            if (!_fillTestData) return;
            // for example
            _items = new()
            {
                new InventoryItemData(0, "Wood", 4),
                new InventoryItemData(1, "Gems", 7),
                new InventoryItemData(2, "Axe", 1),
                new InventoryItemData(3, "Sticks", 29)
            };
        }

        public object SaveHandle()
        {
            return _items;
        }

        public void LoadHandle(object state)
        {
            if (state is List<InventoryItemData> savedItems)
            {
                _items.Clear();
                _items.AddRange(savedItems);
                foreach (var item in _items)
                    Debug.Log($"[INVENTORY] Загружено: Id: {item.id}, Name: {item.name}, Amount: {item.amount}");
            }
        }
    }

    [Serializable]
    public class InventoryItemData
    {
        public int id;
        public string name;
        public int amount;

        public InventoryItemData(int id, string name, int amount)
        {
            this.id = id;
            this.name = name;
            this.amount = amount;
        }
    }
}
