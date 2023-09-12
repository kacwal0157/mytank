using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class SceneAnimator : MonoBehaviour
{
    //---------------------------------------------
    // VARIABLES
    //---------------------------------------------

    private static SceneAnimator instance = null;
    private Material m_Material = null;
    private string levelName = "";
    private int levelIndex = 0;
    private bool fading = false;

    //---------------------------------------------
    // SINGLETON HOLDER
    //---------------------------------------------

    private static SceneAnimator Instance
    {
        get
        {
            if (instance == null)
            {
                instance = (new GameObject("SceneAnimator")).AddComponent<SceneAnimator>();
            }
            return instance;
        }
    }

    //---------------------------------------------
    // CLASS LOGIC
    //---------------------------------------------

    
    private void StartFade(float aFadeOutTime, float aFadeInTime, Color aColor)
    {
        fading = true;
        StartCoroutine(Fade(aFadeOutTime, aFadeInTime, aColor));
    }

    public static void loadLevel(string aLevelName, float aFadeOutTime, float aFadeInTime, Color aColor)
    {
        if (isFading)
        {
            return;
        }
        Instance.levelName = aLevelName;
        Instance.StartFade(aFadeOutTime, aFadeInTime, aColor);
    }

    public static void loadLevel(int aLevelIndex, float aFadeOutTime, float aFadeInTime, Color aColor)
    {
        if (isFading)
        {
            return;
        }
        Instance.levelName = "";
        Instance.levelIndex = aLevelIndex;
        Instance.StartFade(aFadeOutTime, aFadeInTime, aColor);
    }

    private void Awake()
    {
        DontDestroyOnLoad(this);
        instance = this;

        Shader shader = Resources.Load("Shaders/SceneAnimator") as Shader;
        m_Material = new Material(shader);
    }

    private void DrawQuad(Color aColor, float aAlpha)
    {
        aColor.a = aAlpha;
        m_Material.SetPass(0);
        GL.PushMatrix();
        GL.LoadOrtho();
        GL.Begin(GL.QUADS);
        GL.Color(aColor);
        GL.Vertex3(0, 0, -1);
        GL.Vertex3(0, 1, -1);
        GL.Vertex3(1, 1, -1);
        GL.Vertex3(1, 0, -1);
        GL.End();
        GL.PopMatrix();
    }

    //---------------------------------------------
    // GETTERS AND SETTERS
    //---------------------------------------------

    public static bool isFading
    {
        get { return Instance.fading; }
    }

    private IEnumerator Fade(float fadeOutTime, float fadeInTime, Color color)
    {
        float t = 0.0f;
        while (t < 1.0f)
        {
            yield return new WaitForEndOfFrame();
            t = Mathf.Clamp01(t + Time.deltaTime / fadeOutTime);
            DrawQuad(color, t);
        }

        if (levelName != "")
        {
            SceneManager.LoadScene(levelName);
        }
        else
        {
            SceneManager.LoadScene(levelIndex);
        }
        while (t > 0.0f)
        {
            yield return new WaitForEndOfFrame();
            t = Mathf.Clamp01(t - Time.deltaTime / fadeInTime);
            DrawQuad(color, t);
        }
        fading = false;
    }
}