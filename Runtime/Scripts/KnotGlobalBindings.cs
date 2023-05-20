using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Knot.Bindings
{
    public static class KnotGlobalBindings
    {
        internal static KnotGlobalBindingsManager Manager => _manager;
        private static KnotGlobalBindingsManager _manager;


        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterAssembliesLoaded)]
        static void Init()
        {
            _manager = GetManager();
        }

        static KnotGlobalBindingsManager GetManager()
        {
            var manager = new GameObject(nameof(KnotGlobalBindingsManager)).AddComponent<KnotGlobalBindingsManager>();
            Object.DontDestroyOnLoad(manager);

            return manager;
        }

        public class KnotGlobalBindingsManager : MonoBehaviour
        {
            
        }
    }
}
