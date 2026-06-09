using System;
using UnityEngine;

namespace BaPlayerLocation.Subscriber
{
    internal static class ModLog
    {
        private const string Prefix = "[LIB_BaPlayerLocation]";

        internal static void Info(string message) =>
            Debug.Log(Prefix + " " + message);

        internal static void Error(string message, Exception ex = null)
        {
            if (ex == null)
                Debug.LogError(Prefix + " " + message);
            else
                Debug.LogError(Prefix + " " + message + " | " + ex.GetType().Name + ": " + ex.Message);
        }
    }
}
