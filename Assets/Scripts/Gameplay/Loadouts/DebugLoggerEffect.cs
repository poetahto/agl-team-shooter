using UnityEngine;

namespace Gameplay
{
    public class DebugLoggerEffect : MonoBehaviour
    {
        public string message = "Test Message";

        public void Play()
        {
            Debug.Log(message);
        }
    }
}