using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private ImageSettings imageSettings;

    [SerializeField]
    private SpriteGenerator spriteGenerator;
    [SerializeField]
    private SpriteLoader spriteLoader;
    [SerializeField]
    private GameObject colorPicker;

    [Header("Input Fields")]
    [SerializeField]
    private TMP_InputField widthInput;
    [SerializeField]
    private TMP_InputField heightInput;
    [SerializeField]
    private TMP_InputField seedInput;
    [SerializeField]
    private TMP_InputField sunXInput;
    [SerializeField]
    private TMP_InputField sunYInput;

    [Header("Slider")]
    [SerializeField]
    private Slider amplitudeSlider;
    [SerializeField]
    private Slider bgColorStepsSlider;
    [SerializeField]
    private Slider sunRadiusSlider;

    [Header("Value Texts")]
    [SerializeField]
    private TextMeshProUGUI amplitudeValue;
    [SerializeField]
    private TextMeshProUGUI bgColorStepsValue;
    [SerializeField]
    private TextMeshProUGUI sunRadiusValue;

    [Header("Buttons")]
    [SerializeField]
    private Button color1Button;
    [SerializeField]
    private Button color2Button;
    [SerializeField]
    private Button randomiseButton;
    [SerializeField]
    private Button regenerateButton;
    [SerializeField]
    private Button exportButton;

    [Header("Randomiser Buttons")]
    [SerializeField]
    private Button seedRandomiserButton;
    [SerializeField]
    private Button amplitudeRandomiserButton;
    [SerializeField]
    private Button bgColorStepsRandomiserButton;
    [SerializeField]
    private Button sunPositionRandomiserButton;
    [SerializeField]
    private Button sunRadiusRandomiserButton;
    [SerializeField]
    private Button color1RandomiserButton;
    [SerializeField]
    private Button color2RandomiserButton;


    private void Start()
    {
        colorPicker.SetActive(false);
        
        //Buttons
        color1Button.onClick.AddListener(() => colorPicker.SetActive(true));
        color2Button.onClick.AddListener(() => colorPicker.SetActive(true));
        randomiseButton.onClick.AddListener(RandomiseClicked);
        regenerateButton.onClick.AddListener(RegenerateClicked);
        exportButton.onClick.AddListener(ExportClicked);

        //Input Fields
        seedInput.onValueChanged.AddListener(seed => imageSettings.Seed = ValidateInput(seed));
        seedInput.onEndEdit.AddListener(_ => UpdateInputs());
        widthInput.onValueChanged.AddListener(width => imageSettings.Width = ValidateInput(width));
        widthInput.onEndEdit.AddListener(_ => UpdateInputs());
        heightInput.onValueChanged.AddListener(height => imageSettings.Height = ValidateInput(height));
        heightInput.onEndEdit.AddListener(_ => UpdateInputs());
        sunXInput.onValueChanged.AddListener(x => imageSettings.SunPosition = new Vector2Int(ValidateInput(x), imageSettings.SunPosition.y));
        sunXInput.onEndEdit.AddListener(_ => UpdateInputs());
        sunYInput.onValueChanged.AddListener(y => imageSettings.SunPosition = new Vector2Int(imageSettings.SunPosition.x, ValidateInput(y)));
        sunYInput.onEndEdit.AddListener(_ => UpdateInputs());

        //Slider
        amplitudeSlider.onValueChanged.AddListener(amplitude =>
        {
            imageSettings.Amplitude = amplitude;
            UpdateInputs();
        });
        bgColorStepsSlider.onValueChanged.AddListener(steps =>
        { 
            imageSettings.BackgroundColorSteps = Mathf.RoundToInt(steps);
            UpdateInputs();
        });
        sunRadiusSlider.onValueChanged.AddListener(radius =>
        {
            imageSettings.SunRadius = Mathf.RoundToInt(radius);
            UpdateInputs();
        });
        
        //Randomiser Buttons
        seedRandomiserButton.onClick.AddListener(() =>
        { 
            imageSettings.RandomiseSeed();
            UpdateInputs();
        });
        amplitudeRandomiserButton.onClick.AddListener(() =>
        { 
            imageSettings.RandomiseAmplitude();
            UpdateInputs(); 
        });
        bgColorStepsRandomiserButton.onClick.AddListener(() =>
        {
            imageSettings.RandomiseBackgroundColorSteps();
            UpdateInputs();
        });
        sunPositionRandomiserButton.onClick.AddListener(() => 
        { 
            imageSettings.RandomiseSunPosition();
            UpdateInputs();
        });
        sunRadiusRandomiserButton.onClick.AddListener(() =>
        {
            imageSettings.RandomiseSunRadius();
            UpdateInputs();
        });
        color1RandomiserButton.onClick.AddListener(() =>
        {
            Colors.SkyTop = Colors.NewRandomColor();
            UpdateInputs();
        });
        color2RandomiserButton.onClick.AddListener(() =>
        {
            Colors.SkyBottom = Colors.NewRandomColor();
            UpdateInputs();
        });

        RandomiseClicked();
    }

    private void OnDestroy()
    {
        randomiseButton.onClick?.RemoveListener(RandomiseClicked);
        regenerateButton.onClick?.RemoveListener(RegenerateClicked);
        widthInput.onValueChanged.RemoveAllListeners();
        widthInput.onEndEdit.RemoveAllListeners();
        heightInput.onValueChanged.RemoveAllListeners();
        heightInput.onEndEdit.RemoveAllListeners();
        seedInput.onValueChanged.RemoveAllListeners();
        amplitudeSlider.onValueChanged.RemoveAllListeners();
        bgColorStepsSlider.onValueChanged.RemoveAllListeners();
        sunXInput.onValueChanged.RemoveAllListeners();
        sunXInput.onEndEdit.RemoveAllListeners();
        sunYInput.onValueChanged.RemoveAllListeners();
        sunYInput.onEndEdit.RemoveAllListeners();
        sunRadiusSlider.onValueChanged.RemoveAllListeners();
        seedRandomiserButton.onClick?.RemoveAllListeners();
        amplitudeRandomiserButton.onClick?.RemoveAllListeners();
        bgColorStepsRandomiserButton.onClick?.RemoveAllListeners();
        sunPositionRandomiserButton.onClick?.RemoveAllListeners();
        sunRadiusRandomiserButton.onClick?.RemoveAllListeners();
        color1RandomiserButton.onClick?.RemoveAllListeners();
        color2RandomiserButton.onClick?.RemoveAllListeners();
    }

    /// <summary>
    /// Aktualisiert die UI-Elemente entsprechend der aktuellen Bild-Parameter
    /// </summary>
    public void UpdateInputs()
    {
        widthInput.text = imageSettings.Width.ToString();
        heightInput.text = imageSettings.Height.ToString();
        seedInput.text = imageSettings.Seed.ToString();
        amplitudeSlider.value = imageSettings.Amplitude;
        amplitudeValue.text = Mathf.RoundToInt(imageSettings.Amplitude * 100f).ToString() + '%';
        bgColorStepsSlider.value = imageSettings.BackgroundColorSteps;
        bgColorStepsValue.text = imageSettings.BackgroundColorSteps.ToString();
        sunXInput.text = imageSettings.SunPosition.x.ToString();
        sunYInput.text = imageSettings.SunPosition.y.ToString();
        sunRadiusSlider.value = imageSettings.SunRadius;
        sunRadiusValue.text = imageSettings.SunRadius.ToString();
        color1Button.image.color = Colors.SkyTop;
        color2Button.image.color = Colors.SkyBottom;
    }

    /// <summary>
    /// Randomisiert alle Bildparameter (außer Bildabmessungen) und lässt ein neues Bild erstellen
    /// </summary>
    private void RandomiseClicked()
    {
        imageSettings.Randomise();
        RegenerateClicked();
    }

    /// <summary>
    /// Lässt ein neues Bild erstellen und Updates das UI
    /// </summary>
    private void RegenerateClicked()
    {
        Sprite sprite = spriteGenerator.Generate();
        UpdateInputs();
        spriteLoader.LoadSprite(sprite);
    }

    /// <summary>
    /// Stößt den Export-Vorgang an
    /// </summary>
    private void ExportClicked()
    {
        Sprite sprite = spriteLoader.GetCurrentSprite();
        ImageExporter.SaveSpriteToAssets(sprite);
    }

    /// <summary>
    /// Überprüft einen string input ob er nicht leer ist und gibt ihn als Zahlenwert zurück
    /// </summary>
    /// <param name="input">Zu überprüfende Eingabe</param>
    /// <returns>input als Ganzzahl</returns>
    private int ValidateInput(string input)
    {
        if (int.TryParse(input, out int value))
            return value;
        else
            return 0;
    }
}
