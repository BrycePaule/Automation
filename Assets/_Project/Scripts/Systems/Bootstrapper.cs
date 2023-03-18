using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace bpdev
{
    public static class Bootstrapper
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        public static void Execute() => Object.DontDestroyOnLoad(Object.Instantiate(Resources.Load("PersistentSingletons")));

    }
}