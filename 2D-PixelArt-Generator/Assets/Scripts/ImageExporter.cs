// Dieses Script verwendet den Standalone File Browser von Gökhan Gökçe
// Hier zu finden: https://github.com/gkngkc/UnityStandaloneFileBrowser
// Verwendet unter folgender Lizenz:
//MIT License
//Copyright(c) 2017 Gökhan Gökçe
//
//Permission is hereby granted, free of charge, to any person obtaining a copy
//of this software and associated documentation files (the "Software"), to deal
//in the Software without restriction, including without limitation the rights
//to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
//copies of the Software, and to permit persons to whom the Software is
//furnished to do so, subject to the following conditions:
//
//The above copyright notice and this permission notice shall be included in all
//copies or substantial portions of the Software.
//
//THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
//AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
//LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
//OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
//SOFTWARE.

using SFB;
using System.IO;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.EventSystems;

public static class ImageExporter
{
    public static ImageSettings imageSettings;

#if UNITY_WEBGL && !UNITY_EDITOR
    // WebGL
    [DllImport("__Internal")]
    private static extern void DownloadFile(string gameObjectName, string methodName, string filename, byte[] byteArray, int byteArraySize);
    
    /// <summary>
    /// Speichert den Sprite sprite als .png-Datei mithilfe des StandaloneFileBrowser von Gökhan Gökçe ab.
    /// </summary>
    /// <param name="sprite">Der zu speichernde Sprite</param>
    public static void ExortSprite(Sprite sprite)
    {
        string fileName = "PixelArt" + imageSettings.Seed + ".png";
        byte[] textureBytes = sprite.texture.EncodeToPNG();
        DownloadFile("", "OnFileDownload", fileName, textureBytes, textureBytes.Length);
    }
#else
    //Standalone & editor
    /// <summary>
    /// Speichert den Sprite sprite als .png-Datei mithilfe des StandaloneFileBrowser von Gökhan Gökçe ab.
    /// </summary>
    /// <param name="sprite">Der zu speichernde Sprite</param>
    public static void ExortSprite(Sprite sprite)
    {
        string fileName = "PixelArt" + imageSettings.Seed;
        string path = StandaloneFileBrowser.SaveFilePanel("Save image", "", fileName, "png");
        File.WriteAllBytes(path, ImageConversion.EncodeToPNG(sprite.texture));
    }
#endif
}
