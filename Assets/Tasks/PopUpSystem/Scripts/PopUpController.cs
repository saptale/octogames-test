using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace EvgeniiMaklaev.PopUp
{
    public class PopUpController : MonoBehaviour
    {
        public static PopUpController Instance;
        private PopUpPool _popUpPool;
        private Queue<PopUpRequest> _queue = new Queue<PopUpRequest>();
        private bool _isShowing = false;
        void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                _popUpPool = new PopUpPool();
            }
        }

        public async Awaitable ShowMessagePopUp(string message, List<PopUpButtonConfig> buttons, string header = "")
        {
            var completionSource = new AwaitableCompletionSource();
            _queue.Enqueue(new PopUpRequest
            {
                Message = message,
                Header = header,
                Buttons = buttons,
                CompletionSource = completionSource
            });
            ProcessQueue();
            await completionSource.Awaitable;
        }
        private async void ProcessQueue()
        {
            if (_isShowing || _queue.Count == 0) return;
            _isShowing = true;
            var request = _queue.Dequeue();
            var instPopUp = await _popUpPool.Get(transform);
            instPopUp.Message = request.Message;
            instPopUp.Header = request.Header;
            instPopUp.Initialize(request.Buttons, () =>
            {
                _popUpPool.Return(instPopUp);
                _isShowing = false;
                request.CompletionSource.SetResult();
                ProcessQueue();
            });
        }
    }

    public class PopUpRequest
    {
        public string Message;
        public string Header;
        public List<PopUpButtonConfig> Buttons;
        public AwaitableCompletionSource CompletionSource;
    }
}
