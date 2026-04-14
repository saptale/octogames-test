using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace EvgeniiMaklaev.PopUp
{
    public class PopUp : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _header;
        [SerializeField] private TextMeshProUGUI _message;
        [SerializeField] private List<PopUpButtonView> _buttons = new();
        public string Header
        {
            get => _header.text;
            set => _header.text = value;
        }
        public string Message
        {
            get => _message.text;
            set => _message.text = value;
        }

        public void Initialize(List<PopUpButtonConfig> configs, Action closeAction)
        {
            foreach (var view in _buttons) view.ButtonComponent.gameObject.SetActive(false);

            for (int i = 0; i < configs.Count; i++)
            {
                if (i >= _buttons.Count) break;

                PopUpButtonView view = _buttons[i];
                PopUpButtonConfig config = configs[i];

                view.ButtonComponent.gameObject.SetActive(true);

                if (view.LabelComponent != null)
                    view.LabelComponent.text = config.Label;

                view.ButtonComponent.onClick.RemoveAllListeners();
                view.ButtonComponent.onClick.AddListener(() =>
                {
                    config.OnClick?.Invoke();
                    closeAction?.Invoke();
                });
            }
        }
    }

    [Serializable]
    public struct PopUpButtonView
    {
        public Button ButtonComponent;
        public TextMeshProUGUI LabelComponent;
    }

    public class PopUpButtonConfig
    {
        public string Label;
        public Action OnClick;
    }
}
