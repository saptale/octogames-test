using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace EvgeniiMaklaev.Refactoring
{
    public class CharactersView : MonoBehaviour, IUpdateable
    {
        public static CharactersView Instance;
        // 1. Incorrect property syntax [SerializedField] -> [SerializeField]
        // 2. Logical mistake: we need only Characters in List
        // WAS: [SerializedField] private List<Transform> _characters;
        private List<Character> _characters = new();
        private TMP_Text _text;
        protected string Text
        {
            get => _text.text;
            set => _text.text = value;
        }
        void Awake()
        {
            _text = GetComponent<TextMeshProUGUI>();
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }

        public void Register(Character character)
        {
            if (_characters.Contains(character)) return;
            _characters.Add(character);
        }
        public void Unregister(Character character)
        {
            _characters.Remove(character);
        }


        void Start()
        {
            UpdateManager.Instance.Register(this, UpdateMode.TimeInterval, 1);
        }

        // 3. Incorrect Update Method: FixedUpdate not recommend for this task
        // we need to use out manually update Method -> use IUpdateable
        // WAS: void FixedUpdate()

        public void OnUpdate()
        {
            float totalValue = 0f;

            foreach (Character character in _characters)
            {
                totalValue += character != null ? character.Value : 0f;
            }

            // 5. If we use list, we should call Count, not Length
            // WAS: _characters.Length
            // 6. Text is legacy, lets use TMPro_Text
            // 7. Dont call GetComponent<T> every update, we need cache field -> _text
            // 8. We can delete text property
            // WAS: gameObject.GetComponent<Text>().text = text;
            // 9. Incorrect operation: (_characters.Count / totalValue) --> (totalValue / _characters.Count)
            // 10. Division by zero -> avgValue
            // WAS: totalValue / _characters.Count
            float avgValue = _characters.Count > 0 ? totalValue / _characters.Count : 0f;

            Text = string.Format(
                "<color=green>Characters: {0}</color>\n<color=yellow>Avg value: {1}</color>",
                _characters.Count,
                avgValue
            );
        }
    }
}
