using Core;
using Poetools.UI;
using Poetools.UI.Builders;
using Poetools.UI.Items;
using UniRx;
using UnityEngine;

public class SettingsMenuLogic : MonoBehaviour
{
    [SerializeField]
    private CanvasGroup canvasGroup;

    [SerializeField]
    private GameObject videoSettings;

    [SerializeField]
    private GameObject gameplaySettings;

    private GameplaySettings _settings = new GameplaySettings();

    private IVisibilityTransition _visibility;

    private void Awake()
    {
        _visibility = new DefaultVisibilityTransition(canvasGroup);
        _visibility.Show();
    }

    private void Start()
    {
        string n = nameof(SettingsMenuLogic);

        { // Setting up the video menu.
            new AutoMenuBuilder(n)
                .Register("resolution", CommonMenuItems.ResolutionDropdown().WithLabel("Screen Resolution"))
                .Register("quality", CommonMenuItems.QualityDropdown().WithLabel("Graphic Quality"))
                .Build(videoSettings)
                .AddTo(this);
        }
        { // Setting up the gameplay menu.
            new AutoMenuBuilder(n)
                .Register("sensitivity", new Slider(value => _settings.Sensitivity = value).WithLabel("Mouse Sensitivity"))
                .Register("fov", new Slider(value => _settings.FOV = value).WithLabel("Camera Field-Of-View"))
                .Register("username", new StringInputField(value => _settings.Username = value).WithLabel("Online Username"))
                .Build(gameplaySettings)
                .AddTo(this);
        }

        // Bind logic to manually placed items.
        new ExistingMenuBuilder()
            .Register("exit", new Button(() => _visibility.HideThenDestroy(gameObject), "Exit"))
            .Build(gameObject).AddTo(this);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            _visibility.HideThenDestroy(gameObject);
    }

    private void OnGUI()
    {
        GUILayout.Label($"Sensitivity: {_settings.Sensitivity}");
        GUILayout.Label($"FOV: {_settings.FOV}");
        GUILayout.Label($"Username: {_settings.Username}");
        GUILayout.Label($"Quality: {QualitySettings.names[QualitySettings.GetQualityLevel()]}");
    }
}

public class GameplaySettings
{
    public float Sensitivity;
    public float FOV;
    public string Username;
}
