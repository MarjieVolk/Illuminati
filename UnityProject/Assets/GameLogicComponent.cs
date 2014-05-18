using Assets.Player;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets
{
    public class GameLogicComponent : MonoBehaviour
    {
        private Dictionary<Type, Component> dependencies;

        public TurnController TurnController { get { return getDependency<TurnController>(); } }
        public GraphUtility GraphUtility { get { return getDependency<GraphUtility>(); } }

        private void Awake()
        {
            dependencies = new Dictionary<Type, Component>();
        }

        private T getDependency<T>() where T : Component
        {
            if (!dependencies.ContainsKey(typeof(T)))
            {
                dependencies[typeof(T)] = resolveDependency<T>();
            }
            return (T) dependencies[typeof(T)];
        }

        private T resolveDependency<T>() where T : Component
        {
            return transform.root.GetComponent<T>();
        }
    }
}
