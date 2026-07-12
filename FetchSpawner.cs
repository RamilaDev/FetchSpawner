using UnityEngine;

namespace Ramila
{
    /// <summary>
    /// Generic base class that automates the creation, access, and destruction of
    /// Singleton-like prefab instances.
    ///
    /// Unlike a traditional Singleton, instances are created on demand by calling
    /// <see cref="Fetch(S)"/> instead of existing permanently in the scene.
    ///
    /// This class is useful for systems such as:
    /// - UI Popups
    /// - Notifications
    /// - Dialog windows
    /// - Temporary managers
    /// - Runtime tools
    ///
    /// T : Component type that inherits from FetchSpawner.
    /// S : Initialization data passed when the instance is created.
    /// </summary>
    public abstract class FetchSpawner<T, S> : MonoBehaviour where T : FetchSpawner<T, S>
    {
        /// <summary>
        /// Current active instance.
        /// Returns null if no instance has been spawned.
        /// </summary>
        public static T Instance { get; private set; }

        /// <summary>
        /// Stores the latest initialization data passed through Fetch().
        /// Can be useful if other systems need to inspect the creation parameters.
        /// </summary>
        public static S DataInit { get; private set; }

        /// <summary>
        /// Returns the prefab that should be instantiated.
        ///
        /// Usually this references a prefab stored inside a ScriptableObject,
        /// resource database, service locator, or another centralized asset registry.
        /// </summary>
        protected abstract GameObject GetMyPrefab();

        /// <summary>
        /// Called immediately after the prefab has been instantiated.
        ///
        /// This works similarly to Start(), but receives the initialization data
        /// supplied to Fetch().
        ///
        /// Use this method to initialize UI, managers, or runtime state.
        /// </summary>
        protected abstract void OnFetch(S s);

        /// <summary>
        /// Called when DestroyFetch() requests the instance to be destroyed.
        ///
        /// Override this if additional cleanup, animations or transitions
        /// are required before destruction.
        ///
        /// The default implementation simply destroys the GameObject.
        /// </summary>
        public virtual void OnDestroyFetch()
        {
            Destroy(Instance.gameObject);
        }

        /// <summary>
        /// Creates the singleton instance if one does not already exist.
        ///
        /// If an instance already exists, it is returned immediately.
        /// </summary>
        /// <param name="s">
        /// Initialization data passed to the new instance.
        /// </param>
        /// <returns>
        /// The active instance.
        /// Returns null if no prefab could be obtained.
        /// </returns>
        public static T Fetch(S s)
        {
            DataInit = s;

            if (Instance != null)
                return Instance;

            // Temporary object used only to retrieve the prefab reference.
            GameObject tempObj = new GameObject("TempFetcher");
            T temp = tempObj.AddComponent<T>();

            GameObject prefab = temp.GetMyPrefab();

            Destroy(tempObj);

            if (prefab == null)
            {
                Debug.LogError($"Prefab not found for {typeof(T)}");
                return null;
            }

            Instance = Instantiate(prefab).GetComponent<T>();

            Instance.OnFetch(s);

            return Instance;
        }

        /// <summary>
        /// Creates an instance using the GameObject this component belongs to,
        /// instead of using GetMyPrefab().
        ///
        /// This is useful when multiple prefab variants exist and each one
        /// should be able to spawn itself independently.
        /// </summary>
        /// <param name="s">
        /// Initialization data.
        /// </param>
        /// <returns>
        /// The active instance.
        /// </returns>
        public T FetchLocally(S s)
        {
            DataInit = s;

            if (Instance != null)
                return Instance;

            Instance = Instantiate(gameObject).GetComponent<T>();

            Instance.OnFetch(s);

            return Instance;
        }

        /// <summary>
        /// Destroys the current Fetch instance.
        ///
        /// If no instance exists, nothing happens.
        ///
        /// Internally calls OnDestroyFetch(), allowing subclasses
        /// to customize destruction behaviour.
        /// </summary>
        public static void DestroyFetch()
        {
            if (Instance == null)
                return;

            Instance.OnDestroyFetch();
            Instance = null;
        }
    }
}
