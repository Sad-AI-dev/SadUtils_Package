using UnityEngine;

namespace SadUtils
{
    public abstract class Singleton<T> : MonoBehaviour
    {
        public static T Instance { get; private set; }

        public static WaitUntil WaitForInstance => GetWaitForInstance();
        private static WaitUntil waitForInstance;

        public static bool HasInstance { get; private set; }

        protected abstract void Awake();

        protected void SetInstance(T instance)
        {
            if (Instance != null)
                Destroy(gameObject);
            else
            {
                Instance = instance;
                HasInstance = true;
            }
        }

        private static WaitUntil GetWaitForInstance()
        {
            waitForInstance ??= new(() =>
            {
                return HasInstance;
            });

            return waitForInstance;
        }
    }
}
