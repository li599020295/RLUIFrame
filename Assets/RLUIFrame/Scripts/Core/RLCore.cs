using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace RL
{
    public class RLCore : MonoBehaviour
    {
        public static RLCore Instance { get; private set; }
        private void Awake()
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }

        private void OnDestroy()
        { 
        }
    }
}