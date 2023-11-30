using UnityEngine;
using UnityEngine.UI;

public class SpriteLoader : MonoBehaviour
{
    private Image image;

    private void Start()
    {
        image = GetComponent<Image>();
    }

    /// <summary>
    /// Lädt den Sprite sprite in die Image Komponente des Gameobjects dieser Komponente
    /// </summary>
    /// <param name="sprite">Sprite welcher geladen werden soll</param>
    public void LoadSprite(Sprite sprite)
    {
        if (image == null)
            return;
        
        image.sprite = sprite;

        float aspectRatio = (float)sprite.texture.width / sprite.texture.height;
        float maxResAspectRatio = (float)ImageSettings.MAX_WIDTH / ImageSettings.MAX_HEIGHT;
        if (aspectRatio > maxResAspectRatio)
            image.rectTransform.sizeDelta = new Vector2(ImageSettings.MAX_WIDTH, ImageSettings.MAX_WIDTH * (1 / aspectRatio));
        else
            image.rectTransform.sizeDelta = new Vector2(ImageSettings.MAX_HEIGHT * aspectRatio, ImageSettings.MAX_HEIGHT);
    }

    /// <summary>
    /// Gibt den aktuell dargestellten Sprite zurück
    /// </summary>
    /// <returns>Der aktuell dargestellt Sprite</returns>
    public Sprite GetCurrentSprite()
    {
        return image.sprite;
    }
}
