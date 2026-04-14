using UnityEngine;

namespace EvgeniiMaklaev.Refactoring
{
    public class Character : MonoBehaviour
    {
        public float Value = 0;
        void OnEnable()
        {
            CharactersView.Instance.Register(this);
        }
        void OnDisable()
        {
            CharactersView.Instance.Unregister(this);
        }
    }
}
