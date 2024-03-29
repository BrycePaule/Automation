using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace bpdev
{
    public static class Bootstrapper
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        public static void Execute()
        {
            Object.DontDestroyOnLoad(Object.Instantiate(Resources.Load("PersistentSingletons")));
        }


        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        private static void InitGame()
        {
            if (SaveManager.Instance.saveFile != null)
            {
                SaveManager.Instance.Load();
            }
            else
            {
                MapGenerator.Instance.RandomiseSeed();
            }
        }
    }
}