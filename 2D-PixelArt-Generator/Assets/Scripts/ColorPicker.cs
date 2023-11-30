using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ColorPicker : MonoBehaviour
{
    [SerializeField]
    private UIManager uiManager;
    [SerializeField]
    private TMP_Text colorTitle;
    [SerializeField]
    private Button closeButton;
    [SerializeField]
    private Button outsideOfPanel;
    [SerializeField]
    private Button colorChartButton;
    [SerializeField]
    private Button color1Button;
    [SerializeField]
    private Button color2Button;
    [SerializeField]
    private Image colorChart;
    [SerializeField]
    private Image selector;

    private int colorNumber;

    private void Start()
    {
        closeButton.onClick.AddListener(() => gameObject.SetActive(false));
        outsideOfPanel.onClick.AddListener(() => gameObject.SetActive(false));
        colorChartButton.onClick.AddListener(ChartClicked);
        color1Button.onClick.AddListener(() => colorNumber = 1);
        color2Button.onClick.AddListener(() => colorNumber = 2);
    }

    private void OnDestroy()
    {
        closeButton.onClick?.RemoveAllListeners();
        outsideOfPanel.onClick?.RemoveAllListeners();
        colorChartButton.onClick?.RemoveAllListeners();
        color1Button.onClick?.RemoveAllListeners();
        color2Button.onClick?.RemoveAllListeners();
    }

    private void OnEnable()
    {
        selector.gameObject.SetActive(false);
        colorTitle.text = $"Select Color {colorNumber}";
    }

    private void ChartClicked()
    {
        Vector2 clickPositionOnChart = Input.mousePosition - colorChart.transform.position;
        float textureWidthOnScreen = colorChart.rectTransform.sizeDelta.x * colorChart.transform.lossyScale.x;
        float textureHeightOnScreen = colorChart.rectTransform.sizeDelta.y * colorChart.transform.lossyScale.y;
        float xScaler = clickPositionOnChart.x / textureWidthOnScreen;
        float yScaler = clickPositionOnChart.y / textureHeightOnScreen;
        int xOnTexture = Mathf.RoundToInt(colorChart.sprite.texture.width * xScaler);
        int yOnTexture = Mathf.RoundToInt(colorChart.sprite.texture.height * yScaler);
        Color colorOnClickPosition = colorChart.sprite.texture.GetPixel(xOnTexture, yOnTexture);
        
        if (colorNumber == 1)
            Colors.SkyTop = colorOnClickPosition;
        else if(colorNumber == 2)
            Colors.SkyBottom = colorOnClickPosition;

        uiManager.UpdateInputs();

        selector.gameObject.SetActive(true);
        selector.transform.position = Input.mousePosition;
        selector.color = colorOnClickPosition;
    }
}
