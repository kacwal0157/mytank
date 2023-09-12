using UnityEngine;
using UnityEngine.UI;

public class TargetOnScreenService
{
    //DEFAULT VALUES IN CASE OF ERROR
    private float whiteRingSize = 1.05f;
    private float blackRingSize = 0.9f;
    private float blueRingSize = 0.75f;
    private float redRingSize = 0.5f;
    private float yellowRingSize = 0.2f;
    private float sizeMultiplier = 130f;

    private GameObject aimTarget;

    public TargetOnScreenService(GameObject aimTarget, PlayerConfigurationService playerConfigurationService)
    {
        this.aimTarget = aimTarget;
        sizeMultiplier = playerConfigurationService.projectorSizeMultiplier;

        whiteRingSize = playerConfigurationService.whiteProjectorSize * sizeMultiplier;
        blackRingSize = (playerConfigurationService.blackProjectorSize / playerConfigurationService.whiteProjectorSize) * sizeMultiplier;
        blueRingSize = (playerConfigurationService.blueProjectorSize / playerConfigurationService.whiteProjectorSize) * sizeMultiplier;
        redRingSize = (playerConfigurationService.redProjectorSize / playerConfigurationService.whiteProjectorSize) * sizeMultiplier;
        yellowRingSize = (playerConfigurationService.yellowProjectorSize / playerConfigurationService.whiteProjectorSize) * sizeMultiplier;
    }

    public void onAwake()
    {
        RectTransform[] children = aimTarget.transform.GetComponentsInChildren<RectTransform>();
        foreach (RectTransform child in children)
        {
            if (child.name.Contains("Yellow"))
            {
                child.sizeDelta = new Vector2(yellowRingSize, yellowRingSize);
            }

            if (child.name.Contains("Red"))
            {
                child.sizeDelta = new Vector2(redRingSize, redRingSize);
            }

            if (child.name.Contains("Blue"))
            {
                child.sizeDelta = new Vector2(blueRingSize, blueRingSize);
            }

            if (child.name.Contains("Black"))
            {
                child.sizeDelta = new Vector2(blackRingSize, blackRingSize);
            }

            if (child.name.Contains("White"))
            {
                child.sizeDelta = new Vector2(whiteRingSize, whiteRingSize);
            }

            if (child.name.Contains("Horizontal") || child.name.Contains("Vertical"))
            {
                child.sizeDelta = new Vector2(child.sizeDelta.x * sizeMultiplier, child.sizeDelta.y * sizeMultiplier);
            }
        }
    }

}
