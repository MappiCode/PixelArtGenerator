using System.IO;
using UnityEditor;
using UnityEngine;

public static class ImageExporter
{
    private static int counter = 0;

    /// <summary>
    /// Speichert den Sprite sprite als .png-Datei in den Assets-Ordner des Projekts ab.
    /// </summary>
    /// <param name="sprite">Der zu speichernde Sprite</param>
    public static void SaveSpriteToAssets(Sprite sprite)
    {
        string fileName = "PixelArt" + counter++ + ".png";

        switch (Application.platform)
        {
            case RuntimePlatform.WindowsPlayer:
                SaveOnWindows(sprite, fileName);
                break;
            case RuntimePlatform.WindowsEditor: 
                SaveOnWindows(sprite, fileName);
                break;
            default:
                return;
        }
    }

    private static void SaveOnWindows(Sprite sprite, string fileName)
    {
        DirectoryInfo dir = new DirectoryInfo(EditorUtility.SaveFilePanel
        (
            "Save texture as PNG",
            "",
            fileName,
            "png"
        ));
        string path = dir.FullName;

        if (string.IsNullOrEmpty(path))
            return;

        File.WriteAllBytes(path, ImageConversion.EncodeToPNG(sprite.texture));
    }
}
