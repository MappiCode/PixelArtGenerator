using UnityEngine;

public class ImageSettings : MonoBehaviour
{
    // 480p 4:3
    public const int MAX_WIDTH = 640;
    public const int MAX_HEIGHT = 480;
    private const int MIN_WIDTH = 16;
    private const int MIN_HEIGHT = 16;
    private const int SEEDRANGE = 1000000;
    private const float MIN_AMPLITUDE = .01f;
    private const float MAX_AMPLITUDE = .5f;
    private const int MIN_BGCOLORSTEPS = 4;
    private const int MAX_BGCOLORSTEPS = 16;
    private const int MIN_SUNRADIUS = 0;
    private const int MAX_SUNRADIUS = 48;

    [SerializeField]
    private UIManager uiManager;

    [SerializeField]
    private int width;
    public int Width
    {
        get { return width; }
        set
        {
            width = value switch
            {
                < MIN_WIDTH => MIN_WIDTH,
                > MAX_WIDTH => MAX_WIDTH,
                _ => value,
            };
        }
    }

    [SerializeField] 
    private int height;
    public int Height
    {
        get { return height; }
        set
        {
            height = value switch
            {
                < MIN_HEIGHT => MIN_HEIGHT,
                > MAX_HEIGHT => MAX_HEIGHT,
                _ => value,
            };
        }
    }

    [SerializeField]
    private int seed;
    public int Seed
    {
        get { return seed; }
        set
        {
            seed = value switch
            {
                < 0 => -value,
                _ => value,
            };
            Initialize();
        }
    }

    [SerializeField]
    private float amplitude;
    public float Amplitude
    {
        get { return amplitude; }
        set
        {
            amplitude = value switch
            {
                < MIN_AMPLITUDE => MIN_AMPLITUDE,
                > MAX_AMPLITUDE => MAX_AMPLITUDE,
                _ => value,
            };
        }
    }

    [SerializeField]
    private int backgroundColorSteps;
    public int BackgroundColorSteps
    {
        get { return backgroundColorSteps; }
        set
        {
            backgroundColorSteps = value switch
            {
                < MIN_BGCOLORSTEPS => MIN_BGCOLORSTEPS,
                > MAX_BGCOLORSTEPS => MAX_BGCOLORSTEPS,
                _ => value,
            }; 
        }
    }

    [SerializeField]
    private Vector2Int sunPosition;
    public Vector2Int SunPosition
    {
        get { return sunPosition; }
        set
        {
            if (value.y < 0)
                sunPosition.y = 0;
            else if (value.y >= height)
                sunPosition.y = height - 1;
            else
                sunPosition.y = value.y;
            
            if (value.x < 0)
                sunPosition.x = 0;
            else if (value.x >= width)
                sunPosition.x = width - 1;
            else
                sunPosition.x = value.x;
        }
    }

    [SerializeField]
    private int sunRadius;
    public int SunRadius
    {
        get { return sunRadius; }
        set
        {
            sunRadius = value switch
            {
                < MIN_SUNRADIUS => MIN_SUNRADIUS,
                > MAX_SUNRADIUS => MAX_SUNRADIUS,
                _ => value,
            };
        }
    }

    private void Awake()
    {
        ImageExporter.ImageParameters = this;
        Randomise();
    }

    /// <summary>
    /// Setzt einen neuen zufälligen Seed und Initialisiert alle anderen Parameter
    /// </summary>
    public void Randomise()
    {
        RandomiseSeed();
        Initialize();
    }

    /// <summary>
    /// Initialisiert alle Parameter abhängig vom aktuellen Seed
    /// </summary>
    private void Initialize()
    {
        Random.InitState(Seed);
        Colors.GenerateNewSkyColors();
        RandomiseAmplitude();
        RandomiseBackgroundColorSteps();
        RandomiseSunPosition();
        RandomiseSunRadius();
        uiManager.UpdateInputs();
    }

    public void RandomiseSeed()
    {
        Seed = Random.Range(0, SEEDRANGE);
    }

    public void RandomiseAmplitude()
    {
        Amplitude = Random.Range(MIN_AMPLITUDE, MAX_AMPLITUDE);
    }

    public void RandomiseBackgroundColorSteps()
    {
        BackgroundColorSteps = Random.Range(MIN_BGCOLORSTEPS, MAX_BGCOLORSTEPS + 1);
    }

    public void RandomiseSunPosition()
    {
        int x = Random.Range(0, width);
        int y = height - Random.Range(0, height / 3);
        SunPosition = new Vector2Int(x, y);
    }

    public void RandomiseSunRadius()
    {
        int maxRadius = width/4 < MAX_SUNRADIUS ? width/4 : MAX_SUNRADIUS;
        SunRadius = Random.Range(MIN_SUNRADIUS, maxRadius + 1);
    }
}
