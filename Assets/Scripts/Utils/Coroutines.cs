using System.Collections;
using UnityEngine;

namespace SorcererRush
{
    public static class Coroutines
    {
        private static CoroutinesMono go;

        static Coroutines()
        {
            go = new GameObject("Coroutines").AddComponent<CoroutinesMono>();
            Object.DontDestroyOnLoad(go);
        }

        public static Coroutine Start(IEnumerator enumerator)
        {
            return go.StartCoroutine(enumerator);
        }
    }

    internal class CoroutinesMono : MonoBehaviour
    {
    }
}