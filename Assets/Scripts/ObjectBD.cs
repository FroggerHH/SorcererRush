using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace SorcererRush
{
    [AddComponentMenu("Game/Object BD")]
    public class ObjectBD : MonoBehaviour
    {
        private static ObjectBD m_instance;

        [HideInInspector] public static ObjectBD Instance => m_instance;

        [SerializeField] private List<GameObject> m_prefabs = new();
        [SerializeField] private List<ItemData> m_items = new();
        private Dictionary<int, GameObject> m_prefabsByHash = new();
        private Dictionary<int, ItemData> m_itemsByHash = new();

        private void Awake()
        {
            InitSingleton();
            UpdateHashes();
        }

        private void UpdateHashes()
        {
            m_prefabsByHash.Clear();
            foreach (GameObject gameObject in m_prefabs)
                m_prefabsByHash.Add(gameObject.name.GetStableHashCode(), gameObject);
        }

        private void InitSingleton()
        {
            if (m_instance)
            {
                Debug.LogError("Double singleton");
                Destroy(gameObject);
                return;
            }

            m_instance = this;
        }

        public GameObject GetPrefab(string name) => GetPrefab(name.GetStableHashCode());

        public GameObject GetPrefab(int hash)
        {
            GameObject result;
            return m_prefabsByHash.TryGetValue(hash, out result) ? result : null;
        }

        public ItemData GetItem(string name) => GetItem(name.GetStableHashCode());

        public ItemData GetItem(int hash)
        {
            ItemData result;
            return m_itemsByHash.TryGetValue(hash, out result) ? result : null;
        }
    }
}