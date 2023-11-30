using UnityEngine;

public static class Colors
{
    private static Color skyBottom;
    public static Color SkyBottom
    {
        get {  return skyBottom; }
        set { skyBottom = value; }
    }

    private static Color skyTop;
    public static Color SkyTop
    {
        get { return skyTop; }
        set { skyTop = value; }
    }

    public static readonly Color Grass = new Color(0.468f, 0.718f, 0.351f);
    public static readonly Color Stone = new Color(0.5f, 0.5f, 0.5f);
    public static readonly Color Sun = new Color(1, 0.912f, 0.016f);
    public static readonly Color TreeTrunk = new Color(0.378f, 0.203f, 0.141f);
    public static readonly Color Water = new Color(0.512f, 0.726f, 0.953f);

    /// <summary>
    /// Generiert neue Himmelsfarben.
    /// </summary>
    public static void GenerateNewSkyColors()
    {
        skyBottom = NewRandomColor();
        skyTop = NewRandomColor();
    }

    /// <summary>
    /// Prüft zwei Colors c1 und c2 auf Gleichheit
    /// </summary>
    /// <param name="c1">Farbe 1</param>
    /// <param name="c2">Farbe 2</param>
    /// <returns>true wenn c1 und c2 gleich sind, false wenn c1 und c2 verschieden sind</returns>
    public static bool Equal(Color c1, Color c2)
    {
        if(Mathf.RoundToInt(c1.r * 255) != Mathf.RoundToInt(c2.r * 255))
            return false;
        if(Mathf.RoundToInt(c1.g * 255) != Mathf.RoundToInt(c2.g * 255))
            return false;
        if(Mathf.RoundToInt(c1.b * 255) != Mathf.RoundToInt(c2.b * 255))
            return false;
        if(c1.a != c2.a)
            return false;
        return true;
    }

    /// <summary>
    /// Erzeugt eine neue Farbe, mit zufälligen rgb Werten
    /// </summary>
    /// <returns>Die erzeugte Farbe als Color</returns>
    public static Color NewRandomColor()
    {
        float r = Random.Range(0.0f, 1.0f);
        float g = Random.Range(0.0f, 1.0f);
        float b = Random.Range(0.0f, 1.0f);
        return new Color(r, g, b);
    } 
}
