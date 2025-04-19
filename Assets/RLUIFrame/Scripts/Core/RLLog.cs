using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace RL
{
    public static class RLLog
    {
        public static bool EnableLog = true;
        public static void Log(string format, params object[] args)
        {
            if (!EnableLog)
                return;
            string msg = format;
            if (args != null)
                msg = string.Format(format, args);
            Debug.Log(msg);
        }
    }
}