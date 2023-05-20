using Core;
using Poetools.UI.Builders;
using Poetools.UI.Items;
using UnityEngine;

namespace DefaultNamespace
{
    public class ConnectToGamePromptLogic : MonoBehaviour
    {
        [SerializeField]
        private CanvasGroup canvasGroup;

        private string _port;
        private string _address;
        private IVisibilityTransition _visibility;

        private void Awake()
        {
            _visibility = CommonTransitions.Popup(canvasGroup);
            _visibility.Show();
        }

        private void Start()
        {
            new ExistingMenuBuilder()
                .Register("connection_address", new StringInputField(value => _address = value))
                .Register("connection_port", new StringInputField(value => _port = value))
                .Register("connect", new Button(HandleConnectToGame, "Connect"))
                .Register("cancel", new Button(() => _visibility.HideThenDestroy(gameObject), "Cancel"))
                .Build(gameObject);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
                _visibility.HideThenDestroy(gameObject);
        }

        private void HandleConnectToGame()
        {
            if (ushort.TryParse(_port, out ushort port))
                Services.GameplaySystem.ConnectToGame(_address, port);
        }
    }
}
