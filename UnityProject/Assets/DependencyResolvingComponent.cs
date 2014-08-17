using Assets.Player;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets
{
    public class DependencyResolvingComponent : MonoBehaviour
    {
        private Dictionary<Type, Component> dependencies;

        public TurnController turnController { get { return getDependency<TurnController>(); } }
        public GraphUtility graphUtility { get { return getDependency<GraphUtility>(); } }

        public DependencyResolvingComponent()
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
