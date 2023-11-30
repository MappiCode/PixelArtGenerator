using System.Collections.Generic;
using UnityEngine;

public class TexturePainter : MonoBehaviour
{
    [SerializeField]
    private ImageSettings imageSettings;

    private Texture2D activeTexture;

    public void SetActiveTexture(Texture2D texture)
    {
        activeTexture = texture;
    }

    /// <summary>
    /// Malt eine Linie auf der Texture texture beginnend bei den Koordinaten start und endend  end mit der Farbe color
    /// </summary>
    /// <param name="start">X-Koordinate für den Startpunkt der Linie</param>
    /// <param name="end">X-Koordinate für den Endpunkt der Linie</param>
    /// <param name="color">Farbe der Linie</param>
    private void DrawLine(Vector2Int start, Vector2Int end, Color color)
    {
        int x, y;
        float slope;

        int xDifference = end.x - start.x;
        int yDifference = end.y - start.y;
        
        if (xDifference == 0)
        {
            for (y = start.y; y <= end.y; y++)
                activeTexture.SetPixel(start.x, y, color);

            return;
        }

        slope = (float)yDifference / (float)xDifference;

        if (Mathf.Abs(slope) <= 1)
        {
            for (x = start.x; x <= end.x; x++)
            {
                if (!CheckXInBounds(x))
                    continue;
                y = Mathf.RoundToInt(slope * x);
                activeTexture.SetPixel(x, y, color);
            }
        }
        else
        {
            for (y = start.y; y <= end.y; y++)
            {
                if (!CheckYInBounds(y))
                    continue;
                x = Mathf.RoundToInt((y - start.y) / slope) + start.x;
                activeTexture.SetPixel(x, y, color);
            }
        }
    }

    /// <summary>
    /// Malt auf der aktiven Textur einen Kreis mit dem Mittelpunkt bei position und einem Radius radius in der Farbe color
    /// </summary>
    /// <param name="position">Mittelpunkt des Kreises</param>
    /// <param name="radius">Radius des Kreises</param>
    /// <param name="color">Farbe des Kreises</param>
    private void DrawCircle(Vector2Int position, int radius, Color color)
    {
        DrawEllipse(position, radius, radius, color);
    }

    /// <summary>
    /// Malt eine Ellipse mit dem Zentrum position, dem horizontalen Radius radiusX und dem vertikalen Radius radiusY, in der Farbe color
    /// </summary>
    /// <param name="position">Mittelpunkt der Ellipse</param>
    /// <param name="radiusX">Horizontaler Radius der Ellipse</param>
    /// <param name="radiusY">Vertikaler Radius der Ellipse</param>
    /// <param name="color">Farbe der Ellipse</param>
    private void DrawEllipse(Vector2Int position, int radiusX, int radiusY, Color color)
    {
        if (radiusX <= 0 || radiusY <= 0)
            return;

        int x, y, previousX = -1, previousY = -1;
        
        for (float a = 0f; a < 2*Mathf.PI; a += Mathf.PI/360f)
        {
            x = Mathf.RoundToInt(position.x + (radiusX * Mathf.Cos(a)));
            y = Mathf.RoundToInt(position.y + (radiusY * Mathf.Sin(a)));

            if (previousX == x && previousY == y)
                continue;

            previousX = x;
            previousY = y;

            //x liegt außerhalb der Textur
            if (!CheckXInBounds(x))
                continue;

            int drawheight = position.y;
            if (drawheight <= y)
            {
                while (drawheight <= y)
                {
                    if (!CheckYInBounds(drawheight))
                        break;
                    activeTexture.SetPixel(x, drawheight++, color);
                }
                continue;
            }
            while (drawheight > y)
            {
                if (!CheckYInBounds(drawheight))
                    break;
                activeTexture.SetPixel(x, drawheight--, color);
            }
        }
    }

    /// <summary>
    /// Generiert einen Himmel
    /// </summary>
    private void DrawSky()
    {
        Color color = Colors.SkyBottom;
        int stepsize = activeTexture.height / imageSettings.BackgroundColorSteps;
        for(int y = 0; y < activeTexture.height; y++)
        {
            if(y % stepsize == 0)
                color = Color.Lerp(Colors.SkyBottom, Colors.SkyTop, (float)y / (activeTexture.height - stepsize));

            for (int x = 0; x < activeTexture.width; x++)
            { 
                activeTexture.SetPixel(x, y, color);
            }
        }
    }

