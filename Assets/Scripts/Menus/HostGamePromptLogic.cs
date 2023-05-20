using Core;
using Poetools.UI.Builders;
using Poetools.UI.Items;
using UnityEngine;

namespace DefaultNamespace
{
    public class HostGamePromptLogic : MonoBehaviour
    {
        [SerializeField]
        private CanvasGroup canvasGroup;

        private string _port;
        private IVisibilityTransition _visibility;

        private void Awake()
        {
            _visibility = CommonTransitions.Popup(canvasGroup);
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
            if (ushort.TryParse(_port, out ushort result))
                Services.GameplaySystem.HostGame(result);
        }
    }
}
