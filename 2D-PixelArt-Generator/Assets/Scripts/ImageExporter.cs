using System;
using System.Diagnostics;
using System.IO;
using UnityEditor;
using UnityEngine;

public static class ImageExporter
{
    public static ImageSettings ImageParameters;
    /// <summary>
    /// Speichert den Sprite sprite als .png-Datei in den Assets-Ordner des Projekts ab.
    /// </summary>
    /// <param name="sprite">Der zu speichernde Sprite</param>
    public static void SaveSpriteToAssets(Sprite sprite)
    {
        string fileName = "PixelArt" + ImageParameters.Seed + ".png";

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
        //DirectoryInfo dir = new DirectoryInfo(EditorUtility.SaveFilePanel
        //(
        //    "Save texture as PNG",
        //    "",
        //    fileName,
        //    "png"
        //));
        //string path = dir.FullName;

        //if (string.IsNullOrEmpty(path))
        //    return;

        string path = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures) + "\\PixelArtGenerator";
        string filePath = Path.Combine(path, fileName);
        Directory.CreateDirectory(path);

        File.WriteAllBytes(filePath, ImageConversion.EncodeToPNG(sprite.texture));

        Process.Start("explorer.exe", "/select, " + filePath);
    }
}
