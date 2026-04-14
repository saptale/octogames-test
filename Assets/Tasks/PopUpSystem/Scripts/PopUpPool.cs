using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace EvgeniiMaklaev.PopUp
{
    public class PopUpPool
    {
        private Queue<PopUp> _pool = new Queue<PopUp>();

        public async Awaitable<PopUp> Get(Transform root)
        {
            PopUp obj;
            if (_pool.Count == 0)
            {
                var handle = Addressables.InstantiateAsync("MessagePopUp", root);
                var inst = await handle.Task;
                obj = inst.GetComponent<PopUp>();
            }
            else
            {
                obj = _pool.Dequeue();
            }
            obj.gameObject.SetActive(true);
            return obj;
        }

        public void Return(PopUp returnObject)
        {
            returnObject.gameObject.SetActive(false);
            _pool.Enqueue(returnObject);
        }
    }
}
