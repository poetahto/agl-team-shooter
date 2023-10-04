using FishNet.Utility;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace DefaultNamespace
{
    public class InitialSceneLoader : MonoBehaviour
    {
        [Scene]
        [SerializeField]
        private string firstScene;

        private void Start()
        {
            SceneManager.LoadScene(firstScene);
        }
    }
}
