using Core;
using Cysharp.Threading.Tasks;
using Poetools.UI.Builders;
using Poetools.UI.Items;
using UnityEngine;

namespace DefaultNamespace
{
    public class HostGamePromptLogic : MonoBehaviour
    {
        [SerializeField]
        private CanvasGroup canvasGroup;

        [SerializeField]
        private Transform target;

        private string _port;
        private IVisibilityTransition _visibility;

        private void Awake()
        {
            _visibility = CommonTransitions.Popup(canvasGroup, target);
            _visibility.Show();
        }

        private void Start()
        {
            new ExistingMenuBuilder()
                .Register("hosting_port", new StringInputField(value => _port = value))
                .Register("start", new Button(HandleHostGame))
                .Register("cancel", new Button(() => _visibility.HideThenDestroy(gameObject)))
                .Build(gameObject);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
                _visibility.HideThenDestroy(gameObject);
        }

        private void HandleHostGame()
        {
            _visibility.HideThenDestroy();

            if (ushort.TryParse(_port, out ushort result))
                Services.GameplayRunner.HostGame(result).Forget();
        }
    }
}
