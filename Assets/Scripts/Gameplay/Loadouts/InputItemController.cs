using UnityEngine;

namespace Gameplay
{
    public abstract class InputItemController : MonoBehaviour
    {
        public abstract void OnClientStart();
        public abstract void OnClientLogic();
    }
}
