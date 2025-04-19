using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace RL
{
    public class Singleton<T> : SingletonIn where T : class
    {
        protected static T instance;
        public static T Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = (T)Activator.CreateInstance(typeof(T), true);
                    (instance as Singleton<T>).OnInit();
                    if(SingletonManager.Instance == null)
                        SingletonManager.Create();
                    SingletonIn singletonIn = instance as SingletonIn;
                    SingletonManager.Instance.AddSingleton(singletonIn);
                }
                return instance;
            }
        }

        protected void AddUpdate()
        {
            if (SingletonManager.Instance != null)
            {
                SingletonIn singletonIn = instance as SingletonIn;
                SingletonManager.Instance.AddSingletonUpdate(singletonIn);
            }
        }

        protected void RemoveUpdate()
        {
            if (SingletonManager.Instance != null)
            {
                SingletonIn singletonIn = instance as SingletonIn;
                SingletonManager.Instance.RemoveSingletonUpdate(singletonIn);
            }
        }

        public void OnUpdate()
        {
            Update();
        }

        public void OnDestroy()
        {
            Destroy();
        }

        public void OnInit()
        {
            Init();
        } 

        protected virtual void Init()
        {
        }

        protected virtual void Update()
        {
        }

        protected virtual void Destroy()
        {
        }
    }
}
