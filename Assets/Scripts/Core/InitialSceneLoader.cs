using System.Collections;
using FishNet.Utility;
using UnityEngine;

namespace DefaultNamespace
{
    public class InitialSceneLoader : MonoBehaviour
    {
        [Scene]
        [SerializeField]
        private string firstScene;

        private IEnumerator Start()
        {
            yield return null;
#if !UNITY_EDITOR
            UnityEngine.SceneManagement.SceneManager.LoadScene(firstScene);
#endif
        }
    }
}
