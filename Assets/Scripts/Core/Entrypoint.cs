using UnityEngine;
using UnityEngine.SceneManagement;

namespace Core
{
    /// <summary>
    /// A runtime utility for ensuring a scene titled "Entrypoint" is always loaded first.
    /// </summary>
    // todo: test to make sure this works.
    public class Entrypoint
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void SetupEntrypoint()
        {
            if (SceneManager.GetActiveScene().name != "Entrypoint")
            {
#if UNITY_EDITOR
                Debug.Log($"{"[EDITOR ONLY]".Bold()} Loading Entrypoint...");
                SceneManager.LoadScene("Entrypoint", LoadSceneMode.Single);
#else
                Debug.LogError($"{SceneManager.GetActiveScene().name} - Entrypoint must be initialized before anything else!");
                // Application.Quit();
#endif
            }
        }
    }
}
