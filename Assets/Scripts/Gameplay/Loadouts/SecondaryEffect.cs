using UnityEngine;
using UnityEngine.Events;

namespace Gameplay
{
    public class SecondaryEffect : MonoBehaviour
    {
        public UnityEvent onPlay;

        public void Play()
        {
            onPlay.Invoke();
        }
    }
}