using UnityEngine;

public class PendulumEffectController
{
    private int horizontalLineCounter = 0;
    private int verticalLineCounter = 0;
    private float sinAmplitude = 60f;
    private float speedFactor = 8f;

    private float horizontalLinePosition = 0f;
    private float verticalLinePosition = 0f;

    public PendulumEffectController(PlayerConfigurationService playerConfigurationService)
    {
        sinAmplitude = playerConfigurationService.projectorSizeMultiplier / 2f;
    }

    public float getHorizontalLinePosition()
    {
        horizontalLineCounter++;
        float x = horizontalLineCounter / (speedFactor * Mathf.PI);
        horizontalLinePosition = Mathf.Sin(x) * sinAmplitude;
        return horizontalLinePosition;
    }

    public float getVerticalLinePosition()
    {
        verticalLineCounter++;
        float x = verticalLineCounter / (speedFactor * Mathf.PI);
        verticalLinePosition = Mathf.Sin(x) * sinAmplitude;
        return verticalLinePosition;
    }

    public float getHorizontalLinePositionInPercentage()
    {
        return horizontalLinePosition / sinAmplitude;
    }

    public float getVerticalLinePositionInPercentage()
    {
        return verticalLinePosition / sinAmplitude;
    }
}
