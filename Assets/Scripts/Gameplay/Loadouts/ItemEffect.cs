using UnityEngine;
using UnityEngine.Events;

namespace Gameplay
{
    public class ItemEffect : MonoBehaviour
    {
        public UnityEvent onPlay;

        public void Play()
        {
            onPlay.Invoke();
        }
    }
}