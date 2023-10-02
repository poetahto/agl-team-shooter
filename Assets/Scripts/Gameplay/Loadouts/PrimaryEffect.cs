using UnityEngine;
using UnityEngine.Events;

namespace Gameplay
{
    public class PrimaryEffect : MonoBehaviour
    {
        public UnityEvent onPlay;

        public void Play()
        {
            onPlay.Invoke();
        }
    }
}