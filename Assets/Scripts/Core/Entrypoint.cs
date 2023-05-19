using System.Threading.Tasks;
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
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        private static async Task CheckForAwakeAsync()
        {
            if (SceneManager.GetActiveScene().name != "Entrypoint")
            {
#if UNITY_EDITOR
                Debug.Log($"{"[EDITOR ONLY]".Bold()} Loading Entrypoint...");
                string originalScene = SceneManager.GetActiveScene().name;
                // await Task.Yield(); // We have to wait one frame here, so the Entrypoint can initialize itself
                SceneManager.LoadScene("Entrypoint", LoadSceneMode.Single);
                await Task.Yield(); // We have to wait one frame here, so the Entrypoint can initialize itself
                Debug.Log($"{"[EDITOR ONLY]".Bold()} Trying to load {originalScene} after Entrypoint...");
                SceneManager.LoadScene(originalScene);
#else
                Debug.LogError("Entrypoint must be initialized before anything else!");
                Application.Quit();
#endif
            }
        }
    }
}
