
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace EvgeniiMaklaev.StateLogic
{
    public static class StateEntityTrackingSystem
    {
        private static HashSet<Entity> _entities = new();
        public static IReadOnlyCollection<Entity> ActiveEntities => _entities;

        // we need clear list on start, i turned off domain reload :)
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        private static void Init()
        {
            _entities.Clear();
        }

        public static void Register(Entity entity)
        {
            _entities.Add(entity);
        }
        public static void Unregister(Entity entity)
        {
            _entities.Remove(entity);
        }
    }

    public class EntityEditor : Editor
    {
        [MenuItem("Tools/State Logic/Entity")]
        public static void GetActiveEntities()
        {
            foreach (var entity in StateEntityTrackingSystem.ActiveEntities)
                Debug.Log(entity.name);
        }
    }
}
