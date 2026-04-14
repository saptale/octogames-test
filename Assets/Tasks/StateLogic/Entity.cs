using UnityEngine;

namespace EvgeniiMaklaev.StateLogic
{
    public class Entity : MonoBehaviour
    {
        void Awake()
        {
            // for example, give a different name to each entity
            gameObject.name = $"Entity {gameObject.GetInstanceID()}";
        }
        void OnEnable()
        {
            StateEntityTrackingSystem.Register(this);
        }
        void OnDisable()
        {
            StateEntityTrackingSystem.Unregister(this);
        }
    }
}
