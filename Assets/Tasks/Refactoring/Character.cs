using UnityEngine;

namespace EvgeniiMaklaev.Refactoring
{
    public class Character : MonoBehaviour
    {
        public float Value = 0;
        void Awake()
        {
            CharactersView.Instance.Register(this);
        }
        void OnDestroy()
        {
            CharactersView.Instance.Unregister(this);
        }
    }
}
