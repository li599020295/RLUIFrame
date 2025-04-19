using RL;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace RL
{
    public class SingletonManager : MonoBehaviour
    {
        public static SingletonManager Instance { get; private set; }
        public List<SingletonIn> singletons = new List<SingletonIn>();
        public List<SingletonIn> singletonUpdates = new List<SingletonIn>();
        public static void Create()
        {
            GameObject obj = new GameObject("[SingletonManager]");
            obj.AddComponent<SingletonManager>();
            DontDestroyOnLoad(obj);
        }

        public void AddSingleton(SingletonIn singleton)
        {
            singletons.Add(singleton);
        }

        public void AddSingletonUpdate(SingletonIn singleton)
        {
            singletonUpdates.Add(singleton);
        }

        public void RemoveSingletonUpdate(SingletonIn singleton)
        {
            singletonUpdates.Remove(singleton);
        }

        // Start is called before the first frame update
        void Awake()
        {
            Instance = this;
        }

        private void Update()
        {
            if (singletonUpdates.Count == 0)
                return;
            for(int i= singletonUpdates.Count - 1;i>=0;i--)
            {
                SingletonIn singleton = singletonUpdates[i];
                singleton.OnUpdate();
            }
        }

        void OnDestroy()
        {
            for(int i = 0; i < singletons.Count; i++)
            {
                SingletonIn singleton = singletons[i];
                singleton.OnDestroy();
            }
            singletons.Clear();
            singletons = null;
        }
    }
}