    /// <summary>
    /// Generiert eine Hügellandschaft auf der aktiven Textur
    /// </summary>
    /// <param name="scaler">Skalierungsfaktor für einige Parameter</param>
    /// <param name="offset">Höhe auf der die Landschaft entstehen soll</param>
    /// <param name="amplitude">Maximale Erhebung/Senkung der Hügel</param>
    /// <param name="color">Farbe der Hügel</param>
    private void DrawHills(float scaler, int offset, int amplitude, Color color)
    {
        float perlinOffset = imageSettings.Seed * scaler;
        float perlinScaler = Mathf.Lerp(0f, 15f, scaler);
        //int snowheight = amplitude / 2 + offset;
        //Color snowColor = Color.Lerp(color, Color.white, .85f);
        int height;
        for (int x = 0; x < activeTexture.width; x++)
        {
            float xCoord1 = (float)x / activeTexture.width * perlinScaler + perlinOffset;
            float xCoord2 = (float)x / activeTexture.width * 2 * perlinScaler + perlinOffset;
            float sample1 = Mathf.PerlinNoise1D(xCoord1);
            float sample2 = Mathf.PerlinNoise1D(xCoord2) / 2;
            float value = (sample1 + sample2) / 1.5f;
            height = Mathf.RoundToInt(value * amplitude + offset);

            if (height < 0)
                continue;

            activeTexture.SetPixel(x, height, color);

            //if (height > snowheight)
            //{
            //    int snowPixels = Mathf.RoundToInt(amplitude/20f * Mathf.Pow(sample1 + sample2 * 2, 2));
            //    for (int y = height; y >= height - snowPixels; y--)
            //    {
            //        activeTexture.SetPixel(x, y, snowColor);
            //    }
            //    height -= snowPixels;
            //}

            for (int y = 0; y <= height; y++)
            {
                activeTexture.SetPixel(x, y, color);
            }
        }
    }

    /// <summary>
    /// Generiert Bäume auf der aktiven Textur
    /// </summary>
    /// <param name="minY">Minimale Y-Koordniate an der ein Baum entstehen kann</param>
    /// <param name="maxY">Maximale Y-Koordinate an der ein Baum entstehen kann</param>
    private void DrawTrees(int minY, int maxY)
    {
        if(minY > maxY) 
            return;

        Random.State stateBeforeTrees = Random.state;
        Random.InitState(imageSettings.Seed);

        float probability = (float)imageSettings.Seed % maxY / maxY / 10f;

        List<Vector2Int> treePositons = new List<Vector2Int>();
        for(int x = 0; x <= imageSettings.Width; x++)
        {
            if(probability > Random.value)
            {
                int y = Random.Range(minY, maxY);
                treePositons.Add(new Vector2Int(x, y));
            }
        }

        treePositons.Sort((tree1, tree2) => -tree1.y.CompareTo(tree2.y));

        Color shadowColor = Color.Lerp(Colors.Grass * Colors.SkyBottom + new Color(.25f, .25f, .25f), Color.black, .25f);

        foreach (Vector2Int tree in treePositons)
        {
            int treeAmount = Random.Range(1, 3);
            for (int i = 0; i < treeAmount; i++)
            {
                int size = (imageSettings.Seed % 3) + Random.Range(1, 4);
                int sunInfluence = -Mathf.RoundToInt(Mathf.Sign(imageSettings.SunPosition.x - imageSettings.Width / 2));
                Vector2Int treeTop = new Vector2Int(tree.x + Random.Range(-3, 4), tree.y + Random.Range(4, 9) + size);
                Vector2Int shadowEnd = new Vector2Int(tree.x + (treeTop.x - tree.x) / 2 + sunInfluence, tree.y - (treeTop.y - tree.y) / 2);
                DrawLine(shadowEnd, new Vector2Int(tree.x, tree.y), shadowColor);                                       //tree trunk shadow
                DrawEllipse(shadowEnd, size, size / 2, shadowColor);                                                    //treetop shadow
                DrawLine(new Vector2Int(tree.x, tree.y), treeTop, Colors.TreeTrunk);                                    //tree trunk
                DrawCircle(treeTop, size, Color.Lerp(Colors.Grass, Colors.SkyTop * 1.5f, .5f));                          //treetop outer
                DrawCircle(treeTop + new Vector2Int(0, -1), size, Color.Lerp(Colors.Grass, Colors.SkyTop * 1.25f, .5f)); //treetop inner
            }
        }

        Random.state = stateBeforeTrees;
    }

