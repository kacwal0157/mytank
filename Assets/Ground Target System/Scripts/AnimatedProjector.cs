using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(Projector))]
//[AddComponentMenu("Game Native/Ground Target System/Animated Projector")]
public class AnimatedProjector : MonoBehaviour
{
    public delegate void TransitionCompleted();
    public event TransitionCompleted OnPulseCompleted;

    #region variables

    private Projector projector;

    public Texture texture;
    public Color color;

    // Rotation
    public bool rotate = false;
    public float rotation = 0;
    public float rotationSpeed;

    // Pulse
    public bool pulse = false;
    public bool pulseLoop = false;

    public float pulseSpeed = 0;
    public float pulseMin;
    public float pulseMax;

    private bool pulseFlip = false;
    private float pulseTime = 0;

    // Color
    public bool colorBlend = false;
    public float colorSpeed = 0;
    public List<Color> colors;

    private int colorIndex = 0;
    private float colorTime = 0;

    private const float rotationOffset = 360.0f;

    private float Scale
    {
        get
        {
            return this.transform.localScale.x;
        }
        set
        {
            this.transform.localScale = new Vector3(value, value, value);
        }
    }

    #endregion

    void Awake()
    {
        projector = GetComponent<Projector>();

        DefaultTexture = texture;
        DefaultColor = color;

        if (pulse)
            DefaultSize = pulseMin;
    }

    void FixedUpdate()
    {
        if (rotate)
            transform.Rotate(Vector3.forward * rotationSpeed * rotationOffset * Time.deltaTime);

        if (pulse)
        {
            float pulseSize = projector.orthographicSize;
            float stepAmount = Time.deltaTime * pulseSpeed;

            if (!pulseFlip)
                pulseSize = Mathf.Lerp(pulseSize, pulseMax, stepAmount);
            else
                pulseSize = Mathf.Lerp(pulseSize, pulseMin, stepAmount);


            if (pulseTime < 1)
                pulseTime += stepAmount;
            else
            {
                pulseTime = 0;

                if (pulseLoop)
                    pulseFlip = !pulseFlip;
                else
                    pulse = false;

                if (OnPulseCompleted != null)
                    OnPulseCompleted();
            }

            projector.orthographicSize = pulseSize;
        }

        if (colorBlend && colors.Count > 0)
        {
            float stepAmount = Time.deltaTime * colorSpeed;

            projector.material.color = Color.Lerp(projector.material.color, colors[colorIndex], stepAmount);

            if (colorTime < 1)
                colorTime += stepAmount;
            else
            {
                colorTime = 0;

                if (colorIndex < colors.Count - 1)
                    colorIndex++;
                else
                    colorIndex = 0;
            }
        }
    }

    /// <summary>
    /// The default size of the projector.
    /// </summary>
    public float DefaultSize
    {
        get
        {
            if (projector == null)
                projector = GetComponent<Projector>();

            if (projector != null)
                return projector.orthographicSize;

            return 0;
        }

        set
        {
            if (projector == null)
                projector = GetComponent<Projector>();

            if (projector != null)
                projector.orthographicSize = value * Scale;
        }
    }

    /// <summary>
    /// The default texture used by the projector.
    /// </summary>
    public Texture DefaultTexture
    {
        get
        {
            if (projector == null)
                projector = GetComponent<Projector>();

            if (projector != null && projector.material != null)
                return projector.material.GetTexture("_ShadowTex");

            return null;
        }

        set
        {
            if (projector == null)
                projector = GetComponent<Projector>();

            if (projector != null && projector.material != null)
                projector.material.SetTexture("_ShadowTex", value);
        }
    }

    /// <summary>
    /// The default color used by the projector.
    /// </summary>
    public Color DefaultColor
    {
        get
        {
            if (projector == null)
                projector = GetComponent<Projector>();

            if (projector != null && projector.material != null)
                return projector.material.GetColor("_Color");

            return Color.black;
        }

        set
        {
            if (projector == null)
                projector = GetComponent<Projector>();

            if (projector != null && projector.material != null)
                projector.material.SetColor("_Color", value);
        }
    }

    /// <summary>
    /// Adjusts the scale of the current projector based on a defined size value.
    /// </summary>
    /// <param name="size"></param>
    /// <param name="scale"></param>
    public void SetScale(float size, float scale)
    {
        Scale = scale;
        DefaultSize = size;

        pulseMin *= scale;
        pulseMax *= scale;
    }

    /// <summary>
    /// Sets the projectors material.
    /// </summary>
    /// <param name="material"></param>
    public void SetMaterial(Material material)
    {
        if (projector == null)
            projector = GetComponent<Projector>();

        if (projector != null)
            projector.material = material;
    }

    /// <summary>
    /// Updates default values when creating the prefab.
    /// </summary>
    public void Initialize()
    {
        if (projector == null)
            projector = GetComponent<Projector>();

        projector.orthographic = true;
    }

    /// <summary>
    /// Forces an updated to reflect the current values.
    /// </summary>
    public void Refresh()
    {
        Vector3 lastRotation = this.transform.localRotation.eulerAngles;
        this.transform.localRotation = Quaternion.Euler(lastRotation.x, rotation, lastRotation.z);
    }
}
