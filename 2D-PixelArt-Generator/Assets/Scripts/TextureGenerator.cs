using UnityEngine;

public class TextureGenerator : MonoBehaviour
{
    [SerializeField]
    private TexturePainter texturePainter;
    
    private Texture2D texture;

    /// <summary>
    /// Erzeugt eine leere Textur
    /// </summary>
    /// <param name="width">Breite der Textur</param>
    /// <param name="height">H�he der Textur</param>
    /// <returns>Die erzeugte Textur mit entsprechenden Ma�en</returns>
    public Texture2D GenerateEmpty(int width, int height)
    {
        texture = new Texture2D(width, height);
        texture.filterMode = FilterMode.Point;
        return texture;
    }

    /// <summary>
    /// Erezugt eine zuf�llig bemalte Textur
    /// </summary>
    /// <param name="width">Breite der Textur</param>
    /// <param name="height">H�he der Textur</param>
    /// <returns>Die erzeugte Textur</returns>
    public Texture2D GenerateRandom(int width, int height)
    {
        texture = GenerateEmpty(width, height);
        texturePainter.SetActiveTexture(texture);
        texturePainter.DrawRandom();
        return texture;
    }
}