    /// <summary>
    /// Malt eine Wasser-Ebene
    /// </summary>
    /// <param name="maxHeight">Maximale Höhe bis zu welcher das Wasser gemalt wird</param>
    private void DrawWater(int maxHeight)
    {
        bool addWave;
        int waveStart = 0, waveLength = 0;
        Color waterColor = Color.Lerp(Colors.Water, Colors.SkyTop, .5f);
        Color waveColor = Color.Lerp(waterColor, Color.white, .25f);

        Random.State stateBeforeWater = Random.state;
        Random.InitState(imageSettings.Seed);

        for (int y = 0; y < maxHeight; y++)
        {
            addWave = Random.Range(0, 1f) <= .5f ? false : true;
            if (addWave)
            {
                waveStart = Random.Range(0, imageSettings.Width);
                waveLength = Random.Range(3, 10);
            }
            for (int x = 0; x < imageSettings.Width; x++)
            {
                if (addWave && x >= waveStart && x <= waveStart + waveLength)
                    activeTexture.SetPixel(x, y, waveColor);
                else
                    activeTexture.SetPixel(x, y, waterColor);
            }
        }

        Random.state = stateBeforeWater;
    }

    /// <summary>
    /// Generiert Wolken auf der aktiven Textur
    /// </summary>
    /// <param name="scaler">Skalierungsfaktor für einige Parameter</param>
    /// <param name="offset">Abstand von der Unterkante des Bildes bevor Wolken erzeugt werden</param>
    private void DrawClouds(float scaler, int offset)
    {
        float perlinOffset = imageSettings.Seed * scaler;
        float perlinScaler = Mathf.Lerp(4f, 6f, scaler);
        float threshold = Mathf.Lerp(.65f, .75f, scaler);
        Color colorToPaint;
        Color cloudColor = Color.Lerp(Colors.SkyBottom, Color.white, .9f);
        for(int x =  0; x < activeTexture.width; x++)
        {
            for (int y = offset; y < activeTexture.height; y++)
            {
                float xCoord = (float)x / activeTexture.width * perlinScaler + perlinOffset;
                float yCoord = (float)y / activeTexture.height * 3 * perlinScaler + perlinOffset;
                float sample = Mathf.PerlinNoise(xCoord, yCoord);
                float value = sample;
                if (value > threshold)
                {
                    Color colorBelow = activeTexture.GetPixel(x, y - 1);
                    if (!Colors.Equal(colorBelow, Colors.SkyBottom) && !Colors.Equal(colorBelow, cloudColor))
                        colorToPaint = Colors.SkyBottom;
                    else
                        colorToPaint = cloudColor;
                    activeTexture.SetPixel(x, y, colorToPaint);
                }
            }
        }
    }

    /// <summary>
    /// Malt eine zufällige Landschaft
    /// </summary>
    public void DrawRandom()
    {
        if (activeTexture == null)
            return;

        float scaler = (imageSettings.Seed % 100) / 100f;     //die letzten beiden Stellen des Seed als Kommazahl
        int amplitude = Mathf.RoundToInt(imageSettings.Amplitude * activeTexture.height);   //Amplitude in Pixel
        int mountainOffsetFromBottom = (imageSettings.Seed/100) % (activeTexture.height / 3) + activeTexture.height/10;
        int maxWaterHight = mountainOffsetFromBottom - amplitude / 4;

        DrawSky();
        DrawCircle(imageSettings.SunPosition, imageSettings.SunRadius, Colors.Sun);
        DrawHills(scaler, mountainOffsetFromBottom, amplitude, Colors.Stone * Colors.SkyBottom);
        DrawHills(scaler / 3, mountainOffsetFromBottom, amplitude / 3, Colors.Grass * Colors.SkyBottom + new Color(.25f, .25f, .25f));
        DrawWater(maxWaterHight);
        DrawTrees(maxWaterHight + 8, mountainOffsetFromBottom);
        DrawClouds(scaler, maxWaterHight + (int)amplitude);
        activeTexture.Apply();
    }

    /// <summary>
    /// Überprüft ob x innerhalb der Breite des Bildes liegt
    /// </summary>
    /// <param name="x">Zu überprüfende X-Koordinate</param>
    /// <returns>true wenn x innerhalb der Breite des Bildes liegt, sonst false</returns>
    private bool CheckXInBounds(int x)
    {
        if(x < 0 || x >= imageSettings.Width)
            return false;
        return true;
    }

    /// <summary>
    /// Überprüft ob y innerhalb der Höhe des Bildes liegt
    /// </summary>
    /// <param name="y">Zu überprüfende Y-Koordinate</param>
    /// <returns>true wenn y innerhalb der Höhe des Bildes liegt, sonst false</returns>
    private bool CheckYInBounds(int y)
    {
        if (y < 0 || y >= imageSettings.Height)
            return false;
        return true;
    }
}
