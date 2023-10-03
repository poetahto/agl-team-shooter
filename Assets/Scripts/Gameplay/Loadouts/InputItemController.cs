using UnityEngine;

namespace Gameplay
{
    public abstract class InputItemController : MonoBehaviour
    {
        public virtual void OnClientStart()
        {
        }

        public virtual void OnClientLogic()
        {
        }

        public virtual void OnClientSelectStart()
        {
        }

        public virtual void OnClientSelectStop()
        {
        }
    }
}
