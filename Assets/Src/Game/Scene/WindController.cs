using UnityEngine;
using TMPro;

public class WindController
{
    private PlayerConfigurationService playerConfigurationService;
    private PublicGameObjects publicGameObjects;
    private GameObject windArrow;
    private TextMeshProUGUI windDirectionBasedOnValue;

    public static Vector2 windDirection;
    public static int windStrengh;

    private Quaternion newWindArrowRotation;
    private int windDirectionX;
    private int windDirectionZ;

    public WindController(PlayerConfigurationService playerConfigurationService, PublicGameObjects publicGameObjects)
    {
        this.playerConfigurationService = playerConfigurationService;
        this.publicGameObjects = publicGameObjects;

        onStart();
    }

    private void onStart()
    {
        windDirectionX = playerConfigurationService.directionOfWindX;
        windDirectionZ = playerConfigurationService.directionOfWindZ;

        windArrow = publicGameObjects.windArrow;
        windArrow.transform.rotation = Quaternion.Euler(0f, 0f, 0f);
        windDirectionBasedOnValue = publicGameObjects.windDirectionBasedOnValue;

        windStrengh = playerConfigurationService.windStrengh;
        windDirection = new Vector2(windDirectionX, windDirectionZ);

        setDirectionOfWind();
    }

    public void onUpdate()
    {
        //TODO: Make wind strengh change in update state if it is changed in other script or make it here (same with wind direction)
        updateArrowRotation();
    }

    private void updateArrowRotation()
    {
        if (windDirectionX > 0 && windDirectionZ == 0)
        {
            newWindArrowRotation = Quaternion.Euler(0f, 0f, -90f);
            windArrow.transform.rotation = Quaternion.Lerp(windArrow.transform.rotation, newWindArrowRotation, Time.deltaTime);
        }
        else if (windDirectionX < 0 && windDirectionZ == 0)
        {
            newWindArrowRotation = Quaternion.Euler(0f, 0f, 90f);
            windArrow.transform.rotation = Quaternion.Lerp(windArrow.transform.rotation, newWindArrowRotation, Time.deltaTime);
        }
        else if (windDirectionX == 0 && windDirectionZ > 0)
        {
            newWindArrowRotation = Quaternion.Euler(0f, 0f, 0f);
            windArrow.transform.rotation = Quaternion.Lerp(windArrow.transform.rotation, newWindArrowRotation, Time.deltaTime);
        }
        else if (windDirectionX == 0 && windDirectionZ < 0)
        {
            newWindArrowRotation = Quaternion.Euler(0f, 0f, 180f);
            windArrow.transform.rotation = Quaternion.Lerp(windArrow.transform.rotation, newWindArrowRotation, Time.deltaTime);
        }
        else if (windDirectionX > 0 && windDirectionZ > 0)
        {
            newWindArrowRotation = Quaternion.Euler(0f, 0f, -45f);
            windArrow.transform.rotation = Quaternion.Lerp(windArrow.transform.rotation, newWindArrowRotation, Time.deltaTime);
        }
        else if (windDirectionX > 0 && windDirectionZ < 0)
        {
            newWindArrowRotation = Quaternion.Euler(0f, 0f, -135f);
            windArrow.transform.rotation = Quaternion.Lerp(windArrow.transform.rotation, newWindArrowRotation, Time.deltaTime);
        }
        else if (windDirectionX < 0 && windDirectionZ > 0)
        {
            newWindArrowRotation = Quaternion.Euler(0f, 0f, 45f);
            windArrow.transform.rotation = Quaternion.Lerp(windArrow.transform.rotation, newWindArrowRotation, Time.deltaTime);
        }
        else if (windDirectionX < 0 && windDirectionZ < 0)
        {
            newWindArrowRotation = Quaternion.Euler(0f, 0f, 135f);
            windArrow.transform.rotation = Quaternion.Lerp(windArrow.transform.rotation, newWindArrowRotation, Time.deltaTime);
        }
        else
        {
            newWindArrowRotation = Quaternion.Euler(0f, 0f, 0f);
            windArrow.transform.rotation = Quaternion.Lerp(windArrow.transform.rotation, newWindArrowRotation, Time.deltaTime);
        }
    }

    private void setDirectionOfWind()
    {
        if (windDirectionX > 0 && windDirectionZ == 0)
        {
            windDirectionBasedOnValue.text = windStrengh.ToString("") + "E";
        }
        else if (windDirectionX < 0 && windDirectionZ == 0)
        {
            windDirectionBasedOnValue.text = windStrengh.ToString("") + "W";
        }
        else if (windDirectionX == 0 && windDirectionZ > 0)
        {
            windDirectionBasedOnValue.text = windStrengh.ToString("") + "N";
        }
        else if (windDirectionX == 0 && windDirectionZ < 0)
        {
            windDirectionBasedOnValue.text = windStrengh.ToString("") + "S";
        }
        else if (windDirectionX > 0 && windDirectionZ > 0)
        {
            windDirectionBasedOnValue.text = windStrengh.ToString("") + "NE";
        }
        else if (windDirectionX > 0 && windDirectionZ < 0)
        {
            windDirectionBasedOnValue.text = windStrengh.ToString("") + "NW";
        }
        else if (windDirectionX < 0 && windDirectionZ > 0)
        {
            windDirectionBasedOnValue.text = windStrengh.ToString("") + "SE";
        }
        else if (windDirectionX < 0 && windDirectionZ < 0)
        {
            windDirectionBasedOnValue.text = windStrengh.ToString("") + "SW";
        }
        else
        {
            windDirectionBasedOnValue.text = windStrengh.ToString("") + "N";
        }
    }

    public float handleDirectionOfWind(float newPosition, float windDirection, int enemyDifficulty)
    {
        var diff = 5 / enemyDifficulty;

        if(windStrengh == 0 || windDirection == 0)
        {
            return newPosition;
        }

        if(windDirection == 1)
        {
            newPosition -= windStrengh * PlayerController.currentTimeOfFlight * diff;
        }

        if (windDirection == -1)
        {
            newPosition += windStrengh * PlayerController.currentTimeOfFlight * diff;
        }

        return Mathf.RoundToInt(newPosition);
    }
}