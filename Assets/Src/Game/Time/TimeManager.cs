using TMPro;
using UnityEngine;

public class TimeManager
{
    private TextMeshProUGUI timeRemaining;
    public float roundTime { get; set; }
    private float defaultRoundTime;
    public bool stopTime { get; set; } = false;
    public bool isNowPlayerTurn;
    private GameObject playerPrefab;
    private TextMeshProUGUI timeOfFlight;


    public TimeManager(TextMeshProUGUI timeRemaining, float roundTime, bool stopTime, GameObject playerPrefab, PublicGameObjects publicGameObjects)
    {
        this.timeRemaining = timeRemaining;
        this.roundTime = roundTime;
        this.stopTime = stopTime;
        this.playerPrefab = playerPrefab;
        
        defaultRoundTime = roundTime;
        timeRemaining.text = roundTime.ToString("F1");
        timeOfFlight = publicGameObjects.timeOfFlight;
    }

    public void OnUpdate()
    {
        timeRemaining.text = roundTime.ToString("F1");
        countTime();
    }

    private void countTime()
    {
        if (stopTime == false)
        {
            if (roundTime < 0)
            {
                GameManager.currentStage = GameManager.Stage.END_OF_TIME;
                roundTime = 0;
                stopTime = true;
            }
            else
            {
                roundTime -= Time.deltaTime;
            }
        }
        else
        {
            roundTime = roundTime;
        }
       
    }

    public void resetTimer()
    {
        roundTime = defaultRoundTime;
    }

    public void setDefaultLeftTime()
    {
        defaultRoundTime = roundTime;
    }

    public void countTimeOfFlight()
    {
        timeOfFlight.text = Mathf.Clamp(PlayerController.lastShotTimeOfFlight - (Time.time - PlayerController.lastShotTime), 0, float.MaxValue).ToString("F1");
    }

    public void setTimeOfFlight()
    {
        timeOfFlight.text = Mathf.Clamp(PlayerController.currentTimeOfFlight, 0, float.MaxValue).ToString("F1");
    }

    public void whoseTimeIsUp(GameObject activePlayer)
    {
        if (activePlayer == playerPrefab)
        {
            FireController.turnOfTargetShield();
            GameManager.getInstance.changeState( GameManager.Stage.PLAYER_TURN_AIMING);
        }
        else
        {
            FireController.turnOfTargetShield();
            GameManager.getInstance.changeState(GameManager.Stage.OPONNENT_TURN_AIMING);
        }
    }
}
