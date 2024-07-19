using UnityEngine;

namespace SadUtils
{
    public abstract class Singleton<T> : MonoBehaviour
    {
        public static T Instance { get; private set; }

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

        public static WaitUntil WaitForInstance = new(() =>
        {
            return HasInstance;
        });
    }
}
