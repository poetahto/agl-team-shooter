using System.Collections;
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

        private IEnumerator Start()
        {
            yield return null;
#if !UNITY_EDITOR
            SceneManager.LoadScene(firstScene);
#endif
        }
    }
}
