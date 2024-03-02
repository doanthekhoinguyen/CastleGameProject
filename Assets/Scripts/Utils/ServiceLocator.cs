using System.Collections;
using System.Collections.Generic;
using Castle.CustomUtil.EventManager;
using UnityEngine;

namespace Castle.CustomUtil
{
    /// <summary>
    /// The helper to get manager as unique instance and avoid creating singleton instance everywhere
    /// </summary>
    public class ServiceLocator : MonoBehaviour
    {
        [SerializeField] private GameObject gameCollectionPrefab;
        [SerializeField] private GameObject inputManagerPrefab;
        [SerializeField] private GameObject gameEventManagerPrefab;
        [SerializeField] private GameObject audioManagerPrefab;
        [SerializeField] private GameObject gameAssetManagerPrefab;
        public static ServiceLocator Instance { get; private set; }

        // managers
        public AudioManager AudioManager { get; private set; }
        public InputManager InputManager { get; private set; }
        public GameEventManager GameEventManager { get; private set; }
        public GameCollectionManager GameCollectionManager { get; private set; }
        public GameAssetManager GameAssetManager { get; private set; }
        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(this);
                return;
            }

            Instance = this;

            DontDestroyOnLoad(gameObject);

            // Binding
            AudioManager = GetComponentInChildren<AudioManager>();
            if (AudioManager == null)
            {
                var go = Instantiate(audioManagerPrefab, transform);
                AudioManager = go.GetComponent<AudioManager>();
            }

            InputManager = GetComponentInChildren<InputManager>();
            if (InputManager == null)
            {
                var go = Instantiate(inputManagerPrefab, transform);
                InputManager = go.GetComponent<InputManager>();
            }

            GameEventManager = GetComponentInChildren<GameEventManager>();
            if (GameCollectionManager == null)
            {
                var go = Instantiate(gameEventManagerPrefab, transform);
                GameEventManager = go.GetComponent<GameEventManager>();
            }

            GameCollectionManager = GetComponentInChildren<GameCollectionManager>();
            if (GameCollectionManager == null)
            {
                var go = Instantiate(gameCollectionPrefab, transform);
                GameCollectionManager = go.GetComponent<GameCollectionManager>();
            }

            GameAssetManager = GetComponentInChildren<GameAssetManager>();
            if (GameAssetManager == null)
            {
                var go = Instantiate(gameAssetManagerPrefab, transform);
                GameAssetManager = go.GetComponent<GameAssetManager>();
            }
        }
    }
}
