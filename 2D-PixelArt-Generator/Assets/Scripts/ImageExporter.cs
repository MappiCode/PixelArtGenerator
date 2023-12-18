using SFB;
using System.IO;
using UnityEngine;

public static class ImageExporter
{
    public static ImageSettings imageSettings;

    /// <summary>
    /// Speichert den Sprite sprite als .png-Datei mithilfe des StandaloneFileBrowser von Gökhan Gökçe ab.
    /// </summary>
    /// <param name="sprite">Der zu speichernde Sprite</param>
    public static void SaveSpriteToAssets(Sprite sprite)
    {
        string fileName = "PixelArt" + imageSettings.Seed + ".png";
        string path = StandaloneFileBrowser.SaveFilePanel("Save image", "", fileName, "png");
        File.WriteAllBytes(path, ImageConversion.EncodeToPNG(sprite.texture));
    }
}
