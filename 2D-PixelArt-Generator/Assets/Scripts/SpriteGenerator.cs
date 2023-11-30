using UnityEngine;

public class SpriteGenerator : MonoBehaviour 
{
    [SerializeField]
    private TextureGenerator textureGenerator;
    [SerializeField]
    private ImageSettings imageSettings;

    private Texture2D texture;

    /// <summary>
    /// Erzeugt einen Sprite mit zufällig bemalter Textur
    /// </summary>
    /// <returns>Der erzeugte Sprite</returns>
    public Sprite Generate()
    {
        texture = textureGenerator.GenerateRandom(imageSettings.Width, imageSettings.Height);
        return Sprite.Create(texture, new Rect(0, 0, imageSettings.Width, imageSettings.Height), new Vector2(.5f, .5f), 16);
    }
}
