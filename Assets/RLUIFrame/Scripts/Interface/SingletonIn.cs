using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace RL
{
    public interface SingletonIn
    {
        public void OnInit();
        public void OnUpdate();
        public void OnDestroy();
    }
}