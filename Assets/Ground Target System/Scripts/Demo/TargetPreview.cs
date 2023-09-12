using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class TargetPreview : MonoBehaviour
{
    #region Variables

    [SerializeField] private Texture[] previewTextures;
    [SerializeField] private AnimatedProjector[] projectors;

    [SerializeField] private Toggle projectorToggle;
    [SerializeField] private Text projectorTitle;
    [SerializeField] private Text currentTexture;

    [SerializeField] private Light sceneLight;

    [SerializeField] private Text rotationTitle;
    [SerializeField] private Slider rotationSlider;

    [SerializeField] private Toggle autoRotationToggle;

    [SerializeField] private Text autoRotationTitle;
    [SerializeField] private Slider autoRotationSlider;

    [SerializeField] private Text scaleTitle;
    [SerializeField] private Slider scaleSlider;

    [SerializeField] private Text colorTitle;
    [SerializeField] private Slider colorSliderR;
    [SerializeField] private Slider colorSliderG;
    [SerializeField] private Slider colorSliderB;
    [SerializeField] private Slider colorSliderA;

    private AnimatedProjector currentProjector;

    private int projectorIndex;
    private int textureIndex;

    private Color color;

    #endregion

    void Awake()
    {
        UpdateProjector();
    }

    /// <summary>
    /// Selects the next available projector.
    /// </summary>
    public void NextProjector()
    {
        if (projectorIndex < projectors.Length - 1)
            projectorIndex++;
        else
            projectorIndex = 0;

        UpdateProjector();
    }

    /// <summary>
    /// Selects the previous projector.
    /// </summary>
    public void PreviousProjector()
    {
        if (projectorIndex > 0)
            projectorIndex--;
        else
            projectorIndex = projectors.Length - 1;

        UpdateProjector();
    }

    /// <summary>
    /// Selects the next texture.
    /// </summary>
    public void NextTexture()
    {
        if (textureIndex < previewTextures.Length - 1)
            textureIndex++;
        else
            textureIndex = 0;

        UpdateTexture();
    }

    /// <summary>
    /// Selects the previous texture.
    /// </summary>
    public void PreviousTexture()
    {
        if (textureIndex > 0)
            textureIndex--;
        else
            textureIndex = previewTextures.Length - 1;

        UpdateTexture();
    }

    /// <summary>
    /// Enable/Disable the currently selected projector.
    /// </summary>
    /// <param name="enable"></param>
    public void ToggleProjector(bool enable)
    {
        currentProjector.gameObject.SetActive(enable);
    }

    /// <summary>
    /// Enable/Disable auto rotation for the currently selected projector.
    /// </summary>
    /// <param name="enable"></param>
    public void ToggleAutoRotation(bool enable)
    {
        SetAutoRotation(enable, autoRotationSlider.value);
        UpdateAutoRotation();
    }

    /// <summary>
    /// Enable/Disable the scene lighting.
    /// </summary>
    /// <param name="enable"></param>
    public void ToggleSceneLight(bool enable)
    {
        sceneLight.enabled = enable;
    }

    /// <summary>
    /// Updates the scale of the projector based on the slider value.
    /// </summary>
    public void UpdateScale()
    {
        float value = scaleSlider.value;

        currentProjector.DefaultSize = value;

        scaleTitle.text = string.Format("Scale: {0:0.00}", value);
        scaleSlider.value = value;
    }

    /// <summary>
    /// Updates the rotation of the projector based on the slider value.
    /// </summary>
    public void UpdateRotation()
    {
        currentProjector.transform.localRotation = Quaternion.Euler(90, rotationSlider.value, 0);
        rotationTitle.text = string.Format("Rotation: {0}", rotationSlider.value);
    }

    /// <summary>
    /// Updates the speed of auto rotation based on the slider value.
    /// </summary>
    public void UpdateAutoRotation()
    {
        currentProjector.rotate = autoRotationToggle.isOn;
        currentProjector.rotationSpeed = autoRotationSlider.value;

        autoRotationTitle.text = string.Format("Speed: {0:0.00}", autoRotationSlider.value);
    }

    /// <summary>
    /// Updates the color based on the R,G,B slider values.
    /// </summary>
    public void UpdateColor()
    {
        color = new Color(colorSliderR.value, colorSliderG.value, colorSliderB.value, colorSliderA.value);
        currentProjector.DefaultColor = color;
    }

    /// <summary>
    /// Updates all elements of the currently selected projector.
    /// </summary>
    private void UpdateProjector()
    {
        projectorTitle.text = string.Format("Projector #{0}", projectorIndex + 1);

        currentProjector = projectors[projectorIndex];

        textureIndex = GetTextureIndex();
        UpdateTexture();

        SetScale(currentProjector.DefaultSize);
        SetRotation(currentProjector.transform.localRotation.y);
        SetColor(currentProjector.DefaultColor);

        SetAutoRotation(currentProjector.rotate, currentProjector.rotationSpeed);

        projectorToggle.isOn = currentProjector.gameObject.activeInHierarchy;
    }

    /// <summary>
    /// Updates the texture currently assigned to the selected projector.
    /// </summary>
    private void UpdateTexture()
    {
        currentProjector.DefaultTexture = previewTextures[textureIndex];

        if (currentTexture != null)
            currentTexture.text = string.Format("Texture: {0}", previewTextures[textureIndex].name);
    }

    /// <summary>
    /// Sets the scale of the currently selected projector.
    /// </summary>
    /// <param name="scale"></param>
    private void SetScale(float scale)
    {
        scaleSlider.value = scale;

        UpdateScale();
    }

    /// <summary>
    /// Sets the auto rotation properties of the currently selected projector.
    /// </summary>
    /// <param name="enable"></param>
    /// <param name="speed"></param>
    private void SetAutoRotation(bool enable, float speed)
    {
        autoRotationToggle.isOn = enable;
        autoRotationSlider.value = speed;

        if (autoRotationToggle.isOn)
        {
            rotationTitle.transform.parent.gameObject.SetActive(false);
            rotationSlider.gameObject.SetActive(false);

            autoRotationTitle.transform.parent.gameObject.SetActive(true);
            autoRotationSlider.gameObject.SetActive(true);
        }
        else
        {
            autoRotationTitle.transform.parent.gameObject.SetActive(false);
            autoRotationSlider.gameObject.SetActive(false);

            rotationTitle.transform.parent.gameObject.SetActive(true);
            rotationSlider.gameObject.SetActive(true);
        }
    }

    /// <summary>
    /// Sets the rotation of the currently selected projector.
    /// </summary>
    /// <param name="rotation"></param>
    private void SetRotation(float rotation)
    {
        rotationSlider.value = rotation;

        UpdateRotation();
    }

    /// <summary>
    /// Sets the color of the currently selected projector.
    /// </summary>
    /// <param name="color"></param>
    private void SetColor(Color color)
    {
        colorSliderR.value = color.r;
        colorSliderG.value = color.g;
        colorSliderB.value = color.b;
        colorSliderA.value = color.a;

        UpdateColor();
    }

    /// <summary>
    /// Returns the texture id for the currently selected projector.
    /// </summary>
    /// <returns></returns>
    private int GetTextureIndex()
    {
        int index = previewTextures.Select((s, i) => new { i, s })
            .Where(t => t.s.name == currentProjector.DefaultTexture.name)
            .Select(t => t.i).FirstOrDefault();

        return index;
    }
}
