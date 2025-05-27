using UnityEngine;

public class GameSpeedManager : MonoBehaviour
{
    public enum SpeedState
    {
        Paused,
        Normal,
        Fast,
        UltraFast
    }

    private SpeedState currentSpeed = SpeedState.Normal;

    // Met à jour le timeScale en fonction de l'état actuel
    private void UpdateTimeScale()
    {
        switch (currentSpeed)
        {
            case SpeedState.Paused:
                Time.timeScale = 0f;
                break;
            case SpeedState.Normal:
                Time.timeScale = 1f;
                break;
            case SpeedState.Fast:
                Time.timeScale = 2f;
                break;
            case SpeedState.UltraFast:
                Time.timeScale = 4f;
                break;
        }

        Debug.Log("Speed set to: " + currentSpeed + " (Time.timeScale = " + Time.timeScale + ")");
    }

    // Méthodes publiques à lier aux boutons dans Unity
    public void SetPaused()
    {
        currentSpeed = SpeedState.Paused;
        UpdateTimeScale();
    }

    public void SetNormal()
    {
        currentSpeed = SpeedState.Normal;
        UpdateTimeScale();
    }

    public void SetFast()
    {
        currentSpeed = SpeedState.Fast;
        UpdateTimeScale();
    }

    public void SetUltraFast()
    {
        currentSpeed = SpeedState.UltraFast;
        UpdateTimeScale();
    }

    public SpeedState GetCurrentSpeed()
    {
        return currentSpeed;
    }
}